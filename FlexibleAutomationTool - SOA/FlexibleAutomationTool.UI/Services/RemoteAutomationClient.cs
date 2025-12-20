using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using FlexibleAutomationTool.Common.Contracts;
using System.Collections.Generic;
using System;
using System.Security.Cryptography;

namespace FlexibleAutomationTool.UI.Services
{
    public class RemoteAutomationClient
    {
        private readonly HttpClient _http;

        public RemoteAutomationClient(HttpClient http, string? jwtToken = null)
        {
            _http = http ?? throw new ArgumentNullException(nameof(http));
            if (!string.IsNullOrEmpty(jwtToken))
                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);
        }

        public async Task<IEnumerable<RuleDto>> GetRulesAsync()
        {
            var resp = await _http.GetFromJsonAsync<IEnumerable<RuleDto>>("/api/rules");
            return resp ?? Array.Empty<RuleDto>();
        }

        public async Task ExecuteRuleAsync(int id, string? sensitiveMacro = null)
        {
            HttpContent? content = null;
            if (!string.IsNullOrEmpty(sensitiveMacro))
            {
                try
                {
                    var pubResp = await _http.GetFromJsonAsync<Dictionary<string, string>>("/api/crypto/publickey");
                    if (pubResp != null && pubResp.TryGetValue("PublicKey", out var pubBase64) && !string.IsNullOrEmpty(pubBase64))
                    {
                        var cipher = EncryptWithServerPublicKey(pubBase64, sensitiveMacro);
                        content = new StringContent("ENC:" + Convert.ToBase64String(cipher), Encoding.UTF8, "text/plain");
                    }
                    else
                    {
                        content = new StringContent(sensitiveMacro, Encoding.UTF8, "text/plain");
                    }
                }
                catch
                {
                    content = new StringContent(sensitiveMacro, Encoding.UTF8, "text/plain");
                }
            }

            var resp = await _http.PostAsync($"/api/rules/{id}/execute", content ?? new StringContent(string.Empty));
            resp.EnsureSuccessStatusCode();
        }

        private static byte[] EncryptWithServerPublicKey(string publicKeyBase64, string plain)
        {
            var spki = Convert.FromBase64String(publicKeyBase64);
            using var rsa = RSA.Create();
            rsa.ImportSubjectPublicKeyInfo(spki, out _);
            var bytes = Encoding.UTF8.GetBytes(plain);
            return rsa.Encrypt(bytes, RSAEncryptionPadding.OaepSHA256);
        }
    }
}
