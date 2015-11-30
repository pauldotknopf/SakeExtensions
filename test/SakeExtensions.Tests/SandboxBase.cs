using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SakeExtensions.Tests
{
    public class SandboxBase
    {
        string _sandBoxDirectory;
        string _sakeExePath;
        string _sakeDirectory;
        string _buildScriptsDirectory;

        public SandboxBase()
        {
            var appEnv = CallContextServiceLocator.Locator.ServiceProvider.GetService(typeof(IApplicationEnvironment)) as IApplicationEnvironment;

            _sandBoxDirectory = Path.GetFullPath(Path.Combine(Path.Combine(appEnv.ApplicationBasePath, ".."), "sandbox"));

            if (Directory.Exists(_sandBoxDirectory))
                Directory.Delete(_sandBoxDirectory, true);

            Directory.CreateDirectory(_sandBoxDirectory);

            _sakeDirectory = Path.GetFullPath(Path.Combine(Path.Combine(Path.Combine(appEnv.ApplicationBasePath, ".."), ".."), "tools", "sake"));
            _sakeExePath = Path.Combine(_sakeDirectory, "sake.exe");

            _buildScriptsDirectory = Path.GetFullPath(Path.Combine(Path.Combine(Path.Combine(appEnv.ApplicationBasePath, ".."), ".."), "build"));
        }

        protected string RunShade(string shade)
        {
            var result = new StringBuilder();

            var temp = Guid.NewGuid().ToString().Replace("-", "") + ".shade";

            File.WriteAllText(Path.Combine(_sandBoxDirectory, temp), shade);

            var processStartInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                WorkingDirectory = _sandBoxDirectory,
                FileName = _sakeExePath,
                Arguments = string.Format("-I \"{0}\" -I \"{1}\" -f \"{2}\"", Path.Combine(_sakeDirectory, "Shared"), _buildScriptsDirectory, temp)
            };
            
            processStartInfo.RedirectStandardError = true;
            processStartInfo.RedirectStandardOutput = true;

            using (var process = Process.Start(processStartInfo))
            {
                process.EnableRaisingEvents = true;
              
                process.ErrorDataReceived += (sender, eventArgs) =>
                {
                    if (!string.IsNullOrWhiteSpace(eventArgs.Data))
                    {
                        Console.WriteLine(eventArgs.Data);
                        result.AppendLine(eventArgs.Data);
                    }
                };

                process.OutputDataReceived += (sender, eventArgs) =>
                {
                    Console.WriteLine(eventArgs.Data);
                    result.AppendLine(eventArgs.Data);
                };

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    throw new Exception(string.Format("Exit code {0} from {1}", process.ExitCode, "sake"));
                }
            }

            return result.ToString();
        }

        protected bool SandboxFileExists(params string[] parts)
        {
            var path = _sandBoxDirectory;

            foreach(var part in parts)
                path = Path.Combine(path, part);

            return File.Exists(path);
        }

        protected bool SandboxDirectoryExists(params string[] parts)
        {
            var path = _sandBoxDirectory;

            foreach (var part in parts)
                path = Path.Combine(path, part);

            return Directory.Exists(path);
        }
    }
}
