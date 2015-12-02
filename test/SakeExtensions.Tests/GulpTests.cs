using System.IO;
using Xunit;

namespace SakeExtensions.Tests
{
    public class GulpTests : SandboxBase
    {
        [Fact]
        public void Can_install_gulp()
        {
            File.WriteAllText(SandboxPath("gulpfile.js"), @"
var gulp = require('gulp');

gulp.task('ci', function() {
  console.log('test output from gulp.js');
});
");

            var result = RunShade(@"
var VERSION='0.1'
var FULL_VERSION='0.1'
var AUTHORS='Paul Knopf'

use import='Common'

#default
    gulp-install
    gulp gulpCommand='ci'
");

            Assert.True(result.Contains("test output from gulp.js"));
        }
    }
}
