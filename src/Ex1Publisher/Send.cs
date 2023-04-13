using System;
using System.Text;
using RabbitMQ.Client;

namespace Ex1Publisher
{
    class Send
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var count = 0;
                while (true)
                {
                    string message = $"PEDIDO: 23/325900{count++}";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                                         routingKey: "hello",
                                         basicProperties: null,
                                         body: body);
                    Console.WriteLine($"[x] Sent: {message}");

                    System.Threading.Thread.Sleep(100);
                }
            }
        }
    }
}
