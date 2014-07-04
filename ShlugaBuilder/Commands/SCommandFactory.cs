using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using ShlugaBuilder.Commands.Specific;

namespace ShlugaBuilder.Commands
{
    public class SCommandFactory
    {
        public Dictionary<string, Tuple<Type, SCommandMetaDataAttribute>> SCommandMapping;

        public SCommandFactory()
        {
            SCommandMapping = new Dictionary<string, Tuple<Type, SCommandMetaDataAttribute>>();

            IEnumerable<Type> sCommandTypes =
                Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsAssignableFrom(typeof (ISCommand)));

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

        public ISCommand CreateSCommand(string commandLine)
        {
            List<string> parts = Regex.Matches(commandLine, @"[\""].+?[\""]|[^ ]+")
                .Cast<Match>()
                .Select(m => m.Value)
                .ToList();

            string commandName = parts[0].ToLower();
            parts.RemoveAt(0);

            if (!SCommandMapping.ContainsKey(commandName))
                return new BatchCommand(commandName, parts);

            Tuple<Type, SCommandMetaDataAttribute> mapping = SCommandMapping[commandName];

            return Activator.CreateInstance(mapping.Item1, parts) as ISCommand;
        }
    }
}