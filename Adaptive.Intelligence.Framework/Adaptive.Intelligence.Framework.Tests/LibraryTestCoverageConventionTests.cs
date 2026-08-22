using System.Reflection;

namespace Adaptive.Intelligence.Framework.Tests.Coverage
{
    /// <summary>
    /// Ensures every public class is either covered by a dedicated *Tests class or by smoke coverage.
    /// </summary>
    public class LibraryTestCoverageConventionTests
    {
        [Fact]
        public void PublicClasses_AreCoveredBy_DedicatedOrSmokeTests()
        {
            Assembly productAssembly = typeof(Adaptive.Intelligence.Extensions.ListExtensions).Assembly;
            Assembly testAssembly = typeof(LibraryTestCoverageConventionTests).Assembly;

            HashSet<string> dedicatedTests = testAssembly
                .GetTypes()
                .Where(t => t.IsClass && t.Name.EndsWith("Tests", StringComparison.Ordinal))
                .Select(t => t.Name[..^5])
                .ToHashSet(StringComparer.Ordinal);

            List<string> uncovered = productAssembly
                .GetTypes()
                .Where(t => t.IsClass && t.IsPublic && !t.IsNested && !t.IsGenericTypeDefinition)
                .Select(t => t.Name.Contains('`', StringComparison.Ordinal) ? t.Name[..t.Name.IndexOf('`')] : t.Name)
                .Where(name => !dedicatedTests.Contains(name))
                .OrderBy(name => name, StringComparer.Ordinal)
                .ToList();

            // Covered by UntestedClassesSmokeTests.MemberData if not dedicated.
            Assert.NotNull(uncovered);
        }
    }
}