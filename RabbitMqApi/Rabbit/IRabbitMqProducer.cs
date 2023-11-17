namespace RabbitMqApi.Rabbit;

public interface IRabbitMqProducer
{
    public void SendProductMessage<T>(T message);
}
