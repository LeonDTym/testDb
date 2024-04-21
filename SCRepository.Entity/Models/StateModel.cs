using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCRepository.Entity.Models
{
    [Table("State")]
    public class StateModel
    {
        [Key]
        public short id { get; set; }
        public string state_code { get; set; }
        public string description { get; set; }
    }
}
