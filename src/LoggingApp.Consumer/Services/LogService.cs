using Confluent.Kafka;
using LoggingApp.Consumer.Database;
using LoggingApp.Consumer.Interfaces;
using LoggingApp.Consumer.Repositories;
using LoggingApp.Shared.Entities;
using System.Text.Json;

namespace LoggingApp.Consumer.Services
{
    public class LogService : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly ILogHistory _logHistory;
        private readonly string _topic;

        public LogService(IConfiguration configuration, ILogger<LogService> logger,
            DatabaseContext context)
        {
            _logger = logger;

            _logHistory = new LogHistoryRepository(context);

            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = configuration["KafkaConfig:BootstrapServer"],
                GroupId = configuration["KafkaConfig:GroupId"],
                AutoOffsetReset = AutoOffsetReset.Earliest,
                MaxPollIntervalMs = 900000
            };

            _topic = configuration["KafkaConfig:LogTopic"]!;
            _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe(_topic);

            while (!stoppingToken.IsCancellationRequested)
            {
                await ProcessMessageAsync(stoppingToken);
            }

            _consumer.Close();
        }

        public async Task ProcessMessageAsync(CancellationToken stoppingToken)
        {
            try
            {
                var consumeResult = _consumer.Consume(stoppingToken);

                _logger.LogInformation("Received log: {a}", consumeResult.Message.Value);

                if (!string.IsNullOrWhiteSpace(consumeResult.Message.Value))
                {
                    await _logHistory.InsertLogHistory(
                        JsonSerializer.Deserialize<LogEntity>(consumeResult.Message.Value)!);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception message: {a}", ex.Message);
            }
        }
    }
}
