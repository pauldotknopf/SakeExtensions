using System.IO;
using Xunit;

namespace SakeExtensions.Tests
{
    public class DnvmTests : SandboxBase
    {
        [Fact]
        public void Can_install_dnvm()
        {
            RunShade(@"
use import='Common'

#default
    dnvm-install
");

            Assert.True(SandboxFileExists("bin", "dnvm", "dnvm.cmd"));
            Assert.True(SandboxFileExists("bin", "dnvm", "dnvm.ps1"));
            Assert.True(SandboxFileExists("bin", "dnvm", "dnvm.sh"));
        }

        [Fact]
        public void Can_use_runtime()
        {
            RunShade(@"
use import='Common'

#default
    dnvm-install
    dnvm-use runtime='CLR'
    exec-command commandLine='dnx --version > output1.txt'
    dnvm-use runtime='CoreCLR'
    exec-command commandLine='dnx --version > output2.txt'
    dnvm-use runtime='CLR'
    exec-command commandLine='dnx --version > output3.txt'
");

            Assert.True(File.Exists(SandboxPath("output1.txt")));
            Assert.True(File.ReadAllText(SandboxPath("output1.txt")).Contains("Type:         Clr"));
            Assert.True(File.Exists(SandboxPath("output2.txt")));
            Assert.True(File.ReadAllText(SandboxPath("output2.txt")).Contains("Type:         CoreClr"));
            Assert.True(File.Exists(SandboxPath("output3.txt")));
            Assert.True(File.ReadAllText(SandboxPath("output3.txt")).Contains("Type:         Clr"));
        }
    }
}
