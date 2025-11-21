using Microsoft.AspNetCore.Http;

namespace RiseApi.Helpers.Hateoas
{
    public class LinkBuilder
    {
        private readonly LinkGenerator _link;
        private readonly IHttpContextAccessor _http;

        public LinkBuilder(LinkGenerator link, IHttpContextAccessor http)
        {
            _link = link;
            _http = http;
        }

        public string Build(string action, string controller, object? values = null)
        {
            var httpContext = _http.HttpContext;

            if (httpContext == null)
                return string.Empty;

            return _link.GetUriByAction(
                httpContext,
                action: action,
                controller: controller,
                values: values
            ) ?? string.Empty;
        }
    }
}
