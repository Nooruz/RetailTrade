using RetailTrade.Domain.Models;
using System;

namespace RetailTrade.Domain.Exceptions
{
    public class InvalidUsernameOrPasswordException : Exception
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public Shift Shift { get; set; }

        public InvalidUsernameOrPasswordException(Shift shift)
        {
            Shift = shift;
        }

        public InvalidUsernameOrPasswordException(Shift shift, string message) : base(message)
        {
            Shift = shift;
        }

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
    }
}
