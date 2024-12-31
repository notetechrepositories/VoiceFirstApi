using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace VoiceFirstApi.Utilities
{
    public class SecurityUtilities
    {
        private IConfiguration _iconfiguration;

        public SecurityUtilities(IConfiguration configuration)
        {
            _iconfiguration = configuration;
        }



        public string GetToken(Dictionary<string, string> userClaims)
        {
            // Validate configuration keys
            if (string.IsNullOrWhiteSpace(_iconfiguration["Jwt:Key"]) ||
                string.IsNullOrWhiteSpace(_iconfiguration["Jwt:Issuer"]) ||
                string.IsNullOrWhiteSpace(_iconfiguration["Jwt:Audience"]))
            {
                throw new InvalidOperationException("JWT configuration is missing or invalid.");
            }

            // Initialize a list for the claims
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, _iconfiguration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            // Add user-specific claims from the dictionary
            foreach (var claim in userClaims)
            {
                claims.Add(new Claim(claim.Key, claim.Value));
            }

            // Signing key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_iconfiguration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create token
            var token = new JwtSecurityToken(
                issuer: _iconfiguration["Jwt:Issuer"],
                audience: _iconfiguration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1), // Adjust token expiration
                signingCredentials: signIn
            );

            // Generate token string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static string Encryption(string encryptString)
        {
            string EncryptionKey = "Iu2TXhw0hqwfuRcpjUuMlWKPWA7P4jnX";
            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
                    0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
                });
                encryptor.Key = pdb.GetBytes(32);

                // Generate a new, random IV for each encryption
                encryptor.GenerateIV();
                byte[] iv = encryptor.IV; // Store IV for use in decryption

                using (MemoryStream ms = new MemoryStream())
                {
                    // Prepend IV to the stream for later use in decryption
                    ms.Write(iv, 0, iv.Length);

                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }

                    // Convert the entire stream (IV + encrypted data) to Base64
                    encryptString = Convert.ToBase64String(ms.ToArray());
                }
            }

            return encryptString;
        }

        public static string Decryption(string cipherText)
        {
            string EncryptionKey = "Iu2TXhw0hqwfuRcpjUuMlWKPWA7P4jnX";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
                    0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
                });
                encryptor.Key = pdb.GetBytes(32);

                // Extract IV from the beginning of the cipherBytes array
                byte[] iv = new byte[16];
                Array.Copy(cipherBytes, 0, iv, 0, iv.Length);
                encryptor.IV = iv;

                // Extract the encrypted data (excluding the IV part)
                byte[] actualCipherBytes = new byte[cipherBytes.Length - iv.Length];
                Array.Copy(cipherBytes, iv.Length, actualCipherBytes, 0, actualCipherBytes.Length);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(actualCipherBytes, 0, actualCipherBytes.Length);
                        cs.Close();
                    }
                    return Encoding.Unicode.GetString(ms.ToArray());
                }
            }
        }
        public static string EncryptToken(string token)
        {
            string EncryptionKey = "KJhSbawMrhfqlICFIgaWfaW7ZLSnkLhN";
            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(EncryptionKey);
                aes.IV = new byte[16]; // Initialize IV, ideally use a random IV and prepend it to the encrypted token for enhanced security

                using (var encryptor = aes.CreateEncryptor())
                {
                    byte[] tokenBytes = Encoding.UTF8.GetBytes(token);
                    byte[] encryptedBytes = encryptor.TransformFinalBlock(tokenBytes, 0, tokenBytes.Length);
                    return Convert.ToBase64String(encryptedBytes);
                }
            }
        }

        public static string EncryptModel(string value, string userDeatils)
        {
            string res = string.Empty;
            try
            {
                EncryptvalueModel model = new EncryptvalueModel
                {
                    CurrentDate = DateTime.Now,
                    value = value,
                    userDeatils = userDeatils,

                };
                res = Encryption(JsonConvert.SerializeObject(model));
            }
            catch (Exception)
            {

            }
            return res;
        }



        public static EncryptvalueModel DecryptModel(string valueModel)
        {
            EncryptvalueModel res = null;
            try
            {
                res = JsonConvert.DeserializeObject<EncryptvalueModel>(Decryption(valueModel));
            }
            catch (Exception)
            {

            }
            return res;
        }

        public class EncryptvalueModel
        {
            public DateTime CurrentDate { get; set; }

            public string value { get; set; }
            public string? userDeatils { get; set; }
        }
        
    }
}
