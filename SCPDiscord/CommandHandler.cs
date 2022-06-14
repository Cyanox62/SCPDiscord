using Exiled.API.Features;
using Newtonsoft.Json.Linq;
using SCPDiscord.DataObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace SCPDiscord
{
	class CommandHandler
	{
		private static WebClient webclient = new WebClient();

		public static void HandleCommand(JObject o)
		{
			try
			{
				string type = (string)o["type"];
				if (type == "IDENT")
				{
					if ((string)o["data"] == "PASS") Log.Debug($"Server {ServerConsole.Port} passed identification.");
					else if ((string)o["data"] == "FAIL")
					{
						Log.Warn($"Server {ServerConsole.Port} failed identification.");
						EventHandlers.tcp.Disconnect();
					}
				}
				else if (type == "UPDATE")
				{
					EventHandlers.tcp.WriteStream(new Update());
				}
				else if (type == "COMMAND")
				{
					//Log.Warn(o);
					//Server.RunCommand($"/{(string)o["command"]}");
					GameCore.Console.singleton.TypeCommand($"/{(string)o["command"]}");
				}
				else if (type == "BAN")
				{
					bool isuid = false;
					string uid = (string)o["user"];
					if (!uid.Contains("@steam") && !uid.Contains("@discord"))
					{
						if (!uid.Contains("."))
						{
							isuid = true;
							uid += "@steam";
						}
					}
					else
					{
						isuid = true;
					}
					Player player = Player.Get(uid);
					int min = (int)o["min"];
					string reason = (string)o["reason"];

					Ban ban = new Ban
					{
						player = null,
						duration = min,
						success = true,
						offline = false
					};

					if (player != null)
					{
						PlayerManager.localPlayer.GetComponent<BanPlayer>().BanUser(player.GameObject, min, reason, "Server");

						ban.player = new User
						{
							name = player.Nickname,
							userid = player.UserId
						};
					}
					else
					{
						if (isuid)
						{
							ban.offline = true;

							ban.player = new User
							{
								name = "Offline Player",
								userid = uid
							};

							if (SCPDiscord.instance.Config.SteamApiKey != string.Empty)
							{
								string data = null;
								try
								{
									data = webclient.DownloadString($"https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v2/?key={SCPDiscord.instance.Config.SteamApiKey}&format=json&steamids={uid.Replace("@steam", "")}");
								}
								catch
								{
									Log.Debug("Failed to get profile data from SteamAPI.");
								}
								JObject o2 = JObject.Parse(data);

								if (o2 != null)
								{
									ban.player.name = (string)o2["response"]["players"][0]["personaname"];
								}
							}

							BanHandler.IssueBan(new BanDetails()
							{
								OriginalName = ban.player.name,
								Id = uid,
								IssuanceTime = TimeBehaviour.CurrentTimestamp(),
								Expires = DateTime.UtcNow.AddMinutes((double)min).Ticks,
								Reason = reason,
								Issuer = "Server"
							}, BanHandler.BanType.UserId);
						}
						else if (uid.Contains("."))
						{
							ban.offline = true;

							BanHandler.IssueBan(new BanDetails()
							{
								OriginalName = "IP Address",
								Id = uid,
								IssuanceTime = TimeBehaviour.CurrentTimestamp(),
								Expires = DateTime.UtcNow.AddMinutes((double)min).Ticks,
								Reason = reason,
								Issuer = "Server"
							}, BanHandler.BanType.IP);
						}
						else
						{
							ban.success = false;
						}
					}
					EventHandlers.tcp.WriteStream(ban);
				}
				else if (type == "KICK")
				{
					string uid = (string)o["user"];
					if (!uid.Contains("@steam") && !uid.Contains("@discord"))
					{
						uid += "@steam";
					}
					Player player = Player.Get(uid);

					Kick kick = new Kick
					{
						player = null
					};

					if (player != null)
					{
						kick.player = new User
						{
							name = player.Nickname,
							userid = player.UserId
						};

						ServerConsole.Disconnect(player.GameObject, (string)o["reason"]);
					}
					EventHandlers.tcp.WriteStream(kick);
				}
				else if (type == "UNBAN")
				{
					Unban unban = new Unban();

					List<string> ipBans = File.ReadAllLines(SCPDiscord.ipBans).ToList();
					List<string> userIDBans = File.ReadAllLines(SCPDiscord.useridBans).ToList();

					string id = (string)o["user"];
					if (!id.Contains("."))
					{
						if (!id.Contains("@steam") && !id.Contains("@discord"))
						{
							id += "@steam";
						}
					}
					List<string> matchingIPBans = ipBans.FindAll(s => s.Contains(id));
					List<string> matchingSteamIDBans = userIDBans.FindAll(s => s.Contains(id));

					if (matchingIPBans.Count == 0 && matchingSteamIDBans.Count == 0)
					{
						unban.success = false;
						EventHandlers.tcp.WriteStream(unban);
						return;
					}

					ipBans.RemoveAll(s => s.Contains(id));
					userIDBans.RemoveAll(s => s.Contains(id));

					foreach (var row in matchingIPBans) userIDBans.RemoveAll(s => s.Contains(row.Split(';').Last()));
					foreach (var row in matchingSteamIDBans) ipBans.RemoveAll(s => s.Contains(row.Split(';').Last()));

					File.WriteAllLines(SCPDiscord.ipBans, ipBans);
					File.WriteAllLines(SCPDiscord.useridBans, userIDBans);

					EventHandlers.tcp.WriteStream(unban);
				}
			}
			catch (Exception x)
			{
				Log.Error("SCPDiscord handle command error: " + x.Message + "\n" + x.StackTrace);
			}
		}
	}
}