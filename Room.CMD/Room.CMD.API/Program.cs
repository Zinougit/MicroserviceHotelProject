using Confluent.Kafka;
using CQRS.Core.Domain;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producer;
using CQRS.Core.Topics;
using Room.CMD.Infrastracture.Repository;
using Room.CMD.Infrastructure.Config;
using Room.CMD.Infrastructure.EventStore;
using Room.CMD.Infrastructure.Producer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<MongoConfig>(builder.Configuration.GetSection(nameof(MongoConfig)));
builder.Services.Configure<KafkaTopic>(builder.Configuration.GetSection(nameof(KafkaTopic)));
builder.Services.Configure<ProducerConfig>(builder.Configuration.GetSection(nameof(ProducerConfig)));
builder.Services.AddScoped<IEventStoreRepository, EventStoreRepository>();
builder.Services.AddScoped<IEventProducer,EventProducer>();
builder.Services.AddScoped<IEventStore,EventStore>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();

