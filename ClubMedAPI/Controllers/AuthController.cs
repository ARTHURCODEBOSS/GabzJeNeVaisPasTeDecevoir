using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ClubMedAPI.Models.EntityFramework;
using ClubMedAPI.Authorization;
using ClubMedAPI.Services;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace ClubMedAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ClubMedContext _context;
        private readonly IConfiguration _configuration;
        private readonly ITwoFactorService _twoFactorService;

        public AuthController(ClubMedContext context, IConfiguration configuration, ITwoFactorService twoFactorService)
        {
            _context = context;
            _configuration = configuration;
            _twoFactorService = twoFactorService;
        }

        // POST /api/login
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _context.Clients
                .Include(c => c.Adresse)
                .FirstOrDefaultAsync(c => c.Email == dto.Email);

            if (user == null)
                return Unauthorized(new { status = "error", message = "Identifiants incorrects" });

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.MotDePasseCrypter))
                return Unauthorized(new { status = "error", message = "Identifiants incorrects" });

            if (user.A2fActive)
            {
                if (user.A2fMethod == "SMS" && !string.IsNullOrEmpty(user.Telephone))
                {
                    var code = _twoFactorService.GenerateSmsCode(user.NumClient);
                    await _twoFactorService.SendSmsAsync(user.Telephone, code);

                    return Ok(new
                    {
                        status = "2fa_required",
                        method = "SMS",
                        message = "Code envoyé par SMS",
                        idclient = user.NumClient,
                        telephone_masque = user.Telephone.Length > 4
                            ? user.Telephone.Substring(0, 4) + "******"
                            : user.Telephone
                    });
                }
                else if (user.A2fMethod == "TOTP")
                {
                    return Ok(new
                    {
                        status = "2fa_required",
                        method = "TOTP",
                        message = "Entrez le code de votre application d'authentification",
                        idclient = user.NumClient
                    });
                }
            }

            var reservations = await _context.Reservations
                .Include(r => r.Club)
                .Where(r => r.NumClient == user.NumClient)
                .ToListAsync();

            var token = GenerateJwtToken(user);
            SetAuthCookie(token);

            return Ok(new
            {
                status = "success",
                message = "Connexion réussie",
                token,
                user = MapUserResponse(user, reservations),
                role = user.Role
            });
        }

        // POST /api/login/verify-2fa
        [HttpPost("login/verify-2fa")]
        public async Task<ActionResult> VerifyLogin2FA([FromBody] Verify2FALoginDto dto)
        {
            var user = await _context.Clients
                .Include(c => c.Adresse)
                .FirstOrDefaultAsync(c => c.NumClient == dto.IdClient);

            if (user == null)
                return Unauthorized(new { message = "Utilisateur introuvable" });

            // Vérifier le code selon la méthode 2FA
            bool codeValid;
            if (user.A2fMethod == "TOTP" && !string.IsNullOrEmpty(user.TotpSecret))
            {
                codeValid = _twoFactorService.VerifyTotpCode(user.TotpSecret, dto.Code);
            }
            else
            {
                codeValid = _twoFactorService.VerifySmsCode(user.NumClient, dto.Code);
            }

            if (!codeValid)
                return Unauthorized(new { status = "error", message = "Code incorrect ou expiré" });

            var reservations = await _context.Reservations
                .Include(r => r.Club)
                .Where(r => r.NumClient == user.NumClient)
                .ToListAsync();

            var token = GenerateJwtToken(user);
            SetAuthCookie(token);

            return Ok(new
            {
                status = "success",
                token,
                user = MapUserResponse(user, reservations),
                message = "Connexion sécurisée réussie"
            });
        }

        // POST /api/inscription
        [HttpPost("inscription")]
        public async Task<ActionResult> Inscription([FromBody] InscriptionDto dto)
        {
            if (await _context.Clients.AnyAsync(c => c.Email == dto.Email))
                return Conflict(new { message = "Email déjà utilisé" });

            var adresse = new Adresse
            {
                NumRue = dto.NumRue,
                NomRue = dto.NomRue,
                CodePostal = dto.CodePostal,
                Ville = dto.Ville,
                Pays = dto.Pays
            };
            _context.Adresses.Add(adresse);
            await _context.SaveChangesAsync();

            var user = new Client
            {
                Prenom = dto.Prenom,
                Nom = dto.Nom,
                Genre = dto.Genre,
                DateNaissance = dto.DateNaissance != null ? DateOnly.Parse(dto.DateNaissance) : null,
                Email = dto.Email,
                Telephone = dto.Telephone,
                MotDePasseCrypter = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "CLIENT",
                NumAdresse = adresse.NumAdresse
            };
            _context.Clients.Add(user);
            await _context.SaveChangesAsync();

            user.Adresse = adresse;
            var token = GenerateJwtToken(user);
            SetAuthCookie(token);

            return Created("", new
            {
                status = "success",
                message = "Compte créé avec succès",
                token,
                user = MapUserResponse(user, new List<Reservation>())
            });
        }

        // POST /api/modification
        [Authorize]
        [HttpPost("modification")]
        public async Task<ActionResult> ModifierClient([FromBody] ModificationDto dto)
        {
            var client = await _context.Clients
                .Include(c => c.Adresse)
                .FirstOrDefaultAsync(c => c.NumClient == dto.NumClient);

            if (client == null)
                return NotFound(new { message = "Client introuvable" });

            if (client.NumAdresse.HasValue)
            {
                var adresse = await _context.Adresses.FindAsync(client.NumAdresse.Value);
                if (adresse != null)
                {
                    adresse.NumRue = dto.NumRue;
                    adresse.NomRue = dto.NomRue;
                    adresse.CodePostal = dto.CodePostal;
                    adresse.Ville = dto.Ville;
                    adresse.Pays = dto.Pays;
                }
            }
            else
            {
                var adresse = new Adresse
                {
                    NumRue = dto.NumRue,
                    NomRue = dto.NomRue,
                    CodePostal = dto.CodePostal,
                    Ville = dto.Ville,
                    Pays = dto.Pays
                };
                _context.Adresses.Add(adresse);
                await _context.SaveChangesAsync();
                client.NumAdresse = adresse.NumAdresse;
            }

            client.Prenom = dto.Prenom;
            client.Nom = dto.Nom;
            client.Genre = dto.Genre;
            client.Telephone = dto.Telephone;
            if (dto.DateNaissance != null)
                client.DateNaissance = DateOnly.Parse(dto.DateNaissance);

            await _context.SaveChangesAsync();

            var reservations = await _context.Reservations
                .Include(r => r.Club)
                .Where(r => r.NumClient == client.NumClient)
                .ToListAsync();

            client = await _context.Clients
                .Include(c => c.Adresse)
                .FirstOrDefaultAsync(c => c.NumClient == dto.NumClient);

            return Ok(new
            {
                status = "success",
                message = "Informations mises à jour avec succès",
                user = MapUserResponse(client!, reservations)
            });
        }

        // GET /api/check-token
        [Authorize]
        [HttpGet("check-token")]
        public async Task<ActionResult> CheckToken()
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
                return Unauthorized(new { message = "Non autorisé" });

            var userId = ValidateJwtToken(token);
            if (userId == null)
                return Unauthorized(new { message = "Non autorisé" });

            var user = await _context.Clients
                .Include(c => c.Adresse)
                .FirstOrDefaultAsync(c => c.NumClient == userId);

            if (user == null)
                return Unauthorized(new { message = "Non autorisé" });

            var reservations = await _context.Reservations
                .Include(r => r.Club)
                .Include(r => r.ReservationActivites).ThenInclude(ra => ra.Activite)
                .Where(r => r.NumClient == user.NumClient)
                .ToListAsync();

            return Ok(new
            {
                status = "success",
                message = "Session valide",
                user = MapUserResponse(user, reservations)
            });
        }

        // POST /api/logout
        [Authorize]
        [HttpPost("logout")]
        public ActionResult Logout()
        {
            Response.Cookies.Delete("auth_token");
            return Ok(new { message = "Déconnexion réussie" });
        }

        // POST /api/auth/google/callback
        [HttpPost("auth/google/callback")]
        public async Task<ActionResult> GoogleCallback([FromBody] GoogleCallbackDto dto)
        {
            // Verify Google token
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"https://oauth2.googleapis.com/tokeninfo?id_token={dto.Token}");

            if (!response.IsSuccessStatusCode)
                return Unauthorized(new { message = "Token Google invalide" });

            var googleData = await response.Content.ReadFromJsonAsync<GoogleTokenInfo>();
            if (googleData == null || string.IsNullOrEmpty(googleData.Email))
                return Unauthorized(new { message = "Token Google invalide" });

            var user = await _context.Clients
                .Include(c => c.Adresse)
                .FirstOrDefaultAsync(c => c.Email == googleData.Email);

            if (user == null)
            {
                user = new Client
                {
                    Email = googleData.Email,
                    Prenom = googleData.GivenName ?? "Inconnu",
                    Nom = googleData.FamilyName ?? "",
                    MotDePasseCrypter = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString()),
                    Role = "CLIENT",
                    A2fActive = false
                };
                _context.Clients.Add(user);
                await _context.SaveChangesAsync();
            }

            if (user.A2fActive)
            {
                if (user.A2fMethod == "SMS" && !string.IsNullOrEmpty(user.Telephone))
                {
                    var smsCode = _twoFactorService.GenerateSmsCode(user.NumClient);
                    await _twoFactorService.SendSmsAsync(user.Telephone, smsCode);

                    return Ok(new
                    {
                        status = "2fa_required",
                        method = "SMS",
                        message = "Code envoyé par SMS",
                        idclient = user.NumClient,
                        telephone_masque = user.Telephone.Substring(0, Math.Min(4, user.Telephone.Length)) + "******"
                    });
                }
                else if (user.A2fMethod == "TOTP")
                {
                    return Ok(new
                    {
                        status = "2fa_required",
                        method = "TOTP",
                        message = "Entrez le code de votre application d'authentification",
                        idclient = user.NumClient
                    });
                }
            }

            var reservations = await _context.Reservations
                .Include(r => r.Club)
                .Where(r => r.NumClient == user.NumClient)
                .ToListAsync();

            var token = GenerateJwtToken(user);
            SetAuthCookie(token);

            return Ok(new
            {
                status = "success",
                message = "Connexion Google réussie",
                token,
                user = MapUserResponse(user, reservations),
                role = user.Role
            });
        }

        // POST /api/compte/reinitialiser-mdp
        [Authorize]
        [HttpPost("compte/reinitialiser-mdp")]
        public async Task<ActionResult> ReinitialiserMotDePasse([FromBody] ResetPasswordDto dto)
        {
            var authToken = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
            var userId = ValidateJwtToken(authToken ?? "");
            if (userId == null) return Unauthorized();

            var user = await _context.Clients.FindAsync(userId);
            if (user == null) return NotFound();

            if (!BCrypt.Net.BCrypt.Verify(dto.AncienMdp, user.MotDePasseCrypter))
                return UnprocessableEntity(new { errors = new { ancien_mdp = new[] { "L'ancien mot de passe est incorrect." } } });

            user.MotDePasseCrypter = BCrypt.Net.BCrypt.HashPassword(dto.NouveauMdp);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Mot de passe mis à jour avec succès !" });
        }

        // POST /api/send-2fa
        [Authorize]
        [HttpPost("send-2fa")]
        public async Task<ActionResult> Send2FA([FromBody] Send2FADto dto)
        {
            var user = await _context.Clients.FindAsync(dto.IdClient);
            if (user == null) return NotFound(new { message = "Utilisateur introuvable" });
            if (string.IsNullOrEmpty(user.Telephone))
                return BadRequest(new { message = "Aucun numéro de téléphone." });

            var code = _twoFactorService.GenerateSmsCode(user.NumClient);
            var sent = await _twoFactorService.SendSmsAsync(user.Telephone, code);

            if (!sent)
                return StatusCode(500, new { message = "Erreur lors de l'envoi du SMS." });

            return Ok(new
            {
                status = "success",
                message = $"Un code a été envoyé au {user.Telephone}"
            });
        }

        // POST /api/verifier-2fa (activer la 2FA SMS)
        [Authorize]
        [HttpPost("verifier-2fa")]
        public async Task<ActionResult> Verifier2FA([FromBody] Verify2FADto dto)
        {
            var user = await _context.Clients.FindAsync(dto.IdClient);
            if (user == null) return NotFound();

            if (!_twoFactorService.VerifySmsCode(user.NumClient, dto.Code))
                return BadRequest(new { status = "error", message = "Code incorrect ou expiré." });

            user.A2fActive = true;
            user.A2fMethod = "SMS";
            user.TotpSecret = null;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                status = "success",
                message = "Authentification à deux facteurs (SMS) activée avec succès !"
            });
        }

        // POST /api/desactiver-2fa
        [Authorize]
        [HttpPost("desactiver-2fa")]
        public async Task<ActionResult> Desactiver2FA([FromBody] Verify2FADto dto)
        {
            var user = await _context.Clients.FindAsync(dto.IdClient);
            if (user == null) return NotFound();

            // Vérifier le code avant de désactiver
            bool codeValid;
            if (user.A2fMethod == "TOTP" && !string.IsNullOrEmpty(user.TotpSecret))
                codeValid = _twoFactorService.VerifyTotpCode(user.TotpSecret, dto.Code);
            else
                codeValid = _twoFactorService.VerifySmsCode(user.NumClient, dto.Code);

            if (!codeValid)
                return BadRequest(new { status = "error", message = "Code incorrect ou expiré." });

            user.A2fActive = false;
            user.A2fMethod = null;
            user.TotpSecret = null;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                status = "success",
                message = "Authentification à deux facteurs désactivée."
            });
        }

        // POST /api/setup-totp (générer le secret TOTP + URI pour QR code)
        [Authorize]
        [HttpPost("setup-totp")]
        public async Task<ActionResult> SetupTotp([FromBody] Send2FADto dto)
        {
            var user = await _context.Clients.FindAsync(dto.IdClient);
            if (user == null) return NotFound();

            var secret = _twoFactorService.GenerateTotpSecret();
            var uri = _twoFactorService.GetTotpUri(secret, user.Email);

            // Stocker temporairement le secret (pas encore activé)
            user.TotpSecret = secret;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                status = "success",
                secret,
                otpauth_uri = uri,
                message = "Scannez le QR code avec votre application d'authentification, puis confirmez avec un code."
            });
        }

        // POST /api/verifier-totp (activer la 2FA TOTP après scan du QR)
        [Authorize]
        [HttpPost("verifier-totp")]
        public async Task<ActionResult> VerifierTotp([FromBody] Verify2FADto dto)
        {
            var user = await _context.Clients.FindAsync(dto.IdClient);
            if (user == null) return NotFound();

            if (string.IsNullOrEmpty(user.TotpSecret))
                return BadRequest(new { message = "Aucun secret TOTP configuré. Appelez d'abord /setup-totp." });

            if (!_twoFactorService.VerifyTotpCode(user.TotpSecret, dto.Code))
                return BadRequest(new { status = "error", message = "Code TOTP incorrect." });

            user.A2fActive = true;
            user.A2fMethod = "TOTP";
            await _context.SaveChangesAsync();

            return Ok(new
            {
                status = "success",
                message = "Authentification à deux facteurs (TOTP) activée avec succès !"
            });
        }

        // ==================== HELPERS ====================

        private string GenerateJwtToken(Client user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"] ?? "ClubMedSuperSecretKeyForJwtToken2026!!"
            ));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.NumClient.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role ?? "CLIENT")
            };

            var token = new JwtSecurityToken(
                issuer: "ClubMedAPI",
                audience: "ClubMedFrontend",
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private void SetAuthCookie(string token)
        {
            Response.Cookies.Append("auth_token", token, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Lax,
                Secure = false,
                Expires = DateTimeOffset.Now.AddDays(30),
                Path = "/"
            });
        }

        private int? ValidateJwtToken(string token)
        {
            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    _configuration["Jwt:Key"] ?? "ClubMedSuperSecretKeyForJwtToken2026!!"
                ));
                var handler = new JwtSecurityTokenHandler();
                var result = handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = true,
                    ValidIssuer = "ClubMedAPI",
                    ValidateAudience = true,
                    ValidAudience = "ClubMedFrontend",
                    ClockSkew = TimeSpan.Zero
                }, out _);

                var userId = result.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return userId != null ? int.Parse(userId) : null;
            }
            catch
            {
                return null;
            }
        }

        private object MapUserResponse(Client user, List<Reservation> reservations)
        {
            return new
            {
                numclient = user.NumClient,
                numadresse = user.NumAdresse,
                genre = user.Genre,
                prenom = user.Prenom,
                nom = user.Nom,
                datenaissance = user.DateNaissance?.ToString("yyyy-MM-dd"),
                email = user.Email,
                telephone = user.Telephone,
                role = user.Role,
                a2f_active = user.A2fActive,
                a2f_method = user.A2fMethod,
                adresse = user.Adresse != null ? new
                {
                    numadresse = user.Adresse.NumAdresse,
                    numrue = user.Adresse.NumRue,
                    nomrue = user.Adresse.NomRue,
                    codepostal = user.Adresse.CodePostal,
                    ville = user.Adresse.Ville,
                    pays = user.Adresse.Pays
                } : null,
                reservations = reservations.Select(r => new
                {
                    numreservation = r.NumReservation,
                    idclub = r.IdClub,
                    idtransport = r.IdTransport,
                    numclient = r.NumClient,
                    datedebut = r.DateDebut?.ToString("yyyy-MM-dd"),
                    datefin = r.DateFin?.ToString("yyyy-MM-dd"),
                    nbpersonnes = r.NbPersonnes,
                    lieudepart = r.LieuDepart,
                    prix = r.Prix,
                    statut = r.Statut,
                    etat_calcule = r.EtatCalcule,
                    veut_annuler = r.VeutAnnuler,
                    club = r.Club != null ? new
                    {
                        idclub = r.Club.IdClub,
                        titre = r.Club.Titre,
                        description = r.Club.Description,
                        numphoto = r.Club.NumPhoto,
                        numpays = r.Club.NumPays
                    } : null
                })
            };
        }
    }

    // ==================== DTOs ====================

    public class LoginDto
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }

    public class Verify2FALoginDto
    {
        public int IdClient { get; set; }
        public string Code { get; set; } = "";
    }

    public class InscriptionDto
    {
        public string Prenom { get; set; } = "";
        public string Nom { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string? Telephone { get; set; }
        public string? Genre { get; set; }
        public string? DateNaissance { get; set; }
        public int NumRue { get; set; }
        public string NomRue { get; set; } = "";
        public string CodePostal { get; set; } = "";
        public string Ville { get; set; } = "";
        public string Pays { get; set; } = "";
    }

    public class ModificationDto
    {
        public int NumClient { get; set; }
        public string Prenom { get; set; } = "";
        public string Nom { get; set; } = "";
        public string Email { get; set; } = "";
        public string? Telephone { get; set; }
        public string? Genre { get; set; }
        public string? DateNaissance { get; set; }
        public int? NumAdresse { get; set; }
        public int NumRue { get; set; }
        public string NomRue { get; set; } = "";
        public string CodePostal { get; set; } = "";
        public string Ville { get; set; } = "";
        public string Pays { get; set; } = "";
    }

    public class GoogleCallbackDto
    {
        public string Token { get; set; } = "";
    }

    public class GoogleTokenInfo
    {
        public string? Email { get; set; }
        public string? GivenName { get; set; }
        public string? FamilyName { get; set; }
    }

    public class ResetPasswordDto
    {
        public string AncienMdp { get; set; } = "";
        public string NouveauMdp { get; set; } = "";
    }

    public class Send2FADto
    {
        public int IdClient { get; set; }
    }

    public class Verify2FADto
    {
        public int IdClient { get; set; }
        public string Code { get; set; } = "";
    }
}
