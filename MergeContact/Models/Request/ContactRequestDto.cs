namespace MergeContact.Models.Request
{
    public class ContactRequestDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string ContactType_ID { get; set; }
    }
}
