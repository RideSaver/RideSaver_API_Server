using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace UserService.Security
{
    public static class Argon2
    {
        public static byte[] CreateSalt()
        {
            var buffer = new byte[16];
            var RNG = RandomNumberGenerator.Create();
            RNG.GetBytes(buffer);
            return buffer;
        }
        public static byte[] HashPassword(string password, byte[] salt)
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));
            argon2.Salt = salt;
            argon2.DegreeOfParallelism = 8; // four cores
            argon2.Iterations = 4;
            argon2.MemorySize = 1024 * 1024; // 1 GB
            return argon2.GetBytes(16);
        }

        public static bool VerifyHash(string password, byte[] hash, byte[] salt)
        {
            var newHash = HashPassword(password, salt);
            return hash.SequenceEqual(newHash);
        }
    }
}
