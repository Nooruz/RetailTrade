using RetailTrade.Domain.Models;
using System;

namespace RetailTradeClient.State.Users
{
    public interface IUserStore
    {
        User CurrentUser { get; set; }
        Organization Organization { get; set; }
        public event Action CurrentUserChanged;
        public event Action CurrentOrganizationChanged;
    }
}
