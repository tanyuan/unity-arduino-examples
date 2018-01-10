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

	// Write to Serial immediately once change in degrees
	public bool realtimeUpdate;

	public int sensor;

	// Servo degrees
	[Range(0, 180)]
	public int [] degs = new int[2];
	int [] lastDegs = new int[2];
	const int degSize = 2;

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

		// Initialize lastDegs for detecting degs change
		for (int i = 0; i < degSize; ++i) {
			lastDegs [i] = degs [i];
		}
			
	}

	void Update() {

		ReadFromArduino ();

		if (realtimeUpdate) {
			// Only write to Serial if any degree changes
			if ( !lastDegs.SequenceEqual (degs) ) {
				WriteDegsToSerial ();
				for (int i = 0; i < degSize; ++i) {
					lastDegs [i] = degs [i];
				}
			}
		}
	}

	public void Reset () {
		// Reset all servos to 0 degree
		for (int i = 0; i < degSize; ++i) {
			degs [i] = 0;
		}
		WriteDegsToSerial ();
	}

	public void WriteDegsToSerial () {
		if (arduinoSerial.IsOpen) {

			string [] strs = new string [2];

			// Convert all integers to 3-digit string
			strs[0] = degs [0].ToString ("000");
			strs[1] = degs [1].ToString ("000");

			// The string to write to Serial
			string str = strs[0] + strs[1];

			arduinoSerial.WriteLine (str);
			arduinoSerial.BaseStream.Flush();

			Debug.Log ("Send Serial: " + str);
		}
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