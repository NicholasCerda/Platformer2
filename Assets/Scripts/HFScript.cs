using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HFScript : MonoBehaviour
{
    Transform myTransform;
    public AnimatorMoveScript amSc;
    public GuiScript guiSc;
    public GameObject Mario;
    public GameObject EventMnger;
    public GameObject otherhs;
    public bool sprint;
    public Rigidbody rigidbody,origidbody;
    private float move;
    private bool gameOver;
    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;
        amSc = Mario.GetComponent<AnimatorMoveScript>();
        guiSc = EventMnger.GetComponent<GuiScript>();
        rigidbody = gameObject.GetComponent<Rigidbody>();
        myTransform = transform;
        if (gameObject.name=="Head")
            otherhs= GameObject.Find("Feet");
        else
            otherhs= GameObject.Find("Feet");
        origidbody = otherhs.GetComponent<Rigidbody>();
        move = 0;
    }

    // Update is called once per frame
    void Update()
    {
        while (rigidbody.velocity.y > 6)
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y * 0.9f, rigidbody.velocity.z);
        }
        if (transform.position.y < 0 &&!gameOver)
        {
            gameOver = true;
            guiSc.gameOver("Player fell off map");
        }
        sprint = false;
        if (gameObject.name == "Feet")
        {
            float tempmove = move,jumpspeed=6.0f;
            if (!amSc.airborne)
            {
                move =1.5f*Input.GetAxis("Horizontal");
            }
            if (Input.GetButton("Turbo"))
            {
                tempmove = move * 1.5f;
            }

            if (Input.GetButton("Jump") && !amSc.jump && !amSc.airborne)
            {
                amSc.jump = true;
                amSc.airborne = true;

                if (Mathf.Abs(tempmove) < .3)
                {
                    StartCoroutine("Waitforjump");
                }else
                    rigidbody.AddForce(0, jumpspeed, 0, ForceMode.Impulse);
                while (rigidbody.velocity.y > 6)
                {
                    rigidbody.velocity =new Vector3(rigidbody.velocity.x, rigidbody.velocity.y*0.9f, rigidbody.velocity.z);
                }
            }
            Vector3 temp = new Vector3(myTransform.transform.position.x + tempmove, myTransform.transform.position.y + rigidbody.velocity.y, -0.5f);
            myTransform.position = Vector3.Lerp(myTransform.transform.position, temp, Mathf.Abs(tempmove) * Time.deltaTime);
            amSc.vmove = rigidbody.velocity.y;
            Mario.transform.position = gameObject.transform.position - new Vector3(0, 0.1f, 0);
        }
        if (gameObject.name == "Head")
        {
            gameObject.transform.position = GameObject.Find("EthanHead").transform.position;//Mario.transform.position + new Vector3(0, 1.6f, 0);
        }
        
    }
    private IEnumerator Waitforjump()
    {
        yield return new WaitForSeconds(1.0f);
        rigidbody.AddForce(0, 8, 0, ForceMode.Impulse);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (gameObject.name == "Head" && origidbody.velocity.y > 0)
        {
            if (collision.gameObject.name == "Brick(Clone)")
            {
                Destroy(collision.gameObject);
                guiSc.addScore(100);
            }
            if (collision.gameObject.name == "Question(Clone)")
            {
                GameObject Question = collision.gameObject;
                guiSc.onAddCoin(Question);
            }

            //origidbody.velocity *= 0.0f;
            origidbody.velocity = new Vector3(0, -1, 0);
        }else if (gameObject.name == "Feet"/* || gameObject.name == "EthanRightFoot" || gameObject.name == "EthanLeftFoot"*/)
        {
            if (collision.gameObject.name == "Question(Clone)" || collision.gameObject.name == "Brick(Clone)" || collision.gameObject.name == "Rock(Clone)" || collision.gameObject.name == "Stone(Clone)")
            {
                amSc.airborne = false;
                amSc.jump = false;
                Debug.Log("NotAirborne");
            }
            if (collision.gameObject.name == "GoalPole" && !gameOver)
            {
                gameOver = true;
                guiSc.setWin();
            }
        }
        Debug.Log(gameObject.name + " " + collision.gameObject.name);
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "GoalPole(Clone)" && !gameOver)
        {
            gameOver = true;
            guiSc.setWin();
        }
        Debug.Log("Trigger");
    }
}
