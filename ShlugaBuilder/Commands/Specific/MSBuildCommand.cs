﻿using System.Collections.Generic;
using Microsoft.Build.BuildEngine;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;

namespace ShlugaBuilder.Commands.Specific
{
    public class MSBuildCommand : ISCommand
    {
        private readonly string _projectFile;

        public MSBuildCommand(string[] args)
        {
            _projectFile = args[0];
        }

        public CommandResult Execute()
        {
            var globalProperties = new Dictionary<string, string>();

            var buildRequest = new BuildRequestData(_projectFile, globalProperties, null, new[] {"Build"}, null);

            var buildLogCache = new BuildLogCache();

            var parameters = new BuildParameters
            {
                Loggers =
                    new ILogger[]
                    {
                        new ConfigurableForwardingLogger
                        {
                            BuildEventRedirector = buildLogCache,
                            Verbosity = LoggerVerbosity.Quiet,
                        },
                        new ConsoleLogger()
                    }
            };

            BuildResult buildResult = BuildManager.DefaultBuildManager.Build(parameters, buildRequest);

            return new CommandResult(buildResult.OverallResult == BuildResultCode.Success, buildLogCache.Messages);
        }
    }

    public class BuildLogCache : IEventRedirector
    {
        private readonly List<string> _messages;

        public BuildLogCache()
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