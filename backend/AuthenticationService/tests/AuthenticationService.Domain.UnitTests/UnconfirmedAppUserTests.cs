using AuthenticationService.Domain.Rules;
using AuthenticationService.Domain.UnconfirmedAppUserAggregate;
using SharedKernel.Common;
using SharedKernel.Common.errors;

namespace AuthenticationService.Domain.UnitTests;

public class UnconfirmedAppUserTests
{
    [Fact]
    public void CreateAppUnconfirmedUser_WhenInvalidEmail_ShouldReturnErr() {
        // Arrange
        string invalidEmail = "invalid_email";
        string password = "validPassword123";
        UtcDateTimeProvider dateTimeProvider = new();

        // Act
        var result = UnconfirmedAppUser.Create(
            invalidEmail,
            password,
            (pass) => pass,
            dateTimeProvider
        );

        // Assert
        Assert.True(result.AnyErr((err) => err.Code == Err.ErrCodes.InvalidData));
        Assert.True(result.AnyErr((err) => err.Message == "Invalid email"));
    }

    [Fact]
    public void CreateAppUnconfirmedUser_WhenEverythingIsValid_ShouldReturnUnconfirmedUser() {
        // Arrange
        string invalidEmail = "justvalidemail@email.email";
        string password = "validPassword123";
        UtcDateTimeProvider dateTimeProvider = new();

        // Act
        var result = UnconfirmedAppUser.Create(
            invalidEmail,
            password,
            (pass) => pass,
            dateTimeProvider
        );

        // Assert
        Assert.False(result.AnyErr());
    }

    [Fact]
    public void CreateAppUnconfirmedUser_WhenPasswordInvalid_ShouldReturnErr() {
        // Arrange
        string email = "validemail@email.com";
        UtcDateTimeProvider dateTimeProvider = new();

        // Act & Assert

        // Password too short
        var shortPassword = new string('a', PasswordRules.MinLength - 1);
        var resultShort = UnconfirmedAppUser.Create(
            email,
            shortPassword,
            (pass) => pass,
            dateTimeProvider
        );
        Assert.True(resultShort.AnyErr((err) => err.Message == PasswordRules.IncorrectLengthMessage));

        // Password too long
        var longPassword = new string('a', PasswordRules.MaxLength + 1);
        var resultLong = UnconfirmedAppUser.Create(
            email,
            longPassword,
            (pass) => pass,
            dateTimeProvider
        );
        Assert.True(resultLong.AnyErr((err) => err.Message == PasswordRules.IncorrectLengthMessage));

        // Password without letter
        var noLetterPassword = "12345678";
        var resultNoLetter = UnconfirmedAppUser.Create(
            email,
            noLetterPassword,
            (pass) => pass,
            dateTimeProvider
        );
        Assert.True(resultNoLetter.AnyErr((err) => err.Message == PasswordRules.MustContainLetterMessage));

        // Password without number
        var noNumberPassword = "abcdefgh";
        var resultNoNumber = UnconfirmedAppUser.Create(
            email,
            noNumberPassword,
            (pass) => pass,
            dateTimeProvider
        );
        Assert.True(resultNoNumber.AnyErr((err) => err.Message == PasswordRules.MustContainNumberMessage));
    }
}
