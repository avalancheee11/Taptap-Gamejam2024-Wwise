#if UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CustomJson;
using UnityEditor;
using UnityEngine;
using XHFrameWork;


public class ConvertCsvToJson
{
    private static string SaveRootPath = CustomGlobalConfig.streamingAssetBasePath + "/mods/"+ CustomGlobalConfig.ModName +"/config/";
    
    [MenuItem("Tools/配置or数据/CsvToJson")]
    public static void convertCsvToJson()
    {
        if (!Directory.Exists(SaveRootPath)) {
            Directory.CreateDirectory(SaveRootPath);
        }
        else {
            DirectoryInfo direction = new DirectoryInfo(SaveRootPath);
            FileInfo[] files = direction.GetFiles("*");

            var list = new List<string>();
            foreach (var fileInfo in files) {
                var fileName = fileInfo.Name;
                list.Add(fileName);
            }

            foreach (var fileName in list) {
                File.Delete(SaveRootPath + "/" + fileName);
            }
        }
        
        var originFilePath = Path.Combine(Application.dataPath, "CsvConfig");

        var originDir = new DirectoryInfo(originFilePath);
        foreach (var fileInfo in originDir.GetFiles("*")) {
            var originFileName = fileInfo.Name;
            if (!originFileName.EndsWith(".csv")) {
                continue;
            }

            var csvPath = Path.Combine(originFilePath, originFileName);

            var list = FileUtils.ReadCSV(csvPath);
            var datas = DataUtils.Instance.popDict();
            
            var heads = list[0];
            for (int i = 0; i < list.Count; i++) {
                if (i == 0) {
                    continue;
                }
                
                var d = DataUtils.Instance.popDict();
                for (int j = 0; j < heads.Length; j++) {
                    var head = heads[j];
                    if (head == null) {
                        continue;
                    }
                    var value = list[i][j];
                    d[head] = value;
                }

                datas[i.toString()] = d;
            }
            
            //datas转Json
            var dd = DataUtils.Instance.popDict();
            dd["config"] = datas;
            var json = MiniJson.JsonEncode(dd);
            
            //写文件
            var sw = new StreamWriter(SaveRootPath + "/" + originFileName.Replace(".csv", string.Empty) + ".json", false, Encoding.UTF8);
            sw.Write(json);
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            sw.Dispose();
        }
    }
}

#endif