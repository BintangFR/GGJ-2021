using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    private int chickCollect;

    public int ChickCollect { get => chickCollect; set => chickCollect = value; }

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        
    }
}
