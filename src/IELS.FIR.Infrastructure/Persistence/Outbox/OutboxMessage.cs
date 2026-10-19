
using System.Text.Json;

namespace IELS.FIR.Infrastructure.Persistence.Outbox;

public sealed class OutboxMessage
{
    public Guid Id { get; private set; }
    public string EventType { get; private set; } = string.Empty;
    public int EventVersion { get; private set; }
    public string Payload { get; private set; } = string.Empty;
    public DateTimeOffset OccurredAtUtc { get; private set; }
    public DateTimeOffset? ProcessedAtUtc { get; private set; }
    public int RetryCount { get; private set; }
    public string? LastError { get; private set; }

    private OutboxMessage() { }

    public static OutboxMessage Create<T>(
        string eventType,
        int eventVersion,
        T payload,
        DateTimeOffset occurredAtUtc)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(eventType);
        ArgumentNullException.ThrowIfNull(payload);

        if (eventVersion < 1)
            throw new ArgumentOutOfRangeException(
                nameof(eventVersion));

        return new OutboxMessage
        {
            Id = Guid.NewGuid(),
            EventType = eventType,
            EventVersion = eventVersion,
            Payload = JsonSerializer.Serialize(payload),
            OccurredAtUtc = occurredAtUtc.ToUniversalTime()
        };
    }

    public void MarkProcessed(DateTimeOffset processedAtUtc)
    {
        ProcessedAtUtc = processedAtUtc.ToUniversalTime();
        LastError = null;
    }

    public void RecordFailure(string error)
    {
        RetryCount++;
        LastError = error.Length > 2000
            ? error[..2000]
            : error;
    }
}
