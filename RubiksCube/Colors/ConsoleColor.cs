namespace RubiksCube
{
	using System;
	using System.Runtime.InteropServices;

	public class ConsoleColor : IDisposable
	{
		public ConsoleColor(int color)
		{
			CONSOLE_SCREEN_BUFFER_INFO consoleScreenBufferInfo;

			GetConsoleScreenBufferInfo(hConsole, out consoleScreenBufferInfo);

			this.oldTextAttributes = consoleScreenBufferInfo.wAttributes;

			int wTextAttributes = (this.oldTextAttributes & ~0xFF) | color;

			SetConsoleTextAttribute(hConsole, wTextAttributes);
		}

		private const int STD_OUTPUT_HANDLE = -11;

		private static readonly IntPtr hConsole = GetStdHandle(STD_OUTPUT_HANDLE);

		private int oldTextAttributes;

		[StructLayout(LayoutKind.Sequential)]
		struct CONSOLE_SCREEN_BUFFER_INFO
		{
			uint dwSize;
			uint dwCursorPosition;
			public int wAttributes;
			ulong srWindow;
			uint dwMaximumWindowSize;
		};

		[DllImportAttribute("Kernel32.dll")]
		private static extern bool GetConsoleScreenBufferInfo(
			IntPtr hConsoleOutput,
			out CONSOLE_SCREEN_BUFFER_INFO consoleScreenBufferInfo
			);

		// input, output, or error device
		[DllImportAttribute("Kernel32.dll")]
		private static extern IntPtr GetStdHandle(int nStdHandle);

		[DllImportAttribute("Kernel32.dll")]
		private static extern bool SetConsoleTextAttribute(
			IntPtr hConsoleOutput, // handle to screen buffer
			int wAttributes    // text and background colors
			);


		public enum ForeGroundColor : int
		{
			Black = 0x0000,
			Blue = 0x0001,
			Green = 0x0002,
			Cyan = 0x0003,
			Red = 0x0004,
			Magenta = 0x0005,
			Yellow = 0x0006,
			Grey = 0x0007,
			White = 0x0008
		}

		public enum BackGroundColor : int
		{
			Black = 0x0000,
			Blue = 0x00010,
			Green = 0x00020,
			Cyan = 0x00030,
			Red = 0x00040,
			Magenta = 0x00050,
			Yellow = 0x00060,
			Grey = 0x00070,
			White = 0x00080
		}

		public void Dispose()
		{
			SetConsoleTextAttribute(hConsole, oldTextAttributes);
		}
	}

}