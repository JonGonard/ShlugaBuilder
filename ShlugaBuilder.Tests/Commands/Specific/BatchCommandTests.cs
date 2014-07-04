using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using NUnit.Framework;
using ShlugaBuilder.Commands;
using ShlugaBuilder.Commands.Specific;

namespace ShlugaBuilder.Tests.Commands.Specific
{
    public class BatchCommandTests
    {
        private StringBuilder _outputString;
        private StringWriter _outputWriter;
        private TextWriter _consoleWriter;

        [SetUp]
        public void Setup()
        {
            _outputString = new StringBuilder();
            _outputWriter = new StringWriter(_outputString);

            _consoleWriter = Console.Out;

            Console.SetOut(_outputWriter);
        }

        [TearDown]
        public void TearDown()
        {
            _outputWriter.Dispose();

            Console.SetOut(_consoleWriter);
        }

        [Test]
        public void HostName_RunsCorrectly()
        {
            var testable = new BatchCommand("hostname");

            CommandResult result = testable.Execute();

            Assert.IsTrue(result.Success);
            Assert.AreEqual(0, result.Errors.Count);
            Assert.AreEqual(Environment.MachineName, _outputString.ToString().Trim());
        }

        [Test]
        [ExpectedException(typeof (Win32Exception))]
        public void IncorrectCommandName_FailsToRun()
        {
            new BatchCommand("hostnam").Execute();
        }

        [Test]
        public void PingNotExistingHost_CommandFails()
        {
            var testable = new BatchCommand("ping", "IHopeToGodThisHostDoesntExist");

            CommandResult result = testable.Execute();

            Assert.IsFalse(result.Success);
        }
    }
}