using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCRepository.Entity.Models
{
    [Table("School")]
    public class SchoolModel
    {
        [Key]
        public int id { get; set; }
        public string unn { get; set; }
        public string school_name { get; set; }
        public string school_name_short { get; set; }
        public string school_name_card { get; set; }
        public string school_address { get; set; }
        public string school_phone { get; set; }
        public string region { get; set; }
        public string district { get; set; }
        public string locality { get; set; }
        public string email { get; set; }
        public string cbu { get; set; }
    }
}
