using Exiled.API.Features;
using HarmonyLib;
using RemoteAdmin;
using SCPDiscord.DataObjects;
using SCPDiscord.DataObjects.Events;

namespace SCPDiscord.Patches
{
	[HarmonyPatch(typeof(CommandProcessor), nameof(CommandProcessor.ProcessQuery))]
	class RemoteAdminPatch
	{
		public static void Prefix(string q, CommandSender sender)
		{
			if (q != "REQUEST_DATA")
			{
				Log.Warn(Exiled.API.Features.Player.Get(sender).Nickname + " called: " + q);
				EventHandlers.tcp.SendData(new Command
				{
					eventName = "RACommand",
					sender = sender != null ? EventHandlers.PlyToUser(Exiled.API.Features.Player.Get(sender)) : new User()
					{
						name = "Server",
						userid = ""
					},
					command = q
				});
			}
		}
	}
}
