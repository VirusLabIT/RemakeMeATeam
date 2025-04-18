using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DelaunatorSharp;
using Unity.VisualScripting;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class DungeonGenerator : MonoBehaviour
{
    public LayerMask LayerToCheck;
    public NavMeshSurface surface;
    public List<Vector2> RoomsPos = new List<Vector2>();
    public List<GameObject> Doors;
    public List<Vector2> DoorsPos;
    public GameObject[] Rooms;
    public int numberOfRooms;

    public float xOffset;
    public int xMaxRandom;
    public int yMaxRandom;
    // Start is called before the first frame update
    void Start()   
    {
        surface = GetComponent<NavMeshSurface>();
        List<GameObject> list = Rooms.ToList();

        // THIS IS TEMPORARY, I MADE IT TO DEBUG WHAT, AND HOW MANY ROOMS SPAWN
        GameObject[] rooms = new GameObject[numberOfRooms];

        for (int i = 0; i < numberOfRooms; i++)
        {
            int randomNumber = UnityEngine.Random.Range(0, list.Count);
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
                spawnedRoom = Instantiate(rooms[i], new Vector3(UnityEngine.Random.Range(-xMaxRandom, xMaxRandom) + xOffset, 0, UnityEngine.Random.Range(-yMaxRandom, yMaxRandom)), Quaternion.identity);
                tries++;

                if (tries % 50 == 0 && tries != 0)
                {
                    Debug.Log("CANT SPAWN A ROOM. CHANGING MAX RANDOM VALUES");
                    xMaxRandom += 5;
                    yMaxRandom += 5;
                }

            } while (Physics.OverlapBox(spawnedRoom.transform.position, spawnedRoom.transform.localScale / 2, Quaternion.identity, LayerToCheck).Length > 1);
            RoomsPos.Add(new Vector2(spawnedRoom.transform.position.x, spawnedRoom.transform.position.z));
        }

        
        Debug.Log(rooms.Length);
        foreach (var room in rooms)
        {
            foreach (Transform child in room.transform)
                {
                    if (child.gameObject.layer == LayerMask.NameToLayer("Door"))
                    {
                        // Add the child GameObject to the list
                        Doors.Add(child.gameObject);
                        DoorsPos.Add(new Vector2(child.transform.position.x, child.transform.position.z));
                        Debug.Log($"DOOR FOUND AT: {child.transform.position}");
                    }
                 }

        }
        surface.BuildNavMesh();
    }

     private void OnDrawGizmos()
    {   
        //this draws the lines between the rooms, it just help you to visualize the connections, if you tought otherwise, fuck you
        if (RoomsPos.Count >= 3){
        var MST = MSTBuilder.BuildMST(RoomsPos.Select(pos => (IPoint)new Point(pos.x, pos.y)).ToArray());
        foreach (var edge in MST)
        {
            Vector2 u = RoomsPos[edge.U];
            Vector2 v = RoomsPos[edge.V];
            Gizmos.color = Color.green;
            Gizmos.DrawLine(new Vector3(u.x, 0, u.y), new Vector3(v.x, 0, v.y));
        }
        }
    }
}

