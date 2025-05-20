using Backend.Core.Interfaces.Services;
using Backend.Core.Options;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace Backend.Application.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly PasswordOptions _options;

        public PasswordService(
            IOptions<PasswordOptions> options
        )
        {
            _options = options.Value;
        }

        public string Hash(string password)
        {
            var bytes = SHA512.HashData(Encoding.UTF8.GetBytes(password));

            var stringBuilder = new StringBuilder();

            foreach (var t in bytes)
            {
                stringBuilder.Append(t.ToString("x2"));
            }

            return stringBuilder.ToString();
        }

        public bool Check(string hash, string password)
        {
            var bytes = SHA512.HashData(Encoding.UTF8.GetBytes(password));

            var stringBuilder = new StringBuilder();

            foreach (var t in bytes)
            {
                stringBuilder.Append(t.ToString("x2"));
            }

            return stringBuilder.Equals(hash);
        }
    }
}
