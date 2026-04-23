//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class GenerationManager : MonoBehaviour
//{
//    public GameObject[] mapsPrefabs;
//    public int prefabsToGenerate = 10;

//    private GameObject newObj;

//    void Start()
//    {
//        for (int i = 0; i < prefabsToGenerate; i++)
//        {

//            int randomIndex = Random.Range(0, mapsPrefabs.Length);

//            for (int j = 0; j < newObj.GetComponent<PartManager>().MapEnds.Length; j++)
//            {
//                Vector3 spawnPos = transform.position + new Vector3(0, newObj.GetComponent<PartManager>().MapEnds[j].transform.position.y, 0);

//                int yrotation = (new int[] { 0, 60, 120, 240 })[Random.Range(0, 4)];
//                newObj = Instantiate(mapsPrefabs[randomIndex], spawnPos, Quaternion.Euler(new Vector3(0, yrotation, 0)));
//                newObj.transform.parent = this.transform; 
//            }

//        }
//    }
//}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationManager : MonoBehaviour
{
    public GameObject[] mapsPrefabs;
    public int prefabsToGenerate = 10;

    private Transform[] pos;

    void Start()
    {
        int randomIndex = Random.Range(0, mapsPrefabs.Length);
        int yrotation = (new int[] { 0, 60, 120, 240 })[Random.Range(0, 4)];
        GameObject firstObj = Instantiate(mapsPrefabs[randomIndex], transform.position, Quaternion.Euler(new Vector3(0, yrotation, 0)));
        firstObj.transform.parent = this.transform;

        pos = firstObj.GetComponent<PartManager>().MapEnds;

        for (int i = 1; i < prefabsToGenerate; i++)
        {
            List<Transform> newEndsList = new List<Transform>();

            for (int j = 0; j < pos.Length; j++)
            {
                randomIndex = Random.Range(0, mapsPrefabs.Length);
                yrotation = (new int[] { 0, 60, 120, 240 })[Random.Range(0, 4)];

                Vector3 spawnPos = pos[j].position;
                GameObject newObj = Instantiate(mapsPrefabs[randomIndex], spawnPos, Quaternion.Euler(new Vector3(0, yrotation, 0)));
                newObj.transform.parent = this.transform;

                Transform[] ends = newObj.GetComponent<PartManager>().MapEnds;
                newEndsList.AddRange(ends);
            }

            pos = newEndsList.ToArray();
        }
    }
}