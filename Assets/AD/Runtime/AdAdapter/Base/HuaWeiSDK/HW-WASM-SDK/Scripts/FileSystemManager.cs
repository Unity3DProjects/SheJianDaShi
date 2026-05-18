using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Text;

namespace HWWASM
{
    public class FileSystemManager
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        public static extern void QG_CopyDataFromJs(string callbackId, byte[] data);

        [DllImport("__Internal")]
        private static extern void QG_GetFileSystemManager();

        [DllImport("__Internal")]
        private static extern void QG_FileSystemManagerAccess(string conf, string callbackId);

        [DllImport("__Internal")]
        private static extern string QG_FileSystemManagerAccessSync(string path);

        [DllImport("__Internal")]
        private static extern void QG_FileSystemManagerCopyFile(string conf, string callbackId);

        [DllImport("__Internal")]
        private static extern string QG_FileSystemManagerCopyFileSync(string srcPath, string destPath);

        [DllImport("__Internal")]
        private static extern void QG_FileSystemManagerMkdir(string conf, string callbackId);

        [DllImport("__Internal")]
        private static extern string QG_FileSystemManagerMkdirSync(string dirPath, bool recursive);

        [DllImport("__Internal")]
        private static extern void QG_FileSystemManagerRmdir(string conf, string callbackId);

        [DllImport("__Internal")]
        private static extern string QG_FileSystemManagerRmdirSync(string dirPath, bool recursive);

        [DllImport("__Internal")]
        private static extern void QG_FileSystemManagerReaddir(string conf, string callbackId);

        [DllImport("__Internal")]
        private static extern string QG_FileSystemManagerReaddirSync(string dirPath);

        [DllImport("__Internal")]
        private static extern void QG_FileSystemManagerReadFileString(string conf, string callbackId);

        [DllImport("__Internal")]
        private static extern void QG_FileSystemManagerReadFileBinary(string conf, string callbackId);

        [DllImport("__Internal")]
        private static extern string QG_FileSystemManagerReadFileStringSync(string filePath);

        [DllImport("__Internal")]
        private static extern string QG_FileSystemManagerReadFileBinarySync(string callbackId, string filePath);

        [DllImport("__Internal")]
        private static extern void QG_FileSystemManagerRename(string conf, string callbackId);

        [DllImport("__Internal")]
        private static extern string QG_FileSystemManagerRenameSync(string oldPath, string newPath);

        [DllImport("__Internal")]
        private static extern void QG_FileSystemManagerStat(string conf, string callbackId);

        [DllImport("__Internal")]
        private static extern string QG_FileSystemManagerStatSync(string path, bool recursive);

        [DllImport("__Internal")]
        private static extern void QG_FileSystemManagerUnlink(string conf, string callbackId);

        [DllImport("__Internal")]
        private static extern string QG_FileSystemManagerUnlinkSync(string path);

        [DllImport("__Internal")]
        private static extern void QG_FileSystemManagerUnzip(string conf, string callbackId);

        [DllImport("__Internal")]
        private static extern void QG_FileSystemManagerWriteFileString(string conf, string callbackId);

        [DllImport("__Internal")]
        private static extern void QG_FileSystemManagerWriteFileBinary(string filePath, byte[] data, int dataLength, string callbackId);

        [DllImport("__Internal")]
        private static extern string QG_FileSystemManagerWriteFileStringSync(string filePath, string data);

        [DllImport("__Internal")]
        private static extern string QG_FileSystemManagerWriteFileBinarySync(string filePath, byte[] data, int dataLength);

        [DllImport("__Internal")]
        private static extern void QG_FileSystemManagerSaveFile(string conf, string callbackId);

        [DllImport("__Internal")]
        private static extern string QG_FileSystemManagerSaveFileSync(string tempFilePath, string filePath);

        [DllImport("__Internal")]
        private static extern void QG_FileSystemManagerAppendFileString(string conf, string callbackId);

        [DllImport("__Internal")]
        private static extern void QG_FileSystemManagerAppendFileBinary(string filePath, byte[] data, int dataLength, string callbackId);

        [DllImport("__Internal")]
        private static extern string QG_FileSystemManagerAppendFileStringSync(string filePath, string data);

        [DllImport("__Internal")]
        private static extern string QG_FileSystemManagerAppendFileBinarySync(string filePath, byte[] data, int dataLength);

        [DllImport("__Internal")]
        private static extern void QG_FileSystemManagerGetFileInfo(string conf, string callbackId);

        [DllImport("__Internal")]
        private static extern void QG_FileSystemManagerRemoveSavedFile(string conf, string callbackId);

#else
        public static void QG_CopyDataFromJs(string callbackId, byte[] data)
        {
        }

