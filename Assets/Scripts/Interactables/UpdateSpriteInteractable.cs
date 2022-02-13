using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class UpdateSpriteInteractable : InteractableBehavior
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite sprite;
    public override void Interact()
    {
        spriteRenderer.sprite = sprite;
    }
}
