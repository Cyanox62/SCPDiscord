using Exiled.API.Enums;
using System.Collections.Generic;

namespace SCPDiscord
{
	public static class Conversions
	{
		public static Dictionary<ItemType, string> items = new Dictionary<ItemType, string>()
		{
			{ ItemType.Ammo556x45, "556 Ammo" },
			{ ItemType.Ammo762x39, "762 Ammo" },
			{ ItemType.Ammo9x19, "9mm Ammo" },
			{ ItemType.GrenadeFlash, "Flashbang" },
			{ ItemType.GrenadeHE, "Grenade" },
			{ ItemType.GunAK, "AK-47" },
			{ ItemType.GunCOM15, "COM-15" },
			{ ItemType.GunCOM18, "COM-18" },
			{ ItemType.GunE11SR, "Epsilon 11 Rifle" },
			{ ItemType.GunLogicer, "Logicer" },
			{ ItemType.GunRevolver, "Revolver" },
			{ ItemType.GunShotgun, "Shotgun" },
			{ ItemType.KeycardChaosInsurgency, "Chaos Insurgency Keycard" },
			{ ItemType.KeycardContainmentEngineer, "Containment Engineer Keycard" },
			{ ItemType.KeycardFacilityManager, "Facility Manager Keycard" },
			{ ItemType.KeycardGuard, "Guard Keycard" },
			{ ItemType.KeycardJanitor, "Janitor Keycard" },
			{ ItemType.KeycardNTFCommander, "Commander Keycard" },
			{ ItemType.KeycardNTFLieutenant, "Lieutenant Keycard" },
			{ ItemType.KeycardNTFOfficer, "Officer Keycard" },
			{ ItemType.KeycardO5, "O5 Keycard" },
			{ ItemType.KeycardResearchCoordinator, "Research Coordinator Keycard" },
			{ ItemType.KeycardScientist, "Scientist Keycard" },
			{ ItemType.KeycardZoneManager, "Zone Manager Keycard" },
			{ ItemType.SCP018, "SCP-018" },
			{ ItemType.SCP207, "SCP-207" },
			{ ItemType.SCP268, "SCP-268" },
			{ ItemType.SCP500, "SCP-500" },
			{ ItemType.SCP2176, "SCP-2176" },
			{ ItemType.SCP244a, "SCP-244-A" },
			{ ItemType.SCP244b, "SCP-244-B" },
			{ ItemType.SCP330, "SCP-330" }
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

		public static Dictionary<ItemType, string> grenades = new Dictionary<ItemType, string>()
		{
			{ ItemType.GrenadeHE, "Granade" },
			{ ItemType.GrenadeFlash, "Flashbang" },
			{ ItemType.SCP018, "SCP-018" }
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
