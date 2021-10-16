// ReSharper disable ClassNeverInstantiated.Global

namespace Modulos
{
    using System;

    /// <summary>
    /// Blocks any automatic operation developed by modulos.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
    public sealed class SuppressAutoAttribute : Attribute
    {
    }
}