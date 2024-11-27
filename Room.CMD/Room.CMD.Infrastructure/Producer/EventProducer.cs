using Confluent.Kafka;
using CQRS.Core.Event;
using CQRS.Core.Producer;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Room.CMD.Infrastructure.Producer
{
    public class EventProducer : IEventProducer
    {
        private readonly ProducerConfig _config;

        public EventProducer(IOptions<ProducerConfig> config) {
            _config = config.Value;
        }
        public async Task ProduceAsync<T>(T @event, string topic) where T : BaseEvent
        {
            using var producer = new ProducerBuilder<string, string>(_config).
                SetKeySerializer(Serializers.Utf8)
                .SetValueSerializer(Serializers.Utf8)
                 .Build();
            var message = new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = JsonSerializer.Serialize(@event, typeof(T))
            };
            var deleveryResult = await producer.ProduceAsync(topic,message); 

            if(deleveryResult.Status == PersistenceStatus.NotPersisted)
            {
                throw new Exception($"this message for this event {typeof(T).Name} cannot be published to the topic {topic} duo to the following reason {deleveryResult.Message} ");
            }
        }
    }
}
