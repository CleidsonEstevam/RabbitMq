using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

/***************************************/
// Mensagem gerada por um Produtor     //
// processada e lida por um Consumidor //
// enviada de forma direta.            //
/***************************************/


//Configurando Host
var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

//Configuração da fila
channel.QueueDeclare(queue: "hello",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

Console.WriteLine(" [*] Waiting for messages.");


var consumer = new EventingBasicConsumer(channel);

//Consumidor lendo fila
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);

    //Caso o Canal cair antes da conclusão retorna a menssagem para o emissor
    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);

    //Exibir Menssagem
    Console.WriteLine($" [x] Received {message}");
};
channel.BasicConsume(queue: "hello",
                     autoAck: true,
                     consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();