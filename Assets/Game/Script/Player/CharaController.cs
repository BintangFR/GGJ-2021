using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaController : CharaBehavior
{
    [Header ("Keycode")]
    [SerializeField] private KeyCode jump;
    [HideInInspector] private bool jumpHold, jumpButton;


    void Start()
    {
        Init();
    }

    void Update()
    {
        CheckGround();
        ModifyPhysics();
        jumpHold = Input.GetKey(jump);
        jumpButton = Input.GetKeyDown(jump);
    }

    private void FixedUpdate()
    {
        Move();
        Action();
    }

    public void Action()
    {
        if (canJump)
        {
            if (jumpButton && CheckGround())
            {
                jumpParticle.gameObject.SetActive(true);
                jumpParticle.Play();
                Jump(doubleJump);
            }

            if (jumpButton && doubleJump && !isGliding)
            {
                jumpParticle.Play();
                Jump(doubleJump);
            }

            if (jumpHold && canGliding && !isDoubleJump)
            {
                Glide();
            }
        }
    }
}
