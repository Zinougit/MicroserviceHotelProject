using Confluent.Kafka;
using CQRS.Core.Event;
using CQRS.CORE.Consumer;
using Microsoft.Extensions.Options;
using Room.Query.Infrastructure.Converters;
using Room.Query.Infrastructure.EventHandler;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Room.Query.Infrastructure.Consumers
{
    public class EventConsumer : IEventConsumer
    {
        private readonly ConsumerConfig _consumerConfig;
        private readonly IEventHandler _eventHandler;
        public EventConsumer(IOptions<ConsumerConfig> consumerConfig, IEventHandler eventHandler)
        {
            _consumerConfig = consumerConfig.Value;
            _eventHandler = eventHandler;
        }
        public async void Consume(string topic)
        {
            using var consumer = new ConsumerBuilder<string, string>(_consumerConfig).
                SetKeyDeserializer(Deserializers.Utf8)
                .SetValueDeserializer(Deserializers.Utf8)
                .Build();
            consumer.Subscribe(topic);
            while (true)
            {
                var Result = consumer.Consume();
                if (Result?.Message.Value == null)
                {
                    continue;
                }
                var message = Result.Message.Value;
                var option = new JsonSerializerOptions { Converters = { new EventConverter() } };
                var @event = JsonSerializer.Deserialize<BaseEvent>(message, option);
                if(@event == null)
                {
                    continue;
                }
                var methode = _eventHandler.GetType().GetMethod(nameof(_eventHandler.On), new Type[] { @event.GetType() });
                _ = methode ?? throw new ArgumentNullException(nameof(methode), "cannot find the event handler methode");
                await (Task)methode.Invoke(_eventHandler, new object[] {@event});
                consumer.Commit(Result);
            }

        }
    }
}
