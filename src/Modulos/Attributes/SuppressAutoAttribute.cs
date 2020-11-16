using System;

// ReSharper disable ClassNeverInstantiated.Global

namespace Modulos
{
    /// <summary>
    /// If defined, it blocks any automatic operation developed by modulos.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
    public sealed class SuppressAutoAttribute : Attribute
    {
    }
}