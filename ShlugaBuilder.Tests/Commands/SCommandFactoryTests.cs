using System;
using NUnit.Framework;
using ShlugaBuilder.Commands;
using ShlugaBuilder.Commands.Specific;

namespace ShlugaBuilder.Tests.Commands
{
    [TestFixture]
    public class SCommandFactoryTests
    {
        [SetUp]
        public void Setup()
        {
            _testable = new SCommandFactory();
        }

        private SCommandFactory _testable;

        [Test]
        public void BuildWithComplexArguments_CreateProperCommand()
        {
            const string path = @"C:\TestResources\File With Spaces.txt";
            ISCommand command = _testable.CreateSCommand("Build \"" + path + "\"");

            Assert.AreEqual(typeof(MSBuildCommand), command.GetType());
            Assert.AreEqual(path, ((MSBuildCommand)command).ProjectFile);
        }

        [Test]
        public void BuildWithSimpleArguments_CreateProperCommand()
        {
            const string path = @"C:\TestResources\File.txt";
            ISCommand command = _testable.CreateSCommand("Build " + path);

            Assert.AreEqual(typeof(MSBuildCommand), command.GetType());
            Assert.AreEqual(path, ((MSBuildCommand)command).ProjectFile);
        }

        [Test]
        [ExpectedException]
        public void BuildWithWrongArguments_ThrowsException()
        {
            const string path = @"C:\TestResources\NonExistingFile.txt";
            _testable.CreateSCommand("Build " + path);
        }

        [Test]
        public void Hostname_CreateBatchCommand()
        {
            ISCommand command = _testable.CreateSCommand("hostname");

            Assert.AreEqual(typeof(BatchCommand), command.GetType());
        }
    }
}