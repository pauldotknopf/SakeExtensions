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
    public class SandboxBase : IDisposable
    {
        string _sandBoxDirectory;
        string _sakeExePath;
        string _sakeDirectory;
        string _buildScriptsDirectory;
        string _shadesDirectory;

        public SandboxBase()
        {
            var appEnv = CallContextServiceLocator.Locator.ServiceProvider.GetService(typeof(IApplicationEnvironment)) as IApplicationEnvironment;

            _sandBoxDirectory = Path.GetFullPath(Path.Combine(Path.Combine(appEnv.ApplicationBasePath, ".."), "sb-" + Guid.NewGuid().ToString().Substring(0,2).Replace("-", "")));
            Directory.CreateDirectory(_sandBoxDirectory);

            _shadesDirectory = Path.Combine(appEnv.ApplicationBasePath, "Shades");

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

        protected string RunShadeDnx(string shade)
        {
            // install dnvm first
            RunShade(@"
use import='Common'

#default
    dnvm-install
");

            var result = new StringBuilder();

            var temp = Guid.NewGuid().ToString().Replace("-", "") + ".shade";

            File.WriteAllText(Path.Combine(_sandBoxDirectory, temp), shade);

            var dnvmUseCommand = @"bin\dnvm\dnvm use default";
            var sakeCommand = string.Format("\"" + _sakeExePath + "\" -I \"{0}\" -I \"{1}\" -f \"{2}\"", Path.Combine(_sakeDirectory, "Shared"), _buildScriptsDirectory, temp);

            var processStartInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                WorkingDirectory = _sandBoxDirectory,
                FileName = "cmd",
                Arguments = string.Format("/C {0}", dnvmUseCommand + " && " + sakeCommand)
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

        protected string RunShadeFile(string file)
        {
            if (!file.EndsWith(".shade"))
                file += ".shade";

            return RunShade(File.ReadAllText(Path.Combine(_shadesDirectory, file)));
        }

        protected bool SandboxFileExists(params string[] parts)
        {
            return File.Exists(SandboxPath(parts));
        }
        
        protected bool SandboxDirectoryExists(params string[] parts)
        {
            var path = _sandBoxDirectory;

            foreach (var part in parts)
                path = Path.Combine(path, part);

            return Directory.Exists(path);
        }

        protected string SandboxPath(params string[] parts)
        {
            var path = _sandBoxDirectory;

            foreach (var part in parts)
                path = Path.Combine(path, part);

            return path;
        }

        public void Dispose()
        {

        }
    }
}
