using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; 
/*
* 分支管理记录
 * dev 主开发分支
 * dev-unity 主开发分支下的不接入任何插件的发布分支
 * dev-taptap 主开发分支下，接入了taptap的国内发布分支
 * dev-intertaptap 主开发分支下，接入了taptap的国际发布分支
 * dev-wechat 主开发分支下，接入了微信小游戏的发布分支
 * dev-steam 主开发分支下，接入了steam的发布分支
*/

public class CustomGlobalConfig
{
    public static List<string> LoadingObjectSeq = new List<string>()
    {
        typeof(AssetBundleLoadObject).toString(),
        typeof(ConfigLoadingObject).toString(),
    };
    public static string BuglyAppIOSId = "3f42a5683c";
    public static string BuglyAndriodAppId = "dd042e965d";

    private static List<string> GarbledCharacters = new List<string>()
    {
        "□", "■", "●", //"ᯅ", "o", "★", "♥", "✿",
    };

    public static string ReplaceStringByGarbledCharacter(string desc)
    {
        var str = string.Empty;
        foreach (var c in desc) {
            str += GarbledCharacters.getRandomOne();
        }

        return str;
    }

    public static float MinShieldRate = 1 / 12f;
    public static float MaxShieldRate = 1 / 5f;
    
    public static bool CardCostLife = false;
    public static bool CanSelectCardByEnemyDeath = true;
    public static bool GetCoinByMerge = false;
    public static bool MergeIgnoreLattice = false;
    public const int MergeUnitCount = 3;
    public const int MaxSaveFileCount = 5;

#if ENABLE_TEST
    public static bool ShowAllModBtn = true;
#else
    public static bool ShowAllModBtn = false;
#endif
    
    public static List<float> EndlessEnemyGroupList = new List<float>()
    {
        // 0.3f, 0.25f, 0.2f, 0.15f, 0.1f,
        0.35f, 0.285f, 0.215f, 0.15f,
    };

    public static int BuildId = 18;
    public static bool ShowPlayTime = false;
    public static bool isPC => Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsPlayer;
    // public static List<float> CardGroupWeightList = new List<float>(){1.3f, 1f, 0.5f};
    public static List<float> CardGroupWeightList = new List<float>(){1.05f, 1, 0.95f};
    public static int RewardTalentByClickHero = 100;
    public static int HeroClickCount = 100;
    public static string PrivacyUrl = "https://docs.qq.com/doc/DVENoSVNmZ0FqbnFk?u=da237753cfda45589f32bbfc6b800cdf";
    
    public static string ModName = "native";

#if UNITY_ANDROID && !UNITY_EDITOR
    public static string streamingAssetBasePath = Application.streamingAssetsPath;
#elif UNITY_IOS && !UNITY_EDITOR
    public static string streamingAssetBasePath = Application.streamingAssetsPath;
#else
    public static string streamingAssetBasePath = Application.streamingAssetsPath;
#endif

#if UNITY_EDITOR
    public static string persistentDataPath =  Path.Combine(Application.dataPath, "tempData");
#else
    public static string persistentDataPath = Application.persistentDataPath;
#endif
    
    public static string ConfigStreamingAssetBasePath = Path.Combine(CustomGlobalConfig.streamingAssetBasePath, "mods", CustomGlobalConfig.ModName, "config");
    public static string ConfigPersistentDataPath = Path.Combine(CustomGlobalConfig.persistentDataPath, "mods", CustomGlobalConfig.ModName, "config");
}