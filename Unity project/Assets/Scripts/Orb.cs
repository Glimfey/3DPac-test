using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    
    
    public void Start()
    {
         
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && GameManager.Instance.hasOrb < 2)
        {
            GameManager.Instance.hasOrb ++;
            

            Destroy(gameObject);
        }
       
    }


}
