using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using Newtonsoft.Json;
using System.Text;
using RabbitMqApi.Models;

namespace WebAppConsumer.RabbitCons;

public class RabbitConsumer : IRabbitConsumer
{
    public List<Product> ReadProductsFromQueue()
    {
        var products = new List<Product>();


        //Here we specify the Rabbit MQ Server. we use rabbitmq docker image and use it
        var factory = new ConnectionFactory  {   HostName = "localhost"    };

        //Create the RabbitMQ connection using connection factory details as i mentioned above
        var connection = factory.CreateConnection();
        //Here we create channel with session and model
        using var channel = connection.CreateModel();
        //declare the queue after mentioning name and a few property related to that
        channel.QueueDeclare("product2", exclusive: false,
                     autoDelete: false,
                     arguments: null);
        //Set Event object which listen message from chanel which is sent by producer

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, eventArgs) => {
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            Product? product = JsonConvert.DeserializeObject<Product>(message);
            if (product != null) products.Add(product);
        };
        //read the message // not necessary
         channel.BasicConsume(queue: "product2", autoAck: true, consumer: consumer);
        return products;

    }
}
