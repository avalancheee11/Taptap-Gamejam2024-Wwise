using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public enum DateTimeFormatMaxUnit
{
    Auto,
    Week,
    Day,
    Hour,
    Minute
}

public enum DateTimeFormatType
{
    Normal, //"{0}{1}"
    Space,  //"{0} {1}"
    Colon,  //"{0}:"
    Clock,  //"00:00"
}

public class DateTimeFormatter
{
    //TODO:国际化
    public static string TimeUnitWeek => "W";
    public static string TimeUnitWeeks => "W";
    public static string TimeUnitDay => "D";
    public static string TimeUnitDays => "D";
    public static string TimeUnitW => "W"; 
    public static string TimeUnitD => "D";
    public static string TimeUnitH => "H";
    public static string TimeUnitM => "M";
    public static string TimeUnitS => "S";
    
    public static DateTimeFormatter Default = new DateTimeFormatter(DateTimeFormatMaxUnit.Day, DateTimeFormatType.Normal, 4);
    public static DateTimeFormatter Default2Unit = new DateTimeFormatter(DateTimeFormatMaxUnit.Day, DateTimeFormatType.Normal, 2);
    public static DateTimeFormatter Default1Unit = new DateTimeFormatter(DateTimeFormatMaxUnit.Day, DateTimeFormatType.Normal, 1);
    public static DateTimeFormatter ClockHour = new DateTimeFormatter(DateTimeFormatMaxUnit.Hour, formatType: DateTimeFormatType.Clock, unitCount: 3);
    public static DateTimeFormatter ClockMinute = new DateTimeFormatter(DateTimeFormatMaxUnit.Minute, formatType: DateTimeFormatType.Clock, unitCount: 2);

    public const int MaxUnitCount = 4;
    public static int[] WeekUnits = new int[] { IC.OneWeek, IC.OneDay, IC.OneHour, IC.OneMinute };
    public static int[] DayUnits = new int[] { IC.OneDay, IC.OneHour, IC.OneMinute, IC.OneSecond };
    public static int[] HourUnits = new int[] { IC.OneHour, IC.OneMinute, IC.OneSecond };
    public static int[] MinuteUnits = new int[] { IC.OneMinute, IC.OneSecond };
    public static Dictionary<DateTimeFormatMaxUnit, int[]> UnitMap = new Dictionary<DateTimeFormatMaxUnit, int[]>() {
        { DateTimeFormatMaxUnit.Week, WeekUnits },
        { DateTimeFormatMaxUnit.Day, DayUnits },
        { DateTimeFormatMaxUnit.Hour, HourUnits },
        { DateTimeFormatMaxUnit.Minute, MinuteUnits },
    };

    public static string[] WeekStrings = new string[] { TimeUnitW, TimeUnitD, TimeUnitH, TimeUnitM };
    public static string[] DayStrings = new string[] { TimeUnitD, TimeUnitH, TimeUnitM, TimeUnitS };
    public static string[] HourStrings = new string[] { TimeUnitH, TimeUnitM, TimeUnitS };
    public static string[] MinuteStrings = new string[] { TimeUnitM, TimeUnitS };

    public static Dictionary<DateTimeFormatMaxUnit, string[]> UnitStringMap = new Dictionary<DateTimeFormatMaxUnit, string[]>() {
        { DateTimeFormatMaxUnit.Week, WeekStrings },
        { DateTimeFormatMaxUnit.Day, DayStrings },
        { DateTimeFormatMaxUnit.Hour, HourStrings },
        { DateTimeFormatMaxUnit.Minute, MinuteStrings },
    };

    public static string ZeroSecondTime = string.Format("0{0}", TimeUnitS);

    public DateTimeFormatMaxUnit MaxUnit { get; private set; }   
    public DateTimeFormatType FormatType { get; private set; }
    public int UnitCount { get; private set; }
    public int Ticks { get; private set; }
    public int[] Units { get; private set; }
    public string[] UnitStrings { get; private set; }
    public string FormatString = string.Empty;

