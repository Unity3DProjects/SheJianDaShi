using System;
using System.IO;

public static class FileExtensions
{
    /// <summary>
    /// 重命名文件
    /// </summary>
    /// <param name="fileInfo">文件信息</param>
    /// <param name="newFileName">新文件名（不含路径）</param>
    /// <param name="overwrite">是否覆盖已存在的目标文件</param>
    /// <returns>是否重命名成功</returns>
    public static bool Rename(this FileInfo fileInfo, string newFileName, bool overwrite = false)
    {
        if (fileInfo == null)
            throw new ArgumentNullException(nameof(fileInfo));

        if (string.IsNullOrWhiteSpace(newFileName))
            throw new ArgumentException("文件名不能为空", nameof(newFileName));

        try
        {
            if (!fileInfo.Exists)
            {
                Console.WriteLine($"文件不存在: {fileInfo.FullName}");
                return false;
            }

            string newFilePath = Path.Combine(fileInfo.DirectoryName, newFileName);

            if (File.Exists(newFilePath))
            {
                if (!overwrite)
                {
                    Console.WriteLine($"目标文件已存在: {newFilePath}");
                    return false;
                }

                File.Delete(newFilePath);
            }

            File.Move(fileInfo.FullName, newFilePath);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"重命名文件时出错: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// 通过文件路径重命名文件
    /// </summary>
    public static bool RenameFile(this string filePath, string newFileName, bool overwrite = false)
    {
        return new FileInfo(filePath).Rename(newFileName, overwrite);
    }

    /// <summary>
    /// 重命名文件
    /// </summary>
    /// <param name="fileInfo">文件信息</param>
    /// <param name="newFilePath">新文件路径</param>
    /// <param name="overwrite">是否覆盖已存在的目标文件</param>
    /// <returns>是否重命名成功</returns>
    public static bool MoveFileTo(this FileInfo fileInfo, string newFilePath, bool overwrite = false)
    {
        if (fileInfo == null)
            throw new ArgumentNullException(nameof(fileInfo));

        if (string.IsNullOrWhiteSpace(newFilePath))
            throw new ArgumentException("文件名不能为空", nameof(newFilePath));

        try
        {
            if (!fileInfo.Exists)
            {
                Console.WriteLine($"文件不存在: {fileInfo.FullName}");
                return false;
            }

            if (File.Exists(newFilePath))
            {
                if (!overwrite)
                {
                    Console.WriteLine($"目标文件已存在: {newFilePath}");
                    return false;
                }

                File.Delete(newFilePath);
            }

            File.Move(fileInfo.FullName, newFilePath);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"重命名文件时出错: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// 通过文件路径重命名文件
    /// </summary>
    public static bool MoveFileTo(this string filePath, string newFilePath, bool overwrite = false)
    {
        return new FileInfo(filePath).MoveFileTo(newFilePath, overwrite);
    }
}