const int sensorPin = A0;

void setup() {
  pinMode(sensorPin, INPUT);
  Serial.begin(9600);
}

void loop() {
  int val = analogRead(sensorPin);
  Serial.println(val);
  delay(100);
}
