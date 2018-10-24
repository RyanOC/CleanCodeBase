using System.Threading.Tasks;
using Core.Abstractions;
using Core.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace Tests
{
    public class IdentityServiceTests
    {
        private readonly Mock<IIdentityService> _mockService;

        public IdentityServiceTests()
        {
            _mockService = new Mock<IIdentityService>();
        }

        [Theory,
         InlineData("user", "password", "FakeTokenString"),
         InlineData("user@domain.com", "p@ssword", "FakeTokenString"),
         InlineData("u", "p", "FakeTokenString")]
        [Trait("Category", "Unit")]
        public async void Get_Token_Should_Return_Token(string userName, string password, string expected)
        {
            // Arange
            _mockService.Setup(svc => svc.AuthenticateAsync(It.IsAny<LoginCredentials>())).ReturnsAsync(expected);

            // Act
            var result = await _mockService.Object.AuthenticateAsync(new LoginCredentials {UserName = userName, Password = password});

            // Assert
            result.Should().Be(expected);
        }
    }
}