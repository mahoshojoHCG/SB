using System;
using System.Security.Cryptography;
using System.Text;
using Integrative.Encryption;
using YamlDotNet.Serialization;

namespace AutoReport
{
    public class ReportInformation
    {
        public string UserName { get; set; }
        public string EncryptedPassword { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        [YamlIgnore]
        public string DeEncryptedPassword
        {
            get =>
                Encoding.UTF8.GetString(CrossProtect.Unprotect(Convert.FromBase64String(EncryptedPassword), null,
                    DataProtectionScope.CurrentUser));
            set =>
                EncryptedPassword = Convert.ToBase64String(CrossProtect.Protect(Encoding.UTF8.GetBytes(value), null,
                    DataProtectionScope.CurrentUser));
        }
    }
}