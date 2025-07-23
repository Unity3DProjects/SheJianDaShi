using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using QGMiniGame;
using UnityEngine;

public class QGFileSystemManager
{
	private static Dictionary<string, QGStatOption> statDict;

	private static Dictionary<string, object> asyncDict = new Dictionary<string, object>();

	public void SaveFile(SaveFileOption option)
	{
		string text = Guid.NewGuid().ToString();
		string text2 = HookCallback<SaveFileOption, SaveFileSuccessCallbackResult, FileError>(option, text);
		QG_FS_SaveFile(text2, text);
	}

	public string SaveFileSync(string tempFilePath, string filePath = "")
	{
		return QG_FS_SaveFileSync(tempFilePath, filePath);
	}

	/// <summary>
	/// 创建目录, 详见 https://developers.weixin.qq.com/minigame/dev/api/file/FileSystemManager.mkdir.html
	/// </summary>
	/// <param name="param"></param>
	public void Mkdir(MkdirParam param)
	{
		QG_FS_Mkdir(param.dirPath, param.recursive, QGCallBackManager.Add(param.success), QGCallBackManager.Add(param.fail), QGCallBackManager.Add(param.complete));
	}

	/// <summary>
	/// 同步创建目录,详见 https://developers.weixin.qq.com/minigame/dev/api/file/FileSystemManager.mkdirSync.html
	/// </summary>
	/// <param name="dirPath">创建的目录路径 (本地路径)</param>
	/// <param name="recursive">是否在递归创建该目录的上级目录后再创建该目录。如果对应的上级目录已经存在，则不创建该上级目录。如 dirPath 为 a/b/c/d 且 recursive 为 true，将创建 a 目录，再在 a 目录下创建 b 目录，以此类推直至创建 a/b/c 目录下的 d 目录。</param>
	/// <returns></returns>
	public string MkdirSync(string dirPath, bool recursive)
	{
		return QG_FS_MkdirSync(dirPath, recursive);
	}

	/// <summary>
	/// 读取本地文件内容, 详见 https://developers.weixin.qq.com/minigame/dev/api/file/FileSystemManager.readFile.html
	/// </summary>
	/// <param name="param"></param>
	public void ReadFile(ReadFileParam option)
	{
		string text = Guid.NewGuid().ToString();
		string text2 = HookCallback<ReadFileParam, QGReadFileResponse, FileError>(option, text);
		QG_FS_ReadFile(text2, text);
	}

	/// <summary>
	/// 同步读取本地文件，详见 https://developers.weixin.qq.com/minigame/dev/api/file/FileSystemManager.readFileSync.html
	/// </summary>
	/// <param name="filePath">要读取的文件的路径 (本地路径)</param>
	/// <param name="bytes">返回结果，失败为null</param>
	/// <returns></returns>
	public byte[] ReadFileSync(string filePath, long? position = null, long? length = null)
	{
		ReadFileParam obj = new ReadFileParam
		{
			filePath = filePath,
			position = position,
			length = length
		};
		int num = int.Parse(QG_FS_ReadFileSync(JsonMapper.ToJson(obj)));
		if (num == 0)
		{
			return null;
		}
		byte[] array = new byte[num];
		QGMiniGameManager.QGGetFileBuffer(array, filePath);
		return array;
	}

	/// <summary>
	/// 同步读取本地文件，详见 https://developers.weixin.qq.com/minigame/dev/api/file/FileSystemManager.readFileSync.html
	/// </summary>
	/// <param name="filePath">要读取的文件的路径 (本地路径)</param>
	/// <param name="encoding">指定读取文件的字符编码,不能为空</param>
	/// <returns></returns>
	public string ReadFileSync(string filePath, string encoding = "", long? position = null, long? length = null)
	{
		ReadFileParam obj = new ReadFileParam
		{
			filePath = filePath,
			position = position,
			length = length,
			encoding = encoding
		};
		
		return QG_FS_ReadFileSync(JsonMapper.ToJson(obj));
	}

