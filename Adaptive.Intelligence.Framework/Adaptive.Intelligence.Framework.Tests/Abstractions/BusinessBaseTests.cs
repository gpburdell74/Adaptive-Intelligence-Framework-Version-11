using Adaptive.Intelligence.Abstractions;
using Adaptive.Intelligence.Framework.Tests.Mocks;
using Adaptive.Intelligence.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Adaptive.Intelligence.Framework.Tests.Abstractions
{
    /// <summary>
    /// Provides the tests for the <see cref="BusinessBase"/> abstract class.
    /// </summary>
    public class BusinessBaseTests
    {
        /// <summary>
        /// Tests that a new instance starts valid and with no validation messages.
        /// </summary>
        [Fact]
        /// <summary>
        /// Gets the definition for Initial_State_Is_Valid_And_Contains_No_Validation_Messages.
        /// </summary>
        public void Initial_State_Is_Valid_And_Contains_No_Validation_Messages()
        {
            MockBusinessBase mock = new();

            Assert.True(mock.IsValid);
            Assert.NotNull(mock.ValidationMessages);
            Assert.Empty(mock.ValidationMessages);
            Assert.Equal(1, mock.PerformValidationCallCount);
        }

        /// <summary>
        /// Tests that <see cref="BusinessBase.IsValid"/> returns <see langword="false"/> when validation contains invalid entries.
        /// </summary>
        [Fact]
        /// <summary>
        /// Gets the definition for IsValid_Returns_False_When_Validation_Contains_Invalid_Message.
        /// </summary>
        public void IsValid_Returns_False_When_Validation_Contains_Invalid_Message()
        {
            MockBusinessBase mock = new()
            {
                ValidationToReturn = new ValidationMessageCollection
                {
                    new ValidationMessage
                    {
                        IsValid = false,
                        Level = ValidationLevel.Error,
                        ErrorMessage = "Invalid value"
                    }
                }
            };

            bool isValid = mock.IsValid;

            Assert.False(isValid);
            Assert.NotNull(mock.ValidationMessages);
            Assert.Single(mock.ValidationMessages);
            Assert.Equal(1, mock.PerformValidationCallCount);
        }

        /// <summary>
        /// Tests that <see cref="BusinessBase.Validate()"/> stores messages returned by validation logic.
        /// </summary>
        [Fact]
        /// <summary>
        /// Gets the definition for Validate_Populates_ValidationMessages_From_PerformValidation.
        /// </summary>
        public void Validate_Populates_ValidationMessages_From_PerformValidation()
        {
            MockBusinessBase mock = new()
            {
                ValidationToReturn = new ValidationMessageCollection
                {
                    new ValidationMessage("First", ValidationLevel.Warning, true),
                    new ValidationMessage("Second")
                }
            };

            mock.Validate();

            Assert.NotNull(mock.ValidationMessages);
            Assert.Equal(2, mock.ValidationMessages.Count);
            Assert.Equal("First", mock.ValidationMessages[0].ErrorMessage);
            Assert.Equal("Second", mock.ValidationMessages[1].ErrorMessage);
            Assert.Equal(1, mock.PerformValidationCallCount);
        }

        /// <summary>
        /// Tests that <see cref="BusinessBase.Validate()"/> converts validation exceptions into an error validation message.
        /// </summary>
        [Fact]
        /// <summary>
        /// Gets the definition for Validate_When_PerformValidation_Throws_Creates_Failure_Message.
        /// </summary>
        public void Validate_When_PerformValidation_Throws_Creates_Failure_Message()
        {
            MockBusinessBase mock = new()
            {
                ThrowOnValidation = true
            };

            mock.Validate();

            Assert.NotNull(mock.ValidationMessages);
            Assert.Single(mock.ValidationMessages);
            ValidationMessage message = mock.ValidationMessages[0];
            Assert.False(message.IsValid);
            Assert.Equal(ValidationLevel.Error, message.Level);
            Assert.Contains("The validation process failed", message.ErrorMessage, StringComparison.Ordinal);
            Assert.IsType<InvalidOperationException>(message.Tag);
        }

        /// <summary>
        /// Tests that <see cref="BusinessBase.Validate(ValidationContext)"/> returns validation results from internal messages.
        /// </summary>
        [Fact]
        /// <summary>
        /// Gets the definition for Validate_With_ValidationContext_Returns_ValidationResults.
        /// </summary>
        public void Validate_With_ValidationContext_Returns_ValidationResults()
        {
            MockBusinessBase mock = new()
            {
                ValidationToReturn = new ValidationMessageCollection
                {
                    new ValidationMessage("Error 1")
                }
            };

            IEnumerable<ValidationResult> results = mock.Validate(new ValidationContext(mock));
            List<ValidationResult> resultList = [.. results];

            Assert.Single(resultList);
            Assert.Equal("Error 1", resultList[0].ErrorMessage);
        }

        /// <summary>
        /// Tests that <see cref="BusinessBase.Delete"/> returns the result from <see cref="BusinessBase.PerformDelete"/>.
        /// </summary>
        [Fact]
        /// <summary>
        /// Gets the definition for Delete_Returns_PerformDelete_Result.
        /// </summary>
        public void Delete_Returns_PerformDelete_Result()
        {
            MockBusinessBase mock = new()
            {
                DeleteResult = false
            };

            bool success = mock.Delete();

            Assert.False(success);
        }

        /// <summary>
        /// Tests that <see cref="BusinessBase.Delete"/> returns <see langword="false"/> when delete execution throws.
        /// </summary>
        [Fact]
        /// <summary>
        /// Gets the definition for Delete_When_PerformDelete_Throws_Returns_False.
        /// </summary>
        public void Delete_When_PerformDelete_Throws_Returns_False()
        {
            MockBusinessBase mock = new()
            {
                ThrowOnDelete = true
            };

            bool success = mock.Delete();

            Assert.False(success);
        }

        /// <summary>
        /// Tests that <see cref="BusinessBase.DeleteAsync"/> returns the result from <see cref="BusinessBase.PerformDeleteAsync"/>.
        /// </summary>
        [Fact]
        /// <summary>
        /// Gets the definition for DeleteAsync_Returns_PerformDeleteAsync_Result.
        /// </summary>
        public async Task DeleteAsync_Returns_PerformDeleteAsync_Result()
        {
            MockBusinessBase mock = new()
            {
                DeleteAsyncResult = false
            };

            bool success = await mock.DeleteAsync();

            Assert.False(success);
        }

        /// <summary>
        /// Tests that <see cref="BusinessBase.DeleteAsync"/> returns <see langword="false"/> when delete execution throws.
        /// </summary>
        [Fact]
        /// <summary>
        /// Gets the definition for DeleteAsync_When_PerformDeleteAsync_Throws_Returns_False.
        /// </summary>
        public async Task DeleteAsync_When_PerformDeleteAsync_Throws_Returns_False()
        {
            MockBusinessBase mock = new()
            {
                ThrowOnDeleteAsync = true
            };

            bool success = await mock.DeleteAsync();

            Assert.False(success);
        }

        /// <summary>
        /// Tests that <see cref="BusinessBase.Load{TId}(TId)"/> returns the result from load execution.
        /// </summary>
        [Fact]
        /// <summary>
        /// Gets the definition for Load_Returns_PerformLoad_Result.
        /// </summary>
        public void Load_Returns_PerformLoad_Result()
        {
            MockBusinessBase mock = new()
            {
                LoadResult = true
            };

            bool success = mock.Load(10);

            Assert.True(success);
        }

        /// <summary>
        /// Tests that <see cref="BusinessBase.Load{TId}(TId)"/> returns <see langword="false"/> when load execution throws.
        /// </summary>
        [Fact]
        /// <summary>
        /// Gets the definition for Load_When_PerformLoad_Throws_Returns_False.
        /// </summary>
        public void Load_When_PerformLoad_Throws_Returns_False()
        {
            MockBusinessBase mock = new()
            {
                ThrowOnLoad = true
            };

            bool success = mock.Load(10);

            Assert.False(success);
        }

        /// <summary>
        /// Tests that <see cref="BusinessBase.LoadAsync{TId}(TId)"/> returns the result from async load execution.
        /// </summary>
        [Fact]
        /// <summary>
        /// Gets the definition for LoadAsync_Returns_PerformLoadAsync_Result.
        /// </summary>
        public async Task LoadAsync_Returns_PerformLoadAsync_Result()
        {
            MockBusinessBase mock = new()
            {
                LoadAsyncResult = true
            };

            bool success = await mock.LoadAsync(10);

            Assert.True(success);
        }

        /// <summary>
        /// Tests that <see cref="BusinessBase.LoadAsync{TId}(TId)"/> returns <see langword="false"/> when async load execution throws.
        /// </summary>
        [Fact]
        /// <summary>
        /// Gets the definition for LoadAsync_When_PerformLoadAsync_Throws_Returns_False.
        /// </summary>
        public async Task LoadAsync_When_PerformLoadAsync_Throws_Returns_False()
        {
            MockBusinessBase mock = new()
            {
                ThrowOnLoadAsync = true
            };

            bool success = await mock.LoadAsync(10);

            Assert.False(success);
        }

        /// <summary>
        /// Tests that <see cref="BusinessBase.Save"/> returns the result from save execution.
        /// </summary>
        [Fact]
        /// <summary>
        /// Gets the definition for Save_Returns_PerformSave_Result.
        /// </summary>
        public void Save_Returns_PerformSave_Result()
        {
            MockBusinessBase mock = new()
            {
                SaveResult = false
            };

            bool success = mock.Save();

            Assert.False(success);
        }

        /// <summary>
        /// Tests that <see cref="BusinessBase.Save"/> returns <see langword="false"/> when save execution throws.
        /// </summary>
        [Fact]
        /// <summary>
        /// Gets the definition for Save_When_PerformSave_Throws_Returns_False.
        /// </summary>
        public void Save_When_PerformSave_Throws_Returns_False()
        {
            MockBusinessBase mock = new()
            {
                ThrowOnSave = true
            };

            bool success = mock.Save();

            Assert.False(success);
        }

        /// <summary>
        /// Tests that <see cref="BusinessBase.SaveAsync"/> returns the result from async save execution.
        /// </summary>
        [Fact]
        /// <summary>
        /// Gets the definition for SaveAsync_Returns_PerformSaveAsync_Result.
        /// </summary>
        public async Task SaveAsync_Returns_PerformSaveAsync_Result()
        {
            MockBusinessBase mock = new()
            {
                SaveAsyncResult = false
            };

            bool success = await mock.SaveAsync();

            Assert.False(success);
        }

        /// <summary>
        /// Tests that <see cref="BusinessBase.SaveAsync"/> returns <see langword="false"/> when async save execution throws.
        /// </summary>
        [Fact]
        /// <summary>
        /// Gets the definition for SaveAsync_When_PerformSaveAsync_Throws_Returns_False.
        /// </summary>
        public async Task SaveAsync_When_PerformSaveAsync_Throws_Returns_False()
        {
            MockBusinessBase mock = new()
            {
                ThrowOnSaveAsync = true
            };

            bool success = await mock.SaveAsync();

            Assert.False(success);
        }

        /// <summary>
        /// Tests that registered child property change events are forwarded with qualified property names.
        /// </summary>
        [Fact]
        /// <summary>
        /// Gets the definition for RegisterEvents_Forwards_Child_PropertyChanged_With_Qualified_Name.
        /// </summary>
        public void RegisterEvents_Forwards_Child_PropertyChanged_With_Qualified_Name()
        {
            MockBusinessBase parent = new();
            MockBusinessBase child = new();
            string? propertyName = null;

            parent.PropertyChanged += (_, args) => propertyName = args.PropertyName;
            parent.RegisterChild(child);

            child.RaisePropertyChangedForTest("ChildProperty");

            Assert.Equal("MockBusinessBase.ChildProperty", propertyName);
        }

        /// <summary>
        /// Tests that registered child validation change events are forwarded with qualified property names.
        /// </summary>
        [Fact]
        /// <summary>
        /// Gets the definition for RegisterEvents_Forwards_Child_ValidationChanged_With_Qualified_Name.
        /// </summary>
        public void RegisterEvents_Forwards_Child_ValidationChanged_With_Qualified_Name()
        {
            MockBusinessBase parent = new();
            MockBusinessBase child = new();
            string? propertyName = null;

            parent.PropertyValidationChanged += (_, args) => propertyName = args.PropertyName;
            parent.RegisterChild(child);

            child.RaisePropertyValidationChangedForTest("ChildProperty");

            Assert.Equal("MockBusinessBase.ChildProperty", propertyName);
        }

        /// <summary>
        /// Tests that unregistering child events stops parent event forwarding.
        /// </summary>
        [Fact]
        /// <summary>
        /// Gets the definition for UnregisterEvents_Stops_Forwarding_Child_Events.
        /// </summary>
        public void UnregisterEvents_Stops_Forwarding_Child_Events()
        {
            MockBusinessBase parent = new();
            MockBusinessBase child = new();
            bool propertyChangedRaised = false;
            bool validationChangedRaised = false;

            parent.PropertyChanged += (_, _) => propertyChangedRaised = true;
            parent.PropertyValidationChanged += (_, _) => validationChangedRaised = true;

            parent.RegisterChild(child);
            parent.UnregisterChild(child);

            child.RaisePropertyChangedForTest("ChildProperty");
            child.RaisePropertyValidationChangedForTest("ChildProperty");

            Assert.False(propertyChangedRaised);
            Assert.False(validationChangedRaised);
        }

        /// <summary>
        /// Tests that disposing clears validation messages and removes property validation event handlers.
        /// </summary>
        [Fact]
        /// <summary>
        /// Gets the definition for Dispose_Clears_ValidationMessages_And_PropertyValidation_Handlers.
        /// </summary>
        public void Dispose_Clears_ValidationMessages_And_PropertyValidation_Handlers()
        {
            MockBusinessBase mock = new()
            {
                ValidationToReturn = new ValidationMessageCollection
                {
                    new ValidationMessage("Invalid", ValidationLevel.Error, false)
                }
            };

            bool raised = false;
            mock.PropertyValidationChanged += (_, _) => raised = true;

            mock.Validate();
            Assert.NotNull(mock.ValidationMessages);
            Assert.Single(mock.ValidationMessages);

            mock.Dispose();
            mock.RaisePropertyValidationChangedForTest("AfterDispose");

            Assert.Null(mock.ValidationMessages);
            Assert.False(raised);
        }
    }
}