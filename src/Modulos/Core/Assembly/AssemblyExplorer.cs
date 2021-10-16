namespace Modulos
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class AssemblyExplorer : IAssemblyExplorer
    {
        public AssemblyExplorer(IEnumerable<Assembly> assemblies)
        {
            Assemblies = assemblies.ToArray();
        }

        public Assembly[] Assemblies { get; }
    }
}