using MergeContact.Interefaces;
using MergeContact.Models;
using MergeContact.Models.Request;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ContactsController : ControllerBase
{
    //private readonly ApplicationDbContext _context;
    private readonly IFuzzyComparer _fuzzyComparer;

    public ContactsController(IFuzzyComparer fuzzyComparer)
    {
        //_context = context;
        _fuzzyComparer = fuzzyComparer;
    }

    [HttpPost("merge")]
    public IActionResult MergeContacts([FromBody] ContactsList contacts)
    {
        var mergedContacts = new List<ContactModel>();
        const int threshold = 80;

        for (int i = 0; i < contacts.Items.Count; i++)
        {
            var currentContact = contacts.Items[i];
            var duplicates = new List<ContactModel> { currentContact };

            for (int j = i + 1; j < contacts.Items.Count; j++)
            {
                var comparisonContact = contacts.Items[j];

                bool nameMatch = _fuzzyComparer.AreSimilar(currentContact.Name, comparisonContact.Name, threshold);

                bool phoneMatch = ComparePhones(currentContact, comparisonContact, threshold);

                bool emailMatch = CompareEmails(currentContact, comparisonContact, threshold);

                if (nameMatch && (phoneMatch || emailMatch))
                {
                    duplicates.Add(comparisonContact);
                }
            }

            var mergedContact = duplicates.First();
            mergedContacts.Add(mergedContact);

            foreach (var duplicate in duplicates.Skip(1))
            {
                contacts.Items.Remove(duplicate);
            }
        }
        return Ok(mergedContacts);
    }

    private ContactModel MergeDuplicateContacts(List<ContactRequestDto> duplicates)
    {
        var baseContact = duplicates.First();

        var mergedContact = new ContactModel
        {
            Id = baseContact.Id,
            Name = baseContact.Name,
            Email = baseContact.Email,
            Phone = baseContact.Phone,
            Observations = string.Empty,
            ContactType_ID = baseContact.ContactType_ID,
            /*Country = string.Empty,
            State = string.Empty,
            Town = string.Empty,
            Neighborhood = string.Empty,
            Zone = string.Empty,
            SocialReason = string.Empty,
            Nationality = string.Empty,
            BirthDate = string.Empty,
            NIF = string.Empty,
            BI = string.Empty,
            Job = string.Empty,
            Notes = string.Empty,*/
        };

        foreach (var contact in duplicates.Skip(1))
        {
            mergedContact.Email = MergeUniqueValues(new[] { mergedContact.Email, contact.Email });

            mergedContact.Phone = MergeUniqueValues(new[] { mergedContact.Phone, contact.Phone });

            mergedContact.Observations += "; " + (contact.Name ?? "No Name");

            mergedContact.ContactType_ID = MergeUniqueValues(new[] { mergedContact.ContactType_ID, contact.ContactType_ID });
        }

        if (string.IsNullOrWhiteSpace(mergedContact.ContactType_ID))
        {
            mergedContact.ContactType_ID = "1";
        }

        return mergedContact;
    }

    private string MergeUniqueValues(IEnumerable<string> values)
    {
        return string.Join(",", values.Where(v => !string.IsNullOrWhiteSpace(v)).Distinct());
    }

    // TODO: Implement a unified method
    private bool ComparePhones(ContactModel contact1, ContactModel contact2, int threshold)
    {
        var phones1 = new List<string> { contact1.Phone, contact1.Phone2, contact1.Phone3, contact1.Phone4 };
        var phones2 = new List<string> { contact2.Phone, contact2.Phone2, contact2.Phone3, contact2.Phone4 };

        return phones1.Any(phone1 =>
            phones2.Any(phone2 =>
                _fuzzyComparer.AreSimilar(phone1, phone2, threshold) &&
                MergeUniqueValues(new[] { contact1.Phone, contact2.Phone }) != contact1.Phone
            )
        );
    }

    private bool CompareEmails(ContactModel contact1, ContactModel contact2, int threshold)
    {
        var emails1 = new List<string> { contact1.Email, contact1.Email2, contact1.Email3, contact1.Email4 };
        var emails2 = new List<string> { contact2.Email, contact2.Email2, contact2.Email3, contact2.Email4 };

        return emails1.Any(email1 =>
            emails2.Any(email2 =>
                _fuzzyComparer.AreSimilar(email1, email2, threshold) &&
                MergeUniqueValues(new[] { contact1.Email, contact2.Email }) != contact1.Email
            )
        );
    }

    //private bool ComparePhones(ContactModel contact1, ContactModel contact2, int threshold)
    //{
    //    var phones1 = new List<string> { contact1.Phone, contact1.Phone2, contact1.Phone3, contact1.Phone4 };
    //    var phones2 = new List<string> { contact2.Phone, contact2.Phone2, contact2.Phone3, contact2.Phone4 };

    //    return phones1.Any(phone1 => phones2.Any(phone2 => _fuzzyComparer.AreSimilar(phone1, phone2, threshold)));
    //}

    //private bool CompareEmails(ContactModel contact1, ContactModel contact2, int threshold)
    //{
    //    var emails1 = new List<string> { contact1.Email, contact1.Email2, contact1.Email3, contact1.Email4 };
    //    var emails2 = new List<string> { contact2.Email, contact2.Email2, contact2.Email3, contact2.Email4 };

    //    return emails1.Any(email1 => emails2.Any(email2 => _fuzzyComparer.AreSimilar(email1, email2, threshold)));
    //}
}


