using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Build.BuildEngine;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;

namespace ShlugaBuilder.Commands.Specific
{
    [SCommandMetaData("Build", new[] {"Project Path"})]
    public class MSBuildCommand : ISCommand
    {
        private readonly IDictionary<string, string> _globalProperties;
        internal readonly string ProjectFile;

        public MSBuildCommand(string[] args)
        {
            ProjectFile = args[0].Trim('"');

            if (!File.Exists(ProjectFile))
                throw new ArgumentException("Project file Doesn't exist", "args");

            _globalProperties = new Dictionary<string, string>(args.Length - 1);

            for (int i = 1; i < args.Length; i++)
            {
                string[] property = args[i].Split('=');

                if (property.Length != 2)
                    throw new ArgumentException("args isn't in correct format", "args");

                _globalProperties.Add(property[0], property[1]);
            }
        }

        public CommandResult Execute()
        {
            var buildRequest = new BuildRequestData(ProjectFile, _globalProperties, null, new[] {"Build"}, null);

            var buildLogEvents = new BuildLogEvents();

            var parameters = new BuildParameters
            {
                Loggers =
                    new ILogger[]
                    {
                        new ConfigurableForwardingLogger
                        {
                            BuildEventRedirector = buildLogEvents,
                            Verbosity = LoggerVerbosity.Quiet,
                        },
                        new ConsoleLogger()
                    }
            };

            BuildResult buildResult = BuildManager.DefaultBuildManager.Build(parameters, buildRequest);

            return new CommandResult(buildResult.OverallResult == BuildResultCode.Success, buildLogEvents.Messages);
        }
    }

    public class BuildLogEvents : IEventRedirector
    {
        private readonly List<string> _messages;

        public BuildLogEvents()
        {
            _messages = new List<string>();
        }

        public List<string> Messages
        {
            get { return _messages; }
        }

        public void ForwardEvent(BuildEventArgs buildEvent)
        {
            Messages.Add(buildEvent.Message);
        }
    }
}