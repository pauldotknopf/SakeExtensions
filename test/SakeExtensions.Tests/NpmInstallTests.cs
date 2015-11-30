using System;
using Xunit;

namespace SakeExtensions.Tests
{
    public class NpmInstallTests : SandboxBase
    {
        [Fact]
        public void Can_install_node_and_npm()
        {
            RunShade(
                @"
var VERSION='0.1'
var FULL_VERSION='0.1'
var AUTHORS='Paul Knopf'

use import='Common'

#default
    node-install
");

            Assert.True(SandboxDirectoryExists("bin", "nodejs"));
            Assert.True(SandboxFileExists("bin", "nodejs", "node.exe"));
            Assert.True(SandboxFileExists("bin", "nodejs", "npm.cmd"));
        }
    }
}
