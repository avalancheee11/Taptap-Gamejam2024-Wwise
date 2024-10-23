using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataModule
{
    public const string None = "none";
    public const string Item = "item";
    public const string ItemFormula = "itemformula";
    public const string ItemContainer = "itemcontainer";
    public const string ShopCustom = "shopcustom";


    //配置加载顺序
    public static List<string> ConfigModuleNameList = new List<string>()
    {
        ItemFormula,
    };

    public static List<string> ScriptModuleNameList = new List<string>()
    {
    };
    //数据加载顺序
    public static List<string> DataModuleNameList = new List<string>()
    {
        Item, ItemContainer, ShopCustom
    };

    public static Dictionary<string, string> DataHandlerClassNames = new Dictionary<string, string>()
    {
        {ItemFormula, typeof(ItemFormulaConfigHandler).toString()},
        
        {ItemContainer, typeof(ItemContainerDataHandler).toString()},
        {Item, typeof(ItemDataHandler).toString()},
        {ShopCustom, typeof(ShopCustomDataHandler).toString()},
    };
}

public interface IDataHandler
{
    string module { get;}
    bool saveByUpdate { get;set; }
    void reloadData(Dictionary<string, object> parameters);
    void reloadDataByConfig();
    void reloadDataByData(Dictionary<string, object> parameters);
    Dictionary<string, object> toCache();
    Dictionary<string, object> getCache();
    void saveAction();
    void dropAction();
}