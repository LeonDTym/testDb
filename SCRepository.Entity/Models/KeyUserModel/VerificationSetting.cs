using System;
using System.Collections.Generic;
using System.Text;

namespace SCRepository.Entity.Models.KeyUserModel
{
    public class VerificationSetting
    {
        public string FileCryptographyLoggingError { get; set; }
        public string IpCryptography { get; set; }
        public int PortCryptography { get; set; }
    }
}
