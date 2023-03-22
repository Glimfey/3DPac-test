using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System;

public class Orb : MonoBehaviour
{
    public SerialPort sp = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
    private bool blnPortcanopen = false; //if portcanopen is true the selected comport is open

    //statics to communicate with the serial com thread
    static private int databyte_in; //read databyte from serial port
    static private bool databyteRead = false; //becomes true if there is indeed a character received

    //threadrelated
    private bool stopSerialThread = false; //to stop the thread
    private Thread readWriteSerialThread; //threadvariabele

    bool inRange = true;
    public GameObject orb;
    public void Start()
    {
        OpenConnection(); //init COMPort
                          //define thread and start it
        readWriteSerialThread = new Thread(SerialThread);
        readWriteSerialThread.Start(); //start thread
    }

    public void Update()
    {
        if(databyteRead)
        {
            if(databyte_in == 'K')
            {

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        while(inRange)
        {
            if (databyteRead)
            {
                if (databyte_in == 'K')
                {
                    if (other.tag == "Player" && GameManager.Instance.hasOrb < 2)
                    {
                        GameManager.Instance.hasOrb++;

                        inRange = false;
                        Destroy(gameObject);
                    }
                }
            }
        }
       
    }

    void SerialThread() //separate thread is needed because we need to wait sp.ReadTimeout = 20 ms to see if a byte is received
    {
        while (!stopSerialThread) //close thread on exit program
        {
            if (blnPortcanopen)
            {
                try //trying something to receive takes 20 ms = sp.ReadTimeout
                {
                    databyte_in = sp.ReadChar();
                    databyteRead = true;
                }
                catch (Exception)
                {
                    //Debug.Log(e.Message);
                }
            }
        }
    }


    //Function connecting to Arduino
    public void OpenConnection()
    {
        if (sp != null)
        {
            if (sp.IsOpen)
            {
                string message = "Port is already open!";
                Debug.Log(message);
            }
            else
            {
                try
                {
                    sp.Open();  // opens the connection
                    blnPortcanopen = true;
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                    blnPortcanopen = false;
                }
                if (blnPortcanopen)
                {
                    sp.ReadTimeout = 20;  // sets the timeout value before reporting error
                    Debug.Log("Port Opened!");
                }
            }
        }
        else
        {
            Debug.Log("Port == null");
        }
    }


    void OnApplicationQuit() //proper afsluiten van de thread
    {
        if (sp != null) sp.Close();
        stopSerialThread = true;
        readWriteSerialThread.Abort();
    }


}
