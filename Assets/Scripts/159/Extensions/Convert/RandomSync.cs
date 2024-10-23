using System;
using System.Collections;
using System.Collections.Generic;

public static class RandomSync
{
    private static volatile System.Random _randomSharedInstance;
    private static object syncRoot = new object();

    public static System.Random randomSharedInstance
    {
        get 
        {
            if (_randomSharedInstance == null) 
            {
                lock (syncRoot) 
                {
                    if (_randomSharedInstance == null) 
                        _randomSharedInstance = new System.Random();
                }
            }
            return _randomSharedInstance;
        }
    }

    // game random
    /** @def WDRANDOM_MINUS1_1
     returns a random float between -1 and 1
     */
    public static float WDRANDOM_MINUS1_1 
    {
        get { return (randomSharedInstance.Next() / (float)0x3fffffff )-1.0f; }
    }

    /** @def WDRANDOM_0_1
     returns a random float between 0 and 1
     */
    public static float WDRANDOM_0_1
    {
        get { return randomSharedInstance.Next() / (float)0x7fffffff; }
    }

    /** @def WDRANDOM
     returns a random float between 0 and 0x7fffffff
     */
    public static int WDRANDOM
    {
        get { return randomSharedInstance.Next(); }
    }

    // not game random
    /** @def CCRANDOM
     returns a random float between 0 and 0x7fffffff
     */
    public static int CCRANDOM
    {
        get { return randomSharedInstance.Next(); }
    }

    /** @def CCRANDOM_0_1
     returns a random float between 0 and 1
     */
    public static float CCRANDOM_0_1
    {
        get { return randomSharedInstance.Next() / (float)0x7fffffff; }
    }

    /** @def CCRANDOM_MINUS1_1
     returns a random float between -1 and 1
     */
    public static float CCRANDOM_MINUS1_1 
    {
        get { return (randomSharedInstance.Next() / (float)0x3fffffff )-1.0f; }
    }
}
