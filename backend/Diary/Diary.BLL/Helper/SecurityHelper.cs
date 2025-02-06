using Diary.DAL.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Diary.BLL.Helper
{
    public static class SecurityHelper
    {
        public static string GenerateToken()
        {
            var tokenBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(tokenBytes);
            }
            return Base64UrlEncoder.Encode(tokenBytes);
        }

        public static DateTime GetJwtExpirationDate(ClaimsPrincipal user)
        {
            var expClaim = user.FindFirst("exp")?.Value;


            if (expClaim != null && long.TryParse(expClaim, out long expUnixTime))
            {
                var expirationTime = DateTimeOffset.FromUnixTimeSeconds(expUnixTime).DateTime.ToLocalTime();
                Console.WriteLine("exp date"+expirationTime.ToString());
                return expirationTime;
                
            }
            return new DateTime();
        }
    }
}