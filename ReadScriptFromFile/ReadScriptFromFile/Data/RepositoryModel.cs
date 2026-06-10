using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadScriptFromFile.Data
{
    public sealed class Model
    {
        public string ScriptFile { get; set; }
        public List<Parameter> Parameters { get; set; }
    }

    public class Parameter
    {
        public string ParameterName { get; set; }
        public object Value { get; set; }
    }
}
