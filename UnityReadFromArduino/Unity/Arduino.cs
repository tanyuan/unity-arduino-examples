using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
// For Serial
using System.IO.Ports;
// For compare array
using System.Linq;

public class Arduino : MonoBehaviour {

	// Serial
	public string portName;
	public int baudRate = 9600;
	SerialPort arduinoSerial;

	public int sensor;

	void Start () {
		// Open Serial port
		arduinoSerial = new SerialPort (portName, baudRate);
		// Set buffersize so read from Serial would be normal
		arduinoSerial.ReadTimeout = 1;
		arduinoSerial.ReadBufferSize = 8192;
		arduinoSerial.WriteBufferSize = 128;
		arduinoSerial.ReadBufferSize = 4096;
		arduinoSerial.Parity = Parity.None;
		arduinoSerial.StopBits = StopBits.One;
		arduinoSerial.DtrEnable = true;
		arduinoSerial.RtsEnable = true;;
		arduinoSerial.Open ();
	}

	void Update() {
		ReadFromArduino ();
	}

	public void ReadFromArduino () {
		string str = null;
		try {
			str = arduinoSerial.ReadLine();
//			Debug.Log(str);
			int number;
			if (Int32.TryParse(str, out number)) {
				sensor = number;
			}
		}
		catch (TimeoutException e) {
		}
	}
}