	public void Readdir(ReaddirOption option)
	{
		string text = Guid.NewGuid().ToString();
		string text2 = HookCallback<ReaddirOption, ReaddirSuccessCallbackResult, FileError>(option, text);
		QG_FS_Readdir(text2, text);
	}
	public string[] ReaddirSync(string dirPath)
	{
		return JsonMapper.ToObject<string[]>(QG_FS_ReaddirSync(dirPath));
	}

	public void ReadZipEntry(ReadZipEntryOption option)
	{
		string text = Guid.NewGuid().ToString();
		string text2 = HookCallback<ReadZipEntryOption, ReadZipEntrySuccessCallbackResult, FileError>(option, text);
		QG_FS_ReadZipEntry(text2, text);
	}

	public void ReadZipEntry(ReadZipEntryOptionString option)
	{
		string text = Guid.NewGuid().ToString();
		string text2 = HookCallback<ReadZipEntryOptionString, ReadZipEntrySuccessCallbackResult, FileError>(option, text);
		QG_FS_ReadZipEntryString(text2, text);
	}

	public void ReadCompressedFile(ReadCompressedFileOption option)
	{
		string text = Guid.NewGuid().ToString();
		string text2 = HookCallback<ReadCompressedFileOption, ReadCompressedFileSuccessCallbackResult, FileError>(option, text);
		QG_FS_ReadCompressedFile(text2, text);
	}

	public byte[] ReadCompressedFileSync(ReadCompressedFileSyncOption option)
	{
		string text = Guid.NewGuid().ToString();
		int num = QG_FS_ReadCompressedFileSync(JsonMapper.ToJson(option), text);
		Debug.Log("ReadCompressedFileSync num:" + num);
		if (num != 0)
        {
			byte[] array = new byte[num];
			QGMiniGameManager.QGGetFileBuffer(array, text);
			return array;
		}
		return null;
	}

	/// <summary>
	/// 复制文件 详见 https://developers.weixin.qq.com/minigame/dev/api/file/FileSystemManager.copyFile.html
	/// </summary>
	/// <param name="param"></param>
	public void CopyFile(CopyFileParam param)
	{
		QG_FS_CopyFile(param.srcPath, param.destPath, QGCallBackManager.Add(param.success), QGCallBackManager.Add(param.fail), QGCallBackManager.Add(param.complete));
	}

	/// <summary>
	/// 同步复制文件 详见 https://developers.weixin.qq.com/minigame/dev/api/file/FileSystemManager.copyFileSync.html
	/// </summary>
	/// <param name="srcPath">源文件路径，支持本地路径</param>
	/// <param name="destPath">目标文件路径，支持本地路径</param>
	/// <returns>成功返回 "copyFile:ok" 其他为失败</returns>
	public string CopyFileSync(string srcPath, string destPath)
	{
		return QG_FS_CopyFileSync(srcPath, destPath);
	}

	public void GetFileInfo(GetFileInfoOption option)
	{
		string text = Guid.NewGuid().ToString();
		string text2 = HookCallback<GetFileInfoOption, GetFileInfoSuccessCallbackResult, FileError>(option, text);
		QG_FS_GetFileInfo(text2, text);
	}

	public void GetSavedFileList(GetSavedFileListOption option)
	{
		string text = Guid.NewGuid().ToString();
		string text2 = HookCallback<GetSavedFileListOption, GetSavedFileListSuccessCallbackResult, FileError>(option, text);
		QG_FS_GetSavedFileList(text2, text);
	}

	public void Unzip(UnzipOption option)
	{
		string text = Guid.NewGuid().ToString();
		string text2 = HookCallback<UnzipOption, FileError>(option, text);
		QG_FS_Unzip(text2, text);
	}

