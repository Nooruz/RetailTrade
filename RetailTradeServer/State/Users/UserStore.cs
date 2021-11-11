using RetailTrade.Domain.Models;
using System;

namespace RetailTradeServer.State.Users
{
    public class UserStore : IUserStore
    {
        private User _currentUser;
        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                StateChanged?.Invoke();
            }
        }

        public event Action StateChanged;
    }
}
