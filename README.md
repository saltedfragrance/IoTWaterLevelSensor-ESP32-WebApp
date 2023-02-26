# Waterniveau IoT sensor voor regenwaterput met een informatieve web-app in Blazor

## Project omschrijving

Wij hebben 2 regenwaterputten van elk 10000 liter naast ons huis. Alles in het huis is op regenwater, ook de douche, vaatwasser en wasmachine. Dus het is van belang om de regenputten constant op een bepaald niveau te houden zodat we niet zonder water komen te zitten. Bijvullen gebeurd met een kraantje en dit gaat traag.

Het plan is om een microcontroller zoals Arduino of ESP32, uitgerust met een waterproof ultrasone sensor en een lithium batterij te bevestigen aan de onderkant van het deksel van de regenwaterput. De doos waarin de apparatuur zit zal ook waterproof moeten zijn (ivm condensatie), met een kleine opening langs de onderkant waardoor de ultrasone sensor zijn metingen kan nemen. Alles is waterdicht gemaakt met silicone.

De sensor meet de afstand tot het wateropvlak en stuurt deze waarde door via MQTT naar Microsoft Azure IoT hub en dan naar de Blazor app. De meting gebeurt 1x per dag of via een manuele knop in de interface om batterij te besparen. De rest van de tijd is de microcontroller in deep sleep. De microcontroller word op via een timer gewekt, neemt zijn meting, en gaat dan terug in deep sleep.

Binnenshuis bevindt zich een tablet/klein scherm bevestigd aan de muur, met een interface gemaakt in Blazor.

## Inhoud van web-app

- Een animatie van een watertank met het actuele niveau van de watertank
- Verbruik
    - Totaal verbruik
    - Verbruik per dag/maand/jaar in een grafiek of kalender
- De lithium batterij uitlezen en dit weergeven in de interface
- Een pagina met een kalender waarin voorspeld wordt hoeveel regenwater er zal toegevoegd worden aan de regenput.
    - Hiervoor spreek ik een api aan die de neerslagkansen binnenhaald, en bereken ik adhv de oppervlakte van mijn dak hoeveel water er zal opgevangen worden.
- Een pagina waar je kan abonneren op push notificaties naar je telefoon

## Gebruikte apparatuur (voorlopige suggesties)

-  Microcontroller: Wemos LOLIN D32 V1
    - Heeft een Wifi module en ultralage energieconsumptie in deep sleep modus
- Ultrasone sensor: JSN-SR04T (waterdicht)
    - De microcontroller ondersteunt enkel 3.3V, deze sensor heeft 5V nodig. Maar volgens online reviews gaat het toch. Ik zie geen ander alternatief.
- Li-Po Batterij 3.7V 800mAh (connectie voorzien op microcontroller)
- Breadboard en draden

## Gebruikte technologiën
- c++ in de Arduino IDE op de ESP32
- MQTT
- Microsoft Azure IoT hub
- c# in Blazor

## Roadmap
- Maart: het apparaat bestellen, in elkaar zetten en testen op een goeie werking. Monteren aan het deksel van de regenwaterput.
- April: 
    - Een voorlopige blazor server opzetten waar ik de sensor data via MQTT kan ontvangen.
    - De sensor data via MQTT naar de Blazor app sturen.
    - Beginnen aan de interface in blazor
- Mei: De blazor interface afwerken
- Juni: Bugs fixen


## Extra info
Plaats hier de nodig informatie om het
project te kunnen uitvoeren:

- API keys of nodige secrets
- Logingegevens
- Database configuraties
- ...

## Bronnenlijst
- https://sandervandevelde.wordpress.com/2020/11/04/sending-iot-hub-telemetry-to-a-blazor-web-app/
- https://savjee.be/blog/max17043-battery-monitoring-done-right-arduino-esp32/
- https://randomnerdtutorials.com/esp32-hc-sr04-ultrasonic-arduino/
- https://www.instructables.com/ESP32-Deep-Sleep-Tutorial/
- https://docs.espressif.com/projects/esp-idf/en/v5.0/esp32c3/api-reference/system/sleep_modes.html
