using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int a = 5;
        int b;
        //test(ref a);
        //Debug.Log(a);
        //test2(out b);
        //Debug.Log(b);

        //test2(ref a);
        //test(ref b);
        //test3(b);
        Debug.Log(a);
        //Debug.Log(b);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static void test(ref int x)
    {
        x = x * x;
    }
    static void test2(ref int x)
    {
        x = 6;
        x = x * x;
    }
    static void test3(int x)
    {
        x = x * x;
    }
}
