#include <Preferences.h>
#include <WiFi.h>
#include <WiFiClientSecure.h>
#include <PubSubClient.h>
#include <DFRobot_MAX17043.h>
#include <neotimer.h>
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

//credentials
Preferences preferences;

//meting variabelen
long duration;
float distance;
float batteryPercentage;

//mqtt en wifi verbinding
WiFiClientSecure espClient;
PubSubClient client(espClient);

//login credentials
String wiFiSsid;
String wiFiPassword;
String mqttBroker;
String mqttUsername;
String mqttwiFipassword;
int mqttPort = 0;

//topics
const char *tankLevelTopic = "watersensor_main_tank";
const char *batteryLevelTopic = "battery_level";
const char *intervalTopicSend = "intervalSend";
const char *intervalTopicReceive = "intervalReceive";

void setup() {
    // baudrate snelheid
  Serial.begin(115200);

  //credentials ophalen
  preferences.begin("credentials", false);
  GetCredentials();

  FuelGauge.begin();
  ConnectWiFi();
  ConnectMQTT();

  pinMode(trigPin, OUTPUT);
  pinMode(echoPin, INPUT);

  //metingen uitvoeren en sturen
  MeasureDistanceToWater();
  SendMeasurement();
}

void GetCredentials() {
  wiFiSsid = preferences.getString("wiFissid", "");
  wiFiPassword = preferences.getString("wiFiPassword", "");
  mqttBroker = preferences.getString("mqttBroker", "");
  mqttUsername = preferences.getString("mqttUsername", "");
  mqttwiFipassword = preferences.getString("mqttPassword", "");
  mqttPort = preferences.getString("mqttPort", "").toInt();
}

void ConnectWiFi() {
  WiFi.begin(wiFiSsid.c_str(), wiFiPassword.c_str());

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
  SPIFFS.end();

  //mqtt verbinden
  espClient.setCACert(certificate);
  client.setServer(mqttBroker.c_str(), mqttPort);
  client.setCallback(callback);
  while (!client.connected()) {
    String client_id = "watersensor_publish";
    Serial.printf("The client %s connects to the public mqtt broker\n", client_id.c_str());
    if (client.connect(client_id.c_str(), mqttUsername.c_str(), mqttwiFipassword.c_str())) {
      Serial.println("Public emqx mqtt broker connected");
    } else {
      Serial.print("failed with state ");
      Serial.print(client.state());
      delay(2000);
    }
  }

  client.subscribe(intervalTopicSend);
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

  //beginnen met wachten op antwoord van blazor ivm interval change
  myTimer.start();
}

void callback(char *topic, byte *payload, unsigned int length) {
  std::string value = "";
  for (int i = 0; i < length; i++) {
    value += (char)payload[i];
  }
  value[length] = '\0';
  char *end;
  updateInterval = strtoull(value.c_str(), &end, 10) * 60000000;

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
