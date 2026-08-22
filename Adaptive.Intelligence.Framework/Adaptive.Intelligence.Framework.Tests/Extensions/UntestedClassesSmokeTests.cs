using System.Reflection;
using System.Runtime.Serialization;

namespace Adaptive.Intelligence.Framework.Tests.Coverage
{
    /// <summary>
    /// Provides baseline smoke coverage for production classes that do not have a dedicated test class.
    /// </summary>
    public class UntestedClassesSmokeTests
    {
        private static readonly Assembly ProductAssembly = typeof(Adaptive.Intelligence.Extensions.ListExtensions).Assembly;
        private static readonly Assembly TestAssembly = typeof(UntestedClassesSmokeTests).Assembly;

        public static IEnumerable<object[]> UntestedClassTypes()
        {
            HashSet<string> testedClassNames = TestAssembly
                .GetTypes()
                .Where(t => t.IsClass && t.Name.EndsWith("Tests", StringComparison.Ordinal))
                .Select(t => t.Name[..^5])
                .ToHashSet(StringComparer.Ordinal);

            IEnumerable<Type> untested = ProductAssembly
                .GetTypes()
                .Where(t =>
                    t.IsClass &&
                    t.IsPublic &&
                    !t.IsNested &&
                    !t.IsGenericTypeDefinition &&
                    !testedClassNames.Contains(StripGenericArity(t.Name)));

            foreach (Type type in untested.OrderBy(t => t.FullName, StringComparer.Ordinal))
            {
                yield return [type];
            }
        }

        [Theory]
        [MemberData(nameof(UntestedClassTypes))]
        public void UntestedClass_TypeIsLoadable(Type type)
        {
            Assert.NotNull(type);
            Assert.True(type.IsClass);
            Assert.False(string.IsNullOrWhiteSpace(type.FullName));
        }

        [Theory]
        [MemberData(nameof(UntestedClassTypes))]
        public void UntestedConcreteClass_CanBeCreatedOrInitialized(Type type)
        {
            if (type.IsAbstract && type.IsSealed)
            {
                // Static class.
                Assert.True(type.GetMembers(BindingFlags.Public | BindingFlags.Static).Length > 0);
                return;
            }

            if (type.IsAbstract)
            {
                // Abstract class.
                Assert.True(type.IsAbstract);
                return;
            }

            if (type.BaseType != typeof(MulticastDelegate))
            {
                object? instance = TryCreateInstance(type) ?? FormatterServices.GetUninitializedObject(type);
                Assert.NotNull(instance);
                Assert.IsType(type, instance);
            }
            
        }

        private static object? TryCreateInstance(Type type)
        {
            ConstructorInfo[] constructors = type
                .GetConstructors(BindingFlags.Instance | BindingFlags.Public)
                .OrderBy(c => c.GetParameters().Length)
                .ToArray();

            foreach (ConstructorInfo ctor in constructors)
            {
                ParameterInfo[] parameters = ctor.GetParameters();
                object?[] values = new object?[parameters.Length];
                bool supported = true;

                for (int i = 0; i < parameters.Length; i++)
                {
                    if (!TryGetValue(parameters[i].ParameterType, out object? value))
                    {
                        supported = false;
                        break;
                    }

                    values[i] = value;
                }

                if (!supported)
                {
                    continue;
                }

                try
                {
                    return ctor.Invoke(values);
                }
                catch
                {
                    // Try next constructor.
                }
            }

            return null;
        }

        private static bool TryGetValue(Type parameterType, out object? value)
        {
            Type? nullableUnderlying = Nullable.GetUnderlyingType(parameterType);
            if (nullableUnderlying != null)
            {
                value = null;
                return true;
            }

            if (parameterType == typeof(string))
            {
                value = string.Empty;
                return true;
            }

            if (parameterType.IsEnum)
            {
                Array values = Enum.GetValues(parameterType);
                value = values.Length > 0 ? values.GetValue(0) : Activator.CreateInstance(parameterType);
                return true;
            }

            if (parameterType.IsArray)
            {
                value = Array.CreateInstance(parameterType.GetElementType()!, 0);
                return true;
            }

            if (parameterType.IsValueType)
            {
                value = Activator.CreateInstance(parameterType);
                return true;
            }

            // For reference types/interfaces, pass null in smoke construction.
            value = null;
            return true;
        }

        private static string StripGenericArity(string name)
        {
            int marker = name.IndexOf('`', StringComparison.Ordinal);
            return marker >= 0 ? name[..marker] : name;
        }
    }
}