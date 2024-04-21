namespace StudentCardsAdmin.Models
{
    public class Key
    {
        public string KeySerialNumber { get; set; }
        //[Required(ErrorMessage = Validation.ErrorMessageRequiredField)]
        public string KeyPassword { get; set; }
        public string SerialNumber { get; set; }
    }
}
