using System;

namespace LegacyApp
{
    public class UserService
    {
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (IsFirstNameInCorrect(firstName) || IsLastNameInCorrect(lastName))
            {
                return false;
            }
            

            if (IsEmailInCorrect(email))
            {
                return false;
            }

            var age = CheckAndCalculateAge(dateOfBirth);

            if (age < 21)
            {
                return false;
            }

            var client = CreateClient(firstName, lastName, email, dateOfBirth, clientId, out var user);

            if (IsClientVeryImportant(client))
            {
                user.HasCreditLimit = false;
            }
            else if (IsClientImportant(client))
            {
                DetailsForImportantClient(user);
            }
            else
            {
                DetailsForNormalClient(user);
            }

            if (UserHasCreditLimitLes500(user)) 
            {
                return false; 
            }

            UserDataAccess.AddUser(user);
            return true;
        }

        private static bool UserHasCreditLimitLes500(User user)
        {
            return user.HasCreditLimit && user.CreditLimit < 500;
        }

        private static void DetailsForNormalClient(User user)
        {
            user.HasCreditLimit = true;
            using (var userCreditService = new UserCreditService())
            {
                int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                user.CreditLimit = creditLimit;
            }
        }

        private static void DetailsForImportantClient(User user)
        {
            using (var userCreditService = new UserCreditService())
            {
                int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                creditLimit = creditLimit * 2;
                user.CreditLimit = creditLimit;
            }
        }

        private static bool IsClientImportant(Client client)
        {
            return client.Type == "ImportantClient";
        }

        private static bool IsClientVeryImportant(Client client)
        {
            return client.Type == "VeryImportantClient";
        }

        public static Client CreateClient(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId,
            out User user)
        {
            var clientRepository = new ClientRepository();
            var client = clientRepository.GetById(clientId);

            user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };
            return client;
        }

        private static int CheckAndCalculateAge(DateTime dateOfBirth)
        {
            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)) 
                age--;
            return age;
        }

        private static bool IsEmailInCorrect(string email)
        {
            return !email.Contains("@") && !email.Contains(".");
        }

        private static bool IsLastNameInCorrect(string lastName)
        {
            return string.IsNullOrEmpty(lastName);
        }

        private static bool IsFirstNameInCorrect(string firstName)
        {
            return string.IsNullOrEmpty(firstName);
        }
    }
}
