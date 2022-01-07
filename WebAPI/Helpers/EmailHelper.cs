using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace WebAPI.Helpers
{
    public static class EmailHelper
    {
            public static string Encrypt(string strText, string strPublicKey)
            {
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(strPublicKey);

                byte[] byteText = Encoding.UTF8.GetBytes(strText);
                byte[] byteEntry = rsa.Encrypt(byteText, false);

                return Convert.ToBase64String(byteEntry);
            }


            public static string Decrypt(string strEntryText, string strPrivateKey)
            {
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(strPrivateKey);

                byte[] byteEntry = Convert.FromBase64String(strEntryText);
                byte[] byteText = rsa.Decrypt(byteEntry, false);

                return Encoding.UTF8.GetString(byteText);
            }

            public static Dictionary<string,string> GetKey()
            {
                Dictionary<string, string> dictKey = new Dictionary<string, string>();
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

                dictKey.Add("PublicKey", rsa.ToXmlString(false));
                dictKey.Add("PrivateKey", rsa.ToXmlString(true));

                return dictKey;
        }
    }
}
