using System;
using System.Collections.Generic;

public class RangeObject<T> where T : struct
{
    public static bool Inited = false;
    public static Func<RangeObject<T>, T> operatorFunc;

    public T min;
    public T max;

    public RangeObject(T ele = default(T))
    {
        this.min = ele;
        this.max = ele;

        this.initOperator();
    }

    public RangeObject(T min, T max)
    {
        this.min = min;
        this.max = max;

        this.initOperator();
    }

    //error:必须转换为封闭类型
    //public static implicit operator int(Range<int> range) => range.min + (range.max - range.min).toCCRandomIndex();

    public static implicit operator T(RangeObject<T> range) => operatorFunc(range);

    public void initOperator()
    {
        if (Inited) return;
        Inited = true;
        RangeObject<int>.operatorFunc = IntRangeOperator;
        RangeObject<float>.operatorFunc = FloatRangeOperator;
    }

    public static int IntRangeOperator(RangeObject<int> range)
    {
        return range.min + (range.max - range.min).toCCRandomIndex();
    }

    public static float FloatRangeOperator(RangeObject<float> range)
    {
        return range.min + (range.max - range.min).toCCRandom01();
    }
}