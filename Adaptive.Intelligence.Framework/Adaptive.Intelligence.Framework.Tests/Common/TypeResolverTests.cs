using Adaptive.Intelligence.Common;
using System.Reflection;

namespace Adaptive.Intelligence.Framework.Tests.Common
{
    /// <summary>
    /// Gets the definition for ITestInterface.
    /// </summary>
    public interface ITestInterface { }
    /// <summary>
    /// Gets the definition for TestImplementation.
    /// </summary>
    public class TestImplementation : ITestInterface { }
    /// <summary>
    /// Gets the definition for AbstractTestImplementation.
    /// </summary>
    public abstract class AbstractTestImplementation : ITestInterface { }
    /// <summary>
    /// Gets the definition for IUnrelatedInterface.
    /// </summary>
    public interface IUnrelatedInterface { }

    /// <summary>
    /// Gets the definition for TypeResolverTests.
    /// </summary>
    public class TypeResolverTests
    {
        [Fact]
        /// <summary>
        /// Gets the definition for ResolveTypeForInterface_ReturnsNull_ForNullType.
        /// </summary>
        public void ResolveTypeForInterface_ReturnsNull_ForNullType()
        {
            var result = TypeResolver.ResolveTypeForInterface(null);
            Assert.Null(result);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for ResolveTypeForInterface_ReturnsNull_ForNonInterfaceType.
        /// </summary>
        public void ResolveTypeForInterface_ReturnsNull_ForNonInterfaceType()
        {
            var result = TypeResolver.ResolveTypeForInterface(typeof(TestImplementation));
            Assert.Null(result);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for ResolveTypeForInterface_FindsConcreteType.
        /// </summary>
        public void ResolveTypeForInterface_FindsConcreteType()
        {
            var result = TypeResolver.ResolveTypeForInterface(typeof(ITestInterface));
            Assert.Equal(typeof(TestImplementation), result);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for ResolveTypeForInterface_ReturnsNull_WhenNoImplementation.
        /// </summary>
        public void ResolveTypeForInterface_ReturnsNull_WhenNoImplementation()
        {
            var result = TypeResolver.ResolveTypeForInterface(typeof(IUnrelatedInterface));
            Assert.Null(result);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for DetermineTypeMatch_ReturnsNull_ForAbstractOrInterface.
        /// </summary>
        public void DetermineTypeMatch_ReturnsNull_ForAbstractOrInterface()
        {
            var method = typeof(TypeResolver).GetMethod("DetermineTypeMatch", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(method);

            // Abstract class should return null
            var result = method.Invoke(null, [typeof(ITestInterface), typeof(AbstractTestImplementation)]);
            Assert.Null(result);

            // Interface should return null
            result = method.Invoke(null, [typeof(ITestInterface), typeof(ITestInterface)]);
            Assert.Null(result);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for DetermineTypeMatch_ReturnsType_ForValidImplementation.
        /// </summary>
        public void DetermineTypeMatch_ReturnsType_ForValidImplementation()
        {
            var method = typeof(TypeResolver).GetMethod("DetermineTypeMatch", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(method);

            var result = method.Invoke(null, [typeof(ITestInterface), typeof(TestImplementation)]);
            Assert.Equal(typeof(TestImplementation), result);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for ResolveTypeForInterface_ReturnsCachedResult_OnSecondCall.
        /// </summary>
        public void ResolveTypeForInterface_ReturnsCachedResult_OnSecondCall()
        {
            Type? firstResult = TypeResolver.ResolveTypeForInterface(typeof(ITestInterface));
            Type? secondResult = TypeResolver.ResolveTypeForInterface(typeof(ITestInterface));

            Assert.NotNull(firstResult);
            Assert.Equal(firstResult, secondResult);
        }

    }
}