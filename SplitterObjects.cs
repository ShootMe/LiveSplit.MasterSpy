using System.ComponentModel;
namespace LiveSplit.MasterSpy {
	public enum LogObject {
		CurrentSplit,
		GameStart,
		Level,
		LevelTime,
		GameTime,
		MissionTime
	}
	public enum SplitName {
		[Description("Manual Split (Not Automatic)"), ToolTip("Does not split automatically. Use this for custom splits not yet defined.")]
		ManualSplit,

		[Description("01.01 - Rear Entry"), ToolTip("Splits when finishing 01.01")]
		Level_01_01,
		[Description("01.02 - Park Platz"), ToolTip("Splits when finishing 01.02")]
		Level_01_02,
		[Description("01.03 - Mailroom"), ToolTip("Splits when finishing 01.03")]
		Level_01_03,
		[Description("01.04 - Status Lobby"), ToolTip("Splits when finishing 01.04")]
		Level_01_04,
		[Description("01.05 - Vaulted Office"), ToolTip("Splits when finishing 01.05")]
		Level_01_05,
		[Description("01.06 - Interface"), ToolTip("Splits when finishing 01.06")]
		Level_01_06,
		[Description("01.07 - Zero Tech"), ToolTip("Splits when finishing 01.07")]
		Level_01_07,
		[Description("01.08 - The Torus"), ToolTip("Splits when finishing 01.08")]
		Level_01_08,
		[Description("01.09 - Dude Ranch"), ToolTip("Splits when finishing 01.09")]
		Level_01_09,
		[Description("01.10 - Executive Lounge"), ToolTip("Splits when finishing 01.10")]
		Level_01_10,

		[Description("02.01 - Meadow"), ToolTip("Splits when finishing 02.01")]
		Level_02_01,
		[Description("02.02 - Bridge"), ToolTip("Splits when finishing 02.02")]
		Level_02_02,
		[Description("02.03 - Foyer"), ToolTip("Splits when finishing 02.03")]
		Level_02_03,
		[Description("02.04 - Halls"), ToolTip("Splits when finishing 02.04")]
		Level_02_04,
		[Description("02.05 - Bathroom"), ToolTip("Splits when finishing 02.05")]
		Level_02_05,
		[Description("02.06 - Data Caves"), ToolTip("Splits when finishing 02.06")]
		Level_02_06,
		[Description("02.07 - The Arch"), ToolTip("Splits when finishing 02.07")]
		Level_02_07,
		[Description("02.08 - Auditorium"), ToolTip("Splits when finishing 02.08")]
		Level_02_08,
		[Description("02.09 - Air Exchanger"), ToolTip("Splits when finishing 02.09")]
		Level_02_09,
		[Description("02.10 - Falling Water"), ToolTip("Splits when finishing 02.10")]
		Level_02_10,

		[Description("03.01 - Scaffolding"), ToolTip("Splits when finishing 03.01")]
		Level_03_01,
		[Description("03.02 - A/C"), ToolTip("Splits when finishing 03.02")]
		Level_03_02,
		[Description("03.03 - Shark Fin Soup"), ToolTip("Splits when finishing 03.03")]
		Level_03_03,
		[Description("03.04 - Reaction Training"), ToolTip("Splits when finishing 03.04")]
		Level_03_04,
		[Description("03.05 - Priceless Artifacts"), ToolTip("Splits when finishing 03.05")]
		Level_03_05,
		[Description("03.06 - Original Foundation"), ToolTip("Splits when finishing 03.06")]
		Level_03_06,
		[Description("03.07 - Water Serpent"), ToolTip("Splits when finishing 03.07")]
		Level_03_07,
		[Description("03.08 - The Red Chamber"), ToolTip("Splits when finishing 03.08")]
		Level_03_08,
		[Description("03.09 - The Wooden Man"), ToolTip("Splits when finishing 03.09")]
		Level_03_09,
		[Description("03.10 - Den Of Death"), ToolTip("Splits when finishing 03.10")]
		Level_03_10,

		[Description("04.01 - Reception"), ToolTip("Splits when finishing 04.01")]
		Level_04_01,
		[Description("04.02 - Duct"), ToolTip("Splits when finishing 04.02")]
		Level_04_02,
		[Description("04.03 - Reclamation Sector"), ToolTip("Splits when finishing 04.03")]
		Level_04_03,
		[Description("04.04 - Raw Materials"), ToolTip("Splits when finishing 04.04")]
		Level_04_04,
		[Description("04.05 - Zirconium Coating"), ToolTip("Splits when finishing 04.05")]
		Level_04_05,
		[Description("04.06 - Transformer"), ToolTip("Splits when finishing 04.06")]
		Level_04_06,
		[Description("04.07 - Full Automation"), ToolTip("Splits when finishing 04.07")]
		Level_04_07,
		[Description("04.08 - Haywire Facility"), ToolTip("Splits when finishing 04.08")]
		Level_04_08,
		[Description("04.09 - Zirconium Reactor"), ToolTip("Splits when finishing 04.09")]
		Level_04_09,
		[Description("04.10 - Zirconium Exhaust"), ToolTip("Splits when finishing 04.10")]
		Level_04_10,

		[Description("05.01 - Infiltration"), ToolTip("Splits when finishing 05.01")]
		Level_05_01,
		[Description("05.02 - Crash The Party"), ToolTip("Splits when finishing 05.02")]
		Level_05_02,
		[Description("05.03 - Mediterranean Sonata"), ToolTip("Splits when finishing 05.03")]
		Level_05_03,
		[Description("05.04 - Feel The Burn"), ToolTip("Splits when finishing 05.04")]
		Level_05_04,
		[Description("05.05 - The Aquarium"), ToolTip("Splits when finishing 05.05")]
		Level_05_05,
		[Description("05.06 - Upper Deck"), ToolTip("Splits when finishing 05.06")]
		Level_05_06,
		[Description("05.07 - Private Chambers"), ToolTip("Splits when finishing 05.07")]
		Level_05_07,
		[Description("05.08 - The Engine Room"), ToolTip("Splits when finishing 05.08")]
		Level_05_08,
		[Description("05.09 - Nightvision Equipped"), ToolTip("Splits when finishing 05.09")]
		Level_05_09,
		[Description("05.10 - The Empty Ballroom"), ToolTip("Splits when finishing 05.10")]
		Level_05_10
	}
}