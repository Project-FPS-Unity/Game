using System.Collections;
using UnityEngine;

public abstract class Equipment : MonoBehaviour
{
    // Setting
    private void Start()
    {
        InitAmmo();
    }
    private void Update()
    {
        Action();
    }

    // Override Methods
    protected abstract void Action();
    protected abstract IEnumerator Reload();
    protected abstract void InitAmmo();
}
