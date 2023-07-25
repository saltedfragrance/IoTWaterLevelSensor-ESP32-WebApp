using MQTTnet.Server;
using MQTTnet;
using MQTTnet.Client;
using static MudBlazor.Colors;

namespace Ipr.WaterSensor.Server.Services
{
    public class MQTTService
    {
        public  string value { get; set; }
        public bool BrokerStarted { get; set; }
        public bool ClientStarted { get; set; }
        public MqttServer MQTTBroker { get; set; }
        public IMqttClient MQTTClient { get; set; }
        public async Task StartBroker()
        {
            var options = new MqttServerOptionsBuilder()
                .WithDefaultEndpoint()
                .WithDefaultEndpointPort(1883).Build();

            MQTTBroker = new MqttFactory().CreateMqttServer(options);
            await MQTTBroker.StartAsync();

            BrokerStarted = true;
        }

        public async Task StartClient()
        {
            var factory = new MqttFactory();
            MQTTClient = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
            .WithClientId("watersensor")
            .WithTcpServer("localhost", 1883)
            .WithCleanSession()
            .Build();



            await MQTTClient.ConnectAsync(options);
            await MQTTClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("watersensor").Build());
            ClientStarted = true;
        }

        public async Task CheckForMessage()
        {
            MQTTClient.ApplicationMessageReceivedAsync += e =>
            {
                value = e.ApplicationMessage.ToString();

                return Task.CompletedTask;
            };
        }
    }
}
