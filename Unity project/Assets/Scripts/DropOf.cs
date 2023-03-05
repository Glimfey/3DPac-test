using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class DropOf : MonoBehaviour
{
    public GameObject orb1;
    public GameObject orb2;
    public GameObject orb3;
    public GameObject orb4;
    public GameObject orb5;
    public int counter = 1;
    public List<GameObject> orbList = new List<GameObject>();
    public void Start()
    {
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
        if (other.tag == "Player" && GameManager.Instance.hasOrb == true)
        {
            GameManager.Instance.hasOrb = false;
            GameManager.Instance.counter++;
            Debug.Log(GameManager.Instance.counter);
            if (counter != 6)
            {
                orbList[counter].SetActive(true);
                counter ++;
            }
        }
        
    }

}
