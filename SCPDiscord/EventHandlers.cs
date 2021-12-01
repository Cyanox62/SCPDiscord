using Exiled.API.Features;
using MEC;
using Exiled.Events.EventArgs;
using SCPDiscord.DataObjects;
using SCPDiscord.DataObjects.Events;
using System.Linq;
using System.Collections.Generic;
using Exiled.Permissions.Extensions;
using UnityEngine;

namespace SCPDiscord
{
	partial class EventHandlers
	{
		public static Tcp tcp;

		internal static bool silentRestart;

		private Dictionary<Exiled.API.Features.Player, RoleType> roles = new Dictionary<Exiled.API.Features.Player, RoleType>();

		public EventHandlers()
		{
			//Configs.ReloadConfigs();

			tcp = new Tcp(SCPDiscord.instance.Config.Address, SCPDiscord.instance.Config.Port);
			tcp.Init();
		}

		public void OnWaitingForPlayers()
		{
			//Configs.ReloadConfigs();

			tcp.SendData(new Generic
			{
				eventName = "WaitingForPlayers"
			});
		}

		public void OnRoundStart()
		{
			roles.Clear();

			tcp.SendData(new Generic
			{
				eventName = "RoundStart",
				param = Exiled.API.Features.Player.List.Where(x => x.UserId != null).Count().ToString()
			});
		}

		public void OnRoundEnd(RoundEndedEventArgs ev)
		{
			tcp.SendData(new Generic
			{
				eventName = "RoundEnd",
				param = ((int)(Round.ElapsedTime.TotalSeconds / 60)).ToString()
			});
		}

		public void OnPlayerVerified(VerifiedEventArgs ev)
		{
			Timing.CallDelayed(1f, () => tcp.SendData(new RoleSync
			{
				userid = ev.Player.UserId
			}));

			tcp.SendData(new PlayerParam
			{
				eventName = "PlayerJoin",
				player = PlyToUser(ev.Player),
				param = ev.Player.DoNotTrack
			});
		}

		public void OnSetClass(ChangingRoleEventArgs ev)
		{
			if (!roles.ContainsKey(ev.Player)) roles.Add(ev.Player, RoleType.None);
			if (roles[ev.Player] == ev.NewRole || ev.NewRole == RoleType.Spectator) return;
			roles[ev.Player] = ev.NewRole;

			if (ev.Player.UserId != null)
			{
				tcp.SendData(new PlayerParam
				{
					eventName = "SetClass",
					player = PlyToUser(ev.Player),
					param = Conversions.roles.ContainsKey(ev.NewRole) ? Conversions.roles[ev.NewRole] : ev.NewRole.ToString()
				});
			}
		}

		public void OnDropItem(DroppingItemEventArgs ev)
		{
			tcp.SendData(new PlayerParam
			{
				eventName = "DropItem",
				player = PlyToUser(ev.Player),
				param = Conversions.items.ContainsKey(ev.Item.Type) ? Conversions.items[ev.Item.Type] : ev.Item.Type.ToString()
			});
		}

		public void OnPickupItem(PickingUpItemEventArgs ev)
		{
			tcp.SendData(new PlayerParam
			{
				eventName = "PickupItem",
				player = PlyToUser(ev.Player),
				param = Conversions.items.ContainsKey(ev.Pickup.Type) ? Conversions.items[ev.Pickup.Type] : ev.Pickup.Type.ToString()
			});
		}

		public void OnPlayerLeave(LeftEventArgs ev)
		{
			if (ev.Player.UserId != null)
			{
				tcp.SendData(new DataObjects.Events.Player
				{
					eventName = "PlayerLeave",
					player = PlyToUser(ev.Player)
				});
			}
		}

		//Untested: Recontainment, RagdollLess, FriendlyFireDetector, Flying, Contain
		//Unfixable: Hemorrhage
		public enum DamageTypes
		{
			None,
			Unknown,
			AK,
			Asphyxiation,
			Bleeding,
			Com15,
			Com18,
			Contain,
			CrossVec,
			Decont,
			E11SR,
			Falldown,
			Flying,
			FriendlyFireDetector,
			FSP9,
			Grenade,
			Hemorrhage,
			Logicer,
			Lure,
			MicroHID,
			Nuke,
			Pocket,
			Poison,
			RagdollLess,
			Recontainment,
			Revolver,
			Scp018,
			Scp049,
			Scp0492,
			Scp096,
			Scp096Charge,
			Scp096Pry,
			Scp106,
			Scp173,
			Scp207,
			Scp939,
			Shotgun,
			Tesla,
			Wall
		}

