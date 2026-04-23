using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartManager : MonoBehaviour
{
    public Transform[] MapEnds;

    public GameObject MapStart;

    public void Start()
    {
        transform.position += transform.position - MapStart.transform.position;
    }
}
