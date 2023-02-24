using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOrbs : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject originalOrb;
    void Start()
    {
        List<Vector3> orbList = OrbPositionAssign();
        for (int i = 0; i < 5; i++)
        {
            SpawnOrb(RandomOrbPos(orbList));
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    List<Vector3> OrbPositionAssign()
    {
        List<Vector3> orbPosList = new List<Vector3>();
        orbPosList.Add(new Vector3(-1, 1, -1));
        orbPosList.Add(new Vector3(21, 1, 10));
        orbPosList.Add(new Vector3(22, 1, 61));
        orbPosList.Add(new Vector3(-11, 1, 35));
        orbPosList.Add(new Vector3(-41, 1, 12));
        orbPosList.Add(new Vector3(-41, 1, -17));
        orbPosList.Add(new Vector3(-41, 1, -31));
        orbPosList.Add(new Vector3(-29, 1, -31));
        orbPosList.Add(new Vector3(-31, 1, -18));
        orbPosList.Add(new Vector3(-13, 1, -35));
        return orbPosList;
        
    }
    Vector3 RandomOrbPos(List<Vector3> orbList)
    {
        int randomNum = Random.Range(0, orbList.Count);
        Vector3 assigndLocation = orbList[randomNum];
        orbList.RemoveAt(randomNum);
        return assigndLocation;
    }
    void SpawnOrb(Vector3 vector3)
    {
        GameObject orbClone = Instantiate(originalOrb, vector3, transform.rotation);
    }
}
