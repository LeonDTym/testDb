using System;

namespace StudentCardsAdmin.Models
{

    public class StudentsView
    {

        public string Name { get; set; }
        public string SureName { get; set; }
        public string PatronymicName { get; set; }
        public DateTime YearBirthday { get; set; }
        public int SchoolID { get; set; }
        public string Class { get; set; }
        public DateTime EndTraining { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public short PersData { get; set; }
        public bool ChboxNedCard { get; set; }
    }
}
