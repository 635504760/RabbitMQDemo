using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Rabbit.Product
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672, UserName = "wwj", Password = "123456", VirtualHost = "hosts_wwj" };
            using (var conn = factory.CreateConnection())
            {
                using (var channel = conn.CreateModel())
                {

                    //channel.QueueDeclare("wwjfirst_queue", true, false, false, null);
                    channel.ExchangeDeclare("wwj_exchange", "fanout");
                    while (true)
                    {
                        string message = string.Format("Message_{0}", Console.ReadLine());
                        byte[] buffer = Encoding.UTF8.GetBytes(message);
                        var properties = channel.CreateBasicProperties();
                        properties.DeliveryMode = 2;
                        //channel.BasicPublish("", "wwjfirst_queue", properties, buffer);
                        channel.BasicPublish("wwj_exchange", "", properties, buffer);
                        Console.WriteLine("发送消息成功：" + message);
                    }
                    Console.Read();
                }
            }


            //FanoutProduct.Publish();
        }
    }
}
