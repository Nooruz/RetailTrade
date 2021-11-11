namespace RetailTrade.Domain.Models
{
    public class Branch : DomainObject
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int UserId { get; set; }

        public User User { get; set; }
    }
}
