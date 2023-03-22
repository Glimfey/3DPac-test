using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;
using System.IO.Ports;

public class PlayerMovement : MonoBehaviour
{
    public SerialPort sp = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
    private bool blnPortcanopen = false; //if portcanopen is true the selected comport is open

    //statics to communicate with the serial com thread
    static private int databyte_in; //read databyte from serial port
    static private bool databyteRead = false; //becomes true if there is indeed a character received

    //threadrelated
    private bool stopSerialThread = false; //to stop the thread
    private Thread readWriteSerialThread; //threadvariabele

    public GameObject player;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    [SerializeField]private float playerSpeed = 6.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = Physics.gravity.y;
    private bool isCrouching = false;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        OpenConnection(); //init COMPort
                          //define thread and start it
        readWriteSerialThread = new Thread(SerialThread);
        readWriteSerialThread.Start(); //start thread
    }

    void Update()
    {
        if(databyteRead)
        {
            if(databyte_in <=0 && databyte_in >= 6)
            {
                playerSpeed = databyte_in;
            }
        }

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 movement = Vector3.zero;
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        movement += transform.forward * v * playerSpeed * Time.deltaTime;
        movement += transform.right * h * playerSpeed * Time.deltaTime;
        movement += Physics.gravity;
        controller.Move(movement);

        //if (move != Vector3.zero)
        //{
        //    gameObject.transform.forward = move;
        //}

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        if (Input.GetButtonDown("Crouch") && isCrouching == false)
        {
            Vector3 vector = new Vector3(1, 0.5f, 1);
            player.transform.localScale = vector;
            isCrouching = true;
            StartCoroutine((string)CrouchTime());
            
        }
        IEnumerable CrouchTime()
        {
            yield return new WaitForSeconds(5);
        }
        if (Input.GetButtonDown("Crouch") && isCrouching == true)
        {
            Vector3 vector = new Vector3(1, 1, 1);
            player.transform.localScale = vector;
            isCrouching = false;
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