using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void Interact();
}

public interface IInteractable<T> : IInteractable
{
    void Interact(T data);
}

public abstract class InteractableBehavior : MonoBehaviour, IInteractable
{
    public abstract void Interact();
}

public abstract class InteractableBehavior<T> : MonoBehaviour, IInteractable<T>
{
    public abstract void Interact(T data);
    public void Interact() { Interact(default); }
}