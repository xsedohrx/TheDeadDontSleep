using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    public ObjectPool parent;

    protected virtual void OnDisable() {
        parent?.ReturnObjectToPool(this);
    }
}
