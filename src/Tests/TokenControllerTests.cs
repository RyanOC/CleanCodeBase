using System.Linq;
using Core.Abstractions;
using Core.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using Xunit;

namespace Tests
{
    public class TokenControllerTests
    {
        private readonly Mock<ITokenService> _mockService;

        public TokenControllerTests()
        {
            _mockService = new Mock<ITokenService>();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void Get_Token_Should_Return_Token()
        {
            // Arange
            _mockService.Setup(svc => svc.AuthenticateAsync(It.IsAny<LoginCredentials>())).ReturnsAsync("FakeTokenString");
            TokenController controller = new TokenController(null, _mockService.Object);

            // Act
            var response = controller.Post(new LoginCredentials { UserName = "user@domain.com", Password = "password"}).Result;

            // Assert
            var result = response.Should().BeOfType<OkObjectResult>().Subject;
            var body = result.Value.Should().BeAssignableTo<string>().Subject;
            body.Should().Be("FakeTokenString");
        }
        
        [Fact]
        [Trait("Category", "Unit")]
        public void Get_Token_Should_Return_ForbidRequest()
        {
            // Arange
            _mockService.Setup(svc => svc.AuthenticateAsync(It.IsAny<LoginCredentials>())).ReturnsAsync("FakeTokenString");
            TokenController controller = new TokenController(null, _mockService.Object);

            // Act
            var response = controller.Post(new LoginCredentials { UserName = "", Password = ""}).Result;

            // Assert
            var result = response.Should().BeOfType<ForbidResult>().Subject.AuthenticationSchemes.SingleOrDefault();
            var body = result.Should().BeAssignableTo<string>().Subject;
            body.Should().Be("Login Failed");
        }
    }
}