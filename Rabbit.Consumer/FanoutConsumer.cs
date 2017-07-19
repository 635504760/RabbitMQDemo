using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Rabbit.Consumer
{
    public class FanoutConsumer
    {
        const string EXCHANGE_NAME = "wwj_exchange";
        private const string ROUTING_KEY = "";
        
        //输出
        public static void Subscribe()
        {
            var factory=new ConnectionFactory(){
                HostName = "localhost",
                Port = 5672,
                UserName = "wwj",
                Password = "123456",
                VirtualHost = "hosts_wwj"
            };
            using (var conn = factory.CreateConnection())
            {
                using (var channel = conn.CreateModel())
                {
                    channel.ExchangeDeclare(EXCHANGE_NAME, ExchangeType.Fanout, false, false, null);
                    QueueDeclareOk queue = channel.QueueDeclare("", false, false, true, null);
                    string queueName = queue.QueueName;
                    channel.QueueBind(queueName, EXCHANGE_NAME, ROUTING_KEY, null);
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (sender, args) =>
                    {
                        string message = Encoding.UTF8.GetString(args.Body);
                        Console.WriteLine(message);
                    };
                    channel.BasicConsume(queueName, true, consumer);
                    Console.WriteLine("Press enter to exit");
                    Console.ReadLine();
                }
            }
        }
    }
}
