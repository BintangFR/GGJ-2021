﻿using System.Collections;
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
        if (Input.GetKeyDown(jump) && CheckGround())
        {
            Jump();
        }
    }
}
