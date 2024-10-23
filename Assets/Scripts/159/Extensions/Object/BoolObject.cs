using System;

public class BoolObject
{
    public int _count = 0;
    bool _originResult = true;
    bool _result = true;
    int _a = 1;
    int _b = -1;

    public bool result {
        get { return _result;}
        set {
            _count = _count + (!value ? _a : _b);
            _result = _originResult ? _count == 0 : _count != 0;
        }
    }

    public BoolObject(bool result = true)
    {
        this.reset(result);
    }

    public void clear()
    {
        this.reset(_originResult);
    }

    public void reset(bool result = true)
    {
        _originResult = result;
        _result = result;
        if (result) {
            _a = 1;
            _b = -1;
        }
        else {
            _a = -1;
            _b = 1;
        }
        _count = 0;
    }
}
