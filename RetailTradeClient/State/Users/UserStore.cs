using RetailTrade.Domain.Models;
using System;

namespace RetailTradeClient.State.Users
{
    public class UserStore : IUserStore
    {
        #region Private Members

        private User _currentUser;
        private Organization _organization;

        #endregion

        #region Public Properties

        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                CurrentUserChanged?.Invoke();
            }
        }

        public Organization Organization
        {
            get => _organization;
            set
            {
                _organization = value;
                CurrentOrganizationChanged?.Invoke();
            }
        }

        #endregion

        #region Public Event Actions

        public event Action CurrentUserChanged;
        public event Action CurrentOrganizationChanged;

        #endregion        
    }
}
