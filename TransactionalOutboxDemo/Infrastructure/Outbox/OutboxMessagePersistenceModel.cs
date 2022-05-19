using System.Text.Json;

namespace TransactionalOutboxDemo.Infrastructure;

public class OutboxMessagePersistenceModel
{
    public int Id { get; }
    public MessageDeliveryMode DeliveryMode { get; }
    public string MessageType { get; } = string.Empty;
    public string MessagePayload { get; } = string.Empty;

    protected OutboxMessagePersistenceModel() { }

    private OutboxMessagePersistenceModel(MessageDeliveryMode deliveryMode, string messageType, string messagePayload)
    {
        DeliveryMode = deliveryMode;
        MessageType = messageType;
        MessagePayload = messagePayload;
    }

    public static OutboxMessagePersistenceModel FromMessage(MessageDeliveryMode deliveryMode, object message)
    {
        var serializedMessage = JsonSerializer.Serialize(message);
        var type = message.GetType().AssemblyQualifiedName ?? throw new("Not Supported. Probably a generic type");
        return new OutboxMessagePersistenceModel(deliveryMode, type, serializedMessage);
    }
}
