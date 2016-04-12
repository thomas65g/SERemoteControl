using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Globalization;
using System.Diagnostics.CodeAnalysis;

namespace MT.Platform.Common
{
    /// <summary>
    /// This is a helper class for validation.
    /// </summary>
    public sealed class Validate
    {
        private Validate() { }

        /// <summary>
        /// Determines whether the specified <paramref name="o"/> is null.
        /// In case of null the specified exception will be thrown.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="formatedMessage">The formated message.</param>
        /// <param name="args">The args.</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "o"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "formated"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter"), SuppressMessage("Microsoft.DesignRules", "CA1004", Justification = "For a better readability.")]
        public static void IsNotNull<TExceptionType>(object o, string formatedMessage, params object[] args) where TExceptionType : System.Exception, new()
        {
            IsFalse<TExceptionType>(o == null, formatedMessage, args);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="condition"/> is true.
        /// In case of the condition is false the specified condition is thrown.
        /// </summary>
        /// <param name="condition">if set to <c>true</c> [condition].</param>
        /// <param name="formatedMessage">The formated message.</param>
        /// <param name="args">The args.</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "formated"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter"), SuppressMessage("Microsoft.DesignRules", "CA1004", Justification = "For a better readability.")]
        public static void IsTrue<TExceptionType>(bool condition, string formatedMessage, params object[] args) where TExceptionType : System.Exception, new()
        {
            IsFalse < TExceptionType >(!condition, formatedMessage, args);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="condition"/> is false.
        /// In case of the condition is true the specified exception is thrown.
        /// </summary>
        /// <param name="condition">if set to <c>true</c> [condition].</param>
        /// <param name="formatedMessage">The formated message.</param>
        /// <param name="args">The args.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter"), SuppressMessage("Microsoft.DesignRules", "CA1004", Justification = "For a better readability.")]
        public static void IsFalse<TExceptionType>(bool condition, string formatedMessage, params object[] args) where TExceptionType : System.Exception, new()
        {
            if (condition)
            {
                TExceptionType exception = new TExceptionType();
                SetProperty(exception, "_message", string.Format(CultureInfo.CurrentCulture, formatedMessage, args), false);
                throw exception;
            }
        }

        /// <summary>
        /// Sets a field / property of an object.
        /// </summary>
        /// <param name="obj">The object of which the field / property should be set</param>
        /// <param name="propertyName">The name of the field / property</param>
        /// <param name="propertyValue">The value of the property</param>
        /// <param name="ignoreCase">True if it shall operate case insensitive</param>
        static private void SetProperty(object obj, string propertyName, string propertyValue, bool ignoreCase)
        {
            BindingFlags bindings = BindingFlags.Public | BindingFlags.Instance;
            if (ignoreCase)
            {
                bindings |= BindingFlags.IgnoreCase;
            }
            Type type = obj.GetType();

            // look for a setable property
            PropertyInfo property = type.GetProperty(propertyName, bindings);
            if (property != null && property.CanWrite)
            {
                property.SetValue(obj, propertyValue, null);
                return;
            }
            // look for a public field
            FieldInfo field = type.GetField(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
            {
                field.SetValue(obj, propertyValue);
                return;
            }
        }
    }
}
