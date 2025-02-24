using AuctionService.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddDbContext<AuctionDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMassTransit(
    x =>
    {
        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.ConfigureEndpoints(context);
        });
    }
);

var app = builder.Build();

// Builder option
app.UseAuthorization();
app.MapControllers();

// call the db
try
{
    DbInitializer.InitDb(app);
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}
app.Run();
