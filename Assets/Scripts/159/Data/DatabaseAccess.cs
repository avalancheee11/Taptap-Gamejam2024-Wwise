using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDatabaseAccess
{
    bool hasDemoFile { get; }
    bool saveByUpdate { get; set; }
    void openDBAsync(Action callBack);
    void closeDB();
    void clearDB();
    string getValue(string key);
    void setValue(string key, string value);
    void removeValue(string key);
    void saveDB();
    Dictionary<string, object> exportDB();
    void update(float dt);
}