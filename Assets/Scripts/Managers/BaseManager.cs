using RedMoon.Injector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour, IClient, IProvider
{
    private void OnEnable()
    {
        DepInjector.AddProvider(this);
    }
    private void OnDisable()
    {
        DepInjector.Remove(this);
    }
    private void OnDestroy()
    {
        DepInjector.Remove(this);
    }
    public virtual void NewProviderAvailable(IProvider newProvider) { }
    public virtual void NewProviderFullyInstalled(IProvider newProvider) { }
    public virtual void ProviderRemoved(IProvider removeProvider) { }
}
