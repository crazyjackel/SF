using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ToolInteractable : InteractableBehavior<Tooltype>
{
    [SerializeField]
    private InteractableBehavior deferInteraction;
    [SerializeField]
    private Tooltype matchingTooltype;
    public override void Interact(Tooltype type)
    {
        if(type == matchingTooltype || matchingTooltype == Tooltype.Any)
        {
            deferInteraction.Interact();
        }
    }
}
