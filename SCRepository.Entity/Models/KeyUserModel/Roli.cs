using System;
using System.Collections.Generic;
using System.Text;

namespace SCRepository.Entity.Models.KeyUserModel
{
   public class Roli
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public byte CodeARM { get; set; }
        public byte LevelAccess { get; set; }
        public string PoliticAccess { get; set; }
        public string Comment { get; set; }
        public bool? IsDelete { get; set; }
    }
}
