using RetailTrade.Domain.Models;
using System;

namespace RetailTradeClient.State.Users
{
    public interface IUserStore
    {
        User CurrentUser { get; set; }
        Organization Organization { get; set; }
        event Action StateChanged;
    }
}
