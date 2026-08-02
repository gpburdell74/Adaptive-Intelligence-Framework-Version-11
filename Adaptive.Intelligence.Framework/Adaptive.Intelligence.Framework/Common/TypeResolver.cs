using System.Collections.Concurrent;
using System.Reflection;

namespace Adaptive.Intelligence.Common
{
    /// <summary>
    /// Provides a class for auto-determining which concrete types to instantiate from a specified
    /// interface.
    /// </summary>
    public static class TypeResolver
    {
        #region Private Static Declarations        
        /// <summary>
        /// The type map.
        /// </summary>
        private static readonly ConcurrentDictionary<Type, Type> _typeMap = new();
        #endregion

        #region Public Static Methods / Functions        
        /// <summary>
        /// Attempts to determine which concrete class to instantiate from the provided interface.
        /// </summary>
        /// <param name="interfaceType">
        /// A <see cref="Type"/> indicating the interface data type.
        /// </param>
        /// <returns>
        /// If successful, returns the first <see cref="Type"/> in the same assembly that matches
        /// the specified interface; otherwise, returns <b>null</b>.
        /// </returns>
        public static Type? ResolveTypeForInterface(Type? interfaceType)
        {
            Type? typeToReturn = null;

            if (interfaceType != null && interfaceType.IsInterface)
            {
                // Read the cached instance, if present.
                if (_typeMap.TryGetValue(interfaceType, out Type? value))
                {
                    typeToReturn = value;
                }
                else
                // Otherwise, attempt to find it.
                {
                    typeToReturn = PerformTypeSearch(interfaceType);
                }
            }

            return typeToReturn;
        }
        #endregion

        #region Private Static Methods / Functions        
        /// <summary>
        /// Performs the type search.
        /// </summary>
        /// <param name="interfaceType">
        /// A <see cref="Type"/> indicating the interface data type.
        /// </param>
        /// <returns>
        /// If successful, returns the first <see cref="Type"/> in the same assembly that matches
        /// the specified interface; otherwise, returns <b>null</b>.
        /// </returns>
        private static Type? PerformTypeSearch(Type interfaceType)
        {
            Type? concreteType = null;

            // Load the assembly into memory that the interface definition belongs to.
            Assembly? assembly = SafeLoadAssembly(interfaceType);
            if (assembly != null)
            {
                // Load the list of types in that assembly.
                List<Type>? typeList = SafeLoadAssemblyTypes(assembly);
                if (typeList != null)
                {
                    // Find the first concrete type that matches the interface that is 
                    // not another interface or an abstract class.
                    concreteType = FindConcreteTypeForInterface(typeList, interfaceType);
                    typeList.Clear();

                    // Store this so we don't have to search later.
                    if (concreteType != null)
                    {
                        _typeMap.TryAdd(interfaceType, concreteType);
                    }
                }
            }

            return concreteType;
        }
        /// <summary>
        /// Attempts to load the assembly that contains the specified data type.
        /// </summary>
        /// <param name="dataType">
        /// A <see cref="Type"/> indicating the interface data type.
        /// </param>
        /// <returns>
        /// If successful, returns the <see cref="Assembly"/> instance.
        /// </returns>
        private static Assembly? SafeLoadAssembly(Type dataType)
        {
            Assembly? assembly = null;

            try
            {
                assembly = Assembly.GetAssembly(dataType);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.Message);
            }

            return assembly;
        }
        /// <summary>
        /// Attempts to load the data types from the specified assembly.
        /// </summary>
        /// <param name="assembly">
        /// The <see cref="Assembly"/> instance from which the types are to be read.
        /// </param>
        /// <returns>
        /// If successful, returns a <see cref="List{T}"/> of <see cref="Type"/> instances to be used
        /// as type candidates; otherwise, returns <b>null</b>.
        /// </returns>
        private static List<Type>? SafeLoadAssemblyTypes(Assembly assembly)
        {
            List<Type>? typeList = null;

            try
            {
                Type[] assemblyTypes = assembly.GetTypes();
                typeList = [.. assemblyTypes];
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.Message);
            }

            return typeList;
        }
        /// <summary>
        /// Attempts to finds the concrete type for the specified interface type.
        /// </summary>
        /// <param name="sourceTypeList">
        /// A <see cref="List{T}"/> of <see cref="Type"/> instances to be used as type candidates.
        /// </param>
        /// <param name="interfaceType">
        /// The <see cref="Type"/> indicating the interface whose implementation is being searched for.
        /// </param>
        /// <returns>
        /// If successful, returns the first <see cref="Type"/> in the same assembly that matches
        /// the specified interface; otherwise, returns <b>null</b>.
        /// </returns>
        private static Type? FindConcreteTypeForInterface(List<Type>? sourceTypeList, Type? interfaceType)
        {
            Type? concreteType = null;

            if (sourceTypeList != null && sourceTypeList.Count > 0 && interfaceType != null)
            {
                // Iterate over the list until the first item is found.
                var length = sourceTypeList.Count;
                var index = 0;
                do
                {
                    Type candidateType = sourceTypeList[index++];
                    concreteType = DetermineTypeMatch(interfaceType, candidateType);
                } while ((index < length) && (concreteType == null));
            }

            return concreteType;
        }
        /// <summary>
        /// Determines if the specified candidate type matches the interface type.
        /// </summary>
        /// <param name="interfaceType">
        /// The <see cref="Type"/> indicating the interface whose implementation is being searched for.
        /// </param>
        /// <param name="candidateType">
        /// The <see cref="Type"/> to be checked to see if it is an implementation of the specified interface.
        /// </param>
        /// <returns>
        /// The <see cref="Type"/> reference to the <i>candidateType</i> if the match is successful;
        /// otherwise, returns <b>null</b>.
        /// </returns>
        private static Type? DetermineTypeMatch(Type? interfaceType, Type? candidateType)
        {
            Type? matchedType = null;

            if (interfaceType != null && candidateType != null)
            {
                // Ensure the item is a class, not an interface, not abstract, and implements
                // the specified interface
                if (candidateType.IsClass &&
                    !candidateType.IsInterface &&
                    !candidateType.IsAbstract &&
                    interfaceType.IsAssignableFrom(candidateType))
                {
                    matchedType = candidateType;
                }
            }

            return matchedType;
        }
        #endregion

    }
}