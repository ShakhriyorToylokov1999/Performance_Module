using System;
using System.Diagnostics;
using System.Security.Cryptography;

namespace Task1_Performance
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var password = "as$%&hsjasd";
            var salt = new byte[16];
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            
            ActualMethod_GeneratePasswordHashUsingSalt(password, salt);

            stopwatch.Stop();
            var time1 = stopwatch.ElapsedMilliseconds;

            Console.WriteLine("Time for Non-optimized method: {0}", time1);

            stopwatch.Restart();

            OptimizedMethod_GeneratePasswordHashUsingSalt(password, salt);

            stopwatch.Stop();
            var time2 = stopwatch.ElapsedMilliseconds;

            Console.WriteLine("Time for Optimized method: {0}", time2);

        }

        public static string OptimizedMethod_GeneratePasswordHashUsingSalt(string passwordText, byte[] salt)
        {

            var iterate = 10000;
            var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            var passwordHash = Convert.ToBase64String(hashBytes);

            return passwordHash;

        }
        
        public static string ActualMethod_GeneratePasswordHashUsingSalt(string passwordText, byte[] salt)
        {

            var iterate = 10000;
            var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            var passwordHash = Convert.ToBase64String(hashBytes);

            return passwordHash;

        }

    }
}
