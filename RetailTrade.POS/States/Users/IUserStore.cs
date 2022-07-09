using RetailTrade.Domain.Models;
using System;

namespace RetailTrade.POS.States.Users
{
    public interface IUserStore
    {
        User CurrentUser { get; set; }
        event Action StateChanged;
    }
}
