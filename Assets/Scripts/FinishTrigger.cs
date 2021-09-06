using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    public static FinishTrigger instance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }
}
