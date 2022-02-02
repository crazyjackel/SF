using RedMoon.Injector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour, IClient, IProvider
{
    protected virtual void OnEnable()
    {
        DepInjector.AddProvider(this);
    }
    protected virtual void OnDisable()
    {
        DepInjector.Remove(this);
    }
    protected virtual void OnDestroy()
    {
        DepInjector.Remove(this);
    }
    public virtual void NewProviderAvailable(IProvider newProvider) { }
    public virtual void NewProviderFullyInstalled(IProvider newProvider) { }
    public virtual void ProviderRemoved(IProvider removeProvider) { }
}
