using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShlugaBuilder.Commands
{
    public class CommandResult
    {
        public CommandResult(bool success, List<string> errors)
        {
            Success = success;
            Errors = errors;
        }

        public CommandResult(bool success, string error) : this(success, new List<string> {error})
        {}

        public bool Success { get; private set; }

        public List<string> Errors { get; private set; }
    }

    public static class StringsHelper
    {
        public static string AggregateAppend(this IEnumerable<string> strings)
        {
            return strings.AggregateAppend(true);
        }

        public static string AggregateAppend(this IEnumerable<string> strings, bool newLine)
        {
            return
                strings.Aggregate(new StringBuilder(),
                    (builder, next) => newLine ? builder.AppendLine(next) : builder.AppendFormat("{0} ", next))
                    .ToString();
        }
    }
}