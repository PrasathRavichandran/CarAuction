using AuctionService.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Data;

public class AuctionDbContext : DbContext
{
    public AuctionDbContext(DbContextOptions options) : base(options)
    {
    }

    protected AuctionDbContext()
    {
    }

    // This statement will create a table for Auction and Item, because Auction is related.
    public DbSet<Auction> Auctions { get; set; }
}
