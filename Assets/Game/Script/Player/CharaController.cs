using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TomWill;
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
        CheckWall();
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
                TWAudioController.PlaySFX("PLAYER_SFX", "player_jump");
                Jump(doubleJump);
            }

            if (jumpButton && doubleJump && !isGliding)
            {
                featherParticle.Play();
                TWAudioController.PlaySFX("PLAYER_SFX", "player_doublejump");
                Jump(doubleJump);
            }

            if (jumpHold && canGliding && !isDoubleJump)
            {
                Glide();
            }
        }
    }
}