	/// <summary>
	/// 判断文件/目录是否存在 详见 https://developers.weixin.qq.com/minigame/dev/api/file/FileSystemManager.access.html
	/// </summary>
	/// <param name="param"></param>
	public void Access(AccessParam param)
	{
		QG_FS_AccessFile(param.path, QGCallBackManager.Add(param.success), QGCallBackManager.Add(param.fail), QGCallBackManager.Add(param.complete));
	}

	/// <summary>
	/// 同步判断文件/目录是否存在 详见 https://developers.weixin.qq.com/minigame/dev/api/file/FileSystemManager.accessSync.html
	/// </summary>
	/// <param name="path">要判断是否存在的文件/目录路径 (本地路径)</param>
	/// <returns>成功返回 "access:ok" 其他为失败，不可访问</returns>
	public string AccessSync(string path)
	{
		return QG_FS_AccessFileSync(path);
	}

	public void RemoveSavedFile(RemoveSavedFileOption option)
	{
		string text = Guid.NewGuid().ToString();
		string text2 = HookCallback<RemoveSavedFileOption, FileError>(option, text);
		QG_FS_RemoveSavedFile(text2, text);
	}

	/// <summary>
	/// 删除目录，https://developers.weixin.qq.com/minigame/dev/api/file/FileSystemManager.rmdir.html
	/// </summary>
	/// <param name="param"></param>
	public void Rmdir(RmdirParam param)
	{
		QG_FS_Rmdir(param.dirPath, param.recursive, QGCallBackManager.Add(param.success), QGCallBackManager.Add(param.fail), QGCallBackManager.Add(param.complete));
	}

	/// <summary>
	/// 同步删除目录，https://developers.weixin.qq.com/minigame/dev/api/file/FileSystemManager.rmdirSync.html
	/// </summary>
	/// <param name="dirPath">要删除的目录路径 (本地路径)</param>
	/// <param name="recursive">是否递归删除目录。如果为 true，则删除该目录和该目录下的所有子目录以及文件</param>
	/// <returns></returns>
	public string RmdirSync(string dirPath, bool recursive)
	{
		return QG_FS_RmdirSync(dirPath, recursive);
	}

	/// <summary>
	/// 删除文件 详见 https://developers.weixin.qq.com/minigame/dev/api/file/FileSystemManager.unlink.html
	/// </summary>
	/// <param name="param"></param>
	public void Unlink(UnlinkParam param)
	{
		QG_FS_Unlink(param.filePath, QGCallBackManager.Add(param.success), QGCallBackManager.Add(param.fail), QGCallBackManager.Add(param.complete));
	}

	/// <summary>
	/// 同步删除文件 详见 https://developers.weixin.qq.com/minigame/dev/api/file/FileSystemManager.unlinkSync.html
	/// </summary>
	/// <param name="filePath">源文件路径，支持本地路径</param>
	/// <returns>成功返回 "unlink:ok" 其他为失败</returns>
	public string UnlinkSync(string filePath)
	{
		return QG_FS_UnlinkSync(filePath);
	}

	/// <summary>
	/// 将二进制写入文件, 详见 https://developers.weixin.qq.com/minigame/dev/api/file/FileSystemManager.writeFile.html
	/// </summary>
	/// <param name="param"></param>
	public void WriteFile(WriteFileParam param)
	{
		QG_FS_WriteFile(param.filePath, param.data, param.data.Length, param.encoding, QGCallBackManager.Add(param.success), QGCallBackManager.Add(param.fail), QGCallBackManager.Add(param.complete));
	}

	/// <summary>
	/// 将字符串写入文件, 详见 https://developers.weixin.qq.com/minigame/dev/api/file/FileSystemManager.appendFile.html
	/// </summary>
	/// <param name="param"></param>
	public void WriteFile(WriteFileStringParam param)
	{
		QG_FS_WriteStringFile(param.filePath, param.data, param.encoding, QGCallBackManager.Add(param.success), QGCallBackManager.Add(param.fail), QGCallBackManager.Add(param.complete));
	}

