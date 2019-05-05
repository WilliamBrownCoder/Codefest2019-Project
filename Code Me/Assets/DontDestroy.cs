using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private static DontDestroy _instance;

    private void Awake()
    {
        //if we don't have an [_instance] set yet
        if (!_instance)
            _instance = this;
        //otherwise, if we do, kill this thing
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(gameObject);
    }
}
