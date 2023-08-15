#include <WiFi.h>
#include <WiFiClientSecure.h>
#include <PubSubClient.h>
#include <DFRobot_MAX17043.h>
#include <neotimer.h>
#include <iostream>
#include <cstring>
#include <string>
#include <sstream>
#include <FS.h>
#include <SPIFFS.h>

//pins voor ultrasone sensor
#define trigPin D10  //vuurt de geluidsgolf af
#define echoPin D11  //vangt de gereflecteerde geluidsgolf terug op

//te onthouden update interval in RTC geheugen
RTC_DATA_ATTR uint64_t updateInterval = 1800000000;

//non blocking timer
Neotimer myTimer = Neotimer(10000);

//batterij module
DFRobot_MAX17043 FuelGauge;

//meting variabelen
long duration;
float distance;
float batteryPercentage;

//wifi verbinding
const char *ssid = "Home 2.4ghz";
const char *password = "Telenet12345.";

//mqtt verbinding
WiFiClientSecure espClient;
PubSubClient client(espClient);
const char *mqttBroker = "f07b2dbeeae343528d0b9dad5a13aebf.s1.eu.hivemq.cloud";
const char *tankLevelTopic = "watersensor_main_tank";
const char *batteryLevelTopic = "battery_level";
const char *intervalTopicSend = "intervalSend";
const char *intervalTopicReceive = "intervalReceive";
const char *mqttUsername = "watersensor_publish";
const char *mqttPassword = "ZEEZRrrze4235";
const int mqttPort = 8883;

void setup() {
  // baudrate snelheid
  Serial.begin(115200);

  FuelGauge.begin();

  ConnectWiFi();
  ConnectMQTT();

  pinMode(trigPin, OUTPUT);
  pinMode(echoPin, INPUT);

  //metingen
  MeasureDistanceToWater();
  SendMeasurement();
}

void MeasureDistanceToWater() {
  digitalWrite(trigPin, LOW);
  delayMicroseconds(2);

  digitalWrite(trigPin, HIGH);
  delayMicroseconds(20);
  digitalWrite(trigPin, LOW);

  duration = pulseIn(echoPin, HIGH);
  distance = duration * 0.0344 / 2;

  batteryPercentage = FuelGauge.readPercentage();

  //testing
  Serial.print("Distance: ");
  Serial.print(distance);
  Serial.println(" cm");

  Serial.print("Battery level: ");
  Serial.print(batteryPercentage);
  Serial.println(" %");
}

void ConnectWiFi() {
  WiFi.begin(ssid, password);

  Serial.println("\nConnecting to wifi");

  while (WiFi.status() != WL_CONNECTED) {
    Serial.print(".");
    delay(100);
  }

  Serial.println("\nConnected to the WiFi network");
}


void ConnectMQTT() {
  //ssl certificaat lezen uit SPIFFS geheugen
  char certificate[2000] = { '\0' };
  SPIFFS.begin();
  File file = SPIFFS.open("/emqxsl-ca.crt");

  uint16_t i = 0;
  while (file.available()) {
    certificate[i] = file.read();
    i++;
  }
  certificate[i] = '\0';

  //mqtt verbinden
  espClient.setCACert(VALUE);
  client.setServer(mqttBroker, mqttPort);
  client.setCallback(callback);
  while (!client.connected()) {
    String client_id = "watersensor_publish";
    Serial.printf("The client %s connects to the public mqtt broker\n", client_id.c_str());
    if (client.connect(client_id.c_str(), mqttUsername, mqttPassword)) {
      Serial.println("Public emqx mqtt broker connected");
    } else {
      Serial.print("failed with state ");
      Serial.print(client.state());
      delay(2000);
    }
  }

  client.subscribe(intervalTopicSend);
  delay(3000);
}

void SendMeasurement() {
  String sensorValue_str;
  String interval_str;
  String batteryLevel_str;
  char batteryLevel[50];
  char sensorValue[50];
  char intervalValue[50];

  sensorValue_str = String(distance);
  sensorValue_str.toCharArray(sensorValue, sensorValue_str.length() + 1);

  batteryLevel_str = String(batteryPercentage);
  batteryLevel_str.toCharArray(batteryLevel, batteryLevel_str.length() + 1);

  interval_str = String(updateInterval);
  interval_str.toCharArray(intervalValue, interval_str.length() + 1);

  bool levelPublished = client.publish(tankLevelTopic, sensorValue);
  if (levelPublished == true) Serial.println("Water level published.");
  else Serial.println("Failed to publish water level.");

  bool batteryPublished = client.publish(batteryLevelTopic, batteryLevel);
  if (batteryPublished == true) Serial.println("Battery level published.");
  else Serial.println("Failed to publish battery level.");

  bool intervalPublished = client.publish(intervalTopicReceive, intervalValue);
  if (intervalPublished == true) Serial.println("Interval published.");
  else Serial.println("Failed to publish interval.");

  myTimer.start();
}

void callback(char *topic, byte *payload, unsigned int length) {
  std::string value = "";
  for (int i = 0; i < length; i++) {
    value += (char)payload[i];
  }
  value[length] = '\0';
  char *end;
  updateInterval = strtoull(value.c_str(), &end, 10);

  Serial.print("Interval changed to: ");
  Serial.println(updateInterval);
}

void loop() {
  if (!client.connected()) {
    ConnectMQTT();
  }
  client.subscribe(intervalTopicSend);
  client.loop();
  
  //deep sleep
  if (myTimer.done()) {
    esp_sleep_enable_timer_wakeup(updateInterval);
    Serial.println("Deep sleep enabled");
    esp_deep_sleep_start();
  }
}
