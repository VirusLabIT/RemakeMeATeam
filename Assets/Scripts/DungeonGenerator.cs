using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DelaunatorSharp;
using Unity.VisualScripting;
using UnityEngine.AI;
using Unity.AI.Navigation;
using System.Collections;

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
        
        surface.BuildNavMesh();

        StartCoroutine(FindDoors());
    }

    private IEnumerator FindDoors(){
        yield return new WaitForSeconds(0.3f);

        Door[] myItems = FindObjectsOfType(typeof(Door)) as Door[];
            Debug.Log ("Found " + myItems.Length + " instances with this script attached");
            foreach(Door item in myItems)
            {
                DoorsPos.Add(new Vector2(item.gameObject.transform.position.x, item.gameObject.transform.position.z));
                Doors.Add(item.gameObject);
            }

        /*foreach (var room in Rooms)
        {
            foreach (Transform child in room.transform)
            {
                if (child.gameObject.layer == LayerMask.NameToLayer("Door"))
                {
                    // Add the child GameObject to the list
                    Transform doorTransform = child.transform;
                    GameObject door = child.gameObject;
                    Vector3 DoorPos = room.transform.TransformPoint(doorTransform.localPosition);
                    Vector3 worldPos = child.parent.localToWorldMatrix.MultiplyPoint(child.localPosition);
                    Doors.Add(door);
                    DoorsPos.Add(new Vector2(DoorPos.x, DoorPos.z));
                    Debug.Log($"DOOR FOUND AT: {worldPos}");
                }
            }
        }
        */
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
        

        var edges = ConnectDoors();   
        foreach (var edge in edges)
        {
            Vector2 u = DoorsPos[edge.U];
            Vector2 v = DoorsPos[edge.V];

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(new Vector3(u.x, 0, u.y), new Vector3(v.x, 0, v.y));
        }
        
    }

    //Fuck you TheRKey
    private List<Edge> ConnectDoors(bool debug = false)
    {
        List<Edge> edges = new List<Edge>{};

        // Connect each door to the nearest door in a different room
        foreach (var door in Doors)
        {
            Vector2 doorPos = new Vector2(door.transform.position.x, door.transform.position.z);
            GameObject closestDoor = null;
            List<float> distances = new List<float>{};

            foreach (var otherDoor in Doors)
            {
                if (door == otherDoor) continue; // Skip the same door
                if (door.transform.parent.gameObject == otherDoor.transform.parent.gameObject) continue; // Skip doors in the same room

                Vector2 otherDoorPos = new Vector2(otherDoor.transform.position.x, otherDoor.transform.position.z);
                distances.Add(Vector2.Distance(doorPos, otherDoorPos));
            }
            if (debug == true){Debug.Log($"Distances: {distances.Count}");}

            if (closestDoor == null && distances.Count != 0)
            {
                closestDoor = Doors[distances.IndexOf(distances.Min())];
                if (debug == true){Debug.Log($"Closest Door: {closestDoor.transform.position}");}
            }

            if(debug == true){Debug.Log(DoorsPos.FindIndex(pos => pos == new Vector2(door.transform.position.x, door.transform.position.z)));}
            //Debug.Log(DoorsPos.FindIndex(pos => pos == new Vector2(closestDoor.transform.position.x, closestDoor.transform.position.z)));
                //add to "edges" the INDEX OF POSITIONS OF THE DOORS IN DOORSPOS
            edges.Add(new Edge(DoorsPos.FindIndex(pos => pos == new Vector2(door.transform.position.x, door.transform.position.z)), DoorsPos.FindIndex(pos => pos == new Vector2(closestDoor.transform.position.x, closestDoor.transform.position.z)), Vector2.Distance(doorPos, new Vector2(closestDoor.transform.position.x, closestDoor.transform.position.z))));
            if (debug== true){Debug.Log(edges.Count);}
            
        }
        return edges;
    }

}

