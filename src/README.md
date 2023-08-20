# Waterniveau IoT sensor voor regenwaterput met een informatieve web-app in Blazor

## Project omschrijving

Wij hebben 2 regenwaterputten van elk 10000 liter naast ons huis. Alles in het huis is op regenwater, ook de douche, vaatwasser en wasmachine. Dus het is van belang om de regenputten constant op een bepaald niveau te houden zodat we niet zonder water komen te zitten. Bijvullen gebeurd met een kraantje en dit gaat traag.

Het plan is om een ESP32 microcontroller, uitgerust met een waterproof ultrasone sensor en een lithium batterij te bevestigen aan de zijkant van de regenwaterput. De doos waarin de apparatuur zit zal ook waterproof moeten zijn (ivm condensatie), met een kleine opening langs de onderkant waardoor de ultrasone sensor zijn metingen kan nemen. Alles is waterdicht gemaakt met silicone.

De sensor meet de afstand tot het wateropvlak en stuurt deze waarde door via MQTT naar een cloud MQTT broker (HiveMQ) en dan naar de Blazor app. De meting gebeurt 1x per dag of via een manuele knop in de interface om batterij te besparen. De rest van de tijd is de microcontroller in deep sleep. De microcontroller word op via een timer gewekt, neemt zijn meting, en gaat dan terug in deep sleep. Het update interval in aan te passen via de web-app interface.

## Inhoud van web-app

- Een animatie van een watertank met het actuele niveau van de watertank
- Verbruik
    - Totaal verbruik
    - Verbruik per maand/jaar in een grafiek of kalender
- De lithium batterij uitlezen en dit weergeven in de interface, dmv een 'battery fuel gauge' die exact het % kan uitlezen
- Een pagina met een kalender waarin voorspeld wordt hoeveel regenwater er zal toegevoegd worden aan de regenput.
    - Hiervoor spreek ik een api aan die de neerslagkansen binnenhaald, en bereken ik adhv de oppervlakte van mijn dak hoeveel water er zal opgevangen worden.
- Een pagina waar je kan abonneren op email notificaties/alarms wanneer de batterij of het waterniveau laag komt te staan

## Gebruikte apparatuur

-  Microcontroller: DFRobot FireBeetle 2 ESP32-E IoT
    - Heeft een Wifi module en ultralage energieconsumptie in deep sleep modus
- DFRobot Gravity I2C Battery fuel gauge
- Ultrasone sensor: JSN-SR04T (waterdicht)
- Li-Po Batterij 3.7V 2000mAh (connectie voorzien op microcontroller)
- Breadboard en draden

## Gebruikte technologiën
- c++ in de Arduino IDE op de ESP32
- MQTT via HiveMQ
- c# in Blazor
- Mudblazor
- ChartJS

## Roadmap
- Juni: het apparaat bestellen, in elkaar zetten en testen op een goeie werking. Monteren in de regenwaterput.
- Juli: 
    - Een voorlopige blazor server opzetten waar ik de sensor data via MQTT kan ontvangen.
    - De sensor data via MQTT naar de Blazor app sturen.
    - Beginnen aan de interface in blazor
- Augustus: De blazor interface afwerken en bugs fixen

## Bronnenlijst
- DBContext in Blazor:
    - https://hovermind.com/blazor/recommended-approach-for-dbcontext-in-blazor-server.html
- Kalender:
    - https://danheron.github.io/Heron.MudCalendar/
- ESP32 deep sleep:
    - https://lastminuteengineers.com/esp32-deep-sleep-wakeup-sources/
    - https://www.instructables.com/ESP32-Deep-Sleep-Tutorial/
    - https://docs.espressif.com/projects/esp-idf/en/v5.0/esp32c3/api-reference/system/sleep_modes.html
- Blazor JSCharts:
    - https://www.iheartblazor.com/
- Payload MQTT in c++ decoderen
    - https://www.youtube.com/watch?v=w0i47IA_3z8
- SSL certification file upload in ESP32:
    - https://randomnerdtutorials.com/install-esp32-filesystem-uploader-arduino-ide/
- Credentials storen in ESP32:
    - https://randomnerdtutorials.com/esp32-save-data-permanently-preferences/
- Battery fuel gauge:
    - https://savjee.be/blog/max17043-battery-monitoring-done-right-arduino-esp32/
- Ultrasone sensor:
    - https://randomnerdtutorials.com/esp32-hc-sr04-ultrasonic-arduino/
