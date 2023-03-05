using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject OrbHolding;

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
