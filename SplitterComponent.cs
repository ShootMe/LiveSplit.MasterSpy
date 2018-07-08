using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
namespace LiveSplit.MasterSpy {
	public class SplitterComponent : IComponent {
		public string ComponentName { get { return "Master Spy Autosplitter"; } }
		public TimerModel Model { get; set; }
		public IDictionary<string, Action> ContextMenuControls { get { return null; } }
		private static string LOGFILE = "_MasterSpy.log";
		private Dictionary<LogObject, string> currentValues = new Dictionary<LogObject, string>();
		private SplitterMemory mem;
		private int currentSplit = -1, lastLogCheck = 0, lastValue = 0;
		private bool hasLog = false;
		private string lastLevel;
		private DateTime lastChanged;
		private SplitterSettings settings;

		public SplitterComponent(LiveSplitState state) {
			mem = new SplitterMemory();
			foreach (LogObject key in Enum.GetValues(typeof(LogObject))) {
				currentValues[key] = "";
			}

			if (state != null) {
				Model = new TimerModel() { CurrentState = state };
				state.OnReset += OnReset;
				state.OnPause += OnPause;
				state.OnResume += OnResume;
				state.OnStart += OnStart;
				state.OnSplit += OnSplit;
				state.OnUndoSplit += OnUndoSplit;
				state.OnSkipSplit += OnSkipSplit;
			}

			settings = new SplitterSettings();
		}

