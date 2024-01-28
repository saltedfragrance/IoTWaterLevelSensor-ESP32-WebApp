# Water level IoT sensor for water cistern with an informative web app in Blazor

## Instructions
- Purchase the hardware as listed below, and assemble it in a waterproof enclosure
- Mount the enclosure in your water cistern, the sensor having a clear line of sight to the water level
- I used HiveMQ to communicate the sensor values to the blazor server. Be sure to adjust the login values in the resources.resx file in the WaterSensor.Server project and the SSL certificate in the certificates folder, and the data folder for the Arduino project (if your MQQT service deems it necessary to use one, if not, remove that code checking for a certificate).
- I used the preferences memory in order to save the MQQT, WiFi credentials for the Arduino sketch. Be sure to update those variables using a seperate Arduino sketch.
- In the Infrastructure project, there's some seeding for the database. Migrate it using EF.
- Start the blazor server and listen to the values communicated through MQQT. Adjust the enclosure position if values are faulty.

## Hardware used

- Microcontroller: DFRobot FireBeetle 2 ESP32-E IoT
    - Has a Wifi module and ultra-low power consumption in deep sleep mode
- DFRobot Gravity I2C Battery fuel gauge
- Ultrasonic sensor: JSN-SR04T (waterproof)
- Li-Po Battery 3.7V 2000mAh (connection provided on microcontroller)
- Breadboard and wires

## Technologies used
- c++ in the Arduino IDE on the ESP32
- MQTT via HiveMQ
- c# in Blazor
- Mudblazor
- ChartJS

## Content of web-app
- A water tank animation showing the current level of the water tank
- Consumption
    - Total consumption
    - Consumption per month/year in a graph or calendar
- Read the lithium battery and display it in the interface, using a battery fuel gauge that can read the exact %.
- A page with a calendar that predicts how much rainwater will be added to the cistern.
    - For this I use an api that gets the precipitation probabilities, and I calculate from the surface of my roof how much water will be collected.
- A page where you can subscribe to email notifications/alerts when the battery or water level gets low
- 
## Screenshots
- Main page
![Screenshot](https://github.com/vandekerckhovestijn/IoTWaterLevelSensor-ESP32-WebApp/blob/master/screenshots/screen1.png)

- Statistics page
![Screenshot](https://github.com/vandekerckhovestijn/IoTWaterLevelSensor-ESP32-WebApp/blob/master/screenshots/screen2.png)

- Calendar page
![Screenshot](https://github.com/vandekerckhovestijn/IoTWaterLevelSensor-ESP32-WebApp/blob/master/screenshots/screen3.png)

- Alerts page
![Screenshot](https://github.com/vandekerckhovestijn/IoTWaterLevelSensor-ESP32-WebApp/blob/master/screenshots/screen4.png)
  

## Resource list.
- DBContext in Blazor:
    - https://hovermind.com/blazor/recommended-approach-for-dbcontext-in-blazor-server.html
- Calendar:
    - https://danheron.github.io/Heron.MudCalendar/
- ESP32 deep sleep:
    - https://lastminuteengineers.com/esp32-deep-sleep-wakeup-sources/
    - https://www.instructables.com/ESP32-Deep-Sleep-Tutorial/
    - https://docs.espressif.com/projects/esp-idf/en/v5.0/esp32c3/api-reference/system/sleep_modes.html
- Blazor JSCharts:
    - https://www.iheartblazor.com/
- Payload MQTT decoding in c++
    - https://www.youtube.com/watch?v=w0i47IA_3z8
- SSL certification file upload in ESP32:
    - https://randomnerdtutorials.com/install-esp32-filesystem-uploader-arduino-ide/
- Credentials jamming in ESP32:
    - https://randomnerdtutorials.com/esp32-save-data-permanently-preferences/
- Battery fuel gauge:
    - https://savjee.be/blog/max17043-battery-monitoring-done-right-arduino-esp32/
- Ultrasonic sensor:
    - https://randomnerdtutorials.com/esp32-hc-sr04-ultrasonic-arduino/