	/// <summary>
	/// 同步写文件，详见 https://developers.weixin.qq.com/minigame/dev/api/file/FileSystemManager.writeFileSync.html
	/// </summary>
	/// <param name="filePath">要写入的文件路径 (本地路径)</param>
	/// <param name="data">要写入的文本</param>
	/// <param name="encoding">指定写入文件的字符编码</param>
	/// <returns>错误信息，如果成功的话返回ok</returns>
	public string WriteFileSync(string filePath, string data, string encoding = "utf8")
	{
		return QG_FS_WriteFileSync(filePath, data, encoding);
	}

	/// <summary>
	/// 同步写文件，详见 https://developers.weixin.qq.com/minigame/dev/api/file/FileSystemManager.writeFileSync.html
	/// </summary>
	/// <param name="filePath">要写入的文件路径 (本地路径)</param>
	/// <param name="data">要写入的二进制数据</param>
	/// <param name="encoding">指定写入文件的字符编码</param>
	/// <returns>错误信息，如果成功的话返回ok</returns>
	public string WriteFileSync(string filePath, byte[] data, string encoding = "binary")
	{
		return QG_FS_WriteBinFileSync(filePath, data, data.Length, encoding);
	}

	/// <summary>
	/// 在文件结尾追加二进制内容, 详见 https://developers.weixin.qq.com/minigame/dev/api/file/FileSystemManager.appendFile.html
	/// </summary>
	/// <param name="param"></param>
	public void AppendFile(WriteFileParam param)
	{
		QG_FS_AppendFile(param.filePath, param.data, param.data.Length, param.encoding, QGCallBackManager.Add(param.success), QGCallBackManager.Add(param.fail), QGCallBackManager.Add(param.complete));
	}

	/// <summary>
	/// 在文件结尾追加文本内容, 详见 https://developers.weixin.qq.com/minigame/dev/api/file/FileSystemManager.appendFile.html
	/// </summary>
	/// <param name="param"></param>
	public void AppendFile(WriteFileStringParam param)
	{
		QG_FS_AppendStringFile(param.filePath, param.data, param.encoding, QGCallBackManager.Add(param.success), QGCallBackManager.Add(param.fail), QGCallBackManager.Add(param.complete));
	}

	public void AppendFileSync(string filePath, string data, string encoding = "utf8")
	{
		QG_FS_AppendFileStringSync(filePath, data, encoding);
	}

	public void AppendFileSync(string filePath, byte[] data, string encoding = "utf8")
	{
		QG_FS_AppendFileSync(filePath, data, data.Length, encoding);
	}

	public void Rename(RenameOption option)
	{
		string text = Guid.NewGuid().ToString();
		string text2 = HookCallback<RenameOption, FileError>(option, text);
		QG_FS_Rename(text2, text);
	}

	public void RenameSync(string oldPath, string newPath)
	{
		QG_FS_RenameSync(oldPath, newPath);
	}

	public void Stat(QGStatOption option)
	{
		if (statDict == null)
		{
			statDict = new Dictionary<string, QGStatOption>();
		}
		string callbackId = QGCallBackManager.GetCallbackId(statDict);
		QGStatOption value = new QGStatOption
		{
			success = option.success,
			fail = option.fail,
			complete = option.complete
		};
		statDict.Add(callbackId, value);
		Action<QGStatResponse> success = option.success;
		Action<QGStatResponse> fail = option.fail;
		Action<QGStatResponse> complete = option.complete;
		option.success = null;
		option.fail = null;
		option.complete = null;
		string text = JsonMapper.ToJson(option);
		option.success = success;
		option.fail = fail;
		option.complete = complete;
		QG_FS_Stat(text, callbackId);
	}
	public QGStat[] StatSync(string path, bool recursive = true)
	{
		return JsonMapper.ToObject<QGStat[]>(QG_FS_StatSync(path, recursive));
	}

