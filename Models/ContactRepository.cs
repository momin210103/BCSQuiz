using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCSQuiz.Models
{
    public static class ContactRepository
    {
        public static List<Contact> _contacts = new List<Contact>()
       {
            new Contact { ContactId = 1, Name = "Momin",Email="momincse13@gmail.com" },

            new Contact { ContactId = 2, Name = "Sazzad",Email="Momin17@gmail.com" },

            new Contact { ContactId = 3, Name = "Johab", Email = "momincse13@gmail.com" }
       };
        public static List<Contact> GetContacts() => _contacts;
        public static Contact GetContactById(int contactId)
        {
            return _contacts.FirstOrDefault(x => x.ContactId == contactId);
        }
    }
}
