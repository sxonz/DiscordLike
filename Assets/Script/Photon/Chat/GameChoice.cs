using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;


public class GameChoice : MonoBehaviour
{
    int cur = 0;
    public void Set(int change)
    {
        cur = change;
    }

    public void Choice(Action<int> act)
    {
        StartCoroutine(wait(act));
    }
    IEnumerator wait(Action<int> act)
    {
        while (true)
        {
            yield return null;
            if (cur != 0)
            {
                break;
            }
        }
        act(cur);
        cur = 0;
    }
}
