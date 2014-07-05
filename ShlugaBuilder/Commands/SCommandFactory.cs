using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework.XamlTypes;
using ShlugaBuilder.Commands.Specific;

namespace ShlugaBuilder.Commands
{
    public class SCommandFactory
    {
        public SCommandFactory()
        {
            SCommandMapping = new Dictionary<string, Tuple<Type, SCommandMetaDataAttribute>>();

            Type commandInterface = typeof (ISCommand);

            IEnumerable<Type> sCommandTypes =
                AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(x => x.GetTypes())
                    .Where(commandInterface.IsAssignableFrom);


            foreach (Type sCommandType in sCommandTypes)
            {
                var commandMeta = sCommandType.GetCustomAttribute<SCommandMetaDataAttribute>();

                if (commandMeta != null)
                {
                    SCommandMapping.Add(commandMeta.Name,
                        new Tuple<Type, SCommandMetaDataAttribute>(sCommandType, commandMeta));
                }
            }
        }

        public Dictionary<string, Tuple<Type, SCommandMetaDataAttribute>> SCommandMapping { get; private set; }

        public ISCommand CreateSCommand(string commandLine)
        {
            List<string> parts = Regex.Matches(commandLine, @"[\""].+?[\""]|[^ ]+")
                .Cast<Match>()
                .Select(m => m.Value)
                .ToList();

            string commandName = parts[0].ToLower();
            parts.RemoveAt(0);
            string[] partsArray = parts.ToArray();

            if (!SCommandMapping.ContainsKey(commandName))
                return new BatchCommand(commandName, partsArray);

            Tuple<Type, SCommandMetaDataAttribute> mapping = SCommandMapping[commandName];

            return Activator.CreateInstance(mapping.Item1, (object)partsArray) as ISCommand;
        }
    }
}