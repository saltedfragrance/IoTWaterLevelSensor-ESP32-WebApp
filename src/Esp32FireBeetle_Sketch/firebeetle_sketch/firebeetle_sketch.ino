#include <WiFi.h>
#include <WiFiClientSecure.h>
#include <PubSubClient.h>

#define trigPin D10
#define echoPin D11

//meting variabelen
long duration;
int distance;

//wifi verbinding
const char *ssid = "Home 2.4ghz";
const char *password = "Telenet12345.";

//mqtt verbinding
WiFiClientSecure espClient;
PubSubClient client(espClient);
IPAddress mqtt_server(192,168,0,22);
const char *topic = "watersensor";
const int mqtt_port = 1883;

void setup(){
  // baudrate snelheid
  Serial.begin(115200);

  connectWiFi();

  pinMode(trigPin, OUTPUT); //trigPin als OUTPUT
  pinMode(echoPin, INPUT); //echoPin als INPUT

  doMeasurement();
  sendMeasurement();
}

void doMeasurement(){
  for(int i = 0;i < 2;i++){
    digitalWrite(trigPin, LOW);
    delayMicroseconds(2); 

    digitalWrite(trigPin, HIGH);
    delayMicroseconds(20);
    digitalWrite(trigPin, LOW);

    duration = pulseIn(echoPin, HIGH);
    distance = duration * 0.0344 / 2;

    //testing
    Serial.print("Distance: ");
    Serial.print(distance);
    Serial.println(" cm");
    delay(1000);
  }
}

void connectWiFi(){
  WiFi.begin(ssid, password);

      Serial.println("\nConnecting to wifi");

    while(WiFi.status() != WL_CONNECTED){
        Serial.print(".");
        delay(100);
    }

    Serial.println("\nConnected to the WiFi network");
}

void sendMeasurement(){
  client.setServer(mqtt_server, mqtt_port);
  while (!client.connected()) {
      String client_id = "watersensor_publish";
      Serial.printf("The client %s connects to the public mqtt broker\n", client_id.c_str());
      if (client.connect(client_id.c_str())) {
          Serial.println("Public emqx mqtt broker connected");
      } else {
          Serial.print("failed with state ");
          Serial.print(client.state());


          Serial.println(WiFi.localIP());


          
          delay(2000);
      }
  }

  const char *theDistance = (const char *)distance;
  client.publish(topic, "theDistance"); // publish to the topic-
  client.subscribe(topic);
}

void loop(){
}