using System;
using System.Collections.Generic;
using System.Text;
using Adaptive.Intelligence.Validation;

namespace Adaptive.Intelligence.Framework.Tests.Validation;

public class ValidationMessageTests
{
    private const string MessageNone = "";
    private const string MessageError = "This is an error message.";
    private const string MessageWarning = "This is a warning message.";
    private const string MessageInformational = "This is an informational message.";
    private const string MessageSuccess = "This is a success message.";

    [Fact]
    public void ValidationMessage_DefaultConstructor_ShouldBeValid()
    {
        // Arrange & Act
        var validationMessage = new ValidationMessage();

        // Assert
        Assert.True(validationMessage.IsValid);
        Assert.Equal(MessageNone, validationMessage.ErrorMessage);
        Assert.Equal(ValidationLevel.NoneOrNotSpecified, validationMessage.Level);
    }


    [Fact]
    public void ValidationMessage_DefaultProperty_ShouldBeValid()
    {
        // Arrange & Act
        var validationMessage = ValidationMessage.DefaultSuccess;

        // Assert
        Assert.True(validationMessage.IsValid);
        Assert.Equal(MessageNone, validationMessage.ErrorMessage);
        Assert.Equal(ValidationLevel.NoneOrNotSpecified, validationMessage.Level);
    }

    [Fact]
    public void ValidationMessage_SecondConstructor_ShouldBeValid()
    {
        // Arrange & Act
        var validationMessage = new ValidationMessage(MessageError, ValidationLevel.Error);

        // Assert
        Assert.False(validationMessage.IsValid);
        Assert.Equal(MessageError, validationMessage.ErrorMessage);
        Assert.Equal(ValidationLevel.Error, validationMessage.Level);

        // Arrange & Act
        validationMessage = new ValidationMessage(MessageWarning, ValidationLevel.Warning);

        // Assert
        Assert.True(validationMessage.IsValid);
        Assert.Equal(MessageWarning, validationMessage.ErrorMessage);
        Assert.Equal(ValidationLevel.Warning, validationMessage.Level);

        // Arrange & Act
        validationMessage = new ValidationMessage(MessageInformational, ValidationLevel.Informational);

        // Assert
        Assert.True(validationMessage.IsValid);
        Assert.Equal(MessageInformational, validationMessage.ErrorMessage);
        Assert.Equal(ValidationLevel.Informational, validationMessage.Level);

        // Arrange & Act
        validationMessage = new ValidationMessage(MessageSuccess, ValidationLevel.SuccessInformational);

        // Assert
        Assert.True(validationMessage.IsValid);
        Assert.Equal(MessageSuccess, validationMessage.ErrorMessage);
        Assert.Equal(ValidationLevel.SuccessInformational, validationMessage.Level);
    }


    [Fact]
    public void ValidationMessage_ThirdConstructor_ShouldBeValid()
    {
        // Arrange & Act
        var validationMessage = new ValidationMessage(MessageError, ValidationLevel.Error, false);

        // Assert
        Assert.False(validationMessage.IsValid);
        Assert.Equal(MessageError, validationMessage.ErrorMessage);
        Assert.Equal(ValidationLevel.Error, validationMessage.Level);
        
        // Arrange & Act
        validationMessage = new ValidationMessage(MessageWarning, ValidationLevel.Warning, false);

        // Assert
        Assert.False(validationMessage.IsValid);
        Assert.Equal(MessageWarning, validationMessage.ErrorMessage);
        Assert.Equal(ValidationLevel.Warning, validationMessage.Level);


        // Arrange & Act
        validationMessage = new ValidationMessage(MessageWarning, ValidationLevel.Warning, true);

        // Assert
        Assert.True(validationMessage.IsValid);
        Assert.Equal(MessageWarning, validationMessage.ErrorMessage);
        Assert.Equal(ValidationLevel.Warning, validationMessage.Level);

        // Arrange & Act
        validationMessage = new ValidationMessage(MessageInformational, ValidationLevel.Informational, false);

        // Assert
        Assert.False(validationMessage.IsValid);
        Assert.Equal(MessageInformational, validationMessage.ErrorMessage);
        Assert.Equal(ValidationLevel.Informational, validationMessage.Level);

        // Arrange & Act
        validationMessage = new ValidationMessage(MessageSuccess, ValidationLevel.SuccessInformational, true);

        // Assert
        Assert.True(validationMessage.IsValid);
        Assert.Equal(MessageSuccess, validationMessage.ErrorMessage);
        Assert.Equal(ValidationLevel.SuccessInformational, validationMessage.Level);
    }

    [Fact]
    public void Dispose_Test()
    {
        ValidationMessage validationMessage = new ValidationMessage(MessageWarning, ValidationLevel.Warning, true);

        validationMessage.Dispose();
        validationMessage.Dispose();
        validationMessage.Dispose();
        validationMessage.Dispose();

        validationMessage.Level = ValidationLevel.Error;
        validationMessage.IsValid = true;
        validationMessage.ErrorMessage = MessageError;

        Assert.True(validationMessage.MemberNames.Count() == 0);

        validationMessage.Dispose();

    }
}
