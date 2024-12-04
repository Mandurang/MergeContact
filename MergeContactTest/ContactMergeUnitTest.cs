using MergeContact.Interefaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using MergeContact.Models;
using Microsoft.AspNetCore.Mvc;

namespace MergeContactTest
{
    public class ContactMergeUnitTest
    {
        private readonly Mock<IFuzzyComparer> _mockFuzzyComparer;
        private readonly ContactsController _controller;

        public ContactMergeUnitTest()
        {
            _mockFuzzyComparer = new Mock<IFuzzyComparer>();
            _controller = new ContactsController(_mockFuzzyComparer.Object);
        }

        [Theory]
        [MemberData(nameof(GetContactTestData))]
        public void MergeContacts_ReturnsMergedContacts_WhenDuplicatesExist(ContactsList contacts, int threshold, int expectedCount)
        {
            // Arrange
            _mockFuzzyComparer
                .Setup(c => c.AreSimilar(It.IsAny<string>(), It.IsAny<string>(), threshold))
                .Returns((string s1, string s2, int t) => s1 == s2 || (s1.Contains("Doe") && s2.Contains("Doe")));

            // Act
            var result = _controller.MergeContacts(contacts) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            var mergedContacts = result.Value as List<ContactModel>;
            mergedContacts.Should().NotBeNull();
            Assert.Equal(expectedCount, mergedContacts.Count);
        }

        public static IEnumerable<object[]> GetContactTestData()
        {
            yield return new object[]
            {
                new ContactsList
                {
                    Items = new List<ContactModel>
                    {
                        new ContactModel { Id = 1, Name = "John Doe", Phone = "12345", Email = "john.doe@example.com" },
                        new ContactModel { Id = 2, Name = "Jon Doe", Phone = "12345", Email = "jon.doe@example.com" },
                        new ContactModel { Id = 3, Name = "Jane Smith", Phone = "67890", Email = "jane.smith@example.com" }
                    }
                },
                80, // Threshold
                1  // Expected merged contacts count
            };

            yield return new object[]
            {
                new ContactsList
                {
                    Items = new List<ContactModel>
                    {
                        new ContactModel { Id = 1, Name = "Alice", Phone = "11111", Email = "alice@example.com" },
                        new ContactModel { Id = 2, Name = "Bob", Phone = "22222", Email = "bob@example.com" },
                        new ContactModel { Id = 3, Name = "Charlie", Phone = "33333", Email = "charlie@example.com" }
                    }
                },
                90, // Threshold
                3  // Expected merged contacts count (no duplicates)
            };

            yield return new object[]
            {
                new ContactsList
                {
                    Items = new List<ContactModel>()
                },
                80,
                0  // Empty list results in 0 merged contacts
            };
        }

        [Fact]
        public void MergeContacts_ShouldGroupSimilarContacts()
        {
            // Arrange
            var contacts = new ContactsList
            {
                Items = new List<ContactModel>
                {
                    new ContactModel { Name = "John Doe", Phone = "12345", Email = "john@example.com" },
                    new ContactModel { Name = "John Do", Phone = "12345", Email = "john.doe@example.com" },
                    new ContactModel { Name = "Jon Doe", Phone = "123456", Email = "john@example.com" },
                    new ContactModel { Name = "Jane Smith", Phone = "98765", Email = "jane@example.com" },
                }
            };

            // Act
            var result = MergeContacts(contacts) as OkObjectResult;
            var mergedContacts = result.Value as List<ContactModel>;

            // Assert
            mergedContacts.Should().HaveCount(2); // John Doe и его дубликаты в одной группе, Jane Smith - отдельно
            mergedContacts[0].Name.Should().Be("John Doe"); // Имя из первого контакта
            mergedContacts[0].Email.Should().Contain("john@example.com");
            mergedContacts[0].Email.Should().Contain("john.doe@example.com");
        }

    }
}
