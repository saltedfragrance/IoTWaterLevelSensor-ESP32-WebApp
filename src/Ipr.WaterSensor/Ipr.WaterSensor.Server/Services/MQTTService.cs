using System.Security.Cryptography.X509Certificates;
using MQTTnet;
using MQTTnet.Client;
using System.Security.Authentication;
using MQTTnet.Server;
using System.Text;

namespace Ipr.WaterSensor.Server.Services
{
    public class MQTTService
    {
        private string broker = Properties.Resources.MQTTBroker;
        private int port = int.Parse(Properties.Resources.MQTTPort);
        private string clientId = Guid.NewGuid().ToString();
        private string username = Properties.Resources.MQTTUserName;
        private string password = Properties.Resources.MQTTPassword;
        public string topicMainTank = Properties.Resources.MQTTTopicMainTank;
        public string topicBatteryLevel = Properties.Resources.MQTTTopicBatteryLevel;
        public string topicIntervalReceive = Properties.Resources.MQTTTopicIntervalReceive;
        public string topicIntervalSend = Properties.Resources.MQTTTopicIntervalSend;
        public string MeasuredValueMainTank { get; set; }
        public string MeasuredValueBattery { get; set; }
        public bool ClientStarted { get; set; }
        public IMqttClient? MqttClient { get; set; }

        public async Task StartClient()
        {
            var factory = new MqttFactory();
            MqttClient = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
           .WithTcpServer(broker, port)
           .WithCredentials(username, password)
           .WithClientId(clientId)
           .WithCleanSession()
            .WithTls(
                o =>
                {
                    o.CertificateValidationHandler = _ => true;
                    o.SslProtocol = SslProtocols.Tls12; ;
                    var certificate = new X509Certificate(Properties.Resources.MQTTSSLCertificatePath, "");
                    o.Certificates = new List<X509Certificate> { certificate };
                }
            )
           .Build();

            var connectResult = await MqttClient.ConnectAsync(options);

            if (connectResult.ResultCode == MqttClientConnectResultCode.Success)
            {
                ClientStarted = true;
            }

            var mqttSubscribeOptionsTank = factory.CreateSubscribeOptionsBuilder()
            .WithTopicFilter(
                f =>
                {
                    f.WithTopic(topicMainTank);
                })
            .Build();

            var mqttSubscribeOptionsBattery = factory.CreateSubscribeOptionsBuilder()
                .WithTopicFilter(
                    f =>
                    {
                        f.WithTopic(topicBatteryLevel);
                    })
                .Build();

            var mqttSubscribeOptionsInterval = factory.CreateSubscribeOptionsBuilder()
                .WithTopicFilter(
                    f =>
                    {
                        f.WithTopic(topicIntervalReceive);
                    })
                .Build();

            await MqttClient.SubscribeAsync(mqttSubscribeOptionsTank, CancellationToken.None);
            await MqttClient.SubscribeAsync(mqttSubscribeOptionsBattery, CancellationToken.None);
            await MqttClient.SubscribeAsync(mqttSubscribeOptionsInterval, CancellationToken.None);
        }
    }
}
