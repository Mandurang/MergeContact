using System.ComponentModel.DataAnnotations;

namespace MergeContact.Models
{
    public class ContactModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public string? Email2 { get; set; }
        public string? Email3 { get; set; }
        public string? Email4 { get; set; }
        public string? Phone { get; set; }
        public string? Phone2 { get; set; }
        public string? Phone3 { get; set; }
        public string? Phone4 { get; set; }
        public string? Observations { get; set; }
        public string ContactType_ID { get; set; }
        /*public string Country { get; set; }
        public string State { get; set; }
        public string Town { get; set; }
        public string Neighborhood { get; set; }
        public string Zone { get; set; }
        public string SocialReason { get; set; }
        public string Nationality { get; set; }
        public string BirthDate { get; set; }
        public string NIF { get; set; }
        public string BI { get; set; }
        public string Job { get; set; }
        public string Notes { get; set; }*/
    }
}
