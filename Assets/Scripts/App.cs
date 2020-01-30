using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class App 
{
    public static IQFrameworkContainer Container
    {
        get
        {
            if (mContainer == null)
            {
                mContainer = new QFrameworkContainer();
            }
            return mContainer;
        }
    }

    private static IQFrameworkContainer mContainer = null;
}
