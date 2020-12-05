using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Modulos.Errors
{
    internal class ExceptionTransformer : IExceptionTransformer
    {
        private readonly Dictionary<string, Type> _knownExceptions = new Dictionary<string, Type>();

        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public ExceptionTransformer(IAssemblyExplorer assemblyExplorer)
        {
            foreach (var assembly in assemblyExplorer.Assemblies)
            {
                foreach (var type in assembly.GetExportedTypes()
                    .Where(t => t != typeof(ModulosException)
                                && !t.IsAbstract
                                && t.IsClass
                                && typeof(ModulosException).IsAssignableFrom(t)))
                {

                    ModulosException modulosException;

                    try
                    {
                        modulosException = (ModulosException)Activator.CreateInstance(type);
                    }
                    catch
                    {
                        modulosException = (ModulosException)Activator.CreateInstance(type, string.Empty);
                    }

                    _knownExceptions.Add(modulosException.Code, type);
                }
            }
        }

        public bool ToModulosException(string code, string message, out ModulosException modulosException)
        {
            if (_knownExceptions.ContainsKey(code))
            {
                var exceptionType = _knownExceptions[code];
                modulosException = Activator.CreateInstance(exceptionType, message) as ModulosException;
                return true;
            }

            modulosException = new UnknownModulosException(message);
            return false;
        }

        public bool ToModulosException(Exception error, out ModulosException modulosException)
        {
            if (error is ModulosException exception)
            {
                modulosException = exception;
                return true;
            }

            modulosException = new UnknownModulosException(error.Message, error);

            return false;
        }

    }
}