using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DelaunatorSharp.Unity.Extensions;
using DelaunatorSharp;
using Unity.VisualScripting;

public class DungeonGenerator : MonoBehaviour
{
    public List<Vector2> RoomsPos = new List<Vector2>();
    public GameObject[] Rooms;
    public int numberOfRooms;

    public float xOffset;
    public float xMaxRandom;
    public float yMaxRandom;
    // Start is called before the first frame update
    void Start()
    
    {
        List<GameObject> list = Rooms.ToList();

        // THIS IS TEMPORARY, I MADE IT TO DEBUG WHAT, AND HOW MANY ROOMS SPAWN
        GameObject[] rooms = new GameObject[numberOfRooms];

        for (int i = 0; i < numberOfRooms; i++)
        {
            int randomNumber = Random.Range(0, list.Count);
            rooms[i] = list[randomNumber];

            GameObject spawnedRoom = null;
            list.RemoveAt(randomNumber);

            int tries = 0;
            do
            {
                Debug.Log($"ATTEMPING TO SPAWN A ROOM. TRIES: {tries}");
                if (spawnedRoom != null)
                {
                    Destroy(spawnedRoom);
                }
                spawnedRoom = Instantiate(rooms[i], new Vector3(Random.Range(-xMaxRandom, xMaxRandom) + xOffset, 0, Random.Range(-yMaxRandom, yMaxRandom)), Quaternion.identity);
                tries++;

                if(tries % 50 == 0 && tries != 0)
                {
                    Debug.Log("CANT SPAWN A ROOM. CHANGING MAX RANDOM VALUES");
                    xMaxRandom += 5;
                    yMaxRandom += 5;
                }

            } while (Physics.OverlapBox(spawnedRoom.transform.position, spawnedRoom.transform.localScale / 2, Quaternion.identity).Length > 1);
            RoomsPos.Add(new Vector2(spawnedRoom.transform.position.x, spawnedRoom.transform.position.z));
        }

        
        Debug.Log(rooms.Length);
    }

    private Delaunator delaunay(){
        Delaunator DN;
        DN = new Delaunator(RoomsPos.Select(pos => (IPoint)new Point(pos.x, pos.y)).ToArray());
        return DN;

    }
    // Update is called once per frame
    void Update()
    {

    }
}
