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
            RunShadeFile("Can_execute_command");
            Assert.True(SandboxFileExists("test.txt"));
        }
    }
}