    int[] _currentDisplay;
    string _currentString;
    bool _isDirty;
    StringBuilder _stringBuilder;

    public static Queue<DateTimeFormatter> formatterPools = new Queue<DateTimeFormatter>();

    public static DateTimeFormatter CreateFormatter(DateTimeFormatMaxUnit maxUnit, DateTimeFormatType formatType = DateTimeFormatType.Normal, int unitCount = 3)
    {
        DateTimeFormatter formatter;
        if (formatterPools.Count > 0) {
            formatter = formatterPools.Dequeue();
            formatter.Reset(maxUnit, formatType, unitCount);
        }
        else {
            formatter = new DateTimeFormatter(maxUnit, formatType, unitCount);
        }
        return formatter;
    }

    public static void ChangeLanguage()
    {
        WeekStrings = new string[] { TimeUnitW, TimeUnitD, TimeUnitH, TimeUnitM };
        DayStrings = new string[] { TimeUnitD, TimeUnitH, TimeUnitM, TimeUnitS };
        HourStrings = new string[] { TimeUnitH, TimeUnitM, TimeUnitS };
        MinuteStrings = new string[] { TimeUnitM, TimeUnitS };
        UnitStringMap = new Dictionary<DateTimeFormatMaxUnit, string[]>() {
            { DateTimeFormatMaxUnit.Week, WeekStrings },
            { DateTimeFormatMaxUnit.Day, DayStrings },
            { DateTimeFormatMaxUnit.Hour, HourStrings },
            { DateTimeFormatMaxUnit.Minute, MinuteStrings },
        };

        ZeroSecondTime = string.Format("0{0}", TimeUnitS);

        foreach (var item in formatterPools) {
            item._isDirty = true;
        }
    }

    public static void Clear()
    {
        formatterPools.Clear();
    }

    private DateTimeFormatter(DateTimeFormatMaxUnit maxUnit, DateTimeFormatType formatType = DateTimeFormatType.Normal, int unitCount = 3)
    {
        _currentDisplay = new int[MaxUnitCount];
        _stringBuilder = new StringBuilder();
        Reset(maxUnit, formatType, unitCount);
    }

    void Reset(DateTimeFormatMaxUnit maxUnit, DateTimeFormatType formatType = DateTimeFormatType.Normal, int unitCount = 3)
    {
        _currentString = string.Empty;
        this.FormatString = string.Empty;
        for (int i = 0; i < _currentDisplay.Length; i++) {
            _currentDisplay[i] = 0;
        }

        SetFormatType(maxUnit, formatType, unitCount);
    }

    void SetFormatType(DateTimeFormatMaxUnit maxUnit, DateTimeFormatType formatType = DateTimeFormatType.Normal, int unitCount = 3)
    {
        this.MaxUnit = maxUnit;
        this.FormatType = formatType;
        this.UnitCount = unitCount;

        _isDirty = true;
    }

    public string GetFormatString(int ticks)
    {
        // 时间没变，并且没有更新其他数据的情况下直接返回原来字符串
        if (this.Ticks == ticks && !_isDirty) {
            return GetTimeString();
        }
        this.Ticks = ticks;
        UpdateDisplay();
        if (_isDirty) {
            UpdateFormatString();
        }
        return GetTimeString();
    }

    public void Dispose()
    {
        formatterPools.Enqueue(this);
    }

    string GetTimeString()
    {
        if (string.IsNullOrEmpty(_currentString)) {
            return ZeroSecondTime;
        }
        return _currentString;
    }

