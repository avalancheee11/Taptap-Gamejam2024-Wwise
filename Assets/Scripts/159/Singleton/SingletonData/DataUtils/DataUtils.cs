using System;

public partial class DataUtils : SingletonData<DataUtils>
{
    protected override void OnInit()
    {
        OnInitStack();
        OnInitType();
        OnInitMethod();
        OnInitObject();
    }

    public void battlePrepare()
    {
        this.preallocStack();
    }

    public void battleFinishPrepare()
    {
        
    }

    public void battleClear()
    {
        this.deallocStack();
        this.methodClear();
        this.objectClear();
        this.typeClear();

        UnityEngine.Debug.Log("dataUtils.battleClear-----------");
    }
}