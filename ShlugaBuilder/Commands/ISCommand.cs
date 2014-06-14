using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShlugaBuilder.Commands
{
    public interface ISCommand
    {
        CommandResult Execute();
    }
}
