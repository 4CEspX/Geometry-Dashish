using System.Threading;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public enum Speeds { Slow = 0, Normal = 1, Fast = 2, Faster = 3, Fastest = 4 };

public class Player : MonoBehaviour
{
    public Speeds CurrentSpeed;
    //                       0      1      2       3      4
    float[] SpeedValues = { 4f, 5f, 6f, 7f, 8f };
    public int attempts = 1;

    public float CurrState;
    public Transform GroundCheckTransform;
    public float GroundCheckRadius;
    public LayerMask GroundMask;
    public bool Grounded;
    public Transform playerTransform;
    public Text attemptsCounter;

    Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public Sprite Cube;
    public Sprite Circle;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        CurrState = 1;
        spriteRenderer.sprite = Cube;
    }

    void Update()
    {

        if (CurrState == 1)
        {
            transform.position += Vector3.right * SpeedValues[(int)CurrentSpeed] * Time.deltaTime;
            rb.gravityScale = 1;
            playerTransform.localScale = new Vector3(0.5f, 0.5f, 1);
            if (Input.GetKey(KeyCode.Space))
            {
                //Jump
                if (Grounded == true)
                {
                    rb.AddForce(Vector2.up * 4.5f, ForceMode2D.Impulse);
                    Grounded = false;
                }
            }
        }
        else if (CurrState == 2)
        {
            if (Grounded)
            {
                rb.gravityScale = 0;
            }
            playerTransform.localScale = new Vector3(0.2f, 0.2f, 1);
            transform.position += Vector3.right * SpeedValues[(int)CurrentSpeed] * Time.deltaTime;
            if (Input.GetKey(KeyCode.Space))
            {
                transform.position += Vector3.up * SpeedValues[(int)CurrentSpeed] * Time.deltaTime;
            }
            else if (!Input.GetKey(KeyCode.Space))
            {
                transform.position += Vector3.down * SpeedValues[(int)CurrentSpeed] * Time.deltaTime;
            }

        }
        else if (CurrState == 3)
        {
            transform.position += Vector3.right * SpeedValues[(int)CurrentSpeed] * Time.deltaTime;
            rb.gravityScale = -1;
            playerTransform.localScale = new Vector3(0.5f, 0.5f, 1);
            if (Input.GetKey(KeyCode.Space))
            {
                //Jump
                if (Grounded == true)
                {
                    rb.AddForce(Vector2.up * -4.5f, ForceMode2D.Impulse);
                    Grounded = false;
                }
            }

        }
        else if (CurrState == 4)
        {
            transform.position += Vector3.right * SpeedValues[(int)CurrentSpeed] * Time.deltaTime;
            playerTransform.localScale = new Vector3(0.5f, 0.5f, 1);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (rb.gravityScale < 0)
                {
                    rb.gravityScale = 3;
                    Grounded = false;
                }
                else if (rb.gravityScale > 0)
                {
                    rb.gravityScale = -3;
                    Grounded = false;
                }

            }
        }
        else
        {
            CurrState = 1;
        }


    }


    bool OnGround()
    {
        return Physics2D.OverlapCircle(GroundCheckTransform.position, GroundCheckRadius, GroundMask);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Spike")
        {
            transform.position = new Vector3(-5f, -3.6f, gameObject.transform.position.z);
            CurrState = 1;
            spriteRenderer.sprite = Cube;
            attempts++;
            Debug.Log(attempts);
            attemptsCounter.text = "Attempts: " + attempts.ToString();
        }
        if (collision.gameObject.tag == "Ground")
        {
            Grounded = true;
        }
        else
        {
            Debug.Log(collision.gameObject.name);
            Debug.Log(collision.gameObject.tag);
        }
        Thread.Sleep(1);
    }
    private void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.gameObject.tag == "Portal1")
        {
            CurrState = 2;
            Grounded = false;
            spriteRenderer.sprite = Circle;
        }
        if (trigger.gameObject.tag == "Portal2")
        {
            CurrState = 1;
            spriteRenderer.sprite = Cube;
        }
        if (trigger.gameObject.tag == "ReversePortal")
        {
            CurrState = 3;
            Grounded = false;
            spriteRenderer.sprite = Cube;
        }
        if (trigger.gameObject.tag == "Portal3")
        {
            CurrState = 4;
            rb.gravityScale = 3;
            spriteRenderer.sprite = Circle;
        }
    }

}