        private static void QG_GetFileSystemManager()
        {
        }

        private static void QG_FileSystemManagerAccess(string conf, string callbackId)
        {
        }

        private static string QG_FileSystemManagerAccessSync(string path)
        {
            return null;
        }

        private static void QG_FileSystemManagerCopyFile(string conf, string callbackId)
        {
        }

        private static string QG_FileSystemManagerCopyFileSync(string srcPath, string destPath)
        {
            return null;
        }

        private static void QG_FileSystemManagerMkdir(string conf, string callbackId)
        {
        }

        private static string QG_FileSystemManagerMkdirSync(string dirPath, bool recursive)
        {
            return null;
        }

        private static void QG_FileSystemManagerRmdir(string conf, string callbackId)
        {
        }

        private static string QG_FileSystemManagerRmdirSync(string dirPath, bool recursive)
        {
            return null;
        }

        private static void QG_FileSystemManagerReaddir(string conf, string callbackId)
        {
        }

        private static string QG_FileSystemManagerReaddirSync(string dirPath)
        {
            return null;
        }

        private static void QG_FileSystemManagerReadFileString(string conf, string callbackId)
        {
        }

        private static void QG_FileSystemManagerReadFileBinary(string conf, string callbackId)
        {
        }

        private static string QG_FileSystemManagerReadFileStringSync(string filePath)
        {
            return null;
        }

        private static string QG_FileSystemManagerReadFileBinarySync(string callbackId, string filePath)
        {
            return null;
        }

        private static void QG_FileSystemManagerRename(string conf, string callbackId)
        {
        }

        private static string QG_FileSystemManagerRenameSync(string oldPath, string newPath)
        {
            return null;
        }

        private static void QG_FileSystemManagerStat(string conf, string callbackId)
        {
        }

        private static string QG_FileSystemManagerStatSync(string path, bool recursive)
        {
            return null;
        }

        private static void QG_FileSystemManagerUnlink(string conf, string callbackId)
        {
        }

        private static string QG_FileSystemManagerUnlinkSync(string path)
        {
            return null;
        }

        private static void QG_FileSystemManagerUnzip(string conf, string callbackId)
        {
        }

        private static void QG_FileSystemManagerWriteFileString(string conf, string callbackId)
        {
        }

        private static void QG_FileSystemManagerWriteFileBinary(string filePath, byte[] data, int dataLength, string callbackId)
        {
        }

        private static string QG_FileSystemManagerWriteFileStringSync(string filePath, string data)
        {
            return null;
        }

        private static string QG_FileSystemManagerWriteFileBinarySync(string filePath, byte[] data, int dataLength)
        {
            return null;
        }

        private static void QG_FileSystemManagerSaveFile(string conf, string callbackId)
        {
        }

        private static string QG_FileSystemManagerSaveFileSync(string tempFilePath, string filePath)
        {
            return null;
        }

        private static void QG_FileSystemManagerAppendFileString(string conf, string callbackId)
        {
        }

        private static void QG_FileSystemManagerAppendFileBinary(string filePath, byte[] data, int dataLength, string callbackId)
        {
        }

        private static string QG_FileSystemManagerAppendFileStringSync(string filePath, string data)
        {
            return null;
        }

        private static string QG_FileSystemManagerAppendFileBinarySync(string filePath, byte[] data, int dataLength)
        {
            return null;
        }

        private static void QG_FileSystemManagerGetFileInfo(string conf, string callbackId)
        {
        }

        private static void QG_FileSystemManagerRemoveSavedFile(string conf, string callbackId)
        {
        }

#endif
        private static FileSystemManager _instance;

