using Exiled.API.Interfaces;

namespace SCPDiscord
{
	public class Config : IConfig
	{
		public bool IsEnabled { get; set; } = true;

		public string SteamApiKey { get; set; } = string.Empty;

		public string Address { get; set; } = "127.0.0.1";

		public int Port { get; set; } = 8080;
	}
}
