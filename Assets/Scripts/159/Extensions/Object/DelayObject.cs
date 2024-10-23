using System;

public delegate void DelayObjectUpdate();

public class DelayObject
{
    public float totalDelay;
    public float currentDelay;
    public float delay;
    public bool paused = false;

    public bool reset = true;

    public DelayObjectUpdate onUpdateCall;

    public DelayObject()
    {
        this.onUpdateCall = new DelayObjectUpdate(this.updateCall);
        this.currentDelay = 0;
        this.delay = float.MaxValue;
    }

    public virtual void update(float dt) 
    {
        if(this.paused) {
            return;
        }
        this.totalDelay += dt;
        this.currentDelay += dt;
        if (this.currentDelay < this.delay) {
            return;
        }
        if (this.reset) {
            this.currentDelay = 0;
        }
        this.onUpdateCall();
    }

    void updateCall() {}
}

