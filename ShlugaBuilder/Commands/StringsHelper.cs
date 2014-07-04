using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShlugaBuilder.Commands
{
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