		//Returns the most likely applicable damage type for the damage handler given
		public DamageTypes ParseHandler(PlayerStatsSystem.DamageHandlerBase d)
		{
			if (d.ServerLogsText == null) return DamageTypes.None;
			if (d.ServerLogsText.Contains("Micro H.I.D.")) return DamageTypes.MicroHID;
			if (d.ServerLogsText.Contains("Fall damage")) return DamageTypes.Falldown;
			if (d.ServerLogsText.Contains("Crushed.")) return DamageTypes.Wall;
			if (d.ServerLogsText.Contains("SCP-207")) return DamageTypes.Scp207;
			if (d.ServerLogsText.Contains("SCP-096's charge")) return DamageTypes.Scp096Charge;
			if (d.ServerLogsText.Contains("Melted by a highly corrosive substance")) return DamageTypes.Decont;
			if (d.ServerLogsText.Contains("Tried to pass through a gate being breached by SCP-096")) return DamageTypes.Scp096Pry;
			if (d.ServerLogsText.Contains("Got slapped by SCP-096")) return DamageTypes.Scp096;
			if (d.ServerLogsText.Contains("SCP-018")) return DamageTypes.Scp018;
			if (d.ServerLogsText.Contains("Scp0492")) return DamageTypes.Scp0492;
			if (d.ServerLogsText.Contains("bait for SCP-106")) return DamageTypes.Lure;
			if (d.ServerLogsText.Contains("Died to alpha warhead")) return DamageTypes.Nuke;
			if (d.ServerLogsText.Contains("Friendly Fire")) return DamageTypes.FriendlyFireDetector;
			if (d.ServerLogsText.Contains("Asphyxiated")) return DamageTypes.Asphyxiation;
			if (d.ServerLogsText.Contains("GunCrossvec")) return DamageTypes.CrossVec;
			if (d.ServerLogsText.Contains("GunCOM18")) return DamageTypes.Com18;
			if (d.ServerLogsText.Contains("GunCOM15")) return DamageTypes.Com15;
			if (d.ServerLogsText.Contains("GunShotgun")) return DamageTypes.Shotgun;
			if (d.ServerLogsText.Contains("Explosion.")) return DamageTypes.Grenade;
			foreach (DamageTypes dmgtyp in DamageTypes.GetValues(typeof(DamageTypes)))
			{
				if (d.ServerLogsText.Contains(dmgtyp.ToString())) return dmgtyp;
			}
			return DamageTypes.Unknown;
		}

		public void OnPlayerHurt(HurtingEventArgs ev)
		{
			DamageTypes damageType = ParseHandler(ev.DamageHandler);
			tcp.SendData(new PlayerDamage
			{
				eventName = "PlayerHurt",
				victim = PlyToUser(ev.Target),
				attacker = ev.Attacker == null ? null : PlyToUser(ev.Attacker),
				damage = (int)ev.Amount,
				weapon = damageType.ToString()
			});
		}

		public void OnPlayerDeath(DiedEventArgs ev)
		{
			DamageTypes damageType = ParseHandler(ev.DamageHandler);
			if (ev.Target.IsConnected)
			{
				PlayerDamage data = new PlayerDamage
				{
					eventName = "PlayerDeath",
					victim = PlyToUser(ev.Target),
					attacker = ev.Killer == null ? null : PlyToUser(ev.Killer),
					damage = (int)(-1),
					weapon = damageType.ToString()
				};

				DamageTypes type = damageType;
				if (type == DamageTypes.Tesla) data.eventName += "Tesla";
				else if (type == DamageTypes.Decont) data.eventName += "Decont";
				else if (type == DamageTypes.Falldown) data.eventName += "Fall";
				else if (type == DamageTypes.Flying) data.eventName += "Flying";
				else if (type == DamageTypes.Lure) data.eventName += "Lure";
				else if (type == DamageTypes.Nuke) data.eventName += "Nuke";
				else if (type == DamageTypes.Pocket) data.eventName += "Pocket";
				else if (type == DamageTypes.Recontainment) data.eventName += "Recont";


				Log.Debug(data.eventName + " - " + data.weapon);
				tcp.SendData(data);
			}
		}

		public void OnDecontamination(DecontaminatingEventArgs ev)
		{
			tcp.SendData(new Generic
			{
				eventName = "Decontamination"
			});
		}

