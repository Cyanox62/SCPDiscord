using Newtonsoft.Json;
using System;
using System.Buffers.Binary;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SCPDiscord
{
	public class Tcp
	{
		private string ip;
		private int port;
		private TcpClient client;
		private NetworkStream stream;
		private bool reconLoop;

		public Tcp(string ip, int port)
		{
			this.ip = ip;
			this.port = port;
		}

		public delegate void MessageReceived(MessageReceivedEventArgs ev);
		public event MessageReceived onMessageReceived;

		public delegate void Connected();
		public event Connected onConnected;

		public void Connect()
		{
			reconLoop = true;
			try
			{
				new Thread(AttemptConnection).Start();
			}
			catch (Exception x)
			{
				Exiled.API.Features.Log.Error("Connection thread creation failure.");
			}
		}

		public void Disconnect(bool tryReconnect = false)
		{
			reconLoop = false;
			if (tryReconnect) Connect();
			if (IsConnected()) client.Close();
		}

		private void AttemptConnection()
		{
			while (!IsConnected() && reconLoop)
			{
				try
				{
					client = new TcpClient();
					client.Connect(ip, port);
					stream = client.GetStream();

					onConnected();

					new Thread(Listen).Start();
				}
				catch (Exception x)
				{
					// Failed to connect
					Exiled.API.Features.Log.Error("Failed to connect to host, retrying in 10 seconds...");
				}
				Thread.Sleep(10000);
			}
		}

		private void Listen()
		{
			byte[] bytes;

			while (IsConnected())
			{
				// Get message size
				bytes = new byte[8];
				int size = stream.Read(bytes, 0, 8);
				if (size == 0) Disconnect(true);

				// Parse big endian
				int messageSize = (int)BinaryPrimitives.ReadUInt64BigEndian(bytes);
				int receivedBytes = 0;

				// Create array for incoming bytes
				bytes = new byte[messageSize];

				// While we don't have all the bytes, keep reading
				while (receivedBytes < messageSize)
				{
					// Write to bytes, offset by how much we've already read, total size being the length minus how much we already have read
					receivedBytes += stream.Read(bytes, receivedBytes, bytes.Length - receivedBytes);
				}

				// Emit event
				onMessageReceived(new MessageReceivedEventArgs()
				{
					Bytes = bytes,
					Object = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(bytes))
				});
			}
		}

		public void WriteStream(object data) => WriteStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data)));

		public void WriteStream(byte[] bytes)
		{
			if (IsConnected())
			{
				// Create message size header
				byte[] header = new byte[8];
				ulong headerSize = Convert.ToUInt64(bytes.Length);
				BinaryPrimitives.WriteUInt64BigEndian(header, headerSize);

				// Create variables to track the total size and total bytes sent
				int messageSize = (int)headerSize;
				int sentBytes = 0;

				// Merge header and data
				byte[] bytesToWrite = MergeBytes(header, bytes);

				// Continue writing until all data has gone through
				while (sentBytes < messageSize)
				{
					int size = bytesToWrite.Length - sentBytes;
					stream.Write(bytesToWrite, sentBytes, size);
					sentBytes += size;
				}
			}
		}

		private byte[] MergeBytes(byte[] first, byte[] second)
		{
			byte[] bytes = new byte[first.Length + second.Length];
			Buffer.BlockCopy(first, 0, bytes, 0, first.Length);
			Buffer.BlockCopy(second, 0, bytes, first.Length, second.Length);
			return bytes;
		}

		public bool IsConnected()
		{
			if (client == null || client.Client == null)
			{
				return false;
			}
			try
			{
				return !((client.Client.Poll(1000, SelectMode.SelectRead) && (client.Client.Available == 0)) || !client.Client.Connected);
			}
			catch
			{
				return false;
			}
		}
	}

	public class MessageReceivedEventArgs : EventArgs
	{
		public byte[] Bytes { get; set; }
		public object Object { get; set; }
	}
}
