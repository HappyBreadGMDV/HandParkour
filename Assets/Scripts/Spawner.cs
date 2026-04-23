using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<MonsterData> Monsters = new List<MonsterData>();
    public List<PointSpawn> pointSpawn = new List<PointSpawn>();

    [System.Serializable]
    public class MonsterData
    {
        public GameObject prefab;
    }

    [System.Serializable] // Make PointSpawn visible in Inspector
    public class PointSpawn
    {
        public Transform Point1;
        public Transform Point2;
    }

    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            int pointId = Random.Range(0, pointSpawn.Count);
            Vector3 spawnPosition = new Vector3(
                Random.Range(pointSpawn[pointId].Point1.position.x, pointSpawn[pointId].Point2.position.x),
                pointSpawn[pointId].Point1.position.y,
                Random.Range(pointSpawn[pointId].Point1.position.z, pointSpawn[pointId].Point2.position.z)
            );
            Instantiate(Monsters[Random.Range(0, Monsters.Count)].prefab, spawnPosition, Quaternion.identity);
        }
    }
}