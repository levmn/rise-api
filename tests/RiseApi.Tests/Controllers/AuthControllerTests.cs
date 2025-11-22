using System.Net;
using System.Net.Http.Json;
using RiseApi.DTOs.Auth;

public class AuthControllerTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthControllerTests(TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_Should_Create_User()
    {
        var dto = new AuthDto { Username = "john", Password = "123456" };

        var response = await _client.PostAsJsonAsync("/api/v1/Auth/register", dto);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Usuário criado com sucesso", content);
    }

    [Fact]
    public async Task Register_Should_Fail_When_Username_Already_Exists()
    {
        var dto = new AuthDto { Username = "mary", Password = "123456" };

        var first = await _client.PostAsJsonAsync("/api/v1/Auth/register", dto);
        Assert.Equal(HttpStatusCode.OK, first.StatusCode);

        var second = await _client.PostAsJsonAsync("/api/v1/Auth/register", dto);
        Assert.Equal(HttpStatusCode.BadRequest, second.StatusCode);

        var content = await second.Content.ReadAsStringAsync();
        Assert.Contains("Username já existe", content);
    }

    [Fact]
    public async Task Login_Should_Return_Token()
    {
        await _client.PostAsJsonAsync("/api/v1/Auth/register",
            new AuthDto { Username = "alice", Password = "mypassword" });

        var response = await _client.PostAsJsonAsync("/api/v1/Auth/login",
            new AuthDto { Username = "alice", Password = "mypassword" });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        Assert.NotNull(body);
        Assert.True(body!.ContainsKey("token"));
    }

    [Fact]
    public async Task Login_Should_Fail_With_Wrong_Password()
    {
        await _client.PostAsJsonAsync("/api/v1/Auth/register",
            new AuthDto { Username = "bob", Password = "secret" });

        var response = await _client.PostAsJsonAsync("/api/v1/Auth/login",
            new AuthDto { Username = "bob", Password = "wrongpassword" });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Usuário ou senha inválidos", content);
    }

    [Fact]
    public async Task Login_Should_Fail_User_Not_Found()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/Auth/login",
            new AuthDto { Username = "ghost", Password = "whatever" });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
