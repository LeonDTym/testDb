using System;
using System.Collections.Generic;
using System.Text;

namespace SCRepository.Entity.Models.KeyUserModel
{
   public class ListRoliInfo
    {
        public int? CodeID { get; set; }
        public int? RoliID { get; set; }
        public int? TypeOfAccessRightsID { get; set; }
        public int ArmID { get; set; }
        public int ObjectID { get; set; }
        public int? CodeCrypt { get; set; }
        public string CodeCryptChar { get; set; }
        public string NameObject { get; set; }
        public string NameRight { get; set; }

    }
}
