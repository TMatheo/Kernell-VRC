using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using MelonLoader;

namespace KernelVRC
{
	// Token: 0x020000BB RID: 187
	public static class kernelllogger
	{
		// Token: 0x060009E6 RID: 2534
		[DllImport("kernel32.dll")]
		private static extern bool AllocConsole();

		// Token: 0x060009E7 RID: 2535
		[DllImport("kernel32.dll")]
		private static extern IntPtr GetStdHandle(int nStdHandle);

		// Token: 0x060009E8 RID: 2536
		[DllImport("kernel32.dll")]
		private static extern bool SetStdHandle(int nStdHandle, IntPtr handle);

		// Token: 0x060009E9 RID: 2537 RVA: 0x0003E74D File Offset: 0x0003C94D
		static kernelllogger()
		{
			kernelllogger.InitializeConsole();
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x0003E768 File Offset: 0x0003C968
		private static void InitializeConsole()
		{
			try
			{
				string path = Path.Combine(Environment.CurrentDirectory, "UserData");
				string path2 = Path.Combine(path, ".NOCON");
				bool flag = File.Exists(path2);
				if (flag)
				{
					MelonLogger.Msg("Console disabled by .NOCON file in UserData directory");
					kernelllogger.consoleInitialized = false;
				}
				else
				{
					kernelllogger.AllocConsole();
					IntPtr stdHandle = kernelllogger.GetStdHandle(-11);
					kernelllogger.consoleOut = new StreamWriter(Console.OpenStandardOutput())
					{
						AutoFlush = true
					};
					Console.SetOut(kernelllogger.consoleOut);
					kernelllogger.consoleInitialized = true;
					Console.Title = "Kernell 2.0 Debug Console";
					kernelllogger.DisplayWelcomeScreen();
				}
			}
			catch (Exception ex)
			{
				MelonLogger.Error("Failed to initialize debug console: " + ex.Message);
				kernelllogger.consoleInitialized = false;
			}
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x0003E830 File Offset: 0x0003CA30
		private static void DisplayWelcomeScreen()
		{
			Console.Clear();
			string[] array = new string[]
			{
				"",
				"  ██╗  ██╗███████╗██████╗ ███╗   ██╗███████╗██╗     ██╗         ██████╗    ██████╗ ",
				"  ██║ ██╔╝██╔════╝██╔══██╗████╗  ██║██╔════╝██║     ██║         ╚════██╗  ██╔═████╗",
				"  █████╔╝ █████╗  ██████╔╝██╔██╗ ██║█████╗  ██║     ██║          █████╔╝  ██║██╔██║",
				"  ██╔═██╗ ██╔══╝  ██╔══██╗██║╚██╗██║██╔══╝  ██║     ██║         ██╔═══╝   ████╔╝██║",
				"  ██║  ██╗███████╗██║  ██║██║ ╚████║███████╗███████╗███████╗    ███████╗██╗╚██████╔╝",
				"  ╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝╚═╝  ╚═══╝╚══════╝╚══════╝╚══════╝    ╚══════╝╚═╝ ╚═════╝ ",
				""
			};
			ConsoleColor[] array2 = new ConsoleColor[6];
			RuntimeHelpers.InitializeArray(array2, fieldof(<PrivateImplementationDetails>.0F3E4A16BF85E7FD1B720FBC9F161539DA02C362DB67D8731BCBC14666E91F5D).FieldHandle);
			ConsoleColor[] array3 = array2;
			for (int i = 0; i < array.Length; i++)
			{
				Console.ForegroundColor = array3[i % array3.Length];
				Console.WriteLine(array[i]);
				Thread.Sleep(100);
			}
			Console.ResetColor();
			Console.WriteLine();
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("╔═════════════════════════════════════════════════════╗");
			Console.WriteLine("║                                                     ║");
			Console.Write("║  ");
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write("Kernell 2.0 Debug Console");
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("                            ║");
			Console.Write("║  ");
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.Write("Version: 2.0.0");
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("                                   ║");
			Console.WriteLine("║                                                     ║");
			Console.WriteLine("╚═════════════════════════════════════════════════════╝");
			Console.WriteLine();
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("■ Kernell 2.0 initialized successfully");
			Console.WriteLine("■ Advanced logging system active");
			Console.WriteLine("■ Debug mode: " + (kernelllogger.IsDebugBuild() ? "ENABLED" : "DISABLED"));
			Console.WriteLine();
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine("═════════════════════ LOG OUTPUT ═════════════════════");
			Console.WriteLine();
			Console.ResetColor();
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x0003E9DC File Offset: 0x0003CBDC
		private static bool IsDebugBuild()
		{
			return true;
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x0003E9EF File Offset: 0x0003CBEF
		public static void Msg(string message)
		{
			MelonLogger.Msg(message);
			kernelllogger.WriteToConsole(message, ConsoleColor.White);
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x0003EA02 File Offset: 0x0003CC02
		public static void Msg(ConsoleColor color, string message)
		{
			MelonLogger.Msg(color, message);
			kernelllogger.WriteToConsole(message, color);
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x0003EA15 File Offset: 0x0003CC15
		public static void Error(string message)
		{
			MelonLogger.Error(message);
			kernelllogger.WriteToConsole("[ERROR] " + message, ConsoleColor.Red);
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x0003EA32 File Offset: 0x0003CC32
		public static void Warning(string message)
		{
			MelonLogger.Warning(message);
			kernelllogger.WriteToConsole("[WARNING] " + message, ConsoleColor.Yellow);
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x0003EA4F File Offset: 0x0003CC4F
		public static void LogDebug(string message)
		{
			MelonLogger.Msg("[DEBUG] " + message);
			kernelllogger.WriteToConsole("[DEBUG] " + message, ConsoleColor.Gray);
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x0003EA75 File Offset: 0x0003CC75
		public static void Success(string message)
		{
			MelonLogger.Msg(ConsoleColor.Green, message);
			kernelllogger.WriteToConsole("[SUCCESS] " + message, ConsoleColor.Green);
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x0003EA94 File Offset: 0x0003CC94
		public static void Info(string message)
		{
			MelonLogger.Msg(ConsoleColor.Cyan, message);
			kernelllogger.WriteToConsole("[INFO] " + message, ConsoleColor.Cyan);
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x0003EAB4 File Offset: 0x0003CCB4
		private static void WriteToConsole(string message, ConsoleColor color = ConsoleColor.White)
		{
			bool flag = !kernelllogger.consoleInitialized;
			if (!flag)
			{
				try
				{
					Console.ForegroundColor = ConsoleColor.DarkGray;
					Console.Write(string.Format("[{0:HH:mm:ss}] ", DateTime.Now));
					Console.ForegroundColor = ConsoleColor.Magenta;
					Console.Write(kernelllogger.consoleMarker);
					Console.ForegroundColor = color;
					Console.WriteLine(message);
					Console.ResetColor();
				}
				catch (Exception ex)
				{
					Console.WriteLine("Error writing to console: " + ex.Message);
				}
			}
		}

		// Token: 0x04000514 RID: 1300
		private const int STD_OUTPUT_HANDLE = -11;

		// Token: 0x04000515 RID: 1301
		private const int STD_ERROR_HANDLE = -12;

		// Token: 0x04000516 RID: 1302
		private static TextWriter consoleOut;

		// Token: 0x04000517 RID: 1303
		private static bool consoleInitialized = false;

		// Token: 0x04000518 RID: 1304
		private static readonly string consoleMarker = "Kernell 2.0> ";
	}
}