        public static FileSystemManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new FileSystemManager();
                }

                return _instance;
            }
        }

        private FileSystemManager()
        {
            QG_GetFileSystemManager();
        }

        /// <summary>
        /// 判断文件/目录是否存在。
        /// [FileSystemManager.access(Object object)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-file-0000001084194212#section16835114744317)
        /// </summary>
        /// <param name="accessOption">参数</param>
        public void Access(AccessOption accessOption)
        {
            string callbackId =
                RTCallbackFail<AccessFailResult>.AddCallback(
                    accessOption.success,
                    accessOption.fail,
                    accessOption.complete
                );
            string conf = JsonUtility.ToJson(accessOption);
            QG_FileSystemManagerAccess(conf, callbackId);
        }
        
        /// <summary>
        /// 判断文件/目录是否存在（同步方法）。
        /// [FileSystemManager.accessSync(string path)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-file-0000001084194212#section598113562464)
        /// </summary>
        /// <param name="path">要判断是否存在的文件/目录路径。</param>
        /// <returns>返回结果。</returns>
        public FileSystemBaseResult AccessSync(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return new FileSystemBaseResult
                {
                    isSuccess = false,
                    errMsg = "path is null or empty"
                };
            }
            string res = QG_FileSystemManagerAccessSync(path);
            return JsonUtility.FromJson<FileSystemBaseResult>(res);
        }

        /// <summary>
        /// 复制文件。
        /// [FileSystemManager.copyFile(Object object)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-file-0000001084194212#section133093296490)
        /// </summary>
        /// <param name="copyFileOption">参数</param>
        public void CopyFile(CopyFileOption copyFileOption)
        {
            string callbackId =
                RTCallbackFail<CopyFileFailResult>.AddCallback(
                    copyFileOption.success,
                    copyFileOption.fail,
                    copyFileOption.complete
                );
            string conf = JsonUtility.ToJson(copyFileOption);
            QG_FileSystemManagerCopyFile(conf, callbackId);
        }

        /// <summary>
        /// 复制文件（同步方法）。
        /// [FileSystemManager.copyFileSync(string srcPath, string destPath)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-file-0000001084194212#section108355345414)
        /// </summary>
        /// <param name="srcPath">源文件路径，只可以是本地文件，如果非本地文件，需调用FileSystemManager.saveFile接口将文件保存到本地。</param>
        /// <param name="destPath">目标文件路径。</param>
        /// <returns>返回结果。</returns>
        public FileSystemBaseResult CopyFileSync(string srcPath, string destPath)
        {
            if (string.IsNullOrEmpty(srcPath))
            {
                return new FileSystemBaseResult
                {
                    isSuccess = false,
                    errMsg = "srcPath is null or empty"
                };
            }
            if (string.IsNullOrEmpty(destPath))
            {
                return new FileSystemBaseResult
                {
                    isSuccess = false,
                    errMsg = "destPath is null or empty"
                };
            }
            string res = QG_FileSystemManagerCopyFileSync(srcPath, destPath);
            return JsonUtility.FromJson<FileSystemBaseResult>(res);
        }

        /// <summary>
        /// 创建目录。
        /// [FileSystemManager.mkdir(Object object)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-file-0000001084194212#section138631329185712)
        /// </summary>
        /// <param name="mkdirOption">参数</param>
        public void Mkdir(MkdirOption mkdirOption)
        {
            string callbackId =
                RTCallbackFail<MkdirFailResult>.AddCallback(
                    mkdirOption.success,
                    mkdirOption.fail,
                    mkdirOption.complete
                );
            string conf = JsonUtility.ToJson(mkdirOption);
            QG_FileSystemManagerMkdir(conf, callbackId);
        }

        /// <summary>
        /// 创建目录（同步方法）。
        /// [FileSystemManager.mkdirSync(string dirPath, boolean recursive)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-file-0000001084194212#section5460152414011)
        /// </summary>
        /// <param name="dirPath">创建的目录路径。</param>
        /// <param name="recursive">是否在递归创建该目录的上级目录后再创建该目录。如果对应的上级目录已经存在，则不创建该上级目录。默认值为false。</param>
        /// <returns>返回结果。</returns>
        public FileSystemBaseResult MkdirSync(string dirPath, bool recursive = false)
        {
            if (string.IsNullOrEmpty(dirPath))
            {
                return new FileSystemBaseResult
                {
                    isSuccess = false,
                    errMsg = "dirPath is null or empty"
                };
            }
            string res = QG_FileSystemManagerMkdirSync(dirPath, recursive);
            return JsonUtility.FromJson<FileSystemBaseResult>(res);
        }

        /// <summary>
        /// 删除目录。
        /// [FileSystemManager.rmdir(Object object)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-file-0000001084194212#section24842533716)
        /// </summary>
        /// <param name="rmdirOption">参数</param>
        public void Rmdir(RmdirOption rmdirOption)
        {
            string callbackId =
                RTCallbackFail<RmdirFailResult>.AddCallback(
                    rmdirOption.success,
                    rmdirOption.fail,
                    rmdirOption.complete
                );
            string conf = JsonUtility.ToJson(rmdirOption);
            QG_FileSystemManagerRmdir(conf, callbackId);
        }

        /// <summary>
        /// 删除目录（同步方法）。
        /// [FileSystemManager.rmdirSync(Object object)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-file-0000001084194212#section10335199141115)
        /// </summary>
        /// <param name="dirPath">要删除的目录路径。</param>
        /// <param name="recursive">是否递归删除目录。如果为 true，则删除该目录和该目录下的所有子目录以及文件。默认值为false。</param>
        /// <returns>判断删除目录是否成功</returns>
        public FileSystemBaseResult RmdirSync(string dirPath, bool recursive = false)
        {
            if (string.IsNullOrEmpty(dirPath))
            {
                return new FileSystemBaseResult
                {
                    isSuccess = false,
                    errMsg = "dirPath is null or empty"
                };
            }
            string res = QG_FileSystemManagerRmdirSync(dirPath, recursive);
            return JsonUtility.FromJson<FileSystemBaseResult>(res);
        }

        /// <summary>
        /// 读取目录内文件列表。
        /// [FileSystemManager.readdir(Object object)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-file-0000001084194212#section5668155201515)
        /// </summary>
        /// <param name="readdirOption">参数</param>
        public void Readdir(ReaddirOption readdirOption)
        {
            string callbackId =
                RTCallback<ReaddirSuccessResult, ReaddirFailResult>.AddCallback(
                    readdirOption.success,
                    readdirOption.fail,
                    readdirOption.complete
                );
            string conf = JsonUtility.ToJson(readdirOption);
            QG_FileSystemManagerReaddir(conf, callbackId);
        }

        /// <summary>
        /// 读取目录内文件列表（同步方法）。
        /// [Array FileSystemManager.readdirSync(string dirPath)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-file-0000001084194212#section203828481815)
        /// </summary>
        /// <param name="dirPath">要读取的目录路径。</param>
        /// <returns>返回结果。</returns>
        public ReaddirSyncResult ReaddirSync(string dirPath)
        {
            if (string.IsNullOrEmpty(dirPath))
            {
                return new ReaddirSyncResult
                {
                    isSuccess = false,
                    errMsg = "dirPath is null or empty"
                };
            }
            string res = QG_FileSystemManagerReaddirSync(dirPath);
            return JsonUtility.FromJson<ReaddirSyncResult>(res);
        }

        /// <summary>
        /// 读取本地文件内容。
        /// [FileSystemManager.readFile(Object object)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-file-0000001084194212#section1364511142512)
        /// </summary>
        /// <param name="readFileStringOption">参数</param>
        public void ReadFile(ReadFileStringOption readFileStringOption)
        {
            string callbackId =
                RTCallback<ReadFileStringSuccessResult, ReadFileFailResult>.AddCallback(
                    readFileStringOption.success,
                    readFileStringOption.fail,
                    readFileStringOption.complete
                );
            string conf = JsonUtility.ToJson(readFileStringOption);
            QG_FileSystemManagerReadFileString(conf, callbackId);
        }

        /// <summary>
        /// 读取本地文件内容。
        /// [FileSystemManager.readFile(Object object)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-file-0000001084194212#section1364511142512)
        /// </summary>
        /// <param name="readFileBinaryOption">参数</param>
        public void ReadFile(ReadFileBinaryOption readFileBinaryOption)
        {
            string callbackId =
                FSReadFileCallback.AddCallback(
                    readFileBinaryOption.success,
                    readFileBinaryOption.fail,
                    readFileBinaryOption.complete
                );
            string conf = JsonUtility.ToJson(readFileBinaryOption);
            QG_FileSystemManagerReadFileBinary(conf, callbackId);
        }
        
        /// <summary>
        /// 读取本地文件内容（同步方法）。
        /// [FileSystemManager.readFileSync(string filePath, string encoding)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-file-0000001084194212#section159564163333)
        /// </summary>
        /// <param name="readFileStringSyncOption">参数</param>
        /// <returns>返回结果。</returns>
        public ReadFileStringSyncResult ReadFileSync(ReadFileStringSyncOption readFileStringSyncOption)
        {
            if (string.IsNullOrEmpty(readFileStringSyncOption.filePath))
            {
                return new ReadFileStringSyncResult
                {
                    isSuccess = false,
                    errMsg = "filePath is null or empty"
                };
            }
            string res = QG_FileSystemManagerReadFileStringSync(readFileStringSyncOption.filePath);
            return JsonUtility.FromJson<ReadFileStringSyncResult>(res);
        }

        /// <summary>
        /// 读取本地文件内容（同步方法）。
        /// [FileSystemManager.readFileSync(string filePath, string encoding)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-file-0000001084194212#section159564163333)
        /// </summary>
        /// <param name="readFileBinarySyncOption">参数</param>
        /// <returns>返回结果。</returns>
        public ReadFileBinarySyncResult ReadFileSync(ReadFileBinarySyncOption readFileBinarySyncOption)
        {
            if (string.IsNullOrEmpty(readFileBinarySyncOption.filePath))
            {
                return new ReadFileBinarySyncResult
                {
                    isSuccess = false,
                    errMsg = "filePath is null or empty"
                };
            }
            string callbackId = Guid.NewGuid().ToString();
            string res = QG_FileSystemManagerReadFileBinarySync(callbackId, readFileBinarySyncOption.filePath);
            ReadFileBinarySyncResult obj = new ReadFileBinarySyncResult();
            ReadFileBinarySyncJson readFileBinarySyncJson = JsonUtility.FromJson<ReadFileBinarySyncJson>(res);
            
            if (readFileBinarySyncJson.isSuccess)
            {
                obj.data = new byte[readFileBinarySyncJson.data];
                obj.isSuccess = readFileBinarySyncJson.isSuccess;
                if (readFileBinarySyncJson.data > 0)
                {
                    QG_CopyDataFromJs(callbackId, obj.data);
                }
            }
            else
            {
                obj.isSuccess = readFileBinarySyncJson.isSuccess;
                obj.errMsg = readFileBinarySyncJson.errMsg;
                obj.data = null;
            }
            return obj;
        }

        /// <summary>
        /// 重命名文件，可以把文件从oldPath移动到newPath。
        /// [FileSystemManager.rename(Object object)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-file-0000001084194212#section167117244351)
        /// </summary>
        /// <param name="renameOption">参数</param>
        public void Rename(RenameOption renameOption)
        {
            string callbackId =
                RTCallbackFail<RenameFailResult>.AddCallback(
                    renameOption.success,
                    renameOption.fail,
                    renameOption.complete
                );
            string conf = JsonUtility.ToJson(renameOption);
            QG_FileSystemManagerRename(conf, callbackId);
        }

        /// <summary>
        /// 重命名文件，可以把文件从oldPath移动到newPath（同步方法）。
        /// [FileSystemManager.renameSync(string oldPath, string newPath)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-file-0000001084194212#section11229128113711)
        /// </summary>
        /// <param name="oldPath">源文件路径，可以是普通文件或目录。</param>
        /// <param name="newPath">新文件路径。</param>
        /// <returns>返回结果。</returns>
        public FileSystemBaseResult RenameSync(string oldPath, string newPath)
        {
            if (string.IsNullOrEmpty(oldPath))
            {
                return new FileSystemBaseResult
                {
                    isSuccess = false,
                    errMsg = "oldPath is null or empty"
                };
            }
            if (string.IsNullOrEmpty(newPath))
            {
                return new FileSystemBaseResult
                {
                    isSuccess = false,
                    errMsg = "newPath is null or empty"
                };
            }
            string res = QG_FileSystemManagerRenameSync(oldPath, newPath);
            return JsonUtility.FromJson<FileSystemBaseResult>(res);
        }

        /// <summary>
        /// 获取文件stats对象。
        /// [FileSystemManager.stat(Object object)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-file-0000001084194212#section63881619204019)
        /// </summary>
        /// <param name="statOption">参数</param>
        public void Stat(StatFileStatsOption statOption)
        {
            string callbackId =
                RTCallback<StatSuccessFileStatsResult, StatFailResult>.AddCallback(
                    statOption.success,
                    statOption.fail,
                    statOption.complete
                );
            string conf = JsonUtility.ToJson(statOption);
            QG_FileSystemManagerStat(conf, callbackId);
        }

        /// <summary>
        /// 获取文件stats对象。
        /// [FileSystemManager.stat(Object object)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-file-0000001084194212#section63881619204019)
        /// </summary>
        /// <param name="statOption">参数</param>
        public void Stat(StatStatsOption statOption)
        {
            string callbackId =
                RTCallback<StatSuccessStatsResult, StatFailResult>.AddCallback(
                    statOption.success,
                    statOption.fail,
                    statOption.complete
                );
            string conf = JsonUtility.ToJson(statOption);
            QG_FileSystemManagerStat(conf, callbackId);
        }

        /// <summary>
        /// 获取文件stats对象（同步方法）。
        /// [Stats|Array FileSystemManager.statSync(string path, boolean recursive)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-file-0000001084194212#section118961049134412)
        /// </summary>
        /// <param name="statStatsSyncOption">参数。</param>
        /// <returns>返回结果。</returns>
        public StatStatsSyncResult StatSync(StatStatsSyncOption statStatsSyncOption)
        {
            if (string.IsNullOrEmpty(statStatsSyncOption.path))
            {
                return new StatStatsSyncResult
                {
                    isSuccess = false,
                    errMsg = "path is null or empty"
                };
            }
            string res = QG_FileSystemManagerStatSync(statStatsSyncOption.path, false);
            return JsonUtility.FromJson<StatStatsSyncResult>(res);
        }

        /// <summary>
        /// 获取文件stats对象（同步方法）。
        /// [Stats|Array FileSystemManager.statSync(string path, boolean recursive)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-file-0000001084194212#section118961049134412)
        /// </summary>
        /// <param name="statFileStatsSyncOption">文件/目录路径。</param>
        /// <returns>返回结果。</returns>
        public StatFileStatsSyncResult StatSync(StatFileStatsSyncOption statFileStatsSyncOption)
        {
            if (string.IsNullOrEmpty(statFileStatsSyncOption.path))
            {
                return new StatFileStatsSyncResult
                {
                    isSuccess = false,
                    errMsg = "path is null or empty"
                };
            }
            string res = QG_FileSystemManagerStatSync(statFileStatsSyncOption.path, true);
            return JsonUtility.FromJson<StatFileStatsSyncResult>(res);
        }

        /// <summary>
        /// 删除文件。
        /// [FileSystemManager.unlink(Object object)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-file-0000001084194212#section6341125634614)
        /// </summary>
        /// <param name="unlinkOption">参数</param>
        public void Unlink(UnlinkOption unlinkOption)
        {
            string callbackId =
                RTCallbackFail<UnlinkFailResult>.AddCallback(
                    unlinkOption.success,
                    unlinkOption.fail,
                    unlinkOption.complete
                );
            string conf = JsonUtility.ToJson(unlinkOption);
            QG_FileSystemManagerUnlink(conf, callbackId);
        }

        /// <summary>
        /// 删除文件（同步方法）。
        /// [FileSystemManager.unlinkSync(string path)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-file-0000001084194212#section18213143245617)
        /// </summary>
        /// <param name="path">要删除的文件路径。</param>
        /// <returns>返回结果。</returns>
        public FileSystemBaseResult UnlinkSync(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return new FileSystemBaseResult
                {
                    isSuccess = false,
                    errMsg = "path is null or empty"
                };
            }
            string res = QG_FileSystemManagerUnlinkSync(path);
            return JsonUtility.FromJson<FileSystemBaseResult>(res);
        }

        /// <summary>
        /// 解压缩文件。
        /// [FileSystemManager.unzip(Object object)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-file-0000001084194212#section10435456105819)
        /// </summary>
        /// <param name="unzipOption">参数</param>
        public void Unzip(UnzipOption unzipOption)
        {
            string callbackId =
                RTCallbackFail<UnzipFailResult>.AddCallback(
                    unzipOption.success,
                    unzipOption.fail,
                    unzipOption.complete
                );
            string conf = JsonUtility.ToJson(unzipOption);
            QG_FileSystemManagerUnzip(conf, callbackId);
        }

        /// <summary>
        /// 写文件，写入内容将覆盖原有内容。
        /// [FileSystemManager.writeFile(Object object)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-file-0000001084194212#section1485820518210)
        /// </summary>
        /// <param name="writeFileOption">参数</param>
        public void WriteFile(WriteFileStringOption writeFileOption)
        {
            string callbackId =
                RTCallbackFail<WriteFileFailResult>.AddCallback(
                    writeFileOption.success,
                    writeFileOption.fail,
                    writeFileOption.complete
                );
            string conf = JsonUtility.ToJson(writeFileOption);
            QG_FileSystemManagerWriteFileString(conf, callbackId);
        }

        /// <summary>
        /// 写文件，写入内容将覆盖原有内容。
        /// [FileSystemManager.writeFile(Object object)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-file-0000001084194212#section1485820518210)
        /// </summary>
        /// <param name="writeFileOption">参数</param>
        public void WriteFile(WriteFileBinaryOption writeFileOption)
        {
            if (writeFileOption.data == null)
            {
                return;
            }
            string callbackId =
                RTCallbackFail<WriteFileFailResult>.AddCallback(
                    writeFileOption.success,
                    writeFileOption.fail,
                    writeFileOption.complete
                );
            QG_FileSystemManagerWriteFileBinary(writeFileOption.filePath, writeFileOption.data, writeFileOption.data.Length, callbackId);
        }

        /// <summary>
        /// 写文件（同步方法）。
        /// [FileSystemManager.writeFileSync(string filePath, string|ArrayBuffer data, string encoding)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-file-0000001084194212#section16609015764)
        /// </summary>
        /// <param name="writeFileStringSyncOption">参数。</param>
        /// <returns>返回结果。</returns>
        public FileSystemBaseResult WriteFileSync(WriteFileStringSyncOption writeFileStringSyncOption)
        {
            if (string.IsNullOrEmpty(writeFileStringSyncOption.filePath))
            {
                return new FileSystemBaseResult
                {
                    isSuccess = false,
                    errMsg = "filePath is null or empty"
                };
            }
            if (string.IsNullOrEmpty(writeFileStringSyncOption.data))
            {
                return new FileSystemBaseResult
                {
                    isSuccess = false,
                    errMsg = "data is null or empty"
                };
            }
            string res = QG_FileSystemManagerWriteFileStringSync(writeFileStringSyncOption.filePath, writeFileStringSyncOption.data);
            return JsonUtility.FromJson<FileSystemBaseResult>(res);
        }

        /// <summary>
        /// 写文件（同步方法）。
        /// [FileSystemManager.writeFileSync(string filePath, string|ArrayBuffer data, string encoding)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-file-0000001084194212#section16609015764)
        /// </summary>
        /// <param name="writeFileBinarySyncOption">参数。</param>
        /// <returns>返回结果。</returns>
        public FileSystemBaseResult WriteFileSync(WriteFileBinarySyncOption writeFileBinarySyncOption)
        {
            if (string.IsNullOrEmpty(writeFileBinarySyncOption.filePath))
            {
                return new FileSystemBaseResult
                {
                    isSuccess = false,
                    errMsg = "filePath is null or empty"
                };
            }
            if (writeFileBinarySyncOption.data == null)
            {
                return new FileSystemBaseResult
                {
                    isSuccess = false,
                    errMsg = "data is null or empty"
                };
            }
            string res = QG_FileSystemManagerWriteFileBinarySync(writeFileBinarySyncOption.filePath, writeFileBinarySyncOption.data, writeFileBinarySyncOption.data.Length);
            return JsonUtility.FromJson<FileSystemBaseResult>(res);
        }

        /// <summary>
        /// 保存临时文件到本地。此接口会移动临时文件，因此调用成功后，tempFilePath将不可用。
        /// [FileSystemManager.saveFile(Object object)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-file-0000001084194212#section177563353918)
        /// </summary>
        /// <param name="saveFileOption">参数</param>
        public void SaveFile(SaveFileOption saveFileOption)
        {
            string callbackId =
                RTCallback<SaveFileSuccessResult, SaveFileFailResult>.AddCallback(
                    saveFileOption.success,
                    saveFileOption.fail,
                    saveFileOption.complete
                );
            string conf = JsonUtility.ToJson(saveFileOption);
            QG_FileSystemManagerSaveFile(conf, callbackId);
        }

        /// <summary>
        /// 保存临时文件到本地（同步方法）。此接口会移动临时文件，因此调用成功后，tempFilePath将不可用。
        /// [FileSystemManager.saveFileSync(string tempFilePath, string filePath)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-file-0000001084194212#section19158114731211)
        /// </summary>
        /// <param name="tempFilePath">临时存储文件路径。</param>
        /// <param name="filePath">要存储的文件路径。</param>
        /// <returns>返回结果。</returns>
        public SaveFileSyncResult SaveFileSync(string tempFilePath, string filePath)
        {
            if (string.IsNullOrEmpty(tempFilePath))
            {
                return new SaveFileSyncResult
                {
                    isSuccess = false,
                    errMsg = "tempFilePath is null or empty"
                };
            }
            if (string.IsNullOrEmpty(filePath))
            {
                return new SaveFileSyncResult
                {
                    isSuccess = false,
                    errMsg = "filePath is null or empty"
                };
            }
            string res = QG_FileSystemManagerSaveFileSync(tempFilePath, filePath);
            return JsonUtility.FromJson<SaveFileSyncResult>(res);
        }

        /// <summary>
        /// 在文件结尾追加内容。
        /// [FileSystemManager.appendFile(Object object)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-file-0000001084194212#section5810142314160)
        /// </summary>
        /// <param name="appendFileStringOption">参数</param>
        public void AppendFile(AppendFileStringOption appendFileStringOption)
        {
            string callbackId =
                RTCallbackFail<AppendFileFailResult>.AddCallback(
                    appendFileStringOption.success,
                    appendFileStringOption.fail,
                    appendFileStringOption.complete
                );
            string conf = JsonUtility.ToJson(appendFileStringOption);
            QG_FileSystemManagerAppendFileString(conf, callbackId);
        }

        /// <summary>
        /// 在文件结尾追加内容。
        /// [FileSystemManager.appendFile(Object object)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-file-0000001084194212#section5810142314160)
        /// </summary>
        /// <param name="appendFileStringOption">参数</param>
        public void AppendFile(AppendFileBinaryOption appendFileStringOption)
        {
            if (appendFileStringOption.data == null)
            {
                return;
            }
            string callbackId =
                RTCallbackFail<AppendFileFailResult>.AddCallback(
                    appendFileStringOption.success,
                    appendFileStringOption.fail,
                    appendFileStringOption.complete
                );
            QG_FileSystemManagerAppendFileBinary(appendFileStringOption.filePath, appendFileStringOption.data, appendFileStringOption.data.Length, callbackId);
        }

        /// <summary>
        /// FileSystemManager.appendFile的同步版本。
        /// [FileSystemManager.appendFileSync(Object object)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-file-0000001084194212#section19450217225)
        /// </summary>
        /// <param name="appendFileStringSyncOption">参数。</param>
        /// <returns>返回结果。</returns>
        public FileSystemBaseResult AppendFileSync(AppendFileStringSyncOption appendFileStringSyncOption)
        {
            if (string.IsNullOrEmpty(appendFileStringSyncOption.filePath))
            {
                return new FileSystemBaseResult
                {
                    isSuccess = false,
                    errMsg = "filePath is null or empty"
                };
            }
            if (string.IsNullOrEmpty(appendFileStringSyncOption.data))
            {
                return new FileSystemBaseResult
                {
                    isSuccess = false,
                    errMsg = "data is null or empty"
                };
            }
            string res = QG_FileSystemManagerAppendFileStringSync(appendFileStringSyncOption.filePath, appendFileStringSyncOption.data);
            return JsonUtility.FromJson<FileSystemBaseResult>(res);
        }

        /// <summary>
        /// FileSystemManager.appendFile的同步版本。
        /// [FileSystemManager.appendFileSync(Object object)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-file-0000001084194212#section19450217225)
        /// </summary>
        /// <param name="appendFileBinarySyncOption">参数。</param>
        /// <returns>返回结果。</returns>
        public FileSystemBaseResult AppendFileSync(AppendFileBinarySyncOption appendFileBinarySyncOption)
        {
            if (string.IsNullOrEmpty(appendFileBinarySyncOption.filePath))
            {
                return new FileSystemBaseResult
                {
                    isSuccess = false,
                    errMsg = "filePath is null or empty"
                };
            }
            if (appendFileBinarySyncOption.data == null)
            {
                return new FileSystemBaseResult
                {
                    isSuccess = false,
                    errMsg = "data is null or empty"
                };
            }
            string res = QG_FileSystemManagerAppendFileBinarySync(appendFileBinarySyncOption.filePath, appendFileBinarySyncOption.data, appendFileBinarySyncOption.data.Length);
            return JsonUtility.FromJson<FileSystemBaseResult>(res);
        }

        /// <summary>
        /// 获取本地临时文件或本地用户文件的文件信息。
        /// [FileSystemManager.getFileInfo(Object object)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-file-0000001084194212#section1560164214256)
        /// </summary>
        /// <param name="getFileInfoOption">参数</param>
        public void GetFileInfo(GetFileInfoOption getFileInfoOption)
        {
            string callbackId =
                RTCallback<GetFileInfoSuccessResult, GetFileInfoFailResult>.AddCallback(
                    getFileInfoOption.success,
                    getFileInfoOption.fail,
                    getFileInfoOption.complete
                );
            string conf = JsonUtility.ToJson(getFileInfoOption);
            QG_FileSystemManagerGetFileInfo(conf, callbackId);
        }

        /// <summary>
        /// 删除该快游戏下已保存的本地缓存文件。
        /// [FileSystemManager.removeSavedFile(Object object)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-file-0000001084194212#section1609130102719)
        /// </summary>
        /// <param name="removeSavedFileOption">参数</param>
        public void RemoveSavedFile(RemoveSavedFileOption removeSavedFileOption)
        {
            string callbackId =
                RTCallbackFail<RemoveSavedFileFailResult>.AddCallback(
                    removeSavedFileOption.success,
                    removeSavedFileOption.fail,
                    removeSavedFileOption.complete
                );
            string conf = JsonUtility.ToJson(removeSavedFileOption);
            QG_FileSystemManagerRemoveSavedFile(conf, callbackId);
        }

        private class ReadFileSyncJson
        {
            public string errMsg = null;
            public bool isSuccess = false;
        }

        private class ReadFileBinarySyncJson : ReadFileSyncJson
        {
            public int data = 0;
        }
    }
}