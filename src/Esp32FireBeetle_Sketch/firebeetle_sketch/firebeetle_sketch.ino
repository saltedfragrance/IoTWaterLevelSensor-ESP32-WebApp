#include <WiFi.h>
#include <WiFiClientSecure.h>
#include <PubSubClient.h>
#include <DFRobot_MAX17043.h>

#define trigPin D10
#define echoPin D11

//ssl certificaat voor de mqtt broker
const char *ca_cert =
  "-----BEGIN CERTIFICATE-----\n"
  "MIIDrzCCApegAwIBAgIQCDvgVpBCRrGhdWrJWZHHSjANBgkqhkiG9w0BAQUFADBh\n"
  "MQswCQYDVQQGEwJVUzEVMBMGA1UEChMMRGlnaUNlcnQgSW5jMRkwFwYDVQQLExB3\n"
  "d3cuZGlnaWNlcnQuY29tMSAwHgYDVQQDExdEaWdpQ2VydCBHbG9iYWwgUm9vdCBD\n"
  "QTAeFw0wNjExMTAwMDAwMDBaFw0zMTExMTAwMDAwMDBaMGExCzAJBgNVBAYTAlVT\n"
  "MRUwEwYDVQQKEwxEaWdpQ2VydCBJbmMxGTAXBgNVBAsTEHd3dy5kaWdpY2VydC5j\n"
  "b20xIDAeBgNVBAMTF0RpZ2lDZXJ0IEdsb2JhbCBSb290IENBMIIBIjANBgkqhkiG\n"
  "9w0BAQEFAAOCAQ8AMIIBCgKCAQEA4jvhEXLeqKTTo1eqUKKPC3eQyaKl7hLOllsB\n"
  "CSDMAZOnTjC3U/dDxGkAV53ijSLdhwZAAIEJzs4bg7/fzTtxRuLWZscFs3YnFo97\n"
  "nh6Vfe63SKMI2tavegw5BmV/Sl0fvBf4q77uKNd0f3p4mVmFaG5cIzJLv07A6Fpt\n"
  "43C/dxC//AH2hdmoRBBYMql1GNXRor5H4idq9Joz+EkIYIvUX7Q6hL+hqkpMfT7P\n"
  "T19sdl6gSzeRntwi5m3OFBqOasv+zbMUZBfHWymeMr/y7vrTC0LUq7dBMtoM1O/4\n"
  "gdW7jVg/tRvoSSiicNoxBN33shbyTApOB6jtSj1etX+jkMOvJwIDAQABo2MwYTAO\n"
  "BgNVHQ8BAf8EBAMCAYYwDwYDVR0TAQH/BAUwAwEB/zAdBgNVHQ4EFgQUA95QNVbR\n"
  "TLtm8KPiGxvDl7I90VUwHwYDVR0jBBgwFoAUA95QNVbRTLtm8KPiGxvDl7I90VUw\n"
  "DQYJKoZIhvcNAQEFBQADggEBAMucN6pIExIK+t1EnE9SsPTfrgT1eXkIoyQY/Esr\n"
  "hMAtudXH/vTBH1jLuG2cenTnmCmrEbXjcKChzUyImZOMkXDiqw8cvpOp/2PV5Adg\n"
  "06O/nVsJ8dWO41P0jmP6P6fbtGbfYmbW0W5BjfIttep3Sp+dWOIrWcBAI+0tKIJF\n"
  "PnlUkiaY4IBIqDfv8NZ5YBberOgOzW6sRBc4L0na4UU+Krk2U886UAb3LujEV0ls\n"
  "YSEY1QSteDwsOoBrp+uvFRTp2InBuThs4pFsiv9kuXclVzDAGySj4dzp30d8tbQk\n"
  "CAUw7C29C79Fv1C5qfPrmAESrciIxpg0X40KPMbp1ZWVbd4="
  "-----END CERTIFICATE-----\n";

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
const char *mqtt_broker = "u325aca1.ala.us-east-1.emqxsl.com";
const char *tankLevelTopic = "watersensor_main_tank";
const char *batteryLevelTopic = "battery_level";
const char *mqtt_username = "watersensor_publish";
const char *mqtt_password = "8768678347474";
const int mqtt_port = 8883;

void setup() {
  // baudrate snelheid
  Serial.begin(115200);

  FuelGauge.begin();

  connectWiFi();

  pinMode(trigPin, OUTPUT);  //trigPin als OUTPUT
  pinMode(echoPin, INPUT);   //echoPin als INPUT
}

void doMeasurement() {
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

void connectWiFi() {
  WiFi.begin(ssid, password);

  Serial.println("\nConnecting to wifi");

  while (WiFi.status() != WL_CONNECTED) {
    Serial.print(".");
    delay(100);
  }

  Serial.println("\nConnected to the WiFi network");
}

void sendMeasurement() {
  String sensorValue_str;
  String batteryLevel_str;
  char batteryLevel[50];
  char sensorValue[50];

  espClient.setCACert(ca_cert);
  client.setServer(mqtt_broker, mqtt_port);
  while (!client.connected()) {
    String client_id = "watersensor_publish";
    Serial.printf("The client %s connects to the public mqtt broker\n", client_id.c_str());
    if (client.connect(client_id.c_str(), mqtt_username, mqtt_password)) {
      Serial.println("Public emqx mqtt broker connected");
    } else {
      Serial.print("failed with state ");
      Serial.print(client.state());
      delay(2000);
    }
  }

  sensorValue_str = String(distance);
  sensorValue_str.toCharArray(sensorValue, sensorValue_str.length() + 1);

  batteryLevel_str = String(batteryPercentage);
  batteryLevel_str.toCharArray(batteryLevel, batteryLevel_str.length() + 1);

  bool levelPublished = client.publish(tankLevelTopic, sensorValue);
  if(levelPublished == true) Serial.println("Water level published.");
  else Serial.println("Failed to publish water level.");
  delay(2000);
  bool batteryPublished = client.publish(batteryLevelTopic, batteryLevel);
  if(batteryPublished == true) Serial.println("Battery level published.");
  else Serial.println("Failed to publish battery level.");
}

void loop() {
  //wifi checken
  if (WiFi.status() != WL_CONNECTED) {
    connectWiFi();
  }

  //metingen
  doMeasurement();
  sendMeasurement();

  //deep sleep
  esp_sleep_enable_timer_wakeup(1800000000);
  Serial.println("Deep sleep enabled");
  esp_deep_sleep_start();
}