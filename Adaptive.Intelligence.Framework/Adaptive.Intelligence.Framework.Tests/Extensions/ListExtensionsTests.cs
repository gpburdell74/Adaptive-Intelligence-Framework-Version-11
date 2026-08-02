using Adaptive.Intelligence.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adaptive.Intelligence.Framework.Tests.Extensions
{
    /// <summary>
    /// Gets the definition for ListExtensionsTests.
    /// </summary>
    public class ListExtensionsTests
    {
        [Fact]
        /// <summary>
        /// Gets the definition for ListExtensions_IsNullOrEmpty_Returns_True_When_Null_Or_Empty.
        /// </summary>
        public void ListExtensions_IsNullOrEmpty_Returns_True_When_Null_Or_Empty()
        {
            Assert.True(ListExtensions.IsNullOrEmpty(null));
            var list = new List<int>();
            Assert.True(ListExtensions.IsNullOrEmpty(list));
        }
        [Fact]
        /// <summary>
        /// Gets the definition for ListExtensions_IsNullOrEmpty_Returns_False_When_Not_Null_Or_Empty.
        /// </summary>
        public void ListExtensions_IsNullOrEmpty_Returns_False_When_Not_Null_Or_Empty()
        {
            var list = new List<int> { 1, 2, 3 };
            Assert.False(ListExtensions.IsNullOrEmpty(list));
        }

    }
}