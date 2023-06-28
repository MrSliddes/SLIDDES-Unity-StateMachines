using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLIDDES.StateMachines
{
    public static class TypeExtentions
    {
        /// <summary>
        /// Get the type of an typeName
        /// </summary>
        /// <param name="typeName">The string name of the type</param>
        /// <returns>Type or null if type cannot be found</returns>
        public static Type GetType(string typeName)
        {
            // Try Type.GetType() first. This will work with types defined by the Mono runtime, in the same assembly as the caller, etc.
            Type type = Type.GetType(typeName);
            if(type != null) return type;

            // Search Unity assembly
            Assembly assembly = Assembly.Load("Assembly-CSharp");
            if(assembly != null) type = assembly.GetType(typeName);
            if(type != null) return type;

            // Try loading type from assembly name
            if(typeName.Contains("."))
            {
                // Get assembly name
                var assemblyName = typeName.Substring(0, typeName.LastIndexOf('.'));

                // Try to get type from assembly
                assembly = Assembly.Load(assemblyName);
                if(assembly != null) type = assembly.GetType(typeName);
                if(type != null) return type;
            }

            // Enumerate through loaded assemblys to find type
            assembly = Assembly.GetExecutingAssembly();
            AssemblyName[] referencedAssemblies = assembly.GetReferencedAssemblies();
            foreach(var assemblyName in referencedAssemblies)
            {
                assembly = Assembly.Load(assemblyName);
                if(assembly == null) continue;
                // Get type
                type = assembly.GetType(typeName);
                if(type != null) return type;                
            }

            // Can't find type
            return null;
        }
    }
}
