using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasesLoadingObject : BaseObject
{
    public bool isComplete { get; private set; }
    public abstract string desc { get; }

    public override void start()
    {
        base.start();
        this.isComplete = false;
    }

    public void complete()
    {
        this.isComplete = true;
    }
}