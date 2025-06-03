using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Application;
using UsersService.Domain.Repositories;

namespace UserService.Tests;

public class test
{
    [Theory]
    [InlineData("xyz@gmail.com")]
    [InlineData("something@xyz.pl")]
    [InlineData("1@1.1")]
    [InlineData("x@g.c")]
    public void ValidateEmail_CorrectEmail_ShouldPassCorrectly(string email)
    {
        //Arrange
        var jwtTokenServiceMock = new Mock<IJwtTokenService>();
        var repositoryMock = new Mock<IRepository>();
        var passwordHasherMock = new Mock<IPasswordHasher>();
        var loginService = new LoginService(
        jwtTokenServiceMock.Object,
        repositoryMock.Object,
        passwordHasherMock.Object
    );

        // Act
        var result = loginService.IsValidEmail(email);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData("xyzgmail.com")]
    [InlineData("somet.hing@xyz.pl")]
    [InlineData("xyz@gmailcom")]
    [InlineData("somethingxyzpl")]
    [InlineData("som@ething@xyz.pl")]
    [InlineData("xyz@gmail.com@")]
    [InlineData("something@xy@z.pl")]
    public void ValidateEmail_IncorrectEmail_ShouldPassCorrectly(string email)
    {
        //Arrange
        var jwtTokenServiceMock = new Mock<IJwtTokenService>();
        var repositoryMock = new Mock<IRepository>();
        var passwordHasherMock = new Mock<IPasswordHasher>();

        var loginService = new LoginService(
            jwtTokenServiceMock.Object,
            repositoryMock.Object,
            passwordHasherMock.Object
        );

        // Act
        var result = loginService.IsValidEmail(email);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData("SuperPassword123!")]
    [InlineData("Aaa123$456")]
    [InlineData("Xyzzy99@")]

    public void ValidatePassword_CorrectPassword_ShouldPassCorrectly(string password)
    {
        //Arrange
        var jwtTokenServiceMock = new Mock<IJwtTokenService>();
        var repositoryMock = new Mock<IRepository>();
        var passwordHasherMock = new Mock<IPasswordHasher>();

        var loginService = new LoginService(
            jwtTokenServiceMock.Object,
            repositoryMock.Object,
            passwordHasherMock.Object
        );

        // Act
        var exception = Record.Exception(() => loginService.ValidatePassword(password));


        // Assert
        Assert.Null(exception);
    }

    [Theory]
    [InlineData("SuperPassword1")]
    [InlineData("superpassword1!")]
    [InlineData("SUPERPASSWORD1!")]
    [InlineData("SuperPassword!")]
    [InlineData("Super1!")]
    [InlineData("")]

    public void ValidatePassword_IncorrectPassword_ShouldThrowArgumentException(string password)
    {
        //Arrange
        var jwtTokenServiceMock = new Mock<IJwtTokenService>();
        var repositoryMock = new Mock<IRepository>();
        var passwordHasherMock = new Mock<IPasswordHasher>();

        var loginService = new LoginService(
            jwtTokenServiceMock.Object,
            repositoryMock.Object,
            passwordHasherMock.Object
        );

        // Act & Assert
        Assert.Throws<ArgumentException>(() => loginService.ValidatePassword(password));
    }

    [Fact]
    public void ValidatePassword_MultipleIssues_ShouldContainMultipleErrorMessages()
    {
        // Arrange
        var jwtTokenServiceMock = new Mock<IJwtTokenService>();
        var repositoryMock = new Mock<IRepository>();
        var passwordHasherMock = new Mock<IPasswordHasher>();

        var loginService = new LoginService(
            jwtTokenServiceMock.Object,
            repositoryMock.Object,
            passwordHasherMock.Object
        );

        var invalidPassword = "short";

        // Act
        var ex = Assert.Throws<ArgumentException>(() => loginService.ValidatePassword(invalidPassword));

        // Assert
        Assert.Contains("Password must be at least 8 characters long.", ex.Message);
        Assert.Contains("Password must contain at least one uppercase letter.", ex.Message);
        Assert.Contains("Password must contain at least one number.", ex.Message);
        Assert.Contains("Password must contain at least one special character.", ex.Message);
    }
}

