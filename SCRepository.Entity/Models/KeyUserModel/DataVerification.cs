
namespace SCRepository.Entity.Models.KeyUserModel
{
    public class DataVerification
    {
        public string PublicKeyId { get; set; }

        private string _signingTimeLocal;
        public string SigningTimeLocal
        {
            get { return _signingTimeLocal.Replace('T', ' '); }
            set { _signingTimeLocal = value; }
        }

        public string SigningTimeGreenwich { get; set; }
        /// <summary>
        /// ИД эл.ключа
        /// </summary>
        public string Nick { get; set; }
        public string CommonName { get; set; }
        public string Title { get; set; }
        public string Organization { get; set; }
        public string City { get; set; }
        public string Certificate { get; set; }
        public string Signature { get; set; }
        public int TypeSign { get; set; }
    }
}
