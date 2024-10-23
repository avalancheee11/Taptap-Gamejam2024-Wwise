using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public partial class FileUtils : SingletonData<FileUtils>
{
	protected override void OnInit()
	{
		this.initAssetBundle();
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="path">包含后缀的路径</param>
	/// <returns></returns>
	public string readFileStrByStream(string path)
	{
		if (!File.Exists(path)) {
			return string.Empty;
		}

		var sr = new StreamReader(path, Encoding.UTF8);
		var str = sr.ReadToEnd();
		sr.Close();
		sr.Dispose();
		return str;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="fileName">包含后缀的路径</param>
	/// <param name="value"></param>
	public void writeFileByStream(string path,string fileName, string value)
	{
		if (!Directory.Exists(path)) {
			Directory.CreateDirectory(path);
		}
		var p = Path.Combine(path, fileName);
		var sw = new StreamWriter(p, false, Encoding.UTF8);
		//json方法写入
		sw.Write(value);
		//清空缓冲区
		sw.Flush();
		//关闭流
		sw.Close();
		sw.Dispose();
	}

	/// <summary>
	/// PC和mac支持，其他平台需要访问权限
	/// </summary>
	/// <param name="path">包含后缀的路径</param>
	/// <returns></returns>
	public byte[] readFileByteByFile(string path)
	{
		if (!File.Exists(path)) {
			return null;
		}

		// //PC和mac支持，其他平台需要访问权限
		// FileStream fs = new FileStream(path, FileMode.Open);

		//IOS,IPAD,PC和mac支持
		var fs = File.OpenRead(path);
		byte[] bytes = new byte[fs.Length];
		fs.Read(bytes, 0, bytes.Length);
		fs.Close();

		return bytes;
	}

	public byte[] readFileByteByFileStream(string path)
	{
		if (!File.Exists(path)) {
			return null;
		}

		Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
		byte[] bt = new byte[stream.Length];
		stream.Read(bt, 0, (int) stream.Length);
		return bt;
	}

	public string readFileStrByFileStream(string path)
	{
		var bytes = this.readFileByteByFileStream(path);
		return bytes == null ? string.Empty : Encoding.UTF8.GetString(bytes).Replace("\uFEFF", string.Empty);
		;
	}

	public byte[] readFileBytesByUnityWebRequest(string path)
	{
		var uri = new Uri(path);
		var www = UnityWebRequest.Get(uri);
		www.SendWebRequest();
		while (true) {
			if (www.error != null) {
				Debug.LogWarning("UnityWebRequest Error :" + www.error);
				return null;
			}

			if (www.downloadHandler.isDone) {
				return www.downloadHandler.data;
			}
		}
	}

	public void readFileStringAsync(string path, Action<string> callBack)
	{
		CoroutineUtils.Instance.StartCoroutine(load(path, callBack));

		IEnumerator load(string path, Action<string> callBack)
		{
			var uri = new Uri(path);
			var www = UnityWebRequest.Get(uri);
			yield return www.SendWebRequest();
			if (www.error != null) {
				Debug.LogWarning("UnityWebRequest Error :" + www.error);
				callBack?.Invoke(null);
			}

			if (www.downloadHandler.isDone) {
				var bytes = www.downloadHandler.data;
				var str =bytes == null ? string.Empty : Encoding.UTF8.GetString(bytes).Replace("\uFEFF", string.Empty);
				callBack?.Invoke(str);
			}
		}
	}

	public string readFileStrByUnityWebRequest(string path)
	{
		var bytes = this.readFileBytesByUnityWebRequest(path);
		return bytes == null ? string.Empty : Encoding.UTF8.GetString(bytes).Replace("\uFEFF", string.Empty);
	}

	/// <summary>
	/// 包含后缀的路径
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public Texture2D readTexture(string path)
	{
		if (!File.Exists(path)) {
			return null;
		}

		Texture2D tex = new Texture2D(2, 2);
		var bytes = this.readFileBytesByUnityWebRequest(path);
		if (bytes == null) {
			Debug.LogError("ReadTexture Bytes is null");
		}

		tex.LoadImage(bytes);
		tex.Apply();
		return tex;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="path">包含后缀的路径</param>
	/// <returns></returns>
	public Sprite readSprite(string path)
	{
		var tex = this.readTexture(path);
		if (tex == null) {
			return null;
		}

		var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
		return sprite;
	}

	#region CSV
	public static List<string[]> ReadCSV(string path, char Separator = ',')
	{
		var text = ReadCSVfile(path, Encoding.Default);

		return ConvertStringToCsv(text, Separator);
	}

	public static List<string[]> ConvertStringToCsv(string text, char Separator)
	{
		text = text.Replace("\r\n", "\n");
		text = text.Replace("\r", "\n");
		int iStart = 0;
		List<string[]> CSV = new List<string[]>();

		while (iStart < text.Length) {
			string[] list = ParseCSVline(text, ref iStart, Separator);
			if (list == null) break;
			CSV.Add(list);
		}

		return CSV;
	}

	static string ReadCSVfile(string Path, Encoding encoding)
	{
		string Text = string.Empty;
#if (UNITY_WP8 || UNITY_METRO) && !UNITY_EDITOR
				byte[] buffer = UnityEngine.Windows.File.ReadAllBytes (Path);
				Text = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
#else
		using (var reader = new StreamReader(Path, encoding))
			Text = reader.ReadToEnd();
#endif
		return Text;
	}

	static string[] ParseCSVline(string Line, ref int iStart, char Separator)
	{
		List<string> list = new List<string>();

		//Line = "puig,\"placeres,\"\"cab\nr\nera\"\"algo\"\npuig";//\"Frank\npuig\nplaceres\",aaa,frank\nplaceres";

		int TextLength = Line.Length;
		int iWordStart = iStart;
		bool InsideQuote = false;

		while (iStart < TextLength) {
			char c = Line[iStart];

			if (InsideQuote) {
				if (c == '\"') //--[ Look for Quote End ]------------
				{
					if (iStart + 1 >= TextLength || Line[iStart + 1] != '\"') //-- Single Quote:  Quotation Ends
					{
						InsideQuote = false;
					}
					else if (iStart + 2 < TextLength && Line[iStart + 2] == '\"') //-- Tripple Quotes: Quotation ends
					{
						InsideQuote = false;
						iStart += 2;
					}
					else
						iStart++; // Skip Double Quotes
				}
			}

			else //-----[ Separators ]----------------------

			if (c == '\n' || c == Separator) {
				AddCSVtoken(ref list, ref Line, iStart, ref iWordStart);
				if (c == '\n') // Stop the row on line breaks
				{
					iStart++;
					break;
				}
			}

			else //--------[ Start Quote ]--------------------

			if (c == '\"')
				InsideQuote = true;

			iStart++;
		}

		if (iStart > iWordStart)
			AddCSVtoken(ref list, ref Line, iStart, ref iWordStart);

		return list.ToArray();
	}

	static void AddCSVtoken(ref List<string> list, ref string Line, int iEnd, ref int iWordStart)
	{
		string Text = Line.Substring(iWordStart, iEnd - iWordStart);
		iWordStart = iEnd + 1;

		Text = Text.Replace("\"\"", "\"");
		if (Text.Length > 1 && Text[0] == '\"' && Text[Text.Length - 1] == '\"')
			Text = Text.Substring(1, Text.Length - 2);

		list.Add(Text);
	}
	
	public static void DataTableToCSV(DataTable dtCSV, string path, bool writeHeader, char delimeter)
        {
            var csvFileFullName = Path.Combine(path, dtCSV.TableName + ".csv");

            if (dtCSV == null || dtCSV.Rows.Count == 0) {
                Debug.LogError("表为空");
                return;
            }
            if (!Directory.Exists(path)) {
	            Directory.CreateDirectory(path);
            }
            
            //Delete the old one
            if (File.Exists(csvFileFullName)) {
                File.Delete(csvFileFullName);
            }

            string tmpLineText = "";
            StringBuilder builder = new StringBuilder();
            //Write header
            if (writeHeader) {
                for (int i = 0; i < dtCSV.Columns.Count; i++) {
                    string tmpColumnValue = dtCSV.Columns[i].ColumnName;
                    AppendString(builder, tmpColumnValue, delimeter);
                    if (i < dtCSV.Columns.Count -1) {
                        builder.Append(delimeter);
                    }
                }
                builder.Append ("\n");
            }

            //Write content
            for (int j = 0; j < dtCSV.Rows.Count; j++) {
                for (int k = 0; k < dtCSV.Columns.Count; k++) {
                    string tmpRowValue = dtCSV.Rows[j][k].ToString();
                    AppendString(builder, tmpRowValue, delimeter);
                    if (k < dtCSV.Columns.Count -1) {
                        builder.Append(delimeter);
                    }
                }
                
                builder.Append ("\n");
            }
            using (StreamWriter sw = new StreamWriter(csvFileFullName, true, Encoding.UTF8)) {
	            sw.WriteLine(builder.ToString());
            }
        }

	static void AppendString( StringBuilder Builder, string Text, char Separator )
        {
            if (string.IsNullOrEmpty(Text))
                return;
            Text = Text.Replace ("\\n", "\n");
            if (Text.IndexOfAny((Separator+"\n\"").ToCharArray())>=0)
            {
                Text = Text.Replace("\"", "\"\"");
                Builder.AppendFormat("\"{0}\"", Text);
            }
            else 
            {
                Builder.Append (Text);
            }
        }

	public static DataTable CSVToDataTable(List<string[]> strList)
	{
		var dataTable = new DataTable();
		if (strList.Count == 0) {
			return dataTable;
		}

		var titles = strList[0];
		strList.RemoveAt(0);
		//标题
		foreach (var title in titles) {
			dataTable.Columns.Add(title);
		}
		//列表
		foreach (var strArr in strList) {
			dataTable.Rows.Add(strArr);
		}
		return dataTable;
	}

	#endregion
}