	public static void HandleStatCallback(string msg)
	{
		if (string.IsNullOrEmpty(msg) || statDict == null)
		{
			return;
		}
		QGJSCallback qGJSCallback = JsonMapper.ToObject<QGJSCallback>(msg);
		string callbackId = qGJSCallback.callbackId;
		string type = qGJSCallback.type;
		string res = qGJSCallback.res;
		Debug.Log("HandleStatCallback callbackId= " + callbackId + " type=" + type + " res=" + res);
		if (statDict.ContainsKey(callbackId))
		{
			QGStatOption wXStatOption = statDict[callbackId];
			switch (type)
			{
				case "complete":
					wXStatOption.complete?.Invoke(JsonMapper.ToObject<QGStatResponse>(res));
					wXStatOption.complete = null;
					break;
				case "success":
					wXStatOption.success?.Invoke(JsonMapper.ToObject<QGStatResponse>(res));
					goto default;
				case "fail":
					wXStatOption.fail?.Invoke(JsonMapper.ToObject<QGStatResponse>(res));
					goto default;
				default:
					wXStatOption.success = null;
					wXStatOption.fail = null;
					break;
			}
			if (wXStatOption.complete == null && wXStatOption.success == null && wXStatOption.fail == null)
			{
				statDict.Remove(callbackId);
			}
		}
	}

	public static void HandleMethodCallback(string msg)
	{
		if (string.IsNullOrEmpty(msg))
		{
			return;
		}
		QGJSTypeCallback qGJSTypeCallback = JsonMapper.ToObject<QGJSTypeCallback>(msg);
		string callbackId = qGJSTypeCallback.callbackId;
		string type = qGJSTypeCallback.type;
		string res = qGJSTypeCallback.res;
		string method = qGJSTypeCallback.method;
		Debug.Log("HandleMethodCallback callbackId= " + callbackId + " method= " + method + " type=" + type + " res=" + res);
		switch (method)
		{
			case "saveFile":
				BaseMethodCallback<SaveFileOption>(callbackId, type, res);
				break;
			case "readFile":
				if (type == "success")
				{
					ReadFileParam readFileParam = (ReadFileParam)asyncDict[callbackId];
					Debug.Log("readFile res = " + res);
					QGReadFileResponse qGReadFileResponse = JsonMapper.ToObject<QGReadFileResponse>(res);
					Debug.Log("readFile qGReadFileResponse = " + JsonMapper.ToJson(qGReadFileResponse));
					if (qGReadFileResponse.arrayBufferLength.HasValue)
					{
						byte[] array2 = new byte[qGReadFileResponse.arrayBufferLength.GetValueOrDefault()];
						QGMiniGameManager.QGGetFileBuffer(array2, callbackId);
						QGReadFileResponse obj = new QGReadFileResponse
						{
							errMsg = qGReadFileResponse.errMsg,
							binData = array2
						};
						readFileParam.success?.Invoke(obj);
					}
					else
					{
						readFileParam.success?.Invoke(qGReadFileResponse);
					}
					if (readFileParam.complete == null && readFileParam.success == null && readFileParam.fail == null)
					{
						asyncDict.Remove(callbackId);
					}
				}
				else
				{
					BaseMethodCallback<ReadFileParam>(callbackId, type, res);
				}
				break;
			case "readCompressedFile":
				if (type == "success")
				{
					ReadCompressedFileOption readParam = (ReadCompressedFileOption)asyncDict[callbackId];
					Debug.Log("readCompressedFile res = " + res);
					ReadCompressedFileSuccessCallbackResult qGReadResponse = JsonMapper.ToObject<ReadCompressedFileSuccessCallbackResult>(res);
					Debug.Log("readCompressedFile qGReadResponse = " + JsonMapper.ToJson(qGReadResponse));
					if (qGReadResponse.arrayBufferLength.HasValue)
					{
						byte[] array2 = new byte[qGReadResponse.arrayBufferLength.GetValueOrDefault()];
						QGMiniGameManager.QGGetFileBuffer(array2, callbackId);
						ReadCompressedFileSuccessCallbackResult obj = new ReadCompressedFileSuccessCallbackResult
						{
							errMsg = qGReadResponse.errMsg,
							data = array2
						};
						readParam.success?.Invoke(obj);
					}
					else
					{
						readParam.success?.Invoke(qGReadResponse);
					}
					if (readParam.complete == null && readParam.success == null && readParam.fail == null)
					{
						asyncDict.Remove(callbackId);
					}
				}
				else
				{
					BaseMethodCallback<ReadCompressedFileOption>(callbackId, type, res);
				}
				break;
			case "readdir":
				BaseMethodCallback<ReaddirOption>(callbackId, type, res);
				break;
			case "readZipEntry":
				BaseMethodCallback<ReadZipEntryOption>(callbackId, type, res);
				break;
			case "readZipEntry_string":
				BaseMethodCallback<ReadZipEntryOptionString>(callbackId, type, res);
				break;
			case "getFileInfo":
				BaseMethodCallback<GetFileInfoOption>(callbackId, type, res);
				break;
			case "getSavedFileList":
				BaseMethodCallback<GetSavedFileListOption>(callbackId, type, res);
				break;
			case "unzip":
				BaseMethodCallback<UnzipOption>(callbackId, type, res);
				break;
			case "removeSavedFile":
				BaseMethodCallback<RemoveSavedFileOption>(callbackId, type, res);
				break;
			case "rename":
				BaseMethodCallback<RenameOption>(callbackId, type, res);
				break;
			case "stat":
				BaseMethodCallback<QGStatOption>(callbackId, type, res);
				break;
		}
	}

