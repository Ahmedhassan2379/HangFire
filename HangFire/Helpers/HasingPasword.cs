using System.Security.Cryptography;

namespace HangFire.Helpers
{
    public class HasingPasword
    {
        public static RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        private static readonly int Saltsize = 16; 
        private static readonly int Hashsize = 16; 
        private static readonly int Iterations = 10000;
        
        public static string HashPassword(string password)
        {
            byte[] salt;
            rng.GetBytes(salt = new byte[Saltsize]);
            var key = new Rfc2898DeriveBytes(password,salt,Iterations);
            var hash = key.GetBytes(Hashsize);
            var hashBytes = new byte[Saltsize + Hashsize];
            Array.Copy(salt,0,hashBytes,0,Saltsize);
            Array.Copy(hash, 0, hashBytes, Saltsize, Hashsize);
            var base64hash = Convert.ToBase64String(hashBytes);
            return base64hash;
        }
        public static bool VerifyPassword(string password , string base64hash)
        {
            var hashBytes = Convert.FromBase64String(base64hash);
            var salt = new byte[Saltsize];
            Array.Copy(hashBytes, 0, salt, 0, Saltsize);
            var key = new Rfc2898DeriveBytes(password, salt,Iterations);
            byte[] hash = key.GetBytes(Hashsize);
            for (int i = 0; i < Hashsize; i++)
            {
                if (hashBytes[i+Saltsize] != hash[i])
                {
                    return false;
                }

            }
            return true;
        }
    } 
}
