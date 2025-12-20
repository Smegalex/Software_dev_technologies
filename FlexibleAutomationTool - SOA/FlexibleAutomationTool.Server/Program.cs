using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using FlexibleAutomationTool.Common.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Configuration - for demo store in config; replace with user secrets or Key Vault
var jwtKey = builder.Configuration["Jwt:Key"] ?? "dev-super-secret-key-please-change";
var issuer = "FlexibleAutomation";
var audience = "FlexibleAutomationClients";

builder.Services.AddSingleton<IFlexibleAutomationService, InMemoryFlexibleAutomationService>();
builder.Services.AddSingleton<CryptoService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
    };
});

builder.Services.AddAuthorization();
builder.Services.AddRouting();

var app = builder.Build();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/api/rules", (IFlexibleAutomationService svc) => Results.Ok(svc.GetRules()))
   .RequireAuthorization();

app.MapPost("/api/rules/{id}/execute", async (int id, IFlexibleAutomationService svc, CryptoService crypto, HttpRequest req) =>
{
    using var sr = new StreamReader(req.Body, Encoding.UTF8);
    var body = await sr.ReadToEndAsync();
    string? decrypted = null;

    if (!string.IsNullOrEmpty(body))
    {
        if (body.StartsWith("ENC:", StringComparison.Ordinal))
        {
            var b64 = body.Substring(4);
            decrypted = crypto.DecryptBase64(b64);
        }
        else
        {
            decrypted = body;
        }
    }

    svc.ExecuteRule(id, decrypted);
    return Results.NoContent();
}).RequireAuthorization();

app.MapGet("/api/crypto/publickey", (CryptoService crypto) =>
{
    return Results.Ok(new { PublicKey = crypto.GetPublicKeyBase64() });
});

app.MapPost("/token", () =>
{
    var now = DateTime.UtcNow;
    var claims = new[] { new System.Security.Claims.Claim("sub", "demo-user") };
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
        issuer, audience, claims, expires: now.AddHours(8), signingCredentials: creds);
    var tokenString = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);
    return Results.Ok(new { token = tokenString });
});

app.Run();

// ---- Demo in-memory service (replace with your Injection of existing repository/engine) ----
public class InMemoryFlexibleAutomationService : IFlexibleAutomationService
{
    private readonly List<RuleDto> _rules = new()
    {
        new RuleDto(1, "Hello", "MSG Hello from server"),
        new RuleDto(2, "Open Notepad", "RUN notepad.exe")
    };

    public IEnumerable<RuleDto> GetRules() => _rules;

    public void ExecuteRule(int id, string? decryptedPayload = null)
    {
        var rule = _rules.Find(r => r.Id == id);
        if (rule is null)
            return;

        var macroToRun = decryptedPayload ?? rule.MacroText;
        Console.WriteLine($"[Server] Executing rule {id}: {macroToRun}");
    }
}

// ---- Simple RSA crypto holder used by server to decrypt client payloads ----
public class CryptoService
{
    private readonly RSA _rsa;

    public CryptoService()
    {
        _rsa = RSA.Create(2048);
    }

    public string GetPublicKeyBase64()
    {
        var spki = _rsa.ExportSubjectPublicKeyInfo();
        return Convert.ToBase64String(spki);
    }

    public string DecryptBase64(string base64Cipher)
    {
        var cipher = Convert.FromBase64String(base64Cipher);
        var plain = _rsa.Decrypt(cipher, RSAEncryptionPadding.OaepSHA256);
        return Encoding.UTF8.GetString(plain);
    }
}