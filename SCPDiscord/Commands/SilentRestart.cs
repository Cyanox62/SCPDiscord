using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using SCPDiscord;
using Exiled.Permissions.Extensions;

namespace Watchlist.Commands
{
	[CommandHandler(typeof(RemoteAdminCommandHandler))]
	class ReportCommand : ICommand
	{
		public string[] Aliases { get; set; } = Array.Empty<string>();

		public string Description { get; set; } = "Silently restart the server";

		string ICommand.Command { get; } = "silentrestart";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (sender is PlayerCommandSender player)
			{
				if (player.CheckPermission("scpd.sr"))
				{
					EventHandlers.silentRestart = !EventHandlers.silentRestart;
					response = EventHandlers.silentRestart ? "Server set to silently restart next round." : "Server silent restart cancelled.";
					return true;
				}
				response = "";
				return true;
			}
			else
			{
				response = "Only players may use this command";
				return false;
			}
		}
	}
}
