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
    public Sprite Isometric;
    public float timer = 0.2f;
    public bool onOrbCollision = false;
    BoxCollider2D bc;



    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        CurrState = 1;
        spriteRenderer.sprite = Cube;
    }

    void Update()
    {
        if (CurrState == 5)
        {
            bc.size = new Vector3(1, 0.55f, 1);
        }
        else
        {
            bc.size = new Vector3(1, 1, 1);
        }
        if (onOrbCollision == true)
        {
            timer -= Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (CurrState == 1)
                {
                    rb.velocity = new Vector3(SpeedValues[(int)CurrentSpeed], 0, 0);
                    rb.AddForce(Vector2.up * 6f, ForceMode2D.Impulse);
                    onOrbCollision = false;
                    timer = 0.2f;
                }
                else if (CurrState == 3)
                {
                    rb.velocity = new Vector3(SpeedValues[(int)CurrentSpeed], 0, 0);
                    rb.AddForce(Vector2.up * -6f, ForceMode2D.Impulse);
                    onOrbCollision = false;
                    timer = 0.2f;
                }
            }
            if (timer <= 0)
            {
                onOrbCollision = false;
                timer = 0.2f;
            }
        }

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
                    rb.velocity = new Vector3(SpeedValues[(int)CurrentSpeed], 0, 0);
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
                    rb.velocity = new Vector3(SpeedValues[(int)CurrentSpeed], 0, 0);
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
        else if (CurrState == 5)
        {
            transform.position += Vector3.right * SpeedValues[(int)CurrentSpeed] * Time.deltaTime;
            playerTransform.localScale = new Vector3(0.7f, 0.5f, 1);
            if (Input.GetKey(KeyCode.Space))
            {
                if (rb.gravityScale == 1)
                {
                    rb.gravityScale = -1;
                }
            }
            else
            {
                rb.gravityScale = 1;
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
            transform.position = new Vector3(-6f, -3.6f, gameObject.transform.position.z);
            CurrState = 1;
            spriteRenderer.sprite = Cube;
            attempts++;
            attemptsCounter.text = "Attempts: " + attempts.ToString();
        }
        if (collision.gameObject.tag == "Ground")
        {
            Grounded = true;
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
        if (trigger.gameObject.tag == "Pad1")
        {
            if (CurrState == 1)
            {
                rb.velocity = new Vector3(SpeedValues[(int)CurrentSpeed], 0, 0);
                rb.AddForce(Vector2.up * 6f, ForceMode2D.Impulse);
                Grounded = false;
            }
            if (CurrState == 3)
            {
                rb.velocity = new Vector3(SpeedValues[(int)CurrentSpeed], 0, 0);
                rb.AddForce(Vector2.up * -6f, ForceMode2D.Impulse);
                Grounded = false;
            }
        }
        if (trigger.gameObject.tag == "Orb1")
        {
            onOrbCollision = true;
        }
        if (trigger.gameObject.tag == "Portal4")
        {
            CurrState = 5;
            spriteRenderer.sprite = Isometric;
            
        }
    }
}