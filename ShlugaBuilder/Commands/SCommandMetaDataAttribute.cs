using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShlugaBuilder.Commands
{
    public class SCommandMetaDataAttribute : Attribute
    {
        public SCommandMetaDataAttribute(string name, string[] mandatoryParameters)
        {
            Name = name.ToLower();
            MandatoryParameters = mandatoryParameters;
        }

        public string Name { get; set; }

        public string[] MandatoryParameters { get; set; }

        public string[] OptionalParameters { get; set; }

        public string Help { get; set; }
    }
}
