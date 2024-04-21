using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SCRepository.Entity.Models.KeyUserModel
{
    public class ResultDataVerificationOnServer
    {
        public List<DataVerification> ListDataVerification
        {
            get; private set;
        }

        public ResultDataVerificationOnServer() { }

        public ResultDataVerificationOnServer(string info)
        {
            try
            {
                XDocument doc = XDocument.Parse(info);
                if (doc.Root.Name.ToString() == "VerifingReport")
                {
                    ListDataVerification = (from s in doc.Descendants("SignerInfo")
                                            select new DataVerification
                                            {
                                                PublicKeyId = ElementValue(s, "PublicKeyId"),
                                                Nick = ElementValue(s, "Nick"),
                                                City = ElementValue(s, "City"),
                                                CommonName = ElementValue(s, "CommonName"),
                                                Organization = ElementValue(s, "Organization"),
                                                Title = ElementValue(s, "Title"),
                                                Certificate = ElementValue(s, "Certificate"),
                                                Signature = ElementValue(s, "Signature"),
                                                SigningTimeGreenwich = DeleteMusor(s.Element("SigningTime")?.Attribute("Greenwich").Value ?? ""),
                                                SigningTimeLocal = DeleteMusor(s.Element("SigningTime")?.Attribute("Local").Value ?? "")
                                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter outFile = new StreamWriter(@"d:\asb\WebVerifyErrors.txt", true))
                {
                    outFile.WriteLine("{1}\t>>> VerifyInfo - {0}", ex.Message, DateTime.Now.ToString());
                }
            }
        }

        private string ElementValue(XElement element, string name)
        {
            if (!element.Elements(name).Any())
                return "";
            return DeleteMusor(ValueXElement(element.Element(name)));
        }

        public string MyToString()
        {
            StringBuilder result = new StringBuilder();
            foreach (var item in ListDataVerification)
            {
                //result.AppendFormat("---Электронно цифровая подпись:---------\n");
                result.AppendFormat("ИД эл. кл.: {0} {1}\n", item.Nick, item.PublicKeyId);
                result.AppendFormat("Владелец ключа: {0} {1} {2}\n", item.Organization, item.CommonName, item.Title);
                result.AppendFormat("Подписано: {0}\n", item.SigningTimeLocal);
                result.AppendFormat("{0}\n", item.Signature);
            }
            return result.ToString();
        }

        private string DeleteMusor(string info)
        {
            char[] mas = { '\n', 't' };
            foreach (var item in mas)
                info = info.Replace(item, ' ');
            return info.Trim();
        }

        private string ValueXElement(XElement value)
        {
            return value?.Value ?? "";
        }
    }

}
