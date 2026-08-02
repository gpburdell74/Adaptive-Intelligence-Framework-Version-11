using Adaptive.Intelligence.Abstractions.Logging;
using Adaptive.Intelligence.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Adaptive.Intelligence.Abstractions
{
    /// <summary>
    /// Provides a base implementation for business objects.
    /// </summary>
    /// <seealso cref="LoggableBase" />
    /// <seealso cref="PropertyAwareBase"/>
    /// <seealso cref="DisposableObjectBase"/>
    /// <seealso cref="IValidatableObject"/>
    public abstract class BusinessBase : LoggableBase, IValidatableObject
    {
        #region Public Events

        /// <summary>
        /// Occurs when a property's validation state changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyValidationChanged;

        #endregion

        #region Private Member Declarations

        /// <summary>
        /// The validation messages list.
        /// </summary>
        private ValidationMessageCollection? _validationMessages;

        #endregion

        #region Constructor / Dispose Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessBase"/> class.
        /// </summary>
        /// <remarks>
        /// This is the default constructor.
        /// </remarks>
        protected BusinessBase()
        {
            _validationMessages = [];
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><b>true</b> to release both managed and unmanaged resources;
        /// <b>false</b> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed && disposing)
            {
                _validationMessages?.Clear();
            }

            PropertyValidationChanged = null;
            _validationMessages = null;
            base.Dispose(disposing);
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets a value indicating whether the data on the business object is valid data.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </value>
        [JsonIgnore()]
        public virtual bool IsValid
        {
            get
            {
                Validate();
                return _validationMessages == null || _validationMessages.AreAllValid();
            }
        }

        /// <summary>
        /// Gets the reference to the list of validation messages.
        /// </summary>
        /// <value>
        /// The current <see cref="ValidationMessageCollection"/> instance resulting from the last
        /// <see cref="PerformValidation"/> call.
        /// </value>
        [JsonIgnore()]
        public ValidationMessageCollection? ValidationMessages => _validationMessages;

        #endregion

        #region Abstract / Protected Methods
        /// <summary>
        /// When overridden in a derived class, performs the operation to delete the current instance.
        /// </summary>
        /// <returns>
        /// <b>true</b> if the operation is successful; otherwise, returns <b>false</b> .
        /// </returns>
        public abstract bool PerformDelete();

        /// <summary>
        /// When overridden in a derived class, performs the operation to delete the current instance.
        /// </summary>
        /// <returns>
        /// <b>true</b> if the operation is successful; otherwise, returns <b>false</b> .
        /// </returns>
        public abstract Task<bool> PerformDeleteAsync();

        /// <summary>
        /// When overridden in a derived class, perform the process of loading the content of the instance.
        /// </summary>
        /// <typeparam name="TId">
        /// The data type of the parameter used as an identity value to load the instance.
        /// </typeparam>
        /// <param name="id">
        /// The <typeparamref name="TId"/> data content used to identify the content to be loaded.
        /// </param>
        /// <typeparam name="TResult">
        /// The data type of the return value.
        /// </typeparam>
        /// <returns>
        /// <b>true</b> if the load operation is successful; otherwise, returns <b>false</b>.
        /// </returns>
        protected abstract TResult PerformLoad<TId, TResult>(TId? id);

        /// <summary>
        /// When overridden in a derived class, perform the process of loading the content of the instance.
        /// </summary>
        /// <typeparam name="TId">
        /// The data type of the parameter used as an identity value to load the instance.
        /// </typeparam>
        /// <param name="id">
        /// The <typeparamref name="TId"/> data content used to identify the content to be loaded.
        /// </param>
        /// <typeparam name="TResult">
        /// The data type of the return value.
        /// </typeparam>
        /// <returns>
        /// <b>true</b> if the load operation is successful; otherwise, returns <b>false</b>.
        /// </returns>
        protected abstract Task<TResult> PerformLoadAsync<TId, TResult>(TId? id);

        /// <summary>
        /// When overridden in a derived class, performs the operation to save / store the current instance.
        /// </summary>
        /// <returns>
        /// <b>true</b> if the operation is successful; otherwise, returns <b>false</b> .
        /// </returns>
        protected abstract bool PerformSave();

        /// <summary>
        /// When overridden in a derived class, performs the operation to save / store the current instance.
        /// </summary>
        /// <returns>
        /// <b>true</b> if the operation is successful; otherwise, returns <b>false</b> .
        /// </returns>
        protected abstract Task<bool> PerformSaveAsync();

        /// <summary>
        /// Performs the validation on the current instance.
        /// </summary>
        /// <returns>
        /// A <see cref="ValidationMessageCollection"/> containing the list of validation messages, or
        /// an empty list of all properties are valid.
        /// </returns>
        protected virtual ValidationMessageCollection PerformValidation()
        {
            return [];
        }

        /// <summary>
        /// Registers the child object's property events.
        /// </summary>
        /// <param name="instance">
        /// The <see cref="BusinessBase"/> instance whose events are to be consumed.
        /// </param>
        protected virtual void RegisterEvents(BusinessBase? instance)
        {
            if (instance != null)
            {
                instance.PropertyChanged += HandleChildObjectPropertyChanged;
                instance.PropertyValidationChanged += HandleChildObjectValidationPropertyChanged;
            }
        }

        /// <summary>
        /// Unregisters the child object's property events.
        /// </summary>
        /// <param name="instance">
        /// The <see cref="BusinessBase"/> instance whose events are to be released.
        /// </param>
        protected virtual void UnRegisterEvents(BusinessBase? instance)
        {
            if (instance != null)
            {
                instance.PropertyChanged -= HandleChildObjectPropertyChanged;
                instance.PropertyValidationChanged -= HandleChildObjectValidationPropertyChanged;
            }
        }

        #endregion

        #region Protected Event Methods

        /// <summary>
        /// Raises the <see cref="PropertyValidationChanged" /> event.
        /// </summary>
        /// <param name="propertyName">
        /// A string containing the name of the property whose validation state changed.
        /// </param>
        protected virtual void OnPropertyValidationChanged(string propertyName)
        {
            OnPropertyValidationChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raises the <see cref="PropertyValidationChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnPropertyValidationChanged(PropertyChangedEventArgs e)
        {
            PropertyValidationChanged?.Invoke(this, e);
        }

        #endregion

        #region Protected Event Handlers

        /// <summary>
        /// Handles the event when a property value on a registered child object changes.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void HandleChildObjectPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(RenderChildPropertyName(sender, e));
        }

        /// <summary>
        /// Handles the event when a property validation state on a registered child object changes.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void HandleChildObjectValidationPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            OnPropertyValidationChanged(RenderChildPropertyName(sender, e));
        }

        #endregion

        #region Public Methods / Functions

        /// <summary>
        /// Performs the operation to delete the current instance.
        /// </summary>
        /// <returns>
        /// <b>true</b> if the operation is successful; otherwise, returns <b>false</b> .
        /// </returns>
        public virtual bool Delete()
        {
            bool success;
            try
            {
                success = PerformDelete();
            }
            catch (Exception ex)
            {
                LogError(ex);
                success = false;
            }

            return success;

        }

        /// <summary>
        /// When overridden in a derived class, performs the operation to delete the current instance.
        /// </summary>
        /// <returns>
        /// <b>true</b> if the operation is successful; otherwise, returns <b>false</b> .
        /// </returns>
        public virtual async Task<bool> DeleteAsync()
        {
            bool success;
            try
            {
                success = await PerformDeleteAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                LogError(ex);
                success = false;
            }

            return success;

        }

        /// <summary>
        /// Loads the content of the instance.
        /// </summary>
        /// <typeparam name="TId">
        /// The data type of the parameter used as an identity value to load the instance.
        /// </typeparam>
        /// <param name="id">
        /// The <typeparamref name="TId"/> data content used to identify the content to be loaded.
        /// </param>
        /// <returns>
        /// <b>true</b> if the load operation is successful; otherwise, returns <b>false</b>.
        /// </returns>
        public virtual bool Load<TId>(TId? id)
        {
            bool success;
            try
            {
                success = PerformLoad<TId, bool>(id);
            }
            catch (Exception ex)
            {
                LogError(ex);
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Loads the content of the instance.
        /// </summary>
        /// <typeparam name="TId">
        /// The data type of the parameter used as an identity value to load the instance.
        /// </typeparam>
        /// <param name="id">
        /// The <typeparamref name="TId"/> data content used to identify the content to be loaded.
        /// </param>
        /// <returns>
        /// <b>true</b> if the load operation is successful; otherwise, returns <b>false</b>.
        /// </returns>
        public virtual async Task<bool> LoadAsync<TId>(TId? id)
        {
            bool success;
            try
            {
                success = await PerformLoadAsync<TId, bool>(id).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                LogError(ex);
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Saves the current instance.
        /// </summary>
        /// <returns>
        /// <b>true</b> if the operation is successful; otherwise, returns <b>false</b> .
        /// </returns>
        public virtual bool Save()
        {
            bool success;
            try
            {
                success = PerformSave();
            }
            catch (Exception ex)
            {
                LogError(ex);
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Saves the current instance.
        /// </summary>
        /// <returns>
        /// <b>true</b> if the operation is successful; otherwise, returns <b>false</b> .
        /// </returns>
        public virtual async Task<bool> SaveAsync()
        {
            bool success;
            try
            {
                success = await PerformSaveAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                LogError(ex);
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Validates the content of the business object.
        /// </summary>
        /// <remarks>
        /// Invoking this method will remove any previous validation messages and 
        /// invoke the <see cref="PerformValidation"/> method to retrieve a new list
        /// of validation messages.
        /// </remarks>
        public void Validate()
        {
            // Remove the old messages.
            _validationMessages?.Clear();
            _validationMessages = null;

            // Try to validate.  If an exception occurs, consider the instance invalid.
            try
            {
                _validationMessages = PerformValidation();
            }
            catch (Exception ex)
            {
                _validationMessages =
                [
                    new ValidationMessage
                    {
                        IsValid = false,
                        Level = ValidationLevel.Error,
                        ErrorMessage = $"The validation process failed: {ex.Message}",
                        Tag = ex
                    },
                ];
            }
        }

        /// <summary>
        /// Determines whether the specified object is valid.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>
        /// A collection that holds failed-validation information.
        /// </returns>
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            _validationMessages ??= [];
            Validate();
            return _validationMessages;
        }

        #endregion

        #region Private Methods / Functions

        /// <summary>
        /// Renders the child object's property name.
        /// </summary>
        /// <param name="sender">The sender of the original event.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        /// <returns>
        /// A string containing the qualified property name.
        /// </returns>
        private static string RenderChildPropertyName(object? sender, PropertyChangedEventArgs e)
        {
            return sender != null ? $"{sender.GetType().Name}.{e.PropertyName}" : e.PropertyName ?? string.Empty;
        }

        #endregion
    }
}