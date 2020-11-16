using System;

// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedMember.Global

namespace Modulos
{
    public interface ITypeExplorer
    {
        Type[] GetSearchableClasses(Func<Type, bool> filter = null);
        Type[] GetSearchableClasses<TAssignableTo>(Func<Type, bool> filter = null);
        Type[] GetSearchableInterfaces(Func<Type, bool> filter = null);
        Type[] GetSearchableTypes(Func<Type, bool> filter = null);
        Type[] GetSearchableInterfaces<TAssignableTo>(Func<Type, bool> filter = null);
        Type[] GetSearchableTypes<TAssignableTo>(Func<Type, bool> filter = null);
    }
}