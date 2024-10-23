using System;
using System.Collections.Generic;

public class NSObject
{
    public NSObject() { this.init(); }

    public virtual void init() { }
    public virtual void initialize(Dictionary<string, object> parameters) { this.initializeByParameters(parameters); }
    public virtual void initialize(string key, Dictionary<string, object> parameters) { this.initializeByParameters(parameters); }
    public virtual void initializeByParameters(Dictionary<string, object> parameters) { }

    public virtual object shallowCopy()
    {
        var o = this.MemberwiseClone() as NSObject;
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

public class NSConfigObject : NSObject
{
    public int id { get; protected set; }
    public override void initialize(Dictionary<string, object> parameters) { this.id = parameters.intValue("id"); base.initialize(parameters); }
    public override void initialize(string key, Dictionary<string, object> parameters) { this.id = key.toInt(); base.initialize(key, parameters); }
}

public class NSWeightObject : NSConfigObject, IWeightObject
{
    public int weight { get; set; }

    public override void initializeByParameters(Dictionary<string, object> parameters)
    {
        base.initializeByParameters(parameters);

        this.weight = parameters.intValue("weight");
    }
}

public interface IWeightObject
{
    int weight { get; }
}

