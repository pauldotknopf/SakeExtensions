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
var VERSION='0.1'
var FULL_VERSION='0.1'
var AUTHORS='Paul Knopf'

use import='Common'

#default
    dnvm-install
    dnvm dnvmCommand='version'
    exec-command commandline='dnvm version > test.txt'
");

            Assert.True(File.ReadAllText(SandboxPath("test.txt")).Contains("1.0.0-rc1-15540"));
        }
    }
}