		public void OnGrenadeThrown(ThrowingItemEventArgs ev)
		{
			if (ev.Item.Type == ItemType.GrenadeFlash || ev.Item.Type == ItemType.GrenadeHE 
				|| ev.Item.Type == ItemType.SCP018)
			{
				tcp.SendData(new PlayerParam
				{
					eventName = "GrenadeThrown",
					player = PlyToUser(ev.Player),
					param = Conversions.grenades[ev.Item.Type]
				});
			}
		}

		//public void OnRACommand(SendingRemoteAdminCommandEventArgs ev)
		//{
		//	string cmd = ev.Name;
		//	foreach (string arg in ev.Arguments) cmd += $" {arg}";
		//	tcp.SendData(new Command
		//	{
		//		eventName = "RACommand",
		//		sender = ev.Sender != null ? PlyToUser(ev.Sender) : new User
		//		{
		//			name = "Server",
		//			userid = ""
		//		},
		//		command = cmd
		//	});

		//	cmd = cmd.ToLower();
		//	if ((cmd == "silentrestart" || cmd == "sr") && ev.Sender.CheckPermission("scpd.sr"))
		//	{
		//		ev.IsAllowed = false;
		//		silentRestart = !silentRestart;
		//		ev.Sender.RemoteAdminMessage(silentRestart ? "Server set to silently restart next round." : "Server silent restart cancelled.", true);
		//	}
		//}

		//public void OnConsoleCommand(SendingConsoleCommandEventArgs ev)
		//{
		//	string cmd = ev.Name;
		//	foreach (string arg in ev.Arguments) cmd += $" {arg}";
		//	tcp.SendData(new Command
		//	{
		//		eventName = "ConsoleCommand",
		//		sender = PlyToUser(ev.Player),
		//		command = cmd
		//	});
		//}

		public void OnPreAuth(PreAuthenticatingEventArgs ev)
		{
			tcp.SendData(new UserId
			{
				eventName = "PreAuth",
				userid = ev.UserId,
				ip = ev.Request.RemoteEndPoint.ToString()
			});
		}

		public void OnRoundRestart()
		{
			tcp.SendData(new Generic
			{
				eventName = "RoundRestart"
			});

			if (silentRestart)
			{
				Timing.CallDelayed(2.5f, () =>
				{
					Server.Restart();
				});
				silentRestart = false;
			}
		}

		public void OnScp079TriggerTesla(InteractingTeslaEventArgs ev)
		{
			tcp.SendData(new DataObjects.Events.Player
			{
				eventName = "Scp079TriggerTesla",
				player = PlyToUser(ev.Player)
			});
		}

		public void OnScp914ChangeKnob(ChangingKnobSettingEventArgs ev)
		{
			tcp.SendData(new PlayerParam
			{
				eventName = "Scp914ChangeKnob",
				player = PlyToUser(ev.Player),
				param = Conversions.knobsettings[ev.KnobSetting]
			});
		}

		public void OnTeamRespawn(RespawningTeamEventArgs ev)
		{
			tcp.SendData(new TeamRespawn
			{
				eventName = "TeamRespawn",
				players = Exiled.API.Features.Player.List.Select(x =>
				{
					return PlyToUser(x);
				}).ToArray(),
				team = ev.NextKnownTeam == Respawning.SpawnableTeamType.ChaosInsurgency ? 0 : 1
			});
		}

		public void OnScp106Contain(ContainingEventArgs ev)
		{
			// 'player' is the player who hit the button, not 106
			tcp.SendData(new DataObjects.Events.Player
			{
				eventName = "Scp106Contain",
				player = PlyToUser(ev.Player)
			});
		}

		public void OnScp914Activation(ActivatingEventArgs ev)
		{
			tcp.SendData(new DataObjects.Events.Player
			{
				eventName = "Scp914Activation",
				player = PlyToUser(ev.Player)
			});
		}

		public void OnSetGroup(ChangingGroupEventArgs ev)
		{
			if (ev.Player != null)
			{
							tcp.SendData(new PlayerParam
			{
				eventName = "SetGroup",
				player = PlyToUser(ev.Player),
				param = ev.NewGroup.BadgeText
			});
			}
		}

		public void OnPocketDimensionEnter(EnteringPocketDimensionEventArgs ev)
		{
			tcp.SendData(new DataObjects.Events.Player
			{
				eventName = "PocketDimensionEnter",
				player = PlyToUser(ev.Player)
			});
		}

		public void OnPocketDimensionEscape(EscapingPocketDimensionEventArgs ev)
		{
			tcp.SendData(new DataObjects.Events.Player
			{
				eventName = "PocketDimensionEscape",
				player = PlyToUser(ev.Player)
			});
		}
	}
}