using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo
{
    public static class PasswordHash
    {
        private static string GetRandomSalt() // Den her metode retunerer et random salt som bliver gemt i password hash'en
        {
            return BCrypt.Net.BCrypt.GenerateSalt();
        }

        public static string HashPassword(string password) // Her bliver passwordet hashet, samt ved hjælpe af metoden over kommer der et random salt ind
        {
            return BCrypt.Net.BCrypt.HashPassword(password, GetRandomSalt());
        }

        public static bool ValidatePassword(string password, string correctHash) // Her bliver burger inputtet(passwordet) samt det korrekte hashede password valideret.
        {
            return BCrypt.Net.BCrypt.Verify(password, correctHash);
        }
    }
}
