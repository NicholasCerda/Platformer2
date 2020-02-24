using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioScript : MonoBehaviour
{
    public Rigidbody rigidbody;
    public float movex, movey,maxspeed;
    public bool airborne;
    public GameObject EventMnger;
    public GuiScript guiSc;
    // Start is called before the first frame update
    void Start()
    {
        airborne = false;
        movex = 0;
        movey = 0;
        maxspeed = 2;
        rigidbody = gameObject.GetComponent<Rigidbody>();
        guiSc = EventMnger.GetComponent<GuiScript>();
    }

    // Update is called once per frame
    void Update()
    {
        movex = rigidbody.velocity.x;
        movey = rigidbody.velocity.y;
        bool dirtyFlag = false,sprint=false;
        if (Input.GetButton("Turbo"))
        {
            sprint = true;
        }
        if (sprint)
        {
            maxspeed = 4;
        }
        else
        {
            maxspeed = 2;
        }
        if (movex < -maxspeed)
        {
            movex = -maxspeed;
            dirtyFlag = true;
        }
        if (movex > maxspeed)
        {
            movex = maxspeed;
            dirtyFlag = true;
        }
        if (movey < -maxspeed*8)
        {
            movey = -maxspeed;
            dirtyFlag = true;
        }
        if (movey > maxspeed * 8)
        {
            movey = maxspeed;
            dirtyFlag = true;
        }
        if (dirtyFlag)
            rigidbody.velocity = new Vector3(movex, movey, 0);

        if (Input.GetButton("MoveX") && Input.GetAxisRaw("MoveX") > 0)
        {
            rigidbody.AddForce(-2.5f, 0, 0, ForceMode.Impulse);
        }
        else if (Input.GetButton("MoveX") && Input.GetAxisRaw("MoveX") < 0)
        {
            rigidbody.AddForce(2.5f, 0, 0, ForceMode.Impulse);
        }
        if (Input.GetButton("Jump") && !airborne)
        {
            rigidbody.AddForce(0, 10, 0, ForceMode.Impulse);
            airborne = true;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Brick(Clone)" || collision.gameObject.name == "Question(Clone)" || collision.gameObject.name == "Stone(Clone)" || collision.gameObject.name == "Rock(Clone)")
        {
            airborne = false;
        }
        if (collision.gameObject.name == "Brick(Clone)" && movey >0)
        {
            Destroy(collision.gameObject);
            guiSc.addScore(100);
        }
        if (collision.gameObject.name == "Question(Clone)" && movey > 0)
        {
            GameObject Question = collision.gameObject;
            guiSc.onAddCoin(Question);
        }
    }
}
