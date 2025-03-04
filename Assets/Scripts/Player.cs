using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private static Player instance;

    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<Player>();
                    singletonObject.name = typeof(Player).ToString() + " (Singleton)";
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public bool CanMove;
    public float speed;

    public int MaxJumpCount = 2;
    public int JumpCount = 0;
    public Camera Maincam;

    public GameObject LastEnemy;
    void Start()
    {

    }

    void Update()
    {
        if (CanMove){
            Move();
        }
    }

    private void Move()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (Maincam != null)
        {
            Maincam.transform.position = transform.position - new Vector3(0, -1, 5);
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        rb.MovePosition(transform.position + move * speed);
        if (Input.GetKeyDown(KeyCode.Space) && JumpCount < MaxJumpCount)
        {
            rb.AddForce(Vector3.up * 300, ForceMode.Force);
            JumpCount++;
        }
        else if (IsGrounded())
        {
            JumpCount = 0;
        }
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.1f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        { 
            LastEnemy = collision.gameObject;
            ScenesMGR.Instance.LoadScene("Battle", "SampleScene");
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            LastEnemy = null;
        }
    }
}

