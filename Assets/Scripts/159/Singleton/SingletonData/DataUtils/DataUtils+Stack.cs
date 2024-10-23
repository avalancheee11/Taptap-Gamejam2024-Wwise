using System;
using System.Collections.Generic;

using UnityEngine;

public partial class DataUtils
{
    Stack<Dictionary<string, object>> _dictStack;

    void OnInitStack()
    {
        _dictStack = new Stack<Dictionary<string, object>>();
    }

    void preallocStack()
    {
        for (int t = 0; t < 20; t++) {
            pushDict(new Dictionary<string, object>(), false);
        }
    }

    void deallocStack()
    {
        _dictStack.Clear();
    }
    
    public void pushDict(Dictionary<string, object> list, bool recycle = true) {  }
    public Dictionary<string, object> popDict() { return new Dictionary<string, object>(); }
}
