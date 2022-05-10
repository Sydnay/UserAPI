using Microsoft.EntityFrameworkCore;
using UserAPI.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//RAM
//builder.Services.AddSingleton<IUserRepository, UserMemoryRepository>();

builder.Services.AddDbContext<UserEFRepository>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionToDatabase"));
});

builder.Services.AddTransient<IUserRepository, UserEFRepository>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
