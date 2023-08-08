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
        private const string broker = "u325aca1.ala.us-east-1.emqxsl.com";
        private const int port = 8883;
        private const string topicMainTank = "watersensor_main_tank";
        private string clientId = Guid.NewGuid().ToString();
        private const string username = "watersensor_receive";
        private const string password = "45745737444568745";
        public string MeasuredValueMainTank { get; set; }
        public int MeasuredValueSecondaryTank { get; set; }
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
                    var certificate = new X509Certificate("emqxsl-ca.crt", "");
                    o.Certificates = new List<X509Certificate> { certificate };
                }
            )
           .Build();

            var connectResult = await MqttClient.ConnectAsync(options);

            if (connectResult.ResultCode == MqttClientConnectResultCode.Success)
            {
                ClientStarted = true;
            }

            await Subscribe(MqttClient);
        }

        private async Task Subscribe(IMqttClient mqttClient)
        {
            await mqttClient.SubscribeAsync(topicMainTank);

            mqttClient.ApplicationMessageReceivedAsync += e =>
            {
                MeasuredValueMainTank = Encoding.Default.GetString(e.ApplicationMessage.Payload);
                return Task.CompletedTask;
            };
        }
    }
}
