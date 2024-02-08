using Confluent.Kafka;
using LoggingApp.Producer.Interfaces;
using LoggingApp.Shared.Entities;
using System.Text.Json;

namespace LoggingApp.Producer.Services
{
    public class LogService : ILogService
    {
        private readonly IProducer<Null, string> _producer;
        private readonly string _topic;
        private readonly ILogger _logger;

        public LogService(IConfiguration configuration, ILogger<LogService> logger)
        {
            _topic = configuration["KafkaConfig:LogTopic"];

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = configuration["KafkaConfig:BootstrapServer"]
            };

            _producer = new ProducerBuilder<Null, string>(producerConfig).Build();
            _logger = logger;
        }

        public async Task ProduceAsync(LogEntity logEntity)
        {
            var message = new Message<Null, string> { Value = JsonSerializer.Serialize(logEntity) };
            var response = await _producer.ProduceAsync(_topic, message);

            _logger.LogInformation("Success produce log. Log:{a}", response.Value);
        }
    }
}