	public static void BaseMethodCallback<T>(string callbackId, string type, string res) where T : class
	{
		if (asyncDict.ContainsKey(callbackId))
		{
			T obj = (T)asyncDict[callbackId];
			Type typeFromHandle = typeof(T);
			FieldInfo field = typeFromHandle.GetField("success");
			FieldInfo field2 = typeFromHandle.GetField("fail");
			FieldInfo field3 = typeFromHandle.GetField("complete");
			object obj2;
			if ((object)field == null)
			{
				obj2 = null;
			}
			else
			{
				Type fieldType = field.FieldType;
				obj2 = (((object)fieldType != null) ? fieldType.GetGenericArguments()[0] : null);
			}
			Type convertType = (Type)obj2;
			object obj3;
			if ((object)field2 == null)
			{
				obj3 = null;
			}
			else
			{
				Type fieldType2 = field2.FieldType;
				obj3 = (((object)fieldType2 != null) ? fieldType2.GetGenericArguments()[0] : null);
			}
			Type convertType2 = (Type)obj3;
			object obj4;
			if ((object)field3 == null)
			{
				obj4 = null;
			}
			else
			{
				Type fieldType3 = field3.FieldType;
				obj4 = (((object)fieldType3 != null) ? fieldType3.GetGenericArguments()[0] : null);
			}
			Type convertType3 = (Type)obj4;
			MethodInfo method = typeFromHandle.GetMethod("InvokeSuccess");
			MethodInfo method2 = typeFromHandle.GetMethod("InvokeFail");
			MethodInfo method3 = typeFromHandle.GetMethod("InvokeComplete");
			switch (type)
			{
				case "complete":
					method3?.Invoke(obj, new object[1] { JsonMapper.ToObject(res, convertType3) });
					field3?.SetValue(obj, null);
					break;
				case "success":
					method?.Invoke(obj, new object[1] { JsonMapper.ToObject(res, convertType) });
					goto default;
				case "fail":
					method2?.Invoke(obj, new object[1] { JsonMapper.ToObject(res, convertType2) });
					goto default;
				default:
					field?.SetValue(obj, null);
					field2?.SetValue(obj, null);
					break;
			}
			if (((object)field3 == null || field3.GetValue(obj) == null) && ((object)field == null || field.GetValue(obj) == null) && field2?.GetValue(obj) == null)
			{
				asyncDict.Remove(callbackId);
			}
		}
	}

