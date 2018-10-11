using bbl.BBLSecurity.Cryptography;

namespace BBL.Backend.Form.Siriraj.Cryptography
{
    public class Decryption
    {
        public static string Decrypt(string key, string data)
        {
            var aes = new AES(key);

            return aes.Decrypt(data);
        }
    }
}