		public void GetValues() {
			if (!mem.HookProcess()) { return; }

			if (Model != null) {
				HandleSplits();
			}

			LogValues();
		}
		private void HandleSplits() {
			bool shouldSplit = false;

			if (currentSplit == -1) {
				int start = mem.GameStart();
				if (start != lastValue) {
					lastChanged = DateTime.Now.AddMilliseconds(300);
				}
				shouldSplit = DateTime.Now < lastChanged && mem.Level() == "m_01_lvl_01" && mem.GameTime() == "0.00" && DateTime.Now > mem.LastHooked.AddSeconds(3);
				lastValue = start;
			} else if (Model.CurrentState.CurrentPhase == TimerPhase.Running && currentSplit < Model.CurrentState.Run.Count && currentSplit < settings.Splits.Count) {
				string level = mem.Level();
				SplitName split = settings.Splits[currentSplit];

				switch (split) {
					case SplitName.Level_01_01: shouldSplit = lastLevel == "m_01_lvl_01" && level != lastLevel; break;
					case SplitName.Level_01_02: shouldSplit = lastLevel == "m_01_lvl_02" && level != lastLevel; break;
					case SplitName.Level_01_03: shouldSplit = lastLevel == "m_01_lvl_03" && level != lastLevel; break;
					case SplitName.Level_01_04: shouldSplit = lastLevel == "m_01_lvl_04" && level != lastLevel; break;
					case SplitName.Level_01_05: shouldSplit = lastLevel == "m_01_lvl_05" && level != lastLevel; break;
					case SplitName.Level_01_06: shouldSplit = lastLevel == "m_01_lvl_06" && level != lastLevel; break;
					case SplitName.Level_01_07: shouldSplit = lastLevel == "m_01_lvl_07" && level != lastLevel; break;
					case SplitName.Level_01_08: shouldSplit = lastLevel == "m_01_lvl_08" && level != lastLevel; break;
					case SplitName.Level_01_09: shouldSplit = lastLevel == "m_01_lvl_09" && level != lastLevel; break;
					case SplitName.Level_01_10: shouldSplit = lastLevel == "m_01_lvl_10" && level != lastLevel; break;

					case SplitName.Level_02_01: shouldSplit = lastLevel == "m_02_lvl_01" && level != lastLevel; break;
					case SplitName.Level_02_02: shouldSplit = lastLevel == "m_02_lvl_02" && level != lastLevel; break;
					case SplitName.Level_02_03: shouldSplit = lastLevel == "m_02_lvl_03" && level != lastLevel; break;
					case SplitName.Level_02_04: shouldSplit = lastLevel == "m_02_lvl_04" && level != lastLevel; break;
					case SplitName.Level_02_05: shouldSplit = lastLevel == "m_02_lvl_05" && level != lastLevel; break;
					case SplitName.Level_02_06: shouldSplit = lastLevel == "m_02_lvl_06" && level != lastLevel; break;
					case SplitName.Level_02_07: shouldSplit = lastLevel == "m_02_lvl_07" && level != lastLevel; break;
					case SplitName.Level_02_08: shouldSplit = lastLevel == "m_02_lvl_08" && level != lastLevel; break;
					case SplitName.Level_02_09: shouldSplit = lastLevel == "m_02_lvl_09" && level != lastLevel; break;
					case SplitName.Level_02_10: shouldSplit = lastLevel == "m_02_lvl_10" && level != lastLevel; break;

					case SplitName.Level_03_01: shouldSplit = lastLevel == "m_03_lvl_01" && level != lastLevel; break;
					case SplitName.Level_03_02: shouldSplit = lastLevel == "m_03_lvl_02" && level != lastLevel; break;
					case SplitName.Level_03_03: shouldSplit = lastLevel == "m_03_lvl_03" && level != lastLevel; break;
					case SplitName.Level_03_04: shouldSplit = lastLevel == "m_03_lvl_04" && level != lastLevel; break;
					case SplitName.Level_03_05: shouldSplit = lastLevel == "m_03_lvl_05" && level != lastLevel; break;
					case SplitName.Level_03_06: shouldSplit = lastLevel == "m_03_lvl_06" && level != lastLevel; break;
					case SplitName.Level_03_07: shouldSplit = lastLevel == "m_03_lvl_07" && level != lastLevel; break;
					case SplitName.Level_03_08: shouldSplit = lastLevel == "m_03_lvl_08" && level != lastLevel; break;
					case SplitName.Level_03_09: shouldSplit = lastLevel == "m_03_lvl_09" && level != lastLevel; break;
					case SplitName.Level_03_10: shouldSplit = lastLevel == "m_03_lvl_10" && level != lastLevel; break;

					case SplitName.Level_04_01: shouldSplit = lastLevel == "m_04_lvl_01" && level != lastLevel; break;
					case SplitName.Level_04_02: shouldSplit = lastLevel == "m_04_lvl_02" && level != lastLevel; break;
					case SplitName.Level_04_03: shouldSplit = lastLevel == "m_04_lvl_03" && level != lastLevel; break;
					case SplitName.Level_04_04: shouldSplit = lastLevel == "m_04_lvl_04" && level != lastLevel; break;
					case SplitName.Level_04_05: shouldSplit = lastLevel == "m_04_lvl_05" && level != lastLevel; break;
					case SplitName.Level_04_06: shouldSplit = lastLevel == "m_04_lvl_06" && level != lastLevel; break;
					case SplitName.Level_04_07: shouldSplit = lastLevel == "m_04_lvl_07" && level != lastLevel; break;
					case SplitName.Level_04_08: shouldSplit = lastLevel == "m_04_lvl_08" && level != lastLevel; break;
					case SplitName.Level_04_09: shouldSplit = lastLevel == "m_04_lvl_09" && level != lastLevel; break;
					case SplitName.Level_04_10: shouldSplit = lastLevel == "m_04_lvl_10" && level != lastLevel; break;

					case SplitName.Level_05_01: shouldSplit = lastLevel == "m_05_lvl_01" && level != lastLevel; break;
					case SplitName.Level_05_02: shouldSplit = lastLevel == "m_05_lvl_02" && level != lastLevel; break;
					case SplitName.Level_05_03: shouldSplit = lastLevel == "m_05_lvl_03" && level != lastLevel; break;
					case SplitName.Level_05_04: shouldSplit = lastLevel == "m_05_lvl_04" && level != lastLevel; break;
					case SplitName.Level_05_05: shouldSplit = lastLevel == "m_05_lvl_05" && level != lastLevel; break;
					case SplitName.Level_05_06: shouldSplit = lastLevel == "m_05_lvl_06" && level != lastLevel; break;
					case SplitName.Level_05_07: shouldSplit = lastLevel == "m_05_lvl_07" && level != lastLevel; break;
					case SplitName.Level_05_08: shouldSplit = lastLevel == "m_05_lvl_08" && level != lastLevel; break;
					case SplitName.Level_05_09: shouldSplit = lastLevel == "m_05_lvl_09" && level != lastLevel; break;
					case SplitName.Level_05_10: shouldSplit = lastLevel == "m_05_lvl_10" && mem.MissionTime() == "0.00"; break;
				}

				lastLevel = level;
			}

			HandleSplit(shouldSplit, false);
		}
		private void HandleSplit(bool shouldSplit, bool shouldReset = false) {
			if (shouldReset) {
				if (currentSplit >= 0) {
					Model.Reset();
				}
			} else if (shouldSplit) {
				if (currentSplit < 0) {
					Model.Start();
				} else {
					Model.Split();
				}
			}
		}
		private void LogValues() {
			if (lastLogCheck == 0) {
				hasLog = File.Exists(LOGFILE);
				lastLogCheck = 300;
			}
			lastLogCheck--;

			if (hasLog || !Console.IsOutputRedirected) {
				string prev = string.Empty, curr = string.Empty;
				foreach (LogObject key in Enum.GetValues(typeof(LogObject))) {
					prev = currentValues[key];

					switch (key) {
						case LogObject.CurrentSplit: curr = currentSplit.ToString(); break;
						case LogObject.GameStart: curr = mem.GameStart().ToString(); break;
						case LogObject.Level: curr = mem.Level(); break;
						case LogObject.LevelTime: curr = mem.LevelTime(); break;
						case LogObject.GameTime: curr = mem.GameTime(); break;
						case LogObject.MissionTime: curr = mem.MissionTime(); break;
						default: curr = string.Empty; break;
					}

					if (string.IsNullOrEmpty(prev)) { prev = string.Empty; }
					if (string.IsNullOrEmpty(curr)) { curr = string.Empty; }
					if (!prev.Equals(curr)) {
						WriteLogWithTime(key + ": ".PadRight(16 - key.ToString().Length, ' ') + prev.PadLeft(25, ' ') + " -> " + curr);

						currentValues[key] = curr;
					}
				}
			}
		}
		public void Update(IInvalidator invalidator, LiveSplitState lvstate, float width, float height, LayoutMode mode) {
			GetValues();
		}
		public void OnReset(object sender, TimerPhase e) {
			currentSplit = -1;
			WriteLog("---------Reset----------------------------------");
		}
		public void OnResume(object sender, EventArgs e) {
			WriteLog("---------Resumed--------------------------------");
		}
		public void OnPause(object sender, EventArgs e) {
			WriteLog("---------Paused---------------------------------");
		}
		public void OnStart(object sender, EventArgs e) {
			currentSplit = 0;
			WriteLog("---------New Game " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(3) + "-------------------------");
			if (currentSplit < settings.Splits.Count) {
				WriteLog("---------" + settings.Splits[currentSplit].ToString());
			}
		}
		public void OnUndoSplit(object sender, EventArgs e) {
			currentSplit--;
			WriteLog("---------Undo-----------------------------------");
		}
		public void OnSkipSplit(object sender, EventArgs e) {
			currentSplit++;
			WriteLog("---------Skip-----------------------------------");
		}
		public void OnSplit(object sender, EventArgs e) {
			currentSplit++;
			WriteLog("---------Split----------------------------------");
			if (currentSplit < settings.Splits.Count) {
				WriteLog("---------" + settings.Splits[currentSplit].ToString());
			}
		}
		private void WriteLog(string data) {
			if (hasLog || !Console.IsOutputRedirected) {
				if (Console.IsOutputRedirected) {
					using (StreamWriter wr = new StreamWriter(LOGFILE, true)) {
						wr.WriteLine(data);
					}
				} else {
					Console.WriteLine(data);
				}
			}
		}
		private void WriteLogWithTime(string data) {
			WriteLog(DateTime.Now.ToString(@"HH\:mm\:ss.fff") + (Model != null && Model.CurrentState.CurrentTime.RealTime.HasValue ? " | " + Model.CurrentState.CurrentTime.RealTime.Value.ToString("G").Substring(3, 11) : "") + ": " + data);
		}

		public Control GetSettingsControl(LayoutMode mode) { return settings; }
		public void SetSettings(XmlNode document) { settings.SetSettings(document); }
		public XmlNode GetSettings(XmlDocument document) { return settings.UpdateSettings(document); }
		public void DrawHorizontal(Graphics g, LiveSplitState state, float height, Region clipRegion) { }
		public void DrawVertical(Graphics g, LiveSplitState state, float width, Region clipRegion) { }
		public float HorizontalWidth { get { return 0; } }
		public float MinimumHeight { get { return 0; } }
		public float MinimumWidth { get { return 0; } }
		public float PaddingBottom { get { return 0; } }
		public float PaddingLeft { get { return 0; } }
		public float PaddingRight { get { return 0; } }
		public float PaddingTop { get { return 0; } }
		public float VerticalHeight { get { return 0; } }
		public void Dispose() { }
	}
}