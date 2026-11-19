using System.Net;
using System.Net.Http.Json;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using WebApplication1.Features.Authenticatie;

namespace WebApplication1.Tests;

public class AuthTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public AuthTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Register_ValidCredentials_ReturnsOkAndAssignsGastRole()
    {
        // Arrange
        using var client = _factory.CreateClient(
            new WebApplicationFactoryClientOptions { HandleCookies = true });
        var uniqueEmail = $"newuser_{Guid.NewGuid()}@test.nl";
        var registerDto = new RegisterDto(uniqueEmail, "Test1234!");

        // Act
        var registerResponse = await client.PostAsJsonAsync("api/auth/register", registerDto);
        var loginResponse = await client.PostAsJsonAsync(
            "api/auth/login",
            new LoginDto(registerDto.Email, registerDto.Password));
        var meResponse = await client.GetAsync("api/auth/me");

        // Assert
        Assert.Equal(HttpStatusCode.OK, registerResponse.StatusCode);
        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
        Assert.Equal(HttpStatusCode.OK, meResponse.StatusCode);

        var userInfo = await meResponse.Content.ReadFromJsonAsync<UserInfoDto>();
        Assert.NotNull(userInfo);
        Assert.Equal(uniqueEmail, userInfo.Email);
        Assert.Contains("Gast", userInfo.Roles);
        Assert.DoesNotContain("Editor", userInfo.Roles);
        Assert.DoesNotContain("Admin", userInfo.Roles);
    }

    [Fact]
    public async Task Login_ValidCredentialsReturnsOkAbdSetsAuthCookie()
    {
        // Arrange
        using var client = _factory.CreateClient(
            new WebApplicationFactoryClientOptions { HandleCookies = true });
        var loginDto = new LoginDto("gast@test.nl", "Test1234!");

        // Act 
        var loginResponse = await client.PostAsJsonAsync("api/auth/login", loginDto);
        var meResponse = await client.GetAsync("api/auth/me");  

        // Assert
        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
        Assert.Equal(HttpStatusCode.OK, meResponse.StatusCode);
        Assert.True(loginResponse.Headers.Contains("Set-Cookie"));
    }

    [Fact]
    public async Task Login_InvalidPassword_ReturnsUnathorized()
    {
        // Arrange
        using var client = _factory.CreateClient(
            new WebApplicationFactoryClientOptions { HandleCookies = true });
        var loginDto = new LoginDto("gast@test.nl", "WrongPassword123!");

        // Act
        var loginResponse = await client.PostAsJsonAsync("api/auth/login", loginDto);
        var meResponse = await client.GetAsync("api/auth/me");  

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, loginResponse.StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, meResponse.StatusCode);
    }

    [Fact]
    public async Task AssignEditor_AsGast_ReturnsForbidden()
    {
        // Arrange
        using var client = await _factory.CreateAuthenticatedClientAsync("gast@test.nl", "Test1234!");
        var PromoteGastDto = new PromoteGastDto("user2@test.nl");

        // Act
        var response = await client.PostAsJsonAsync("api/auth/assign-editor", PromoteGastDto);


        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task AssignEditor_AsAdmin_PromotesGastToEditor()
    {
    // Arrange
    var targetEmail = $"promoteme_{Guid.NewGuid()}@test.nl";
    using var anonClient = _factory.CreateClient();
    var registerResponse = await anonClient.PostAsJsonAsync(
        "api/auth/register",
        new RegisterDto(targetEmail, "Test1234!"));
    registerResponse.EnsureSuccessStatusCode();

    using var adminClient = await _factory.CreateAuthenticatedClientAsync("admin@test.nl", "Test1234!");

    // Act
    var response = await adminClient.PostAsJsonAsync(
        "api/auth/assign-editor",
        new PromoteGastDto(targetEmail));

    using var userClient = await _factory.CreateAuthenticatedClientAsync(targetEmail, "Test1234!");
    var meResponse = await userClient.GetAsync("api/auth/me");

    // Assert
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    Assert.Equal(HttpStatusCode.OK, meResponse.StatusCode);

    var userInfo = await meResponse.Content.ReadFromJsonAsync<UserInfoDto>();
    Assert.NotNull(userInfo);
    Assert.Contains("Editor", userInfo.Roles);
}
}
