using CRUD_Operation.Models.Auth;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using Xunit;
using CRUD_Operation.Entities;

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

    [Fact]
    public async Task SuperHero_CompleteCrudLifecycle_EvaluatesSuccessfully()
    {
        // Arrange
        var client = _factory.CreateClient();

        // 1. Authenticate to get a valid token
        var signupRequest = new SignupRequest
        {
            Email = "crudadmin@example.com",
            Password = "SecurePassword123!",
            FullName = "CRUD Admin User"
        };
        var signupResponse = await client.PostAsJsonAsync("/api/auth/signup", signupRequest);

        if (!signupResponse.IsSuccessStatusCode && signupResponse.StatusCode != HttpStatusCode.BadRequest)
        {
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

        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", auth.Token);

        // 2. CREATE (POST)
        var newHero = new SuperHero
        {
            Name = "Integration Hero",
            FirstName = "Test",
            LastName = "User",
            Place = "Test City"
        };

        var createResponse = await client.PostAsJsonAsync("/api/superhero", newHero);
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var createdHero = await createResponse.Content.ReadFromJsonAsync<SuperHero>();
        Assert.NotNull(createdHero);
        Assert.True(createdHero.Id > 0, "Expected a valid Identity ID generated from the DB");
        Assert.Equal("Integration Hero", createdHero.Name);


        // 3. READ (GET by Id)
        var getResponse = await client.GetAsync($"/api/superhero/{createdHero.Id}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var fetchedHero = await getResponse.Content.ReadFromJsonAsync<SuperHero>();
        Assert.NotNull(fetchedHero);
        Assert.Equal(createdHero.Id, fetchedHero.Id);
        Assert.Equal("Test City", fetchedHero.Place);


        // 4. UPDATE (PUT)
        fetchedHero.Name = "Updated Hero";
        fetchedHero.Place = "Updated City";

        var updateResponse = await client.PutAsJsonAsync($"/api/superhero/{fetchedHero.Id}", fetchedHero);
        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

        var updatedHero = await updateResponse.Content.ReadFromJsonAsync<SuperHero>();
        Assert.NotNull(updatedHero);
        Assert.Equal("Updated Hero", updatedHero.Name);
        Assert.Equal("Updated City", updatedHero.Place);

        // Verify the update took effect in the DB by doing another GET
        var verifyUpdateResponse = await client.GetAsync($"/api/superhero/{fetchedHero.Id}");
        var verifyFetchedHero = await verifyUpdateResponse.Content.ReadFromJsonAsync<SuperHero>();
        Assert.Equal("Updated City", verifyFetchedHero!.Place);


        // 5. DELETE (DELETE)
        var deleteResponse = await client.DeleteAsync($"/api/superhero/{fetchedHero.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);


        // 6. Verify Deletion (GET returns 404)
        var getDeletedResponse = await client.GetAsync($"/api/superhero/{fetchedHero.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getDeletedResponse.StatusCode);
    }
}

