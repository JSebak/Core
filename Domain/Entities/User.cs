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
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be empty", nameof(username));
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty", nameof(password));
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty", nameof(email));
            if (string.IsNullOrWhiteSpace(role))
                throw new ArgumentException("Role cannot be empty", nameof(role));

            Username = username;
            Password = password;
            Email = email;
            Role = role;
        }

        public void UpdatePassword(string newPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentException("Password cannot be empty", nameof(newPassword));

            Password = newPassword;
        }

        public void UpdateUserName(string newUsername)
        {
            if (string.IsNullOrWhiteSpace(newUsername))
                throw new ArgumentException("Username cannot be empty", nameof(newUsername));

            Username = newUsername;
        }

        public void UpdateEmail(string newEmail)
        {
            if (string.IsNullOrWhiteSpace(newEmail))
                throw new ArgumentException("Email cannot be empty", nameof(newEmail));

            Email = newEmail;
        }

        public void ChangeRole(string newRole)
        {
            if (string.IsNullOrWhiteSpace(newRole))
                throw new ArgumentException("Role cannot be empty", nameof(newRole));

            Role = newRole;
        }
    }

}
