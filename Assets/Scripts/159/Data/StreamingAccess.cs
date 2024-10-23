using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CustomJson;
using UnityEngine;

public class StreamingAccess : IDatabaseAccess
{
    private Dictionary<string, string> _datas;
    
#if UNITY_EDITOR
    public static string savePath => Path.Combine(Application.dataPath, "tempData", "GameData");
#else
    public static string savePath => Path.Combine(Application.persistentDataPath, "GameData");
#endif

    private static string DemoFileName = "Demo";
    public string demoSavePath => Path.Combine(savePath, DemoFileName, "Player1.save");
    
    // public string currentFileName
    // {
    //     get
    //     {
    //         if (PlayerPrefs.HasKey("currentFileName")) {
    //             return PlayerPrefs.GetString("currentFileName");
    //         }
    //
    //         return string.Empty;
    //     }
    //     private set
    //     {
    //         PlayerPrefs.SetString("currentFileName", value);
    //         PlayerPrefs.Save();
    //     }
    // }

    public string currentFileName { get; private set; }
    public string fullPath => Path.Combine(savePath, this.currentFileName + ".save");

    public string getFullPathByFileName(string fileName) => Path.Combine(savePath, fileName + ".save");

    public List<string> fileNameList
    {
        get
        {
            var l = new List<string>();
            if (Directory.Exists(savePath)) {
                DirectoryInfo direction = new DirectoryInfo(savePath);
                FileInfo[] files = direction.GetFiles("*");
                foreach (var fileInfo in files) {
                    var fileName = fileInfo.Name;
                    if (!fileName.EndsWith(".save")) {
                        continue;
                    }

                    l.Add(fileName.Replace(".save", string.Empty));
                }
            }
            return l;
        }
    }

    public StreamingAccess()
    {
        //检查没有正式版的存档并且有demo存档，则把demo存档拷贝到正式版路径里
        var fullVersionList = this.fileNameList;
        if (fullVersionList.Count == 0 && this.hasDemoFile) {
            var demo = new StreamReader(this.demoSavePath, Encoding.Default);
            FileUtils.Instance.writeFileByStream(savePath, "PlayerDemo.save", demo.ReadToEnd());
        }
        
        long time = 0;
        if (Directory.Exists(savePath)) {
            DirectoryInfo direction = new DirectoryInfo(savePath);
            FileInfo[] files = direction.GetFiles("*");
            foreach (var fileInfo in files) {
                var fileName = fileInfo.Name;
                if (!fileName.EndsWith(".save")) {
                    continue;
                }
                
                var sr = new StreamReader(Path.Combine(savePath, fileName), Encoding.Default);
                var str = sr.ReadToEnd();
                var dic = MiniJson.JsonDecode(str) as Dictionary<string, object>;
                var lastTime = dic.longValue("lastSaveTime");
                if (lastTime > time) {
                    time = lastTime;
                    this.currentFileName = fileName.Replace(".save", string.Empty);
                }
                
                sr.Close();
                sr.Dispose();
            }
        }

        if (this.currentFileName.IsNullOrEmpty()) {
            this.currentFileName = "Player1";
        }
        
        this._datas = new Dictionary<string, string>();

        if (!Directory.Exists(savePath)) {
            Directory.CreateDirectory(savePath);
        }
    }

    public Dictionary<string, object> getFileDataByFileName(string fileName)
    {
        var sr = new StreamReader(this.getFullPathByFileName(fileName), Encoding.Default);
        var str = sr.ReadToEnd();
        var dic = MiniJson.JsonDecode(str) as Dictionary<string, object>;
        sr.Close();
        sr.Dispose();
        return dic;
    }

    public bool hasDemoFile
    {
        get
        {
            if (!File.Exists(this.demoSavePath)) {
                return false;
            }
            return !savePath.Contains(DemoFileName) ;
        }
    }
    public bool saveByUpdate { get; set; }

    public void openDBAsync(Action callBack)
    {
        //根据路径，读取文件

        //判断文件是否存在
        if (!File.Exists(this.fullPath))
        {
            callBack?.Invoke();
            return;
        }
        
        using (var sr = new StreamReader(this.fullPath, Encoding.Default)) {
            var str = sr.ReadToEnd();
            if (str.IsNullOrEmpty()) {
                callBack?.Invoke();
                return;
            }
            var dic = MiniJson.JsonDecode(str) as Dictionary<string, object>;
            foreach (var kvp in dic) {
                this._datas[kvp.Key] = kvp.Value.toString();
            }
            sr.Close();
            sr.Dispose();
        }
        callBack?.Invoke();
    }

    public void closeDB()
    {
        
    }

    public void clearDB()
    {
        //清空文件
        this._datas.Clear();
        this.saveByUpdate = true;
    }

    public string getValue(string key)
    {
        return this._datas.objectValue(key);
    }

    public void setValue(string key, string value)
    {
        this._datas[key] = value;
        this.saveByUpdate = true;
    }

    public void removeValue(string key)
    {
        this._datas.Remove(key);
        this.saveByUpdate = true;
    }

    public void saveDB()
    {
        //TODO:保存文件
        if (this._datas.Count == 0) {
            return;
        }

        this._datas["lastSaveTime"] = (DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds.toLong().toString();
        var sw = new StreamWriter(this.fullPath, false, Encoding.UTF8);
        //json方法写入
        var str = MiniJson.JsonEncode(this._datas);
        sw.Write(str);

        //清空缓冲区
        sw.Flush();
        //关闭流
        sw.Close();
        sw.Dispose();
    }

    public Dictionary<string, object> exportDB()
    {
        var data = new Dictionary<string, object>();
        foreach (var kvp in this._datas) {
            data[kvp.Key] = kvp.Value;
        }
        return data;
    }

    public void update(float dt)
    {
        if (this.saveByUpdate) {
            this.saveDB();
            this.saveByUpdate = false;
        }
    }

    public void saveTargetFile(string targetFileName)
    {
        //准备切换到目标存档钱调用，用来刷新lastFileName
        var datas = new Dictionary<string, string>();

        var fp = this.getFullPathByFileName(targetFileName);
        if (File.Exists(fp))
        {
            using (var sr = new StreamReader(fp, Encoding.Default)) {
                var str = sr.ReadToEnd();
                
                var dic = MiniJson.JsonDecode(str) as Dictionary<string, object>;
                foreach (var kvp in dic) {
                    datas[kvp.Key] = kvp.Value.toString();
                }
            }
        }

        datas["lastSaveTime"] = (DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds.toLong().toString();
        var sw = new StreamWriter(fp, false, Encoding.UTF8);
        var s = MiniJson.JsonEncode(datas);
        sw.Write(s);
        //清空缓冲区
        sw.Flush();
        //关闭流
        sw.Close();
        sw.Dispose();
    }

    public void delectSaveFile(string fileName)
    {
        var fp = this.getFullPathByFileName(fileName);
        if (File.Exists(fp)) {
            File.Delete(fp);
        }
    }
}
