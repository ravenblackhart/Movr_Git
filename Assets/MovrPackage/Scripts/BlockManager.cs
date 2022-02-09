using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    [SerializeField] private GameObject pedestrian;
    [SerializeField] private int PedestriansToSpawn;
    private float nodeRadius;
    private Pedestrian[] pedestrians;

    private void Start()
    {
        SpawnPedestrians();
    }
    public Transform GetNextNode(int c, bool clockwise)
    {
        int i = 0;
        if (clockwise)
        {
            if (c == transform.childCount - 1)
                i = 0;
            else
                i = c + 1;
        }
        else
        {
            if (c == 0)
                i = transform.childCount - 1;
            else
                i = c - 1;
        }
        return transform.GetChild(i);
        
        
    }
    private void SpawnPedestrians()
    {
        pedestrians = new Pedestrian[PedestriansToSpawn];

        for (int i = 0; i < PedestriansToSpawn; i++)
        {
            int nodeA = Random.Range(0, transform.childCount);
            int nodeB = nodeA + 1 <= transform.childCount - 1 ? nodeA + 1 : 0;

            Vector3 spawnPos = Vector3.Lerp(transform.GetChild(nodeA).position, transform.GetChild(nodeB).position, Random.Range(0.2f, 0.8f));
            spawnPos = new Vector3(spawnPos.x, 0f, spawnPos.z);

            GameObject p = Instantiate(pedestrian, spawnPos, Quaternion.identity);
            pedestrians[i] = p.GetComponent<Pedestrian>();
            pedestrians[i].blockManager = this;
            pedestrians[i].SetTargetPos(transform.GetChild(nodeA).transform, transform.GetChild(nodeB).transform);
            
        }
    }
}
