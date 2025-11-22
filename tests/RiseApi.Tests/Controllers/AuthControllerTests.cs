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
        var dto = new AuthDto
        {
            Username = "john",
            Password = "123456"
        };

        var response = await _client.PostAsJsonAsync("/api/v1/Auth/register", dto);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Usuário criado com sucesso", content);
    }
}
