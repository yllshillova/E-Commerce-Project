using API.Data;
using API.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Service for DB Connection
builder.Services.AddDbContext<StoreContext>(options => {
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

/*
Kjo linjë kod shton middleware për politikat e CORS në shërbimin e ASP.NET Core,
duke lejuar kërkesat nga burime të ndryshme dhe duke e bërë aplikacionin të sigurt dhe funksional 
nëse ka nevojë për të marrë resurse nga burime të ndryshme.
*/
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
// the exception handling middleware for the application.
app.UseMiddleware<ExceptionMiddleware>();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// pjesa ku shtohet middleware per me leju kerkesa pi burimit localhost:3000
app.UseCors(opt =>
{                                      //kta e perdorum(allow credentials) per me lehu klientin me pass cookie prej api ne client side dhe anasjelltas
    opt.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:3000");
});

app.UseAuthorization();
//api e din se ku ko me dergu request kur vjen puna te nje API endpoint sepse e shton konfigurimin e rrugetimit
//per kontrollerat duke e perdor ket metod specifike..
app.MapControllers();
// pjesa ku fillohet menaxhimi nese ka databaz ose jo e poashtu ndonje pending migration...
var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<StoreContext>();
var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
try
{
    context.Database.Migrate();
    DbInitializer.Initialize(context);
}
catch (Exception ex)
{
    logger.LogError(ex,"A problem occurred during migration");    
}

app.Run();