    //根据ticks计算要显示的display数据
    void UpdateDisplay()
    {
        var maxUnit = this.MaxUnit;
        if (maxUnit == DateTimeFormatMaxUnit.Auto) {
            if (this.Ticks < IC.OneDay) {
                maxUnit = DateTimeFormatMaxUnit.Hour;
            } else if (this.Ticks < IC.OneWeek) {
                maxUnit = DateTimeFormatMaxUnit.Day;
            } else {
                maxUnit = DateTimeFormatMaxUnit.Week;
            }
        }

        bool displayDirty = false;
        this.Units = UnitMap[maxUnit];
        this.UnitStrings = UnitStringMap[maxUnit];
        int unitCount = this.UnitCount;
        int time = this.Ticks;
        int value = 0;
        for (int i = 0; i < _currentDisplay.Length; i++) {
            if (i >= this.Units.Length) {
                value = 0;
            }
            else {
                if (unitCount <= 0) {
                    value = 0;
                }
                else {
                    int unit = this.Units[i];
                    value = time / unit;
                    time -= value * unit;
                }
            }
            if (value > 0) {
                unitCount--;
            }
            if (_currentDisplay[i] != value) {
                displayDirty = true;
            }
            _currentDisplay[i] = value;
        }
        _isDirty = _isDirty || displayDirty;
    }

    // 根据formatType刷新formatString
    void UpdateFormatString()
    {
        _stringBuilder.Remove(0, _stringBuilder.Length);
        if (this.FormatType == DateTimeFormatType.Normal) {
            for (int i = 0; i < _currentDisplay.Length; i++) {
                int count = _currentDisplay[i];
                if (count > 0) {
                    _stringBuilder.AppendFormat("{0}{1}", count, this.UnitStrings[i]);
                }
            }
            _currentString = UniqueString.Intern(_stringBuilder.ToString());
        } else if (this.FormatType == DateTimeFormatType.Space) {
            if (this.MaxUnit != DateTimeFormatMaxUnit.Week || this.UnitCount > 2) {
                _currentString = string.Empty;
                Debug.LogError("DateTimeFormatter not supported");
                return;
            }
            int week = _currentDisplay[0];
            int day = _currentDisplay[1];
            if (week == 1 && day == 0) {
                _stringBuilder.AppendFormat("{0} {1}", week, UniqueString.Intern(TimeUnitWeek)); 
            } else if (week == 0 && day == 1) {
                _stringBuilder.AppendFormat("{0} {1}", day, UniqueString.Intern(TimeUnitDay)); 
            } else {
                if (week > 0) {
                    _stringBuilder.AppendFormat("{0} {1}", week, week > 1 ? TimeUnitWeeks : TimeUnitWeek);
                }
                if (day > 0) {
                    _stringBuilder.AppendFormat("{0} {1}", day, day > 1 ? TimeUnitDays : TimeUnitDay);
                } 
            }
            _currentString = UniqueString.Intern(_stringBuilder.ToString());
        } else if (this.FormatType == DateTimeFormatType.Colon) {
            for (int i = 0; i < _currentDisplay.Length; i++) {
                int count = _currentDisplay[i];
                if (count > 0) {
                    _stringBuilder.AppendFormat("{0}:", count);
                }
            }
            _stringBuilder.Remove(_stringBuilder.Length - 1, 1);
            _currentString = UniqueString.Intern(_stringBuilder.ToString());
        } else if (this.FormatType == DateTimeFormatType.Clock) {
            for (int i = 0; i < _currentDisplay.Length; i++) {
                int count = _currentDisplay[i];
                if (i >= this.UnitCount) {
                    break;
                }
                if (count <= 9) {
                    _stringBuilder.AppendFormat("0{0}:", count);
                } else {
                    _stringBuilder.AppendFormat("{0}:", count);
                } 
            }
            _stringBuilder.Remove(_stringBuilder.Length - 1, 1);
            _currentString = UniqueString.Intern(_stringBuilder.ToString());
        }
        if (!string.IsNullOrEmpty(this.FormatString)) {
            _currentString = string.Format(this.FormatString, _currentString);
        }
    }
}
