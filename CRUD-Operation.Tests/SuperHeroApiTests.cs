using System.Net;
using System.Net.Http.Json;
using CRUD_Operation;
using CRUD_Operation.Models.Auth;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace CRUD_Operation.Tests;

public class SuperHeroApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public SuperHeroApiTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Health_Db_ReturnsOk()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/health/db");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task SuperHero_Get_WithoutToken_ReturnsUnauthorized()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/superhero");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task SuperHero_Get_WithValidToken_ReturnsOk()
    {
        // Arrange
        var client = _factory.CreateClient();

        // First, sign up or log in to get a token
        var signupRequest = new SignupRequest
        {
            Email = "testuser@example.com",
            Password = "Password123!",
            FullName = "Test User"
        };

        var signupResponse = await client.PostAsJsonAsync("/api/auth/signup", signupRequest);

        if (!signupResponse.IsSuccessStatusCode && signupResponse.StatusCode != HttpStatusCode.BadRequest)
        {
            // If signup failed for any reason other than "user already exists", fail the test
            throw new Xunit.Sdk.XunitException($"Signup failed with status code {signupResponse.StatusCode}");
        }

        var loginRequest = new LoginRequest
        {
            Email = signupRequest.Email,
            Password = signupRequest.Password
        };

        var loginResponse = await client.PostAsJsonAsync("/api/auth/login", loginRequest);
        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

        var auth = await loginResponse.Content.ReadFromJsonAsync<AuthResponse>();
        Assert.NotNull(auth);
        Assert.False(string.IsNullOrWhiteSpace(auth!.Token));

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", auth.Token);

        // Act
        var response = await client.GetAsync("/api/superhero");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}

