using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splat : MonoBehaviour
{
    [SerializeField] private Color backgroundTint;
    [SerializeField] private float minSizeMod = 0.8f;
    [SerializeField] private float maxSizeMod = 1.5f;

    [SerializeField] private Sprite[] sprites;

    [SerializeField] private SpriteRenderer spriteRenderer;


    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize()
    {
        SetSprite();
        SetSize();
        SetRotation();

    }

    public void SetSprite()
    {
        int random = Random.Range(0, sprites.Length);
        spriteRenderer.sprite = sprites[random];
    }

    public void SetSize()
    {
        float random = Random.Range(minSizeMod, maxSizeMod);
        transform.localScale *= random;
    }

    public void SetRotation()
    {
        float randomRotation = Random.Range(-360, 360);
        transform.rotation = Quaternion.Euler(0, 0, randomRotation);
    }

    public void SetLocationProperties()
    {

    }
}
