using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ShlugaBuilder.Commands.Specific
{
    public class BatchCommand : ISCommand
    {
        private readonly List<string> _errors;
        private readonly ProcessStartInfo _processInfo;

        public BatchCommand(string commandName, params string[] args)
        {
            string arguments = args.AggregateAppend(false);

            _processInfo = new ProcessStartInfo(commandName, arguments)
            {
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
            };

            _errors = new List<string>();
        }

        public CommandResult Execute()
        {
            var process = new Process{StartInfo = _processInfo};

            process.OutputDataReceived += LogOutput;
            process.ErrorDataReceived += LogError;

            process.Start();
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();
            process.WaitForExit();

            return new CommandResult(process.ExitCode == 0 && _errors.Count == 0, _errors);
        }

        private void LogError(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Data))
                _errors.Add(e.Data);
        }

        private void LogOutput(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }
    }
}