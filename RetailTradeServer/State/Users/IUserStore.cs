using RetailTrade.Domain.Models;
using System;

namespace RetailTradeServer.State.Users
{
    public interface IUserStore
    {
        User CurrentUser { get; set; }
        Organization CurrentOrganization { get; set; }
        event Action StateChanged;
    }
}
