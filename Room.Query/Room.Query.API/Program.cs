using Confluent.Kafka;
using CQRS.CORE.Consumer;
using Microsoft.EntityFrameworkCore;
using Room.Query.Domain.Repository;
using Room.Query.Infrastructure.Consumers;
using Room.Query.Infrastructure.DataAccess;
using Room.Query.Infrastructure.EventHandler;
using Room.Query.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
Action<DbContextOptionsBuilder> options = (o=>o.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
builder.Services.AddDbContext<DataBaseContext>(options);
builder.Services.AddSingleton<DataBaseContextFactory>(new DataBaseContextFactory(options));
builder.Services.AddScoped<IRoomRepository,RoomRepository>();
builder.Services.AddScoped<IEventHandler, Room.Query.Infrastructure.EventHandler.EventHandler>();
builder.Services.Configure<ConsumerConfig>(builder.Configuration.GetSection(nameof(ConsumerConfig)));
builder.Services.AddScoped<IEventConsumer, IEventConsumer>();
builder.Services.AddHostedService<ConsumerHostedService>();
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
