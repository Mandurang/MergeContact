namespace MergeContact.Models.Result
{
    public class ContactResult
    {
        public int ContactId { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Observations { get; set; }
        public string ContactType_ID { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? Town { get; set; }
        public string? Neighborhood { get; set; }
        public string? Zone { get; set; }
        public string? SocialReason { get; set; }
        public string? Nationality { get; set; }
        public string? BirthDate { get; set; }
        public string? NIF { get; set; }
        public string? BI { get; set; }
        public string? Job { get; set; }
        public string? Notes { get; set; }

        public ContactResult()
        {
        }

        //public ContactResult(ContactRequestDto contactRequest, ContactResult contactResult)
        //{
        //    ContactId = contactRequest.Id;

        //    if (contactResult.Count > 0)
        //    {
        //        Name = contactResult.Name;
        //        Email = contactResult.Email;

        //        Phone = contactResult.Phone;
        //        Country = contactResult.Phone;
        //    }
        //}
    }
}
