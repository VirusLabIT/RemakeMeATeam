using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector3 moveVector = new Vector3 (moveX, 0, moveY);

        if (moveX != 0 && moveY != 0)
        {
            moveVector = moveVector / moveVector.magnitude;
        }

        rb.AddForce(new Vector3(moveX, 0, moveY).normalized * Time.deltaTime * speed, ForceMode.Impulse);
    }
}
