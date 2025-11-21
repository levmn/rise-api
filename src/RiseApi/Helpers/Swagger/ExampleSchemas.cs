using Swashbuckle.AspNetCore.Filters;
using RiseApi.DTOs.Auth;

namespace RiseApi.Helpers.Swagger
{
    public class AuthDtoExample : IExamplesProvider<AuthDto>
    {
        public AuthDto GetExamples()
        {
            return new AuthDto
            {
                Username = "joao.silva",
                Password = "SenhaFort3!"
            };
        }
    }
}
