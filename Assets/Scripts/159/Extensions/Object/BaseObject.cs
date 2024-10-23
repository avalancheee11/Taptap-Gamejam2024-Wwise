using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject
{
    protected virtual bool needStatusObject => false;
    // public EducationNode mainNode => EducationNode.Instance;
    // public EducationData mainData => this.mainNode.mainData;
    // public UnitManager unitManager => this.mainNode.unitManager;
    protected StatusObject _statusObject;
    public string key { get; private set; }
    public event Action onStateChangeAction;

    protected int baseState
    {
        get => _statusObject.status;
        set
        {
            if (this._statusObject == null) {
                Debug.LogError("status赋值时已被释放" + this.GetType().toString());
                return;
            }
            _statusObject.status = value;
            this.onStateChangeAction?.Invoke();
        }
    }
    protected Dictionary<int, Action> statusActions => _statusObject.statusActions;
    protected Dictionary<int, Action<float>> updateActions => _statusObject.updateActions;
    protected Dictionary<int, Action> leaveActions => _statusObject.leaveActions;

    public BaseObject()
    {
        this.init();
    }

    public virtual void init()
    {
        if (this.needStatusObject) {
            this._statusObject = new StatusObject();
        }
        this.key = $"{this.GetType().toString()}{int.MaxValue.toCCRandomIndex()}";
    }

    protected virtual void prepare()
    {
    }

    public virtual void start()
    {
        this.prepare();
    }

    public virtual void update(float dt)
    {
        if (this.needStatusObject) {
            this._statusObject.update(dt);
        }
    }

    public virtual void stop()
    {
        this.over();
    }

    protected virtual void over()
    {
        if (this.needStatusObject) {
            this._statusObject?.clearStatus();
            this._statusObject?.clearAction();
            this._statusObject = null;
        }
    }

    public virtual object shallowCopy()
    {
        var o = this.MemberwiseClone() as BaseObject;
        o.init();
        return o;
    }

    public virtual object copy()
    {
        return this.shallowCopy();
    }

    public object deepCopy()
    {
        return ObjectConvert.deepCopy(this);
    }
}
