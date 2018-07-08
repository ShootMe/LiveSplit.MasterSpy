using System;
using System.Diagnostics;
namespace LiveSplit.MasterSpy {
	public partial class SplitterMemory {
		public Process Program { get; set; }
		public bool IsHooked { get; set; } = false;
		public DateTime LastHooked;
		private bool setupOffsets = false;
		private int levelTime, gameTime, missionTime;
		public SplitterMemory() {
			LastHooked = DateTime.MinValue;
		}

		public int GameStart() {
			return Program.Read<int>(0x2446A1C);
		}
		public string Level() {
			string level = Program.ReadString(11, 0x24581b0, 0x30, 0x60, 0x44, 0x28, 0x0);
			if (!string.IsNullOrEmpty(level) && !setupOffsets) {
				int found = 0;
				for (int i = 1; i < 80; i++) {
					string column = Program.ReadString(25, 0x24581b0, 0x30, 0x60, 0x44, 0xc + (0x50 * i), 0x0);
					if (string.IsNullOrEmpty(column)) { break; }

					switch (column) {
						case "current_game_time": gameTime = 0x50 * i + 0x28; found++; break;
						case "current_level_time": levelTime = 0x50 * i + 0x28; found++; break;
						case "current_mission_time": missionTime = 0x50 * i + 0x28; found++; break;
					}
				}
				if (found < 3) {
					for (int i = -1; i > -50; i--) {
						string column = Program.ReadString(25, 0x24581b0, 0x30, 0x60, 0x44, 0xc + (0x50 * i), 0x0);
						if (string.IsNullOrEmpty(column)) { break; }

						switch (column) {
							case "current_game_time": gameTime = 0x50 * i + 0x28; found++; break;
							case "current_level_time": levelTime = 0x50 * i + 0x28; found++; break;
							case "current_mission_time": missionTime = 0x50 * i + 0x28; found++; break;
						}
					}
				}
				setupOffsets = found == 3;
				if (!setupOffsets) {
					setupOffsets = true;
					System.Windows.Forms.MessageBox.Show("Failed to find all pointers, please restart game to try again.");
				}
			}
			return level;
		}
		public string LevelTime() {
			string time = Program.ReadString(7, 0x24581b0, 0x30, 0x60, 0x44, levelTime);
			if (time.Length > 2) {
				return time.Substring(0, time.Length - 2) + "." + time.Substring(time.Length - 2);
			}
			return "0." + time.PadLeft(2, '0');
		}
		public string GameTime() {
			string time = Program.ReadString(7, 0x24581b0, 0x30, 0x60, 0x44, gameTime);
			if (time.Length > 2) {
				return time.Substring(0, time.Length - 2) + "." + time.Substring(time.Length - 2);
			}
			return "0." + time.PadLeft(2, '0');
		}
		public string MissionTime() {
			string time = Program.ReadString(7, 0x24581b0, 0x30, 0x60, 0x44, missionTime);
			if (time.Length > 2) {
				return time.Substring(0, time.Length - 2) + "." + time.Substring(time.Length - 2);
			}
			return "0." + time.PadLeft(2, '0');
		}
		public bool HookProcess() {
			IsHooked = Program != null && !Program.HasExited;
			if (!IsHooked && DateTime.Now > LastHooked.AddSeconds(1)) {
				LastHooked = DateTime.Now;
				Process[] processes = Process.GetProcessesByName("MasterSpy");
				Program = null;
				for (int i = processes != null ? processes.Length - 1 : -1; i >= 0; i--) {
					if (processes[i].MainWindowHandle != IntPtr.Zero) {
						Program = processes[i];
						break;
					}
				}

				if (Program != null && !Program.HasExited) {
					setupOffsets = false;
					IsHooked = true;
				}
			}

			return IsHooked;
		}
		public void Dispose() {
			if (Program != null) {
				Program.Dispose();
			}
		}
	}
}