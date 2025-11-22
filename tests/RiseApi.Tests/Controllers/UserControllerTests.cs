using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using RiseApi.DTOs;
using RiseApi.Models;

public class UserControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public UserControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Create_Should_Add_User()
    {
        var dto = new UserCreateDto
        {
            Username = "john",
            Password = "123"
        };

        var response = await _client.PostAsJsonAsync("/api/v1/User", dto);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var json = await response.Content.ReadFromJsonAsync<SingleUserDto>();
        json.Should().NotBeNull();

        json.data.Id.Should().NotBe(Guid.Empty);
        json!.data.Username.Should().Be("john");
    }

    [Fact]
    public async Task GetAll_Should_Return_Paginated_Users()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<RiseApi.Data.AppDbContext>();

        db.Users.Add(new User { Username = "u1", PasswordHash = "x" });
        db.Users.Add(new User { Username = "u2", PasswordHash = "x" });
        db.SaveChanges();

        var response = await _client.GetAsync("/api/v1/User?page=1&pageSize=10");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var json = await response.Content.ReadFromJsonAsync<PagedResponse<UserDto>>();
        json.Should().NotBeNull();

        json!.totalItems.Should().Be(2);
        json.data.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetById_Should_Return_User()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<RiseApi.Data.AppDbContext>();

        var user = new User { Username = "alpha", PasswordHash = "x" };
        db.Users.Add(user);
        db.SaveChanges();

        var response = await _client.GetAsync($"/api/v1/User/{user.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var json = await response.Content.ReadFromJsonAsync<SingleUserDto>();
        json.Should().NotBeNull();

        json.data.Id.Should().Be(user.Id);
        json!.data.Username.Should().Be("alpha");
    }

    [Fact]
    public async Task GetById_Should_Return_NotFound_When_User_Not_Exists()
    {
        var id = Guid.NewGuid();

        var response = await _client.GetAsync($"/api/v1/User/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_Should_Remove_User()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<RiseApi.Data.AppDbContext>();

        var user = new User { Username = "victim", PasswordHash = "x" };
        db.Users.Add(user);
        db.SaveChanges();

        var response = await _client.DeleteAsync($"/api/v1/User/{user.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        db.Users.Any(u => u.Id == user.Id).Should().BeFalse();
    }
}
