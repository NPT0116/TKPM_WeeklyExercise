using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using BE.Config;
using BE.Services;
using BE.Interface;

namespace BE.Test.ServiceTest;
public class ValidateStudentEmailTests
{
    [Fact]
    public void IsValidEmail_WithCorrectDomain_ReturnsTrue()
    {
        // Arrange
        var config = InitConfiguration("MockSetting.json");
        var emailSettings = config.GetSection("EmailSetting").Get<EmailSetting>();

        // Tạo IOptions<EmailSetting>
        var options = Options.Create(emailSettings);

        var validateEmailService = new ValidateStudentEmail(options);

        // Act
        var result = validateEmailService.IsValidEmail("example@student.university.edu.vn");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidEmail_WithWrongDomain_ReturnsFalse()
    {
        // Arrange
        var config = InitConfiguration("MockSetting.json");
        var emailSettings = config.GetSection("EmailSetting").Get<EmailSetting>();
        var options = Options.Create(emailSettings);

        var validateEmailService = new ValidateStudentEmail(options);

        // Act
        var result = validateEmailService.IsValidEmail("example@gmail.com");

        // Assert
        Assert.False(result);
    }

    private IConfiguration InitConfiguration(string fileName)
    {
        // fileName = "mockSettings.json" chẳng hạn
        var path = Path.Combine(Directory.GetCurrentDirectory(), "MockSetting.json");
        Console.WriteLine("--------------------------------");
        Console.WriteLine(path);
        Console.WriteLine("--------------------------------");
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) 
            .AddJsonFile(fileName, optional: false, reloadOnChange: false)
            .Build();
    }
}

public class StudentStatusTransitionServiceTests
{
    [Theory]
    [InlineData(1, 2, true)]   // Allowed transition
    [InlineData(1, 3, true)]   // Allowed transition
    [InlineData(2, 3, false)]  // Not allowed
    public void IsValidTransition_ReturnsExpectedResult(int currentStatus, int newStatus, bool expected)
    {
        // Arrange
        var config = InitConfiguration("MockSetting.json");
        var transitionSettings = config.GetSection("StudentStatusTransitionConfig")
                                       .Get<StudentStatusTransitionConfig>();
        var options = Options.Create(transitionSettings);

        var transitionService = new StudentStatusTransitionService(options);

        // Act
        var result = transitionService.IsValidTransition(currentStatus, newStatus);

        // Assert
        Assert.Equal(expected, result);
    }

    private IConfiguration InitConfiguration(string fileName)
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(fileName, optional: false, reloadOnChange: false)
            .Build();
    }
}
