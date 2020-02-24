using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorMoveScript : MonoBehaviour
{
    Transform myTransform;
    Animator anim;
    public bool faceRight, sprint,jump,airborne;
    public float vmove;
    private float maxv;
    public Rigidbody rigidbody, feetRb;
    public HFScript hfSc;
    public GameObject feet;
    // Start is called before the first frame update
    void Start()
    {
        maxv = 0;
        anim = GetComponent<Animator>();
        myTransform = transform;
        faceRight = true;
        jump = false;
        airborne = false;
        rigidbody = gameObject.GetComponent<Rigidbody>();
        vmove = rigidbody.velocity.y;
        hfSc = feet.GetComponent<HFScript>();
        feetRb = feet.GetComponent<Rigidbody>();
    }
    //void fixedupdate
    
    // Update is called once per frame
    void OnAnimatorMove()
    {
        sprint = false;
        float move = Input.GetAxis("Horizontal");
        if (move < 0) {
            myTransform.transform.rotation =  Quaternion.Euler(0,-90,0);
        } else if (move > 0)
        {
            myTransform.transform.rotation= Quaternion.Euler(0,90,0);
        }
        if (Input.GetButton("Turbo"))
        {
            sprint = true;
        }
        if (sprint)
        {
            move *= 2.0f;
        }
        if (Input.GetButton("Jump") && !jump && !airborne)
        {
            jump = true;
            airborne = true;
            //rigidbody.AddForce(0, 10, 0, ForceMode.Impulse);
        }
        
        if (vmove > maxv)
        {
            maxv = vmove;
        }
        anim.SetFloat("maxvert", maxv);
        anim.SetFloat("Speed", Mathf.Abs(move));
        anim.SetFloat("VertSpeed", vmove);
        anim.SetBool("Jump", jump);
        anim.SetBool("IsAirborne", airborne);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (airborne)
        {
            feetRb.velocity = Vector3.Scale(feetRb.velocity, new Vector3(-1, 1, 0));
        }
    }
}
