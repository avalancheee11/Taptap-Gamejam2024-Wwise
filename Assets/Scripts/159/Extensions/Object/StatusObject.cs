using System;
using System.Collections.Generic;

public class StatusObject
{
    public Dictionary<int, Action> statusActions { get; private set; }
    public Dictionary<int, Action<float>> updateActions { get; private set; }
    public Dictionary<int, Action> leaveActions { get; private set; }
    public Action<float> defaultUpdateAction;
    public bool ignoreSameState;

    int _status;
    public event Action onStateChangeAction;
    public int status {
        get { return _status; }
        set {
            if (ignoreSameState && _status == value)
            {
                return;
            }
            if (_status != IC.NotFound) {
                var func = this.leaveActions.objectValue(_status);
                func?.Invoke();
            }
            _status = value;
            this.onStateChangeAction?.Invoke();
            if (_status != IC.NotFound) {
                var func = this.statusActions.objectValue(_status);
                func?.Invoke();
            }
        }
    }

    public StatusObject()
    {
        this.statusActions = new Dictionary<int, Action>();
        this.updateActions = new Dictionary<int, Action<float>>();
        this.leaveActions = new Dictionary<int, Action>();
        this.defaultUpdateAction = null;
        this.ignoreSameState = true;
    }

    public void update(float dt)
    {
        if (_status != IC.NotFound) {
            var func = this.updateActions.objectValue(_status);
            if(func != null) {
                func.Invoke(dt);
            }
            else {
                this.defaultUpdateAction?.Invoke(dt);
            }
        }
    }

    public void clearAction()
    {
        this.defaultUpdateAction = null;
        this.statusActions.Clear();
        this.updateActions.Clear();
        this.leaveActions.Clear();
    }

    public void clearStatus()
    {
        this.status = IC.NotFound;
    }
}
