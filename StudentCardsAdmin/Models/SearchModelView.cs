using System;

namespace StudentCardsAdmin.Models
{
    public class SearchModelView
    {
        public string? UniqueId { get; set; }
        public string? Name { get; set; }
        public string? SName { get; set; }
        public string? TName { get; set; }
        public string? Class { get; set; }
        public string? YearBirthday { get; set; }
        public DateTime? EndTraining { get; set; }
        public string CheckAct { get; set; }
        public string CheckPhoto { get; set; }
        public string CheckSch { get; set; }
    }
}
