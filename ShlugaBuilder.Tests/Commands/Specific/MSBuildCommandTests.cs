using System;
using System.IO;
using NUnit.Framework;
using ShlugaBuilder.Commands;
using ShlugaBuilder.Commands.Specific;

namespace ShlugaBuilder.Tests.Commands.Specific
{
    [TestFixture]
    public class MSBuildCommandTests
    {
        private const string ResourcesPath = @"C:\TestResources\MSBuild\";

        private const string CompilingSolution = @"CompilingSolution\CompilingSolution.sln";
        private const string CompilingProject = @"CompilingSolution\CompilingProject\CompilingProject.csproj";

        private const string NonCompilingSolution = @"NonCompilingSolution\NonCompilingSolution.sln";

        private const string NonCompilingProject =
            @"NonCompilingSolution\NonCompilingProject\NonCompilingProject.csproj";

        [Test]
        [ExpectedException(typeof (ArgumentException))]
        public void CompileNonExistingProject_CommandFaild()
        {
            // ReSharper disable ObjectCreationAsStatement
            new MSBuildCommand(new[] {@"c:\Foo\Bar.sln"});
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void CompileOKProject_BuildSucceedNoErrors()
        {
            string projectPath = Path.Combine(ResourcesPath, CompilingProject);

            var command = new MSBuildCommand(new[] {projectPath});

            CommandResult result = command.Execute();

            Assert.IsTrue(result.Success, result.Errors.AggregateAppend());

            Assert.AreEqual(0, result.Errors.Count);
        }

        [Test]
        public void CompileOKSolution_BuildSucceedNoErrors()
        {
            string solutionPath = Path.Combine(ResourcesPath, CompilingSolution);

            var command = new MSBuildCommand(new[] {solutionPath});

            CommandResult result = command.Execute();

            Assert.IsTrue(result.Success, result.Errors.AggregateAppend());

            Assert.AreEqual(0, result.Errors.Count);
        }

        [Test]
        public void CompileWithCorrectProperties_BuildSucceedNoErrors()
        {
            string projectPath = Path.Combine(ResourcesPath, CompilingProject);

            var command = new MSBuildCommand(new[] {projectPath, "Configuration=Debug"});

            CommandResult result = command.Execute();

            Assert.IsTrue(result.Success, result.Errors.AggregateAppend());

            Assert.AreEqual(0, result.Errors.Count);
        }

        [Test]
        public void CompileWithIncorrectProperties_BuildFailsHasErrors()
        {
            string projectPath = Path.Combine(ResourcesPath, CompilingProject);

            var command = new MSBuildCommand(new[] {projectPath, "Configuration=Foo"});

            CommandResult result = command.Execute();

            Assert.IsFalse(result.Success, result.Errors.AggregateAppend());

            Assert.AreNotEqual(0, result.Errors.Count);
        }

        [Test]
        public void CompileWrongProject_BuildFailsHasErrors()
        {
            string projectPath = Path.Combine(ResourcesPath, NonCompilingProject);

            var command = new MSBuildCommand(new[] {projectPath});

            CommandResult result = command.Execute();

            Assert.IsFalse(result.Success, result.Errors.AggregateAppend());

            Assert.AreNotEqual(0, result.Errors.Count);
        }

        [Test]
        public void CompileWrongSolution_BuildFailsHasErrors()
        {
            string solutionPath = Path.Combine(ResourcesPath, NonCompilingSolution);

            var command = new MSBuildCommand(new[] {solutionPath});

            CommandResult result = command.Execute();

            Assert.IsFalse(result.Success, result.Errors.AggregateAppend());

            Assert.AreNotEqual(0, result.Errors.Count);
        }

        [Test]
        [ExpectedException(typeof (ArgumentException))]
        public void CreateCommandWithWrongArgs_ThrowsException()
        {
// ReSharper disable ObjectCreationAsStatement
            new MSBuildCommand(new[] {"Foo", "Bar"});
// ReSharper restore ObjectCreationAsStatement
        }
    }
}