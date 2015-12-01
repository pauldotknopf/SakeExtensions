using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SakeExtensions.Tests
{
    public class DnxTests : SandboxBase
    {
        [Fact]
        public void Can_run_tests()
        {
            Directory.CreateDirectory(SandboxPath("test"));

            File.WriteAllText(SandboxPath("test", "project.json"), @"
{
  ""version"": ""1.0.0-*"",

  ""dependencies"": {
    ""xunit"": ""2.1.0-*"",
    ""xunit.assert"": ""2.1.0-*"",
    ""xunit.runner.dnx"": ""2.1.0-*""
  },

  ""commands"": {
    ""test"": ""xunit.runner.dnx""
  },

  ""frameworks"": {
    ""dnx451"": { }
  }
}
");

            File.WriteAllText(SandboxPath("test", "Test.cs"), @"
using Xunit;

public class Test
{
    [Fact]
    public void Can_do_something()
    {
        Assert.True(true);
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
    dnu-restore restoreDir='${Path.Combine(Directory.GetCurrentDirectory(), ""test"")}'
    dnx-test dnxDir='test' projectFile='${Path.Combine(Directory.GetCurrentDirectory(), ""test"", ""project.json"")}'
");

            Assert.True(result.Contains("Total: 1, Errors: 0, Failed: 0, Skipped: 0"));
        }
    }
}
