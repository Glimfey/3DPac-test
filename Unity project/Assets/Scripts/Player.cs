using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;
using FishNet.Example.Scened;

public class Player : NetworkBehaviour
{
    public GameObject OrbHolding;
    private Camera playerCamera;
    private float cameraYOffset =  0.4f;
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsOwner)
        {
            playerCamera = Camera.main;
            playerCamera.transform.position = new Vector3(transform.position.x, transform.position.y + cameraYOffset, transform.position.z);
            playerCamera.transform.SetParent(transform);
        }
        else
        {
            gameObject.GetComponent<PlayerController>().enabled = false;
        }
    }
    public void Start()
    {
        OrbHolding.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Orb")
        {
            
            if (GameManager.Instance.hasOrb == true)
            {
                OrbHolding.SetActive(true);
            }
        }
        else if(other.tag == "DropOff")
        {
            if (GameManager.Instance.hasOrb == false)
            {
                OrbHolding.SetActive(false);
            }
        }
        
    }
}
