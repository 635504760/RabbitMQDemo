using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
namespace RabbitMQ.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
           
            //var factory = new ConnectionFactory() {HostName = "localhost"};
            var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672, UserName = "wwj", Password = "123456", VirtualHost = "hosts_wwj" };

            using (var connection = factory.CreateConnection())

            using (var channel = connection.CreateModel())
            {
                
                channel.QueueDeclare(
                    queue: "wwj_queue",     
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );
                var message = GetMesssage(args);
                var body = Encoding.UTF8.GetBytes(message);
                var propertise = channel.CreateBasicProperties();
                propertise.Persistent = true;
                channel.BasicPublish(
                    exchange: "",
                    routingKey: "wwj_queue",
                    basicProperties: propertise,
                    body: body
                );
                Console.WriteLine(" [x] Sent {0}", message);
                
            }
            Console.WriteLine("Press [enter] to exit.");
            Console.ReadLine();

        }

        private static string GetMesssage(string[] args)
        {
            var arg=Console.ReadLine();
            args = new string[] {arg};
            return ((args.Length > 0) ? string.Join("", args) : "Hello World!");
        }
    }
}
