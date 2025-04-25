using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UI;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject DirectionVisualizer;
    public Vector2 Direction;
    // Start is called before the first frame update
    void Start()
    {
        Direction = (new Vector2(DirectionVisualizer.transform.position.x + 2 , DirectionVisualizer.transform.position.z +2 )- new Vector2(transform.position.x + 2, transform.position.z + 2)).normalized;

        //DirectionVisualizer.SetActive(false);
        //$Remove Comment above to hide the visualizer 
        Debug.Log(transform.position);
        
    }
}
