using Confluent.Kafka;
using CQRS.Core.Domain;
using CQRS.Core.Event;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producer;
using CQRS.Core.Topics;
using MongoDB.Bson.Serialization;
using Room.CMD.API.CommandDispatcher;
using Room.CMD.API.Commands;
using Room.CMD.Domain.Aggregate;
using Room.CMD.Domain.Commands;
using Room.CMD.Infrastracture.Repository;
using Room.CMD.Infrastructure.Config;
using Room.CMD.Infrastructure.EventStore;
using Room.CMD.Infrastructure.Handlers;
using Room.CMD.Infrastructure.Producer;
using Room.Common.Events;

var builder = WebApplication.CreateBuilder(args);

BsonClassMap.RegisterClassMap<BaseEvent>();
BsonClassMap.RegisterClassMap<RoomCreatedEvent>();
BsonClassMap.RegisterClassMap<RoomUpdatedEvent>();
BsonClassMap.RegisterClassMap<RoomDeletedEvent>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.Configure<MongoConfig>(builder.Configuration.GetSection(nameof(MongoConfig)));
builder.Services.Configure<KafkaTopic>(builder.Configuration.GetSection(nameof(KafkaTopic)));
builder.Services.Configure<ProducerConfig>(builder.Configuration.GetSection(nameof(ProducerConfig)));
builder.Services.AddScoped<IEventStoreRepository, EventStoreRepository>();
builder.Services.AddScoped<IEventProducer,EventProducer>();
builder.Services.AddScoped<IEventStore,EventStore>();
builder.Services.AddScoped<IEventSourcingHandler<RoomAggregate>, EventSourcingHandler>();
builder.Services.AddScoped<ICommandHandler,CommandHandler>();

var handler = builder.Services.BuildServiceProvider().GetRequiredService<ICommandHandler>();
var Dispatcher = new CommandDispatcher();
Dispatcher.Register<AddRoomCommand>(handler.HandleAsync);
Dispatcher.Register<UpdateRoomCommad>(handler.HandleAsync);
Dispatcher.Register<DeleteRoomCommand>(handler.HandleAsync);
builder.Services.AddScoped<ICommandDispatcher>(_=>Dispatcher);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();

app.Run();

