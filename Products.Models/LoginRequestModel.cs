namespace Products.Models
{
    /// <summary>
    /// Request model for login operations.
    /// Contains the credentials required to authenticate a user.
    /// </summary>
    public class LoginRequestModel
    {
        /// <summary>
        /// Gets or sets the username for authentication.
        /// </summary>
        public required string Username { get; set; }

        /// <summary>
        /// Gets or sets the password for authentication.
        /// </summary>
        public required string Password { get; set; }
    }
}
