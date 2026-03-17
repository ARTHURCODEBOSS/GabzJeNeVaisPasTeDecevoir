using Microsoft.Extensions.Caching.Memory;
using OtpNet;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace ClubMedAPI.Services
{
    public interface ITwoFactorService
    {
        string GenerateSmsCode(int userId);
        bool VerifySmsCode(int userId, string code);
        string GenerateTotpSecret();
        string GetTotpUri(string secret, string email, string issuer = "ClubMed");
        bool VerifyTotpCode(string secret, string code);
        Task<bool> SendSmsAsync(string phoneNumber, string code);
    }

    public class TwoFactorService : ITwoFactorService
    {
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TwoFactorService> _logger;

        public TwoFactorService(IMemoryCache cache, IConfiguration configuration, ILogger<TwoFactorService> logger)
        {
            _cache = cache;
            _configuration = configuration;
            _logger = logger;
        }

        // ===== SMS 2FA =====

        public string GenerateSmsCode(int userId)
        {
            var code = Random.Shared.Next(1000, 9999).ToString();
            var cacheKey = $"2fa_sms_{userId}";

            _cache.Set(cacheKey, code, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });

            return code;
        }

        public bool VerifySmsCode(int userId, string code)
        {
            var cacheKey = $"2fa_sms_{userId}";

            if (_cache.TryGetValue(cacheKey, out string? storedCode) && storedCode == code)
            {
                _cache.Remove(cacheKey);
                return true;
            }

            return false;
        }

        public async Task<bool> SendSmsAsync(string phoneNumber, string code)
        {
            var accountSid = _configuration["Twilio:AccountSid"];
            var authToken = _configuration["Twilio:AuthToken"];
            var fromNumber = _configuration["Twilio:FromNumber"];

            // Mode dev : pas de credentials Twilio → log le code en console
            if (string.IsNullOrEmpty(accountSid) || string.IsNullOrEmpty(authToken))
            {
                _logger.LogWarning("[2FA DEV] Code SMS pour {Phone}: {Code}", phoneNumber, code);
                return true;
            }

            try
            {
                TwilioClient.Init(accountSid, authToken);

                await MessageResource.CreateAsync(
                    body: $"Votre code de vérification ClubMed : {code}",
                    from: new Twilio.Types.PhoneNumber(fromNumber),
                    to: new Twilio.Types.PhoneNumber(phoneNumber)
                );

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur envoi SMS Twilio vers {Phone}", phoneNumber);
                return false;
            }
        }

        // ===== TOTP (Google Authenticator / Microsoft Authenticator) =====

        public string GenerateTotpSecret()
        {
            var key = KeyGeneration.GenerateRandomKey(20);
            return Base32Encoding.ToString(key);
        }

        public string GetTotpUri(string secret, string email, string issuer = "ClubMed")
        {
            return $"otpauth://totp/{issuer}:{email}?secret={secret}&issuer={issuer}&digits=6&period=30";
        }

        public bool VerifyTotpCode(string secret, string code)
        {
            try
            {
                var secretBytes = Base32Encoding.ToBytes(secret);
                var totp = new Totp(secretBytes, step: 30, totpSize: 6);

                // Vérifie le code avec une fenêtre de ±1 step (30s de marge)
                return totp.VerifyTotp(code, out _, new VerificationWindow(previous: 1, future: 1));
            }
            catch
            {
                return false;
            }
        }
    }
}
