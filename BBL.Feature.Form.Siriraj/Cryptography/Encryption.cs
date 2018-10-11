using bbl.BBLSecurity.Cryptography;

namespace BBL.Feature.Form.Siriraj.Cryptography
{
    public class Encryption
    {
        public static string Encrypt(string key, string data)
        {
            var aes = new AES(key);

            return aes.Encrypt(data);
        }
    }
}