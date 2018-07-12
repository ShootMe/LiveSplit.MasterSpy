using System;
using System.Collections.Generic;
using System.Diagnostics;
namespace LiveSplit.MasterSpy {
	public partial class SplitterMemory {
		public Process ProgramInfo { get; set; }
		public Process ProgramWindow { get; set; }
		public bool IsHooked { get; set; } = false;
		public DateTime LastHooked;
		private ProgramPointer levelName = new ProgramPointer(AutoDeref.None, new ProgramSignature(PointerVersion.Steam, "7970656C6974636865640B666C6F61746D", -12));
		private bool setupOffsets = false;
		private IntPtr levelTime, gameTime, missionTime, levelString;
		public SplitterMemory() {
			LastHooked = DateTime.MinValue;
		}

		public int GameStart() {
			return ProgramWindow.Read<int>(0x2446A1C);
		}
		private void AddInfo(HashSet<uint> pointers, Dictionary<string, string> stats, IntPtr start) {
			IntPtr newPtr = (IntPtr)ProgramInfo.Read<uint>(start, 0x0);
			if (pointers.Add((uint)newPtr)) {
				AddInfo(pointers, stats, newPtr);
			}
			newPtr = (IntPtr)ProgramInfo.Read<uint>(start, 0x4);
			if (pointers.Add((uint)newPtr)) {
				AddInfo(pointers, stats, newPtr);
			}
			newPtr = (IntPtr)ProgramInfo.Read<uint>(start, 0x8);
			if (pointers.Add((uint)newPtr)) {
				AddInfo(pointers, stats, newPtr);
			}

			int size = ProgramInfo.Read<int>(start, 0x1c);
			if (size > 0 && size < 256) {
				string column = ProgramInfo.ReadString(start, size, 0xc, 0x0);
				size = ProgramInfo.Read<int>(start, 0x38);
				string value = null;
				if (size <= 8) {
					value = ProgramInfo.ReadString(start, size, 0x28);
				} else {
					value = ProgramInfo.ReadString(start, size, 0x28, 0x0);
				}
				if (!stats.ContainsKey(column)) {
					switch (column) {
						case "current_game_time": gameTime = start; break;
						case "current_level_time": levelTime = start; break;
						case "current_mission_time": missionTime = start; break;
						case "currentLevel": levelString = start; break;
					}

					stats.Add(column, value);
				}
			}
		}
		private string GetColumn(IntPtr start) {
			if (start == IntPtr.Zero) { return string.Empty; }
			int size = ProgramInfo.Read<int>(start, 0x38);
			string value = null;
			if (size <= 8) {
				value = ProgramInfo.ReadString(start, size, 0x28);
			} else {
				value = ProgramInfo.ReadString(start, size, 0x28, 0x0);
			}
			return value;
		}
		public string Level() {
			IntPtr orig = (IntPtr)levelName.Read<uint>(ProgramInfo);
			if (orig != IntPtr.Zero && !setupOffsets) {
				Dictionary<string, string> stats = new Dictionary<string, string>();
				HashSet<uint> pointers = new HashSet<uint>();
				AddInfo(pointers, stats, orig);
			}

			return GetColumn(levelString);
		}
		public string LevelTime() {
			string time = GetColumn(levelTime);
			if (time.Length > 2) {
				return time.Substring(0, time.Length - 2) + "." + time.Substring(time.Length - 2);
			}
			return "0." + time.PadLeft(2, '0');
		}
		public string GameTime() {
			string time = GetColumn(gameTime);
			if (time.Length > 2) {
				return time.Substring(0, time.Length - 2) + "." + time.Substring(time.Length - 2);
			}
			return "0." + time.PadLeft(2, '0');
		}
		public string MissionTime() {
			string time = GetColumn(missionTime);
			if (time.Length > 2) {
				return time.Substring(0, time.Length - 2) + "." + time.Substring(time.Length - 2);
			}
			return "0." + time.PadLeft(2, '0');
		}
		public bool HookProcess() {
			IsHooked = ProgramInfo != null && !ProgramInfo.HasExited;
			if (!IsHooked && DateTime.Now > LastHooked.AddSeconds(1)) {
				LastHooked = DateTime.Now;
				Process[] processes = Process.GetProcessesByName("MasterSpy");
				ProgramInfo = null;
				long maxSize = 0;
				for (int i = processes != null ? processes.Length - 1 : -1; i >= 0; i--) {
					Process process = processes[i];
					if (process.StartTime.AddSeconds(10) > DateTime.Now) { break; }

					if (process.MainWindowHandle != IntPtr.Zero) {
						ProgramWindow = processes[i];
					}
					if (process.WorkingSet64 > maxSize) {
						maxSize = process.WorkingSet64;
						ProgramInfo = process;
					}
				}

				if (ProgramInfo != null && !ProgramInfo.HasExited) {
					setupOffsets = false;
					IsHooked = true;
				}
			}

			return IsHooked;
		}
		public void Dispose() {
			if (ProgramInfo != null) {
				ProgramInfo.Dispose();
			}
			if (ProgramWindow != null) {
				ProgramWindow.Dispose();
			}
		}
	}
}