using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaController : CharaBehavior
{
    [SerializeField] private KeyCode jump;


    void Start()
    {
        Init();
    }

    void Update()
    {
        Move();
        CheckGround();
        Action();
        ModifyPhysics();
    }

    public void Action()
    {
        if (Input.GetKey(jump) && CheckGround() && canJump)
        {
            jumpParticle.gameObject.SetActive(true);
            jumpParticle.Play();
            Jump(doubleJump);
        }

        if (Input.GetKey(jump) && doubleJump && canJump)
        {
            jumpParticle.Play();
            Jump(doubleJump);
        }
    }
}
