using System;

namespace RetailTrade.Domain.Exceptions
{
    public class InvalidUsernameOrPasswordException : Exception
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public InvalidUsernameOrPasswordException(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public InvalidUsernameOrPasswordException(string message, string username, string password) : base(message)
        {
            Username = username;
            Password = password;
        }

        public InvalidUsernameOrPasswordException(string message, Exception innerException, string username, string password) : base(message, innerException)
        {
            Username = username;
            Password = password;
        }
    }
}
