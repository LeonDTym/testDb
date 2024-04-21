using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCRepository.Entity.Models
{
    [Table("Risk_lvl")]
    public class Risk
    {
        [Key]
        public string risk_lvl { get; set; }
        public string bin { get; set; }
        public string description { get; set; }
        public string info { get; set; }
    }
}
