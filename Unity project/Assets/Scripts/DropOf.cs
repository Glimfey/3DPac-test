using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO.Ports;
using System.Threading;
using UnityEngine;

public class DropOf : MonoBehaviour
{
    public SerialPort sp = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
    private bool blnPortcanopen = false; //if portcanopen is true the selected comport is open

    static private int databyte_out; //index in txChars array of possible characters to send
    static private bool databyteWrite = false; //to let the serial com thread know there is a byte to send
    //txChars contains the characters to send: we have to use the index

    //threadrelated
    private bool stopSerialThread = false; //to stop the thread
    private Thread readWriteSerialThread; //threadvariabele

    private char[] txChars = { 'L' };


    public GameObject orb1;
    public GameObject orb2;
    public GameObject orb3;
    public GameObject orb4;
    public GameObject orb5;
    public int counter = 1;
    public List<GameObject> orbList = new List<GameObject>();
    public void Start()
    {
        OpenConnection(); //init COMPort
                          //define thread and start it
        readWriteSerialThread = new Thread(SerialThread);
        readWriteSerialThread.Start(); //start thread

        orb1.SetActive(false);
        orb2.SetActive(false);
        orb3.SetActive(false);
        orb4.SetActive(false);
        orb5.SetActive(false);
        orbList.Add(orb1);
        orbList.Add(orb2);
        orbList.Add(orb3);
        orbList.Add(orb4);
        orbList.Add(orb5);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && GameManager.Instance.hasOrb > 0 )
        {

            databyteWrite = true;
            GameManager.Instance.hasOrb --;
            GameManager.Instance.counter++;
            Debug.Log(GameManager.Instance.counter);
            if (counter != 6)
            {
                orbList[counter].SetActive(true);
                counter ++;
            }
        }
        
    }

    void SerialThread() //separate thread is needed because we need to wait sp.ReadTimeout = 20 ms to see if a byte is received
    {
        while (!stopSerialThread) //close thread on exit program
        {
            if (blnPortcanopen)
            {
                if (databyteWrite)
                {
                    sp.Write(txChars, 0, 1); //tx 'L'
                    databyteWrite = false; //to be able to send again
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
