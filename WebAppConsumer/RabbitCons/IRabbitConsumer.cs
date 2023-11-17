using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using RabbitMqApi.Models;
using System.Text;
using Newtonsoft.Json;

namespace WebAppConsumer.RabbitCons; 

public interface IRabbitConsumer
{
    public List<Product> ReadProductsFromQueue();
}

