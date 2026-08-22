using Adaptive.Intelligence.Abstractions;
using Adaptive.Intelligence.Constants;
using Adaptive.Intelligence.Extensions;
using System.Text;

namespace Adaptive.Intelligence.Utility
{
    /// <summary>
    /// Provides a builder utility object for concatenating person names.
    /// </summary>
    /// <seealso cref="DisposableObjectBase" />
    public sealed class PersonNameBuilder : DisposableObjectBase
    {
        #region Private Member Declarations        
        /// <summary>
        /// The name prefix.
        /// </summary>
        private string? _prefix;
        /// <summary>
        /// The first name.
        /// </summary>
        private string? _firstName;
        /// <summary>
        /// The last name.
        /// </summary>
        private string? _lastName;
        /// <summary>
        /// The middle name.
        /// </summary>
        private string? _middleName;
        /// <summary>
        /// The name suffix.
        /// </summary>
        private string? _suffix;
        #endregion

        #region Constructor / Dispose Methods        
        /// <summary>
        /// Initializes a new instance of the <see cref="PersonNameBuilder"/> class.
        /// </summary>
        /// <remarks>
        /// This is the default constructor.
        /// </remarks>
        public PersonNameBuilder()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PersonNameBuilder"/> class.
        /// </summary>
        /// <param name="originalValue">
        /// A string containing the original value of the person's name to be parsed.
        /// </param>
        public PersonNameBuilder(string originalValue)
        {
            if (!string.IsNullOrEmpty(originalValue))
            {
                Parse(originalValue);
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PersonNameBuilder"/> class.
        /// </summary>
        /// <param name="prefix">
        /// A string containing the name prefix value, or <b>null</b>, or <see cref="string.Empty"/>.
        /// </param>
        /// <param name="firstName">
        /// A string containing the first name value, or <b>null</b>, or <see cref="string.Empty"/>.
        /// </param>
        /// <param name="middleName">
        /// A string containing the middle name value, or <b>null</b>, or <see cref="string.Empty"/>.
        /// </param>
        /// <param name="lastName">
        /// A string containing the last name value, or <b>null</b>, or <see cref="string.Empty"/>.
        /// </param>
        /// <param name="suffix">
        /// A string containing the name suffix value, or <b>null</b>, or <see cref="string.Empty"/>.
        /// </param>
        public PersonNameBuilder(string prefix, string firstName, string middleName, string lastName, string suffix)
        {
            _prefix = prefix;
            _firstName = firstName;
            _middleName = middleName;
            _lastName = lastName;
            _suffix = suffix;
        }
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><b>true</b> to release both managed and unmanaged resources;
        /// <b>false</b> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            _firstName = null;
            _lastName = null;
            _middleName = null;
            _prefix = null;
            _suffix = null;
            base.Dispose(disposing);
        }
        #endregion

        #region Public Properties        
        /// <summary>
        /// Gets or sets the first name value.
        /// </summary>
        /// <value>
        /// A string containing the first name value, or <b>null</b>.
        /// </value>
        public string? FirstName
        {
            get => _firstName;
            set => _firstName = value;
        }
        /// <summary>
        /// Gets a value indicating whether the builder instance has empty data.
        /// </summary>
        /// <value>
        ///   <c>true</c> if all properties are <b>null</b> or empty; otherwise, <c>false</c>.
        /// </value>
        public bool IsEmpty =>
             string.IsNullOrEmpty(_firstName) &&
             string.IsNullOrEmpty(_middleName) &&
             string.IsNullOrEmpty(_lastName) &&
             string.IsNullOrEmpty(_suffix) &&
             string.IsNullOrEmpty(_prefix);

        /// <summary>
        /// Gets or sets the first name value.
        /// </summary>
        /// <value>
        /// A string containing the first name value, or <b>null</b>.
        /// </value>
        public string? LastName
        {
            get => _lastName;
            set => _lastName = value;
        }
        /// <summary>
        /// Gets or sets the first name value.
        /// </summary>
        /// <value>
        /// A string containing the first name value, or <b>null</b>.
        /// </value>
        public string? MiddleName
        {
            get => _middleName;
            set => _middleName = value;
        }
        /// <summary>
        /// Gets or sets the first name value.
        /// </summary>
        /// <value>
        /// A string containing the first name value, or <b>null</b>.
        /// </value>
        public string? Prefix
        {
            get => _prefix;
            set => _prefix = value;
        }
        /// <summary>
        /// Gets or sets the first name value.
        /// </summary>
        /// <value>
        /// A string containing the first name value, or <b>null</b>.
        /// </value>
        public string? Suffix
        {
            get => _suffix;
            set => _suffix = value;
        }

        #endregion

        #region Public Methods / Functions        
        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder builder = new();

            if (!string.IsNullOrEmpty(_prefix))
            {
                if (builder.Length > 0)
                {
                    builder.AppendSpace();
                }

                _ = builder.Append(_prefix);
            }

            if (!string.IsNullOrEmpty(_firstName))
            {
                if (builder.Length > 0)
                {
                    builder.AppendSpace();
                }

                _ = builder.Append(_firstName);
            }

            if (!string.IsNullOrEmpty(_middleName))
            {
                if (builder.Length > 0)
                {
                    builder.AppendSpace();
                }

                _ = builder.Append(_middleName);
            }

            if (!string.IsNullOrEmpty(_lastName))
            {
                if (builder.Length > 0)
                {
                    builder.AppendSpace();
                }

                _ = builder.Append(_lastName);
            }

            if (!string.IsNullOrEmpty(_suffix))
            {
                if (builder.Length > 0)
                {
                    _ = builder.Append(CharacterConstants.CommaWithSpace);
                }

                _ = builder.Append(_suffix);
            }

            return builder.ToString();
        }
        #endregion

        #region Private Methods / Functions
        /// <summary>
        /// Parses the specified original value into a name value.
        /// </summary>
        /// <param name="originalValue">\
        /// A string containing the original value to be parsed.
        /// </param>
        private void Parse(string originalValue)
        {
            string[] elements = originalValue.Split(CharacterConstants.SpaceChar, StringSplitOptions.RemoveEmptyEntries);

            switch (elements.Length)
            {
                case 5:
                    _prefix = elements[0];
                    _firstName = elements[1];
                    _middleName = elements[2];
                    _lastName = elements[3];
                    _suffix = elements[4];
                    break;

                case 4:
                    _prefix = elements[0];
                    _firstName = elements[1];
                    _middleName = elements[2];
                    _lastName = elements[3];
                    break;

                case 3:
                    _firstName = elements[0];
                    _middleName = elements[1];
                    _lastName = elements[2];
                    break;

                case 2:
                    _firstName = elements[0];
                    _lastName = elements[1];
                    break;

                case 1:
                    _firstName = elements[0];
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}
