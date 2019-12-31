using System;
using System.Collections.Generic;
using System.Linq;
using JWT.Algorithms;
using JWT.Builder;
using NetBaires.Api.Auth.CustomsClaims;
using Newtonsoft.Json;

namespace NetBaires.Api.Auth.Tokens
{
    public static class TokenService
    {
        public static string Generate(string secret, List<CustomClaim> claims, DateTime? expire = null) =>
            new JwtBuilder()
                         .WithAlgorithm(new HMACSHA256Algorithm())
                         .ExpirationTime((expire ??= DateTime.UtcNow.AddDays(7)))
                         .WithSecret(secret)
                         .WithVerifySignature(true)
                         .MustVerifySignature()
                         .AddClaims(claims.ToDictionary(x => x.Type, x => (object)x.Value))
                         .Build();
        public static TValidatedToken Validate<TValidatedToken>(string secret, string tokenToValidate) =>
            JsonConvert.DeserializeObject<TValidatedToken>(new JwtBuilder()
                   .WithSecret(secret)
                   .Decode(tokenToValidate));
    }
}