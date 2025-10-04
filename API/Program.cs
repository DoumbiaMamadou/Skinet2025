using API.Middleware;
using Core.Interfaces;
using Infrastructure.Data;
 
 
using Microsoft.EntityFrameworkCore;
 
var builder = WebApplication.CreateBuilder(args);
 
// Add services to the container.
 
builder.Services.AddDbContext<StoreContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
 

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
 
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();
builder.Services.AddControllers(); 
var app = builder.Build();

app.UseCors(x => x.AllowAnyHeader().AllowAnyHeader()
.WithOrigins("http://localhost:4200", "https://localhost:4200")); 

app.UseMiddleware<ExceptionMiddleware>();
 

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}



try
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<StoreContext>();

    if (!context.Database.CanConnect())
    { Console.WriteLine("Impossible de se connecter à la base de données"); }
    await context.Database.MigrateAsync();// Si la Db n'existe pas, elle va la créer
    await StoreContextSeed.SeedAsync(context);// Peupler la Db avec des données de test
}

catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    throw;
}
app.Run();
 