using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Modulos
{
    public class AssemblyExplorer : IAssemblyExplorer
    {
        public Assembly[] Assemblies { get; }

        public AssemblyExplorer(IEnumerable<Assembly> assemblies)
        {
            Assemblies = assemblies.ToArray();
        }
    }
}