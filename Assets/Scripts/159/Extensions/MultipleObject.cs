using System;
using System.Collections.Generic;
using UnityEngine;

public enum UnitCalculate
{
    CostContainsBuy,
    CostIgnoreBuy,

    EffectAddMultiply,
    EffectMultiplyPow,
    EffectMultiplyPowExtra,
    EffectAddNExtra,


    Add,
    Mul,

    Or,
    And,

    Max,
    //是否是隔天完成，隔天清零，当前+1
    Time,
}


public class MultiplyObjectByDouble
{
    Dictionary<string, double> _ps;
    public double result;
    double _baseValue;
    UnitCalculate _calculate;

    public MultiplyObjectByDouble(double baseValue = 1, UnitCalculate calculate = UnitCalculate.Mul)
    {
        _baseValue = baseValue;
        _calculate = calculate;
        _ps = new Dictionary<string, double>();
        this.reset();
    }

    public void set(string key, double value, bool byAdd = false)
    {
        _ps[key] = value + (byAdd ? this.get(key) : 0);
        this.reset();
    }

    public double get(string key)
    {
        var cnt = 0.0;
        _ps.TryGetValue(key, out cnt);
        return cnt;
    }

    void reset()
    {
        this.result = _baseValue;
        foreach (var kvp in _ps) {
            if (_calculate == UnitCalculate.Add) {
                this.result += kvp.Value;
            }
            else if (_calculate == UnitCalculate.Mul) {
                this.result *= kvp.Value;
            }
        }
    }

    public void clear()
    {
        _ps.Clear();
        this.reset();
    }

}

public class MultiplyObjectByFloat
{
    Dictionary<string, float> _ps;
    public float result;
    float _baseValue;
    UnitCalculate _calculate;
    public event Action onValueChangeAction;
    public event Action<float> onAddValueAction;
    public event Action<float> onReduceValueAction;

    public MultiplyObjectByFloat(float baseValue = 1, UnitCalculate calculate = UnitCalculate.Mul)
    {
        _baseValue = baseValue;
        _calculate = calculate;
        _ps = new Dictionary<string, float>();
        this.reset();
    }

    public void set(string key, float value, bool byAdd = false)
    {
        var v =  (double)value + (byAdd ? (double)this.get(key) : 0);
        if (v > float.MaxValue) {
            
        }
        
        _ps[key] = v >= float.MaxValue? float.MaxValue : (float)v;
        this.reset();
    }

    public void removeKey(string key)
    {
        if (this._ps.Remove(key)) {
            this.reset();
        }
    }

    public float get(string key)
    {
        var cnt = 0f;
        _ps.TryGetValue(key, out cnt);
        return cnt;
    }

    void reset()
    {
        var preValue = this.result;
        double res = _baseValue;
        this.result = _baseValue;
        foreach (var kvp in _ps) {
            if (_calculate == UnitCalculate.Add) {
                res += kvp.Value;
            }
            else if (_calculate == UnitCalculate.Mul) {
                res *= kvp.Value;
            }
        }

        if (res >= float.MaxValue) {
            this.result = float.MaxValue;
        }
        else {
            this.result = (float)res;
        }

        if (Math.Abs(preValue - this.result) > float.Epsilon) {
            var v = this.result - preValue;
            if (v > 0) {
                this.onAddValueAction?.Invoke(v);
            }
            else {
                this.onReduceValueAction?.Invoke(v);
            }
            this.onValueChangeAction?.Invoke();
        }
    }

    public void clear()
    {
        _ps.Clear();
        this.reset();
    }
}

public class MultiplyObjectByInt
{
    Dictionary<string, int> _ps;
    public int result;
    int _baseValue;
    UnitCalculate _calculate;
    public event Action onValueChangeAction;
    public event Action<int> onReduceValueAction;
    public event Action<int>  onAddValueAction;

    public MultiplyObjectByInt(int baseValue = 1, UnitCalculate calculate = UnitCalculate.Mul)
    {
        _baseValue = baseValue;
        _calculate = calculate;
        _ps = new Dictionary<string, int>();
        this.reset();
    }

    public void set(string key, int value)
    {
        _ps[key] = value;

        this.reset();
    }


    public int get(string key)
    {
        return this._ps.objectValue(key);
    }

    public void clear()
    {
        _ps.Clear();
        this.reset();
    }
    
    void reset()
    {
        var preValue = this.result;
        this.result = _baseValue;
        foreach (var kvp in _ps) {
            if (_calculate == UnitCalculate.Add) {
                this.result += kvp.Value;
            }
            else if (_calculate == UnitCalculate.Mul) {
                this.result *= kvp.Value;
            }
        }

        if (preValue != this.result) {
            this.onValueChangeAction?.Invoke();
            if (preValue > this.result) {
                this.onReduceValueAction?.Invoke(preValue - this.result);
            }
            else {
                this.onAddValueAction?.Invoke(this.result - preValue);
            }
        }
    }
}

public class MultiplyObjectByBool
{
    Dictionary<string, bool> _ps;
    public bool result;
    bool _baseValue;
    UnitCalculate _calculate;
    public event Action onValueChangeAction;

    public MultiplyObjectByBool(bool baseValue = false, UnitCalculate calculate = UnitCalculate.Or)
    {
        _baseValue = baseValue;
        _calculate = calculate;
        _ps = new Dictionary<string, bool>();
        this.reset();
    }

    public void set(string key, bool value)
    {
        _ps[key] = value;
        this.reset();
    }

    public void removeKey(string key)
    {
        if (this._ps.Remove(key)) {
            this.reset();
        }
    }

    void reset()
    {
        var preValue = this.result;
        this.result = _baseValue;
        foreach (var kvp in _ps) {
            if (_calculate == UnitCalculate.Or) {
                this.result = this.result || kvp.Value;
            }
            else if (_calculate == UnitCalculate.And) {
                this.result = this.result && kvp.Value;
            }
        }

        if (this.result != preValue) {
            this.onValueChangeAction?.Invoke();
        }
    }

    public void clear()
    {
        this._ps.Clear();
        this.reset();
    }
}

public class NumberObjectByFloat
{
    private float _result;

    private float result
    {
        get => this._result;
        set
        {
            if (Math.Abs(this._result - value) < float.Epsilon) {
                return;
            }

            this._result = value;
            this.onValueChangeAction?.Invoke();
        } 
    }
    public event Action onValueChangeAction;

    public NumberObjectByFloat(float baseValue = 1)
    {
        this._result = baseValue;
    }
    
    public void set(float value)
    {
        this.result = value;
    }

    public void add(float value)
    {
        this.result += value;
    }

    public void reduce(float value)
    {
        this.result -= value;
    }
}