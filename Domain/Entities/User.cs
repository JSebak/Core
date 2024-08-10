using System.Text.RegularExpressions;

namespace Domain.Entities
{
    public class User
    {
        public int Id { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Email { get; private set; }
        public string Role { get; private set; }

        public User(string username, string password, string email, string role)
        {
            ValidateUsername(username);
            ValidatePassword(password);
            ValidateEmail(email);
            ValidateRole(role);

            Username = username;
            Password = password;
            Email = email;
            Role = role;
        }

        public void UpdatePassword(string newPassword)
        {
            ValidatePassword(newPassword);
            Password = newPassword;
        }

        public void UpdateUserName(string newUsername)
        {
            ValidateUsername(newUsername);
            Username = newUsername;
        }

        public void UpdateEmail(string newEmail)
        {
            ValidateEmail(newEmail);
            Email = newEmail;
        }

        public void ChangeRole(string newRole)
        {
            ValidateRole(newRole);
            Role = newRole;
        }

        private void ValidateUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be empty", nameof(username));
        }

        private void ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty", nameof(password));

            if (password.Length < 8 ||
                !Regex.IsMatch(password, @"[A-Z]") ||
                !Regex.IsMatch(password, @"\d") ||
                !Regex.IsMatch(password, @"[@$!%*?&#]"))
            {
                throw new ArgumentException("Password must be at least 8 characters long, contain one uppercase letter, one number, and one special character.", nameof(password));
            }
        }

        private void ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty", nameof(email));

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                throw new ArgumentException("Invalid email format", nameof(email));
            }
        }

        private void ValidateRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
                throw new ArgumentException("Role cannot be empty", nameof(role));
        }
    }
}
