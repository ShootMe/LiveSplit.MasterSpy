using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
namespace LiveSplit.MasterSpy {
	public static class MemoryReader {
		public static T Read<T>(this Process targetProcess, params int[] offsets) where T : struct {
			return Read<T>(targetProcess, targetProcess.MainModule.BaseAddress, offsets);
		}
		public static T Read<T>(this Process targetProcess, IntPtr address, params int[] offsets) where T : struct {
			if (targetProcess == null || targetProcess.HasExited || address == IntPtr.Zero) { return default(T); }

			int last = OffsetAddress(targetProcess, ref address, offsets);

			Type type = typeof(T);
			type = (type.IsEnum ? Enum.GetUnderlyingType(type) : type);

			int count = (type == typeof(bool)) ? 1 : Marshal.SizeOf(type);
			byte[] buffer = Read(targetProcess, address + last, count);

			object obj = ResolveToType(buffer, type);
			return (T)((object)obj);
		}
		private static object ResolveToType(byte[] bytes, Type type) {
			if (type == typeof(int)) {
				return BitConverter.ToInt32(bytes, 0);
			} else if (type == typeof(uint)) {
				return BitConverter.ToUInt32(bytes, 0);
			} else if (type == typeof(float)) {
				return BitConverter.ToSingle(bytes, 0);
			} else if (type == typeof(double)) {
				return BitConverter.ToDouble(bytes, 0);
			} else if (type == typeof(byte)) {
				return bytes[0];
			} else if (type == typeof(bool)) {
				return bytes != null && bytes[0] > 0;
			} else if (type == typeof(short)) {
				return BitConverter.ToInt16(bytes, 0);
			} else if (type == typeof(ushort)) {
				return BitConverter.ToUInt16(bytes, 0);
			} else if (type == typeof(long)) {
				return BitConverter.ToInt64(bytes, 0);
			} else if (type == typeof(ulong)) {
				return BitConverter.ToUInt64(bytes, 0);
			} else {
				GCHandle gCHandle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
				try {
					return Marshal.PtrToStructure(gCHandle.AddrOfPinnedObject(), type);
				} finally {
					gCHandle.Free();
				}
			}
		}
		public static byte[] Read(this Process targetProcess, IntPtr address, int numBytes) {
			byte[] buffer = new byte[numBytes];
			if (targetProcess == null || targetProcess.HasExited || address == IntPtr.Zero) { return buffer; }

			int bytesRead;
			WinAPI.ReadProcessMemory(targetProcess.Handle, address, buffer, numBytes, out bytesRead);
			return buffer;
		}
		public static string ReadString(this Process targetProcess, int length, params int[] offsets) {
			return ReadString(targetProcess, targetProcess.MainModule.BaseAddress, length, offsets);
		}
		public static string ReadString(this Process targetProcess, IntPtr address, int length, params int[] offsets) {
			if (targetProcess == null || address == IntPtr.Zero) { return string.Empty; }

			int last = OffsetAddress(targetProcess, ref address, offsets);
			if (address == IntPtr.Zero) { return string.Empty; }

			string temp = Encoding.Unicode.GetString(Read(targetProcess, address + last, 2 * length));
			for (int i = 0; i < temp.Length; i++) {
				if (temp[i] == '\0') {
					if (i > 0) {
						return temp.Substring(0, i);
					}
					return string.Empty;
				}
			}
			return temp;
		}
		public static string ReadAscii(this Process targetProcess, IntPtr address) {
			if (targetProcess == null || targetProcess.HasExited || address == IntPtr.Zero) { return string.Empty; }

			StringBuilder sb = new StringBuilder();
			byte[] data = new byte[128];
			int bytesRead;
			int offset = 0;
			bool invalid = false;
			do {
				WinAPI.ReadProcessMemory(targetProcess.Handle, address + offset, data, 128, out bytesRead);
				int i = 0;
				while (i < bytesRead) {
					byte d = data[i++];
					if (d == 0) {
						i--;
						break;
					} else if (d > 127) {
						invalid = true;
						break;
					}
				}
				if (i > 0) {
					sb.Append(Encoding.ASCII.GetString(data, 0, i));
				}
				if (i < bytesRead || invalid) {
					break;
				}
				offset += 128;
			} while (bytesRead > 0);

			return invalid ? string.Empty : sb.ToString();
		}
		public static void Write<T>(this Process targetProcess, IntPtr address, T value, params int[] offsets) where T : struct {
			if (targetProcess == null || targetProcess.HasExited) { return; }

			int last = OffsetAddress(targetProcess, ref address, offsets);
			byte[] buffer = null;
			if (typeof(T) == typeof(bool)) {
				buffer = BitConverter.GetBytes(Convert.ToBoolean(value));
			} else if (typeof(T) == typeof(byte)) {
				buffer = BitConverter.GetBytes(Convert.ToByte(value));
			} else if (typeof(T) == typeof(int)) {
				buffer = BitConverter.GetBytes(Convert.ToInt32(value));
			} else if (typeof(T) == typeof(uint)) {
				buffer = BitConverter.GetBytes(Convert.ToUInt32(value));
			} else if (typeof(T) == typeof(short)) {
				buffer = BitConverter.GetBytes(Convert.ToInt16(value));
			} else if (typeof(T) == typeof(ushort)) {
				buffer = BitConverter.GetBytes(Convert.ToUInt16(value));
			} else if (typeof(T) == typeof(long)) {
				buffer = BitConverter.GetBytes(Convert.ToInt64(value));
			} else if (typeof(T) == typeof(ulong)) {
				buffer = BitConverter.GetBytes(Convert.ToUInt64(value));
			} else if (typeof(T) == typeof(float)) {
				buffer = BitConverter.GetBytes(Convert.ToSingle(value));
			} else if (typeof(T) == typeof(double)) {
				buffer = BitConverter.GetBytes(Convert.ToDouble(value));
			}

			int bytesWritten;
			WinAPI.WriteProcessMemory(targetProcess.Handle, address + last, buffer, buffer.Length, out bytesWritten);
		}
		private static int OffsetAddress(this Process targetProcess, ref IntPtr address, params int[] offsets) {
			byte[] buffer = new byte[4];
			int bytesWritten;
			for (int i = 0; i < offsets.Length - 1; i++) {
				WinAPI.ReadProcessMemory(targetProcess.Handle, address + offsets[i], buffer, buffer.Length, out bytesWritten);
				address = (IntPtr)BitConverter.ToUInt32(buffer, 0);
			}
			return offsets.Length > 0 ? offsets[offsets.Length - 1] : 0;
		}
		private static class WinAPI {
			[DllImport("kernel32.dll", SetLastError = true)]
			public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);
			[DllImport("kernel32.dll", SetLastError = true)]
			public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out int lpNumberOfBytesWritten);
		}
	}
}