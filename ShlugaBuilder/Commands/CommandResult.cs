using System.Collections.Generic;

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
}