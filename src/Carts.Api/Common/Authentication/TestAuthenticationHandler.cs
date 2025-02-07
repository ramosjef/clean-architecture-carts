﻿using System.Security.Claims;
using System.Text.Encodings.Web;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Carts.Api.Common.Authentication;

public sealed class TestAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{

    public TestAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
                                     ILoggerFactory logger,
                                     UrlEncoder encoder,
                                     ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        Claim[] claims =
        {
            new Claim(ClaimTypes.NameIdentifier, "test"), new Claim(ClaimTypes.Name, "test"),
            new Claim("id", "92b93e37-0995-4849-a7ed-149e8706d8ef")
        };

        ClaimsIdentity identity = new(claims, Scheme.Name);
        ClaimsPrincipal principal = new(identity);
        AuthenticationTicket ticket = new(principal, Scheme.Name);

        return await Task.FromResult(AuthenticateResult.Success(ticket)).ConfigureAwait(false);
    }
}
