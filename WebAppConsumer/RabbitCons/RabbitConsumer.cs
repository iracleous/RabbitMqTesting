using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using Newtonsoft.Json;
using System.Text;
using RabbitMqApi.Models;
using System.Threading.Tasks;
using RabbitMqApi.Migrations;
using System.Threading.Channels;
using RabbitMQ.Client.Exceptions;

namespace WebAppConsumer.RabbitCons;

public class RabbitConsumer : IRabbitConsumer
{
    private ManualResetEvent messageProcessingComplete = new ManualResetEvent(false);

    public List<Product> ReadProductsFromQueue( )
    {
        var products = new List<Product>();

        int processedMessageCount = 0;
        int expectedMessageCount = 1;

        //Here we specify the Rabbit MQ Server. we use rabbitmq docker image and use it
        var factory = new ConnectionFactory { HostName = "localhost" };

        //Create the RabbitMQ connection using connection factory details as i mentioned above
        using var connection = factory.CreateConnection();

        //Here we create channel with session and model
        using var channel = connection.CreateModel();
        //declare the queue after mentioning name and a few property related to that
        channel.QueueDeclare("product2",
                    exclusive: false,
                     autoDelete: false,
                     arguments: null);
        //Set Event object which listen message from chanel which is sent by producer

        var consumer = new EventingBasicConsumer(channel);
  
        consumer.Received += (model, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Product? product = JsonConvert.DeserializeObject<Product>(message);
            if (product != null) products.Add(product);

            // Increment the processed message count
            processedMessageCount++;

            // Check if all expected messages are processed then Signal that message processing is complete
            if (processedMessageCount == expectedMessageCount)
            {
                messageProcessingComplete.Set();
            }



        };
        channel.BasicConsume(queue: "product2", autoAck: true, consumer: consumer);

        // waits to process thw queu but with timeout if the queue is empty
        bool messagesProcessed = messageProcessingComplete.WaitOne(TimeSpan.FromSeconds(1)); // Adjust the timeout as needed
        if (!messagesProcessed)
        {
            return products;
        }

        return products;
    }

   


}
