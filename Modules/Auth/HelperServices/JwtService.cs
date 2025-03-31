
using JWT;
using JWT.Algorithms;
using JWT.Serializers;

namespace Services;

public class JwtService
{
    private readonly string _secret;
    private readonly IJwtAlgorithm _algorithm = new HMACSHA256Algorithm();
    private readonly IJsonSerializer _serializer = new JsonNetSerializer();
    private readonly IBase64UrlEncoder _urlEncoder = new JwtBase64UrlEncoder();

    public JwtService()
    {
        _secret = Environment.GetEnvironmentVariable("JWT_SECRET")
                  ?? throw new InvalidOperationException("JWT secret is not configured.");
    }


    public string GenerateToken(string userId, string email, int expirationMinutes = 60)
    {
        var payload = new Dictionary<string, object>
        {
            { "sub", userId },
            { "email", email },
            { "exp", DateTimeOffset.UtcNow.AddMinutes(expirationMinutes).ToUnixTimeSeconds() }
        };

        var encoder = new JwtEncoder(_algorithm, _serializer, _urlEncoder);
        return encoder.Encode(payload, _secret);
    }

    public IDictionary<string, object>? ValidateToken(string token)
    {
        try
        {
            var decoder = new JwtDecoder(_serializer, new JwtValidator(_serializer, new UtcDateTimeProvider()), _urlEncoder, _algorithm);
            return decoder.DecodeToObject<IDictionary<string, object>>(token, _secret, verify: true);
        }
        catch (Exception)
        {
            return null;
        }
    }
}