	private string HookCallback<A, a>(A P_0, string P_1) where A : QGBaseActionOption<a>
	{
		Type typeFromHandle = typeof(A);
		Action<a> success = P_0.success;
		Action<a> fail = P_0.fail;
		Action<a> complete = P_0.complete;
		asyncDict.Add(P_1, P_0);
		P_0.success = null;
		P_0.fail = null;
		P_0.complete = null;
		string result = JsonMapper.ToJson(P_0);
		P_0.success = success;
		P_0.fail = fail;
		P_0.complete = complete;
		return result;
	}

	private string HookCallback<A, a, B>(A P_0, string P_1) where A : QGBaseActionOption<a, B>
	{
		Type typeFromHandle = typeof(A);
		Action<a> success = P_0.success;
		Action<B> fail = P_0.fail;
		Action<B> complete = P_0.complete;
		asyncDict.Add(P_1, P_0);
		P_0.success = null;
		P_0.fail = null;
		P_0.complete = null;
		string result = JsonMapper.ToJson(P_0);
		P_0.success = success;
		P_0.fail = fail;
		P_0.complete = complete;
		return result;
	}

	[DllImport("__Internal", EntryPoint = "QG_FS_SaveFile")]
	private static extern void QG_FS_SaveFile(string P_0, string P_1);

	[DllImport("__Internal", EntryPoint = "QG_FS_SaveFileSync")]
	private static extern string QG_FS_SaveFileSync(string P_0, string P_1);

	[DllImport("__Internal", EntryPoint = "QG_FS_Mkdir")]
	private static extern void QG_FS_Mkdir(string P_0, bool P_1, string P_2, string P_3, string P_4);

	[DllImport("__Internal", EntryPoint = "QG_FS_MkdirSync")]
	private static extern string QG_FS_MkdirSync(string P_0, bool P_1);

	[DllImport("__Internal", EntryPoint = "QG_FS_ReadFile")]
	private static extern string QG_FS_ReadFile(string P_0, string P_1);

	[DllImport("__Internal", EntryPoint = "QG_FS_ReadFileSync")]
	private static extern string QG_FS_ReadFileSync(string P_0);

	[DllImport("__Internal", EntryPoint = "QG_FS_Readdir")]
	private static extern void QG_FS_Readdir(string P_0, string P_1);

	[DllImport("__Internal", EntryPoint = "QG_FS_ReaddirSync")]
	private static extern string QG_FS_ReaddirSync(string P_0);

	[DllImport("__Internal", EntryPoint = "QG_FS_ReadZipEntry")]
	private static extern void QG_FS_ReadZipEntry(string P_0, string P_1);

	[DllImport("__Internal", EntryPoint = "QG_FS_ReadZipEntryString")]
	private static extern void QG_FS_ReadZipEntryString(string P_0, string P_1);

	[DllImport("__Internal", EntryPoint = "QG_FS_ReadCompressedFile")]
	private static extern void QG_FS_ReadCompressedFile(string P_0, string P_1);

	[DllImport("__Internal", EntryPoint = "QG_FS_ReadCompressedFileSync")]
	private static extern int QG_FS_ReadCompressedFileSync(string P_0, string P_1);

	[DllImport("__Internal", EntryPoint = "QG_FS_CopyFile")]
	private static extern void QG_FS_CopyFile(string P_0, string P_1, string P_2, string P_3, string P_4);

	[DllImport("__Internal", EntryPoint = "QG_FS_CopyFileSync")]
	private static extern string QG_FS_CopyFileSync(string P_0, string P_1);

	[DllImport("__Internal", EntryPoint = "QG_FS_GetFileInfo")]
	private static extern void QG_FS_GetFileInfo(string P_0, string P_1);

