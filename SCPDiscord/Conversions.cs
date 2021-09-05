using Exiled.API.Enums;
using System.Collections.Generic;

namespace SCPDiscord
{
	public static class Conversions
	{
		public static Dictionary<Exiled.API.Enums.ItemType, string> items = new Dictionary<Exiled.API.Enums.ItemType, string>()
		{
			{ Exiled.API.Enums.ItemType.Ammo556X45, "556 Ammo" },
			{ Exiled.API.Enums.ItemType.Ammo762X39, "762 Ammo" },
			{ Exiled.API.Enums.ItemType.Ammo9X19, "9mm Ammo" },
			{ Exiled.API.Enums.ItemType.GrenadeFlash, "Flashbang" },
			{ Exiled.API.Enums.ItemType.GrenadeHe, "Grenade" },
			{ Exiled.API.Enums.ItemType.GunAk, "AK-47" },
			{ Exiled.API.Enums.ItemType.GunCom15, "COM-15" },
			{ Exiled.API.Enums.ItemType.GunE11Sr, "Epsilon 11 Rifle" },
			{ Exiled.API.Enums.ItemType.GunLogicer, "Logicer" },
			{ Exiled.API.Enums.ItemType.GunRevolver, "Revolver" },
			{ Exiled.API.Enums.ItemType.GunShotgun, "Shotgun" },
			{ Exiled.API.Enums.ItemType.KeycardChaosInsurgency, "Chaos Insurgency Keycard" },
			{ Exiled.API.Enums.ItemType.KeycardContainmentEngineer, "Containment Engineer Keycard" },
			{ Exiled.API.Enums.ItemType.KeycardFacilityManager, "Facility Manager Keycard" },
			{ Exiled.API.Enums.ItemType.KeycardGuard, "Guard Keycard" },
			{ Exiled.API.Enums.ItemType.KeycardJanitor, "Janitor Keycard" },
			{ Exiled.API.Enums.ItemType.KeycardNtfCommander, "Commander Keycard" },
			{ Exiled.API.Enums.ItemType.KeycardNtfLieutenant, "Lieutenant Keycard" },
			{ Exiled.API.Enums.ItemType.KeycardNtfOfficer, "Officer Keycard" },
			{ Exiled.API.Enums.ItemType.KeycardO5, "O5 Keycard" },
			{ Exiled.API.Enums.ItemType.KeycardResearchCoordinator, "Research Coordinator Keycard" },
			{ Exiled.API.Enums.ItemType.KeycardScientist, "Scientist Keycard" },
			{ Exiled.API.Enums.ItemType.KeycardZoneManager, "Zone Manager Keycard" },
			{ Exiled.API.Enums.ItemType.Scp018, "SCP-018" },
			{ Exiled.API.Enums.ItemType.Scp207, "SCP-207" },
			{ Exiled.API.Enums.ItemType.Scp268, "SCP-268" },
			{ Exiled.API.Enums.ItemType.Scp500, "SCP-500" }
		};

		public static Dictionary<RoleType, string> roles = new Dictionary<RoleType, string>()
		{
			{ RoleType.ChaosConscript, "Chaos Conscript" },
			{ RoleType.ChaosMarauder, "Chaos Marauder" },
			{ RoleType.ChaosRepressor, "Chaos Repressor" },
			{ RoleType.ChaosRifleman, "Chaos Rifleman" },
			{ RoleType.ClassD, "Class-D" },
			{ RoleType.FacilityGuard, "Facility Guard" },
			{ RoleType.NtfCaptain, "NTF Captain" },
			{ RoleType.NtfPrivate, "NTF Private" },
			{ RoleType.NtfSergeant, "NTF Sergeant" },
			{ RoleType.NtfSpecialist, "NTF Specialist" },
			{ RoleType.Scp049, "SCP-049" },
			{ RoleType.Scp0492, "SCP-049-2" },
			{ RoleType.Scp079, "SCP-079" },
			{ RoleType.Scp096, "SCP-096" },
			{ RoleType.Scp106, "SCP-106" },
			{ RoleType.Scp173, "SCP-173" },
			{ RoleType.Scp93953, "SCP-939" },
			{ RoleType.Scp93989, "SCP-939" }
		};

		public static Dictionary<Exiled.API.Enums.ItemType, string> grenades = new Dictionary<Exiled.API.Enums.ItemType, string>()
		{
			{ Exiled.API.Enums.ItemType.GrenadeHe, "Granade" },
			{ Exiled.API.Enums.ItemType.GrenadeFlash, "Flashbang" },
			{ Exiled.API.Enums.ItemType.Scp018, "SCP-018" }
		};

		public static Dictionary<Scp914.Scp914KnobSetting, string> knobsettings = new Dictionary<Scp914.Scp914KnobSetting, string>()
		{
			{ Scp914.Scp914KnobSetting.Rough, "Rough" },
			{ Scp914.Scp914KnobSetting.Coarse, "Course" },
			{ Scp914.Scp914KnobSetting.OneToOne, "1:1" },
			{ Scp914.Scp914KnobSetting.Fine, "Fine" },
			{ Scp914.Scp914KnobSetting.VeryFine, "Very Fine" }
		};
	}
}
