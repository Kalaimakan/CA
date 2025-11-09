using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        // EF Core-க்காக ஒரு private constructor
        private User() { }

        // புதிய User-ஐ உருவாக்க
        public User(Guid id, string email, string passwordHash, string firstName, string lastName)
        {
            Id = id;
            Email = email;
            PasswordHash = passwordHash;
            FirstName = firstName;
            LastName = lastName;
        }

        // தேவைப்பட்டால் User-ஐ update செய்ய
        public void UpdateDetails(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
