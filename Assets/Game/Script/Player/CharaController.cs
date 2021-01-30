using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TomWill;
public class CharaController : CharaBehavior
{
    [Header ("Keycode")]
    [SerializeField] private KeyCode jump;
    [SerializeField] private bool jumpHold, jumpButton;


    void Start()
    {
        Init();
    }

    void Update()
    {
        CheckGround();
        CheckWall();
        InputKey();
        ModifyPhysics();
    }

    private void FixedUpdate()
    {
        Move();
        Action();
    }

    public void InputKey()
    {
        //jumpHold = Input.GetKey(jump);
        //jumpButton = Input.GetKeyDown(jump);

        if (Input.GetKey(jump)) jumpHold = true;
        if (Input.GetKeyDown(jump)) jumpButton = true;
    }

    public void Action()
    {
        if (canJump)
        {
            if (jumpButton && CheckGround())
            {
                jumpButton = false;
                jumpParticle.gameObject.SetActive(true);
                jumpParticle.Play();
                TWAudioController.PlaySFX("PLAYER_SFX", "player_jump");
                Jump(doubleJump);
            }

            if (jumpButton && doubleJump && !isGliding)
            {
                jumpButton = false;
                featherParticle.Play();
                TWAudioController.PlaySFX("PLAYER_SFX", "player_doublejump");
                Jump(doubleJump);
            }

            if (jumpHold && canGliding && !isDoubleJump)
            {
                jumpHold = false;
                Glide();
            }
        }
    }
}
