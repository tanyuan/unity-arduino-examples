#include <Servo.h>

// Arduino pins
// Servos
const int servoPin [2] = {9, 10};

Servo servos [2];

// Servo degrees read from Serial
int degs [2];

// Temp variables for Serial read
char inData[32];
bool isRead = false;
int index = 0;

void setup() {
  // Initialize Servos
  for (int i = 0; i < 2; ++i) {
    servos[i].attach(servoPin[i]);
  }
  // Open Serial
  Serial.begin(9600);
}

void loop() {
  // Read from Serial
  readSerialStr(degs, 2);

  // Wrtie degrees to servos
  for (int i = 0; i < 2; ++i) {
    servos[i].write(degs[i]);
  }

  // waits for the servo to get there
  delay(15);
}

void readSerialStr(int data [], int size) {
  if (Serial.available() > 0) {
    char incomingByte = Serial.read();
    while (incomingByte != '\n' && isDigit(incomingByte)) {
      isRead = true;
      inData[index] = incomingByte;
      index++;
      incomingByte = Serial.read();
    }
    inData[index] = '\0';
  }

  if (isRead) {
    char data1_char[4];
    char data2_char[4];

    data1_char[0] = inData[0];
    data1_char[1] = inData[1];
    data1_char[2] = inData[2];
    data1_char[3] = '\0';

    data2_char[0] = inData[3];
    data2_char[1] = inData[4];
    data2_char[2] = inData[5];
    data2_char[3] = '\0';

    data [0] = atoi(data1_char);
    data [1] = atoi(data2_char);

    isRead = false;
    index = 0;
  }
}
