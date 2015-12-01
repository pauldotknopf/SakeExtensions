using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SakeExtensions.Tests
{
    public class DnuTests : SandboxBase
    {
        [Fact]
        public void Can_build()
        {
            Directory.CreateDirectory(SandboxPath("testdirectory"));
            File.WriteAllText(SandboxPath("testdirectory", "project.json"), @"
{
  ""version"": ""1.0.0-*"",

  ""frameworks"": {
    ""dnx451"": { }
  }
}
");

            var result = RunShade(@"
var VERSION='0.1'
var FULL_VERSION='0.1'
var AUTHORS='Paul Knopf'

use import=""Common""

#default
    dnvm-install
    dnvm-use
    dnu-restore restoreDir='testdirectory'
    dnu-build projectFile='testdirectory'
");

            Assert.True(result.Contains("Build succeeded."));
        }

        [Fact]
        public void Can_restore()
        {
            Directory.CreateDirectory(SandboxPath("testdirectory"));
            File.WriteAllText(SandboxPath("testdirectory", "project.json"), @"
{
  ""version"": ""1.0.0-*"",

  ""frameworks"": {
    ""dnx451"": { },
    ""dnxcore50"": { }
  }
}
");

            RunShade(@"
var VERSION='0.1'
var FULL_VERSION='0.1'
var AUTHORS='Paul Knopf'

use import=""Common""

#default
    dnvm-install
    dnvm-use
    dnu-restore restoreDir='testdirectory'");

            Assert.True(File.Exists(SandboxPath("testdirectory", "project.lock.json")));
        }
    }
}
