using System.Security.Cryptography;
using System.Text;

namespace Dealer_API.Services
{
    public class Encriptar
    {
        public string ConvertirSha256(string inputString) //Encriptacion
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(inputString));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

    }
}
