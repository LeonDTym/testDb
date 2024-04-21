using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCRepository.Entity.Models
{
    [Table("StatusZayavka")]
    public class StatusZayavkaModel
    {
        [Key]
        public short id { get; set; }
        public string description { get; set; }
    }
}
