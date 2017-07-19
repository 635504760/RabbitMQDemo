using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
namespace Rabbit.Product
{
    public class FanoutProduct
    {
        const string EXCHANGE_NAME = "wwj_exchange";
        private const string ROUTING_KEY = "";

        //直接发送消息到交换机
        public static void Publish()
        {
            var factory = new ConnectionFactory()
            {
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
                    Parallel.For(1, 100, item =>
                    {
                        string message = string.Format("内容：{0}", DateTime.Now.ToString());
                        Thread.Sleep(1000);
                        channel.BasicPublish(EXCHANGE_NAME, ROUTING_KEY, null, Encoding.UTF8.GetBytes(message));
                        Console.WriteLine(message);
                    });
                    Console.WriteLine("Press enter to exit");
                    Console.ReadLine();
                }
            }
        }
    }
}
