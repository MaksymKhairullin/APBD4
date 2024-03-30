using System;

namespace LegacyApp
{
    public class User
    {
        public object Client { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool HasCreditLimit { get; set; }
        public int CreditLimit { get; set; }
    }
}