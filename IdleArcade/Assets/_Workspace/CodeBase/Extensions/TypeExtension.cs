using System;

namespace _Workspace.CodeBase.Extensions
{
    public static class TypeExtension
    {
        public static bool Implements<TInterface>(this Type type) 
            => typeof(TInterface).IsAssignableFrom(type);
    }
}