	[DllImport("__Internal", EntryPoint = "QG_FS_GetSavedFileList")]
	private static extern void QG_FS_GetSavedFileList(string P_0, string P_1);

	[DllImport("__Internal", EntryPoint = "QG_FS_Unzip")]
	private static extern void QG_FS_Unzip(string P_0, string P_1);

	[DllImport("__Internal", EntryPoint = "QG_FS_AccessFile")]
	private static extern void QG_FS_AccessFile(string P_0, string P_1, string P_2, string P_3);

	[DllImport("__Internal", EntryPoint = "QG_FS_AccessFileSync")]
	private static extern string QG_FS_AccessFileSync(string P_0);

	[DllImport("__Internal", EntryPoint = "QG_FS_RemoveSavedFile")]
	private static extern void QG_FS_RemoveSavedFile(string P_0, string P_1);

	[DllImport("__Internal", EntryPoint = "QG_FS_Rmdir")]
	private static extern void QG_FS_Rmdir(string P_0, bool P_1, string P_2, string P_3, string P_4);

	[DllImport("__Internal", EntryPoint = "QG_FS_RmdirSync")]
	private static extern string QG_FS_RmdirSync(string P_0, bool P_1);

	[DllImport("__Internal", EntryPoint = "QG_FS_Unlink")]
	private static extern void QG_FS_Unlink(string P_0, string P_1, string P_2, string P_3);

	[DllImport("__Internal", EntryPoint = "QG_FS_UnlinkSync")]
	private static extern string QG_FS_UnlinkSync(string P_0);

	[DllImport("__Internal", EntryPoint = "QG_FS_WriteFile")]
	private static extern string QG_FS_WriteFile(string P_0, byte[] P_1, int P_2, string P_3, string P_4, string P_5, string P_6);

	[DllImport("__Internal", EntryPoint = "QG_FS_WriteStringFile")]
	private static extern string QG_FS_WriteStringFile(string P_0, string P_1, string P_2, string P_3, string P_4, string P_5);

	[DllImport("__Internal", EntryPoint = "QG_FS_WriteFileSync")]
	private static extern string QG_FS_WriteFileSync(string P_0, string P_1, string P_2);

	[DllImport("__Internal", EntryPoint = "QG_FS_WriteBinFileSync")]
	private static extern string QG_FS_WriteBinFileSync(string P_0, byte[] P_1, int P_2, string P_3);

	[DllImport("__Internal", EntryPoint = "QG_FS_AppendFile")]
	private static extern string QG_FS_AppendFile(string P_0, byte[] P_1, int P_2, string P_3, string P_4, string P_5, string P_6);

	[DllImport("__Internal", EntryPoint = "QG_FS_AppendStringFile")]
	private static extern string QG_FS_AppendStringFile(string P_0, string P_1, string P_2, string P_3, string P_4, string P_5);

	[DllImport("__Internal", EntryPoint = "QG_FS_AppendFileSync")]
	private static extern void QG_FS_AppendFileSync(string P_0, byte[] P_1, int P_2, string P_3);

	[DllImport("__Internal", EntryPoint = "QG_FS_AppendFileStringSync")]
	private static extern void QG_FS_AppendFileStringSync(string P_0, string P_1, string P_2);

	[DllImport("__Internal", EntryPoint = "QG_FS_Rename")]
	private static extern void QG_FS_Rename(string P_0, string P_1);

	[DllImport("__Internal", EntryPoint = "QG_FS_RenameSync")]
	private static extern void QG_FS_RenameSync(string P_0, string P_1);

	[DllImport("__Internal", EntryPoint = "QG_FS_Stat")]
	private static extern void QG_FS_Stat(string P_0, string P_1);

	[DllImport("__Internal", EntryPoint = "QG_FS_StatSync")]
	private static extern string QG_FS_StatSync(string P_0, bool P_1);
}
