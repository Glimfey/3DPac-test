using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;
using FishNet.Example.Scened;

public class Player : NetworkBehaviour
{
    public GameObject OrbHolding;
    public GameObject localHold;
    [SerializeField]
    private float cameraYOffset = 0.4f;
    private Camera playerCamera;

    public void Start()
    {
        OrbHolding.SetActive(false);
    }
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Orb")
        {
            
            if (GameManager.Instance.hasOrb  > 0)
            {
                OrbHolding.SetActive(true);
            }
        }
        else if(other.tag == "DropOff")
        {
            if (GameManager.Instance.hasOrb <= 2)
            {
                OrbHolding.SetActive(false);
            }
        }
        
    }
}
