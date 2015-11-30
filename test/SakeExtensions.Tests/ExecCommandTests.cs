using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SakeExtensions.Tests
{
    public class ExecCommandTests : SandboxBase
    {
        [Fact]
        public void Can_execute_command()
        {
            RunShade(@"
var VERSION='0.1'
var FULL_VERSION='0.1'
var AUTHORS='Paul Knopf'

use import='Common'

#default
    exec-command commandline=""echo TEST > test.txt""
");
            Assert.True(SandboxFileExists("test.txt"));
        }
    }
}
