using System;
using System.IO;
using Xunit;

namespace SakeExtensions.Tests
{
    public class NodeTests : SandboxBase
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

        [Fact]
        public void Can_execute_node_command()
        {
            File.WriteAllText(SandboxPath("test.js"), "console.log(\"test output from script file\");");

            var result = RunShade(
    @"
var VERSION='0.1'
var FULL_VERSION='0.1'
var AUTHORS='Paul Knopf'

use import='Common'

#default
    node nodeCommand='test.js'
");

            Assert.True(result.Contains("test output from script file"));
        }

        [Fact]
        public void Can_working_directory_in_node()
        {
            var testScript = SandboxPath("nested", "test.js");
            Directory.CreateDirectory(Path.GetDirectoryName(testScript));
            File.WriteAllText(testScript, "console.log(\"test output from script file\");");

            var result = RunShade(
    @"
var VERSION='0.1'
var FULL_VERSION='0.1'
var AUTHORS='Paul Knopf'

use import='Common'

#default
    node nodeCommand='test.js' workingdir='${Path.Combine(Directory.GetCurrentDirectory(), ""nested"")}'
");

            Assert.True(result.Contains("test output from script file"));
        }
    }
}
