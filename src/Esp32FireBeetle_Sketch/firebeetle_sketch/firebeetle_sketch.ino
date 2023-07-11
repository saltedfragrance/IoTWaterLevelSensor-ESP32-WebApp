#include <WiFi.h>

#define trigPin D10
#define echoPin D11

const char *ssid = "Home 2.4ghz";
const char *password = "Telenet12345.";
long duration;
int distance;

void setup(){
  // baudrate snelheid
  Serial.begin(115200);
  connectWiFi();

  pinMode(trigPin, OUTPUT); //trigPin als OUTPUT
  pinMode(echoPin, INPUT); //echoPin als INPUT
}

void loop(){
  doMeasurement();
}

void doMeasurement(){
  digitalWrite(trigPin, LOW);
  delayMicroseconds(2); 

  digitalWrite(trigPin, HIGH);
  digitalWrite(trigPin, LOW);

  duration = pulseIn(echoPin, HIGH);
  distance = duration * 0.0344 / 2;

  //testing
  Serial.print("Distance: ");
  Serial.print(distance);
  Serial.println(" cm");
  delay(1000);
}

void connectWiFi(){
  WiFi.begin(ssid, password);

      Serial.println("\nConnecting");

    while(WiFi.status() != WL_CONNECTED){
        Serial.print(".");
        delay(100);
    }

    Serial.println("\nConnected to the WiFi network");
}