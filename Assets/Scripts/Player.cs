using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float laneSpeed;
    public float jumpLength;
    public float jumpHeight;
    public float slideLength;
    public int maxLife = 3;
    public float minSpeed = 10f;
    public float maxSpeed = 30f;
    static int blinkingValue;
    public float invincibleTime;
    public GameObject model;


    private bool jumping=false;
    private float jumpStart;
    private bool sliding = false;
    private float slideStart;
    private int currentLife;
    private bool invincible = false;


    


    private Animator anim;
    private Rigidbody rb;
    private BoxCollider boxCollider;
    private int currentLane = 1;
    private Vector3 verticalTargetPosition;
    private Vector3 boxColliderSize;
    private UIManager uiManager;
    private int coins;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        boxCollider = GetComponent<BoxCollider>();
        boxColliderSize = boxCollider.size;
        currentLife = maxLife;
        speed = minSpeed;
        blinkingValue = Shader.PropertyToID("_BlinkingValue");
        uiManager = FindObjectOfType<UIManager>();
    }
        // Update is called once per frame
        void Update ()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ChangeLane(-1);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                ChangeLane(1);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Jump();
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                Slide();
            }

            if (jumping)
            {
                float ratio = (transform.position.z - jumpStart) / jumpLength;
                if (ratio >= 1f)
                {
                    jumping = false;
                    anim.SetBool("Jumping", false);
                }
                else
                {
                    verticalTargetPosition.y = Mathf.Sin(ratio * Mathf.PI) * jumpHeight;

                }
            }
            else
            {
                verticalTargetPosition.y = Mathf.MoveTowards(verticalTargetPosition.y, 0, 5 * Time.deltaTime);
            }

            if (sliding)
            {
                float ratio = (transform.position.z - slideStart) / slideLength;
                if (ratio >= 1f)
                {
                    sliding = false;
                    anim.SetBool("Sliding", false);
                    boxCollider.size = boxColliderSize;
                }
            }


            Vector3 targetPosition = new Vector3(verticalTargetPosition.x, verticalTargetPosition.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, laneSpeed * Time.deltaTime);
        }
    
    private void FixedUpdate()
    {
        rb.velocity = Vector3.forward * speed;
    }
    void ChangeLane(int direction)
    {
        int targetLane = currentLane + direction;
        if (targetLane < 0 || targetLane >2)
        return;
        currentLane = targetLane;
        verticalTargetPosition = new Vector3((currentLane - 1) * 2 , 0 , 0);
    }
    void Jump()
    {
        if (!jumping)
        {
            jumpStart = transform.position.z;
            anim.SetFloat("JumpSpeed", speed / jumpLength);
            anim.SetBool("Jumping", true);
            jumping = true;
        }
    }

    void Slide()
    {
        if (!jumping && !sliding)
        {
            slideStart = transform.position.z;
            anim.SetFloat("JumpSpeed", speed / slideLength);
            anim.SetBool("Sliding", true) ;
            Vector3 newSize = boxCollider.size;
            newSize.y = newSize.y / 2;
            boxCollider.size = newSize;
            sliding = true;
        }

    }
    private void OnTriggerEnter(Collider other)
    {

        if ( other.CompareTag("Coin"))
        {
            coins++;
            uiManager.UpdateCoins(coins);
            other.transform.parent.gameObject.SetActive(false);
        }




        if (invincible)
            return;
        if(other.CompareTag("Obstacles"))
        {
            currentLife--;
            uiManager.UpdateLives(currentLife);

            anim.SetTrigger("Hit");
            speed = 0;
            if (currentLife <=0)
            {
                //game over
            }
            else
            {
                StartCoroutine(Blinking(invincibleTime));
            }

        }
    }

    IEnumerator Blinking(float time)
    {
        invincible = true;
        float timer = 0;
        float currentBlink = 1f;
        float lastBlink = 0f;
        float blinkPeriod = 0.1f;
        bool enabled = false;
        yield return new WaitForSeconds(1f); 
        speed = minSpeed;
        while (timer< time && invincible)
        {
            model.SetActive(enabled);
            //Shader.SetGlobalFloat(blinkingValue, currentBlink);
            yield return null;
            timer += Time.deltaTime;
            lastBlink += Time.deltaTime;
            if(blinkPeriod < lastBlink)
            {
                lastBlink = 0;
                currentBlink = 1f - currentBlink;
                enabled = !enabled;
            }

        }
        model.SetActive(true);
        //Shader.SetGlobalFloat (blinkingValue,0);
        invincible = false;
    }
}
