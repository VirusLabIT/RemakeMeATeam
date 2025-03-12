using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
     public GameObject ScenesMGR;
    // Start is called before the first frame update
    void Start()
    {
        ScenesMGR = GameObject.FindGameObjectWithTag("ScenesMGR");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnToMainScene(){
       Destroy(Player.Instance.LastEnemy);
       ScenesMGR.GetComponent<ScenesMGR>().UnloadScene("Battle", "SampleScene");
    }
}
