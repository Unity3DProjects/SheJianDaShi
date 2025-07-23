using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Scripting;


namespace QGMiniGame
{
	public class AccessParam : QGBaseActionOption<QGBaseResponse>
	{
		public string path;
	}

	public class CopyFileParam : QGBaseActionOption<QGBaseResponse>
	{
		public string srcPath;

		public string destPath;
	}

	[Preserve]
	public class EntriesResult : Dictionary<string, ZipFileItem>
	{
	}

	[Preserve]
	public class EntryItem
	{
		public string path;

		public string encoding;

		public double? length;

		public double? position;
	}

	[Preserve]
	public class FileError
	{
		public string errMsg;

		public double errCode;
	}

	[Preserve]
	public class FileItem
	{
		public long createTime;

		public string filePath;

		public double size;
	}

	[Preserve]
	public class GetFileInfoOption : QGBaseActionOption<GetFileInfoSuccessCallbackResult, FileError>
	{
		public string filePath;
	}

	[Preserve]
	public class GetFileInfoSuccessCallbackResult
	{
		public double size;

		public string errMsg;
	}

	[Preserve]
	public class GetSavedFileListOption : QGBaseActionOption<GetSavedFileListSuccessCallbackResult, FileError>
	{
	}

	[Preserve]
	public class GetSavedFileListSuccessCallbackResult
	{
		public FileItem[] fileList;

		public string errMsg;
	}

	public class MkdirParam : QGBaseActionOption<QGBaseResponse>
	{
		public string dirPath;

		public bool recursive = false;
	}

	public class QGReadFileResponse : QGBaseResponse
	{
		public byte[] binData;

		public int? arrayBufferLength;

		public string stringData;
	}

	public class QGStat
	{
		public string path;

		public QGStatInfo stats;
	}

	public class QGStatInfo
	{
		public int mode;

		public int size;

		public long lastAccessedTime;

		public long lastModifiedTime;
	}

	public class QGStatOption : QGBaseActionOption<QGStatResponse>
	{
		public string path;

		public bool recursive = true;
	}

	public class QGStatResponse : QGBaseResponse
	{
		public List<QGStat> stats;

		public QGStatInfo one_stat;
	}

	[Preserve]
	public class ReadCompressedFileOption : QGBaseActionOption<ReadCompressedFileSuccessCallbackResult, FileError>
	{
		public string compressionAlgorithm;

		public string filePath;
	}

	[Preserve]
	public class ReadCompressedFileSuccessCallbackResult
	{
		public byte[] data;

		public int? arrayBufferLength;

		public string errMsg;
	}

	[Preserve]
	public class ReadCompressedFileSyncOption
	{
		public string compressionAlgorithm;

		public string filePath;
	}

	[Preserve]
	public class ReaddirOption : QGBaseActionOption<ReaddirSuccessCallbackResult, FileError>
	{
		public string dirPath;
	}

	[Preserve]
	public class ReaddirSuccessCallbackResult
	{
		public string[] files;

		public string errMsg;
	}

	public class ReadFileParam : QGBaseActionOption<QGReadFileResponse, FileError>
	{
		public string filePath;

		public string encoding;

		public long? position;

		public long? length;
	}

	[Preserve]
	public class ReadZipEntryOption : QGBaseActionOption<ReadZipEntrySuccessCallbackResult, FileError>
	{
		public EntryItem[] entries;

		public string filePath;

		public string encoding;
	}

	[Preserve]
	public class ReadZipEntryOptionString : QGBaseActionOption<ReadZipEntrySuccessCallbackResult, FileError>
	{
		public string entries = "all";

		public string filePath;

		public string encoding;
	}

	[Preserve]
	public class ReadZipEntrySuccessCallbackResult
	{
		public EntriesResult entries;

		public string errMsg;
	}

	[Preserve]
	public class RemoveSavedFileOption : QGBaseActionOption<FileError>
	{
		public string filePath;
	}

	[Preserve]
	public class RenameOption : QGBaseActionOption<FileError>
	{
		public string newPath;

		public string oldPath;
	}

	public class RmdirParam : QGBaseActionOption<QGBaseResponse>
	{
		public string dirPath;

		public bool recursive = false;
	}

	[Preserve]
	public class SaveFileOption : QGBaseActionOption<SaveFileSuccessCallbackResult, FileError>
	{
		public string tempFilePath;

		public string filePath;
	}

	[Preserve]
	public class SaveFileSuccessCallbackResult
	{
		public string savedFilePath;

		public string errMsg;
	}

	public class UnlinkParam : QGBaseActionOption<QGBaseResponse>
	{
		public string filePath;
	}

	[Preserve]
	public class UnzipOption : QGBaseActionOption<FileError>
	{
		public string targetPath;

		public string zipFilePath;
	}

	public class WriteFileParam : QGBaseActionOption<QGBaseResponse>
	{
		public string filePath;

		public byte[] data;

		public string encoding;
	}

	public class WriteFileStringParam : QGBaseActionOption<QGBaseResponse>
	{
		public string filePath;

		public string data;

		public string encoding;
	}

	[Preserve]
	public class ZipFileItem
	{
		public string data;

		public string errMsg;
	}

}
