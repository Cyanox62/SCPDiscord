using Exiled.API.Features;
using GameCore;

namespace SCPDiscord.DataObjects
{
	public class Identify
	{
		public string type = "IDENT";
		public int data = Server.Port;
		public int maxUsers = Server.MaxPlayerCount;
	}
}
