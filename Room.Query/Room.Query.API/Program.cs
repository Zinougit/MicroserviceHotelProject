using Microsoft.EntityFrameworkCore;
using Room.Query.Infrastructure.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
var x = builder.Configuration.GetConnectionString("SqlServer");
Action<DbContextOptionsBuilder> options = (o=>o.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
builder.Services.AddDbContext<DataBaseContext>(options);
builder.Services.AddSingleton<DataBaseContextFactory>(new DataBaseContextFactory(options));
var DbContext = builder.Services.BuildServiceProvider().GetRequiredService<DataBaseContext>();
DbContext.Database.EnsureCreated();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
