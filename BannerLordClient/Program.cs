using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
namespace BannerLoadClient
{
	internal static class MBDotNet
	{
		// Token: 0x06000004 RID: 4
		[SuppressUnmanagedCodeSecurity]
		[DllImport("Rgl.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "WotsMain")]
		public static extern int WotsMainDotNet(string args);

		// Token: 0x06000005 RID: 5
		[SuppressUnmanagedCodeSecurity]
		[DllImport("FairyTale.DotNet.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "pass_controller_methods")]
		public static extern void PassControllerMethods(Delegate currentDomainInitializer);

		// Token: 0x06000006 RID: 6
		[SuppressUnmanagedCodeSecurity]
		[DllImport("FairyTale.DotNet.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "pass_managed_initialize_method_pointer")]
		public static extern void PassManagedInitializeMethodPointerDotNet([MarshalAs(UnmanagedType.FunctionPtr)] Delegate initalizer);

		// Token: 0x06000007 RID: 7
		[SuppressUnmanagedCodeSecurity]
		[DllImport("FairyTale.DotNet.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "pass_managed_library_callback_method_pointers")]
		public static extern void PassManagedEngineCallbackMethodPointersDotNet([MarshalAs(UnmanagedType.FunctionPtr)] Delegate methodDelegate);

		// Token: 0x06000008 RID: 8
		[SuppressUnmanagedCodeSecurity]
		[DllImport("Kernel32.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern int SetCurrentDirectory(string args);

		// Token: 0x04000002 RID: 2
		public const string MainDllName = "Rgl.dll";

		// Token: 0x04000003 RID: 3
		public const string DotNetLibraryDllName = "FairyTale.DotNet.dll";
	}

class Program
    {
		// Token: 0x06000001 RID: 1 RVA: 0x00002048 File Offset: 0x00000248
		private static int Starter()
		{
			try
			{
				Assembly.LoadFrom("TaleWorlds.Library.dll");
				Assembly.LoadFrom("TaleWorlds.DotNet.dll").GetType("TaleWorlds.DotNet.Controller").GetMethod("SetEngineMethodsAsDotNet").Invoke(null, new object[]
				{
					
					new Program.ControllerDelegate(MBDotNet.PassControllerMethods),
					new Program.InitializerDelegate(MBDotNet.PassManagedInitializeMethodPointerDotNet),
					new Program.InitializerDelegate(MBDotNet.PassManagedEngineCallbackMethodPointersDotNet)
				});
			}
			catch (FileNotFoundException ex)
			{
				Console.WriteLine("Exception: " + ex);
				Console.WriteLine("Fusion Log: " + ex.FusionLog);
				Console.WriteLine("Exception detailed: " + ex.ToString());
				if (ex.InnerException != null)
				{
					Console.WriteLine("Inner Exception: " + ex.InnerException);
				}
				Console.WriteLine("Press a key to continue...");
				Console.ReadKey();
			}
			catch (Exception ex2)
			{
				Console.WriteLine("Exception: " + ex2);
				if (ex2.InnerException != null)
				{
					Console.WriteLine("Inner Exception: " + ex2.InnerException);
				}
				Console.WriteLine("Press a key to continue...");
				Console.ReadKey();
			}
			string text = "";
			for (int i = 0; i < Program._args.Length; i++)
			{
				string str = Program._args[i];
				text += str;
				if (i + 1 < Program._args.Length)
				{
					text += " ";
				}
			}
			MBDotNet.SetCurrentDirectory(Directory.GetCurrentDirectory());
			return MBDotNet.WotsMainDotNet(text);
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000021D8 File Offset: 0x000003D8
		[STAThread]
		public static int Main(string[] args)
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
			CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
			CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");
			Program._args = args;
			return Program.Starter();
		}

		// Token: 0x04000001 RID: 1
		private static string[] _args;

		// Token: 0x02000004 RID: 4
		// (Invoke) Token: 0x0600000A RID: 10
		private delegate void ControllerDelegate(Delegate currentDomainInitializer);

		// Token: 0x02000005 RID: 5
		// (Invoke) Token: 0x0600000E RID: 14
		private delegate void InitializerDelegate(Delegate argument);

		// Token: 0x02000006 RID: 6
		// (Invoke) Token: 0x06000012 RID: 18
		private delegate void StartMethodDelegate(string args);
	}
}
