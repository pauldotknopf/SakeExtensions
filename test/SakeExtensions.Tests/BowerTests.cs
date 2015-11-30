using System.IO;
using Xunit;

namespace SakeExtensions.Tests
{
    public class BowerTests : SandboxBase
    {
        [Fact]
        public void Can_install_bower_library()
        {
            RunShade(@"
var VERSION='0.1'
var FULL_VERSION='0.1'
var AUTHORS='Paul Knopf'

use import='Common'

#default
    bower-install
    bower bowerCommand='install jquery'
");

            Assert.True(SandboxFileExists("bower_components", "jquery", "dist", "jquery.js"));
        }

        [Fact]
        public void Can_install_bower_library_different_directory()
        {
            Directory.CreateDirectory(SandboxPath("nested"));

            RunShade(@"
var VERSION='0.1'
var FULL_VERSION='0.1'
var AUTHORS='Paul Knopf'

use import='Common'

#default
    bower-install
    bower bowerCommand='install jquery' workingdir='${Path.Combine(Directory.GetCurrentDirectory(), ""nested"")}
");

            Assert.True(SandboxFileExists("nested", "bower_components", "jquery", "dist", "jquery.js"));
        }
    }
}
