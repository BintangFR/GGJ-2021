using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaData : MonoBehaviour
{
    [SerializeField] private float baseSpeed, baseJumpSpeed, baseMoonJump, jumpDelay;

    public float BaseSpeed { get => baseSpeed; set => baseSpeed = value; }
    public float BaseJumpSpeed { get => baseJumpSpeed; set => baseJumpSpeed = value; }
    public float BaseMoonJump { get => baseMoonJump; set => baseMoonJump = value; }
    public float JumpDelay { get => jumpDelay; set => jumpDelay = value; }
}
