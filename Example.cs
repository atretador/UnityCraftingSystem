using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Example : MonoBehaviour
{
    public Inventory inventory = new();
    public Factory factory;
    public Coroutine factoryWork;
    public float tick = 0.1f; //update interval

    void Start()
    {
        if(inventory == null)
            return;

        factory.Init(inventory);
    }

    void Update()
    {
        if(inventory == null)
            return;

        if(factoryWork == null)
        {
            factoryWork = StartCoroutine(FactoryWork());
        }
    }

    public IEnumerator FactoryWork()
    {
        yield return new WaitForSeconds(tick);
        factory.Work();
    }

}