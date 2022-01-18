using SCPDiscord.DataObjects;

namespace SCPDiscord
{
	partial class EventHandlers
	{
		internal static User PlyToUser(Exiled.API.Features.Player player)
		{
			return new User
			{
				name = player.Nickname,
				userid = player.UserId
			};
		}
	}
}
