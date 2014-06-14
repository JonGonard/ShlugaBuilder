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

        public bool Success { get; private set; }

        public List<string> Errors { get; private set; }
    }

    public static class CommandResultErrorHelper
    {
        public static string AggregateAppend(this IEnumerable<string> errors)
        {
            return errors.AggregateAppend(true);
        }

        public static string AggregateAppend(this IEnumerable<string> errors, bool newLine)
        {
            return
                errors.Aggregate(new StringBuilder(),
                    (builder, next) => newLine ? builder.AppendLine(next) : builder.AppendFormat(" {0}", next))
                    .ToString();
        }
    }
}