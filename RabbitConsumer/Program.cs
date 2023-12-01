﻿using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMqApi.Models;
using System.Text;



//Here we specify the Rabbit MQ Server. we use rabbitmq docker image and use it
var factory = new ConnectionFactory{    HostName = "localhost"};

//Create the RabbitMQ connection using connection factory details as i mentioned above
var connection = factory.CreateConnection();
//Here we create channel with session and model
using var channel = connection.CreateModel();
//declare the queue after mentioning name and a few property related to that
channel.QueueDeclare("product2",
   //  durable: true,
                     exclusive: false,
                     autoDelete: false
    );

 

//Set Event object which listen message from chanel which is sent by producer
var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, eventArgs) => {
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Product message received: {message}");
    Product ?product = JsonConvert.DeserializeObject<Product>(message);
    Console.WriteLine(product?.ProductName );

   
};
//read the message
 channel.BasicConsume(queue: "product2", autoAck: true, consumer: consumer);
//Console.ReadKey();


