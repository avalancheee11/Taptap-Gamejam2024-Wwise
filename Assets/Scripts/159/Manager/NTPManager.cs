using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XHFrameWork;

public class NTPManager : Singleton<NTPManager>
{
    private int _time;
    public DateTime NowTime => DateTime.UtcNow;
    /// <summary>
    /// 秒
    /// </summary>
    public int time { get { _time = (int)this.millisecondTime; return _time; } }
    public float floatTime { get { var t = this.millisecondTime; _time = (int)t; return (float)t; } }
    public double doubleTime { get { var t = this.millisecondTime; _time = (int)t; return t; } }
    public double millisecondTime => NowTime.greenwichTimestamp();
}
