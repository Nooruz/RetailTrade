using RetailTrade.Domain.Models;
using System;

namespace RetailTradeServer.State.Users
{
    public class UserStore : IUserStore
    {
        private User _currentUser;
        private Organization _currentOrganization;
        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                StateChanged?.Invoke();
            }
        }
        public Organization CurrentOrganization
        {
            get => _currentOrganization;
            set
            {
                _currentOrganization = value;
                StateChanged?.Invoke();
            }
        }

        public event Action StateChanged;
    }
}
