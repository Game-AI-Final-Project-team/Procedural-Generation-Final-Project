using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb.linearVelocityY = 5;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            rb.linearVelocityY = -5;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.linearVelocityX = -5;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.linearVelocityX = 5;
        }
    }
}
