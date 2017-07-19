using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Rabbit.Consumer
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
                    Console.WriteLine("Listening ... ");
                    channel.ExchangeDeclare("wwj_exchange", "fanout");
                    QueueDeclareOk queueOk = channel.QueueDeclare();
                    string queueName = queueOk.QueueName;
                    channel.QueueBind(queueName, "wwj_exchange", ""); 
                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume(queueName,true, consumer);
                    while (true)
                    {
                        var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();  //挂起的操作
                        byte[] bytes = ea.Body;
                        string str = Encoding.UTF8.GetString(bytes);
                        Console.WriteLine("队列消息：" + str.ToString());
                        //channel.BasicAck(ea.DeliveryTag, false);
                    }
                }
            }
            //FanoutConsumer.Subscribe();
        }
    }
}
