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
    }
}
