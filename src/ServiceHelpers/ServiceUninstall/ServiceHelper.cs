using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Principal;

namespace ServiceUninstall
{
    public static class ServiceHelper
    {
        private static string FetcharrExe => Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, "Fetcharr.Console.exe");

        private static bool IsAnAdministrator()
        {
            var principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public static void Run(string arg)
        {
            if (!File.Exists(FetcharrExe))
            {
                Console.WriteLine("Unable to find Fetcharr.exe in the current directory.");
                return;
            }

            if (!IsAnAdministrator())
            {
                Console.WriteLine("Access denied. Please run as administrator.");
                return;
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = FetcharrExe,
                Arguments = arg,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            var process = new Process { StartInfo = startInfo };
            process.OutputDataReceived += OnDataReceived;
            process.ErrorDataReceived += OnDataReceived;

            process.Start();

            process.BeginErrorReadLine();
            process.BeginOutputReadLine();

            process.WaitForExit();
        }

        private static void OnDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }
    }
}
