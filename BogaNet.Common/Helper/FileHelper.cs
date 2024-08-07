﻿using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text;
using BogaNet.Util;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;
using BogaNet.Extension;

namespace BogaNet.Helper;

/// <summary>
/// Various helper functions for filesystem operations.
/// </summary>
public abstract class FileHelper
{
   #region Variables

   private static readonly ILogger<FileHelper> _logger = GlobalLogging.CreateLogger<FileHelper>();

   private static char[] _invalidFilenameChars = [];
   private static char[] _invalidPathChars = [];

   #endregion

   #region Properties

   /// <summary>
   /// Returns a temporary file.
   /// </summary>
   /// <returns>Temporary file</returns>
   public static string TempFile => Path.GetTempFileName();

   /// <summary>
   /// Returns the temporary directory path.
   /// </summary>
   /// <returns>Temporary directory path</returns>
   public static string TempPath => ValidatePath(Path.GetTempPath());

   /// <summary>
   /// Returns a temporary directory.
   /// </summary>
   /// <returns>Temporary directory</returns>
   public static string TempDirectory
   {
      get
      {
         string name = ValidatePath($"{TempPath}{ShortUID.NewShortUID()}");

         CreateDirectory(name);

         return name;
      }
   }

   /// <summary>
   /// Returns the current directory.
   /// </summary>
   /// <returns>Current directory</returns>
   public static string CurrentDirectory => ValidatePath(Environment.CurrentDirectory);

   #endregion

   #region Static block

   static FileHelper()
   {
      initialize();
   }

   private static void initialize()
   {
      HashSet<char> invalidFilenameChars =
      [
         ..Path.GetInvalidFileNameChars(),
         Path.DirectorySeparatorChar,
         Path.AltDirectorySeparatorChar,
         '<',
         '>',
         ':',
         '"',
         '/',
         '\\',
         '|',
         '?',
         '*'
      ];

      _invalidFilenameChars = invalidFilenameChars.ToArray();

      HashSet<char> invalidPathChars =
      [
         ..Path.GetInvalidPathChars(),
         '<',
         '>',
         '"',
         '|',
         '?',
         '*'
      ];

      _invalidPathChars = invalidPathChars.ToArray();
   }

   #endregion

   #region Public methods

   /// <summary>
   /// Checks if the given path is from a Unix-device.
   /// </summary>
   /// <param name="path">Path to check</param>
   /// <returns>True if the given path is from a Unix-device</returns>
   public static bool IsUnixPath(string? path) //NUnit
   {
      return !string.IsNullOrEmpty(path) && path.StartsWith('/');
   }

   /// <summary>
   /// Checks if the given path is from a Windows-device.
   /// </summary>
   /// <param name="path">Path to check</param>
   /// <returns>True if the given path is from a Windows-device</returns>
   public static bool IsWindowsPath(string? path) //NUnit
   {
      return !string.IsNullOrEmpty(path) && Constants.REGEX_DRIVE_LETTERS.IsMatch(path);
   }

   /// <summary>
   /// Checks if the given path is UNC.
   /// </summary>
   /// <param name="path">Path to check</param>
   /// <returns>True if the given path is UNC</returns>
   public static bool IsUNCPath(string? path) //NUnit
   {
      return !string.IsNullOrEmpty(path) && path.BNStartsWith(@"\\");
   }

   /// <summary>
   /// Checks if the given path is an URL.
   /// </summary>
   /// <param name="path">Path to check</param>
   /// <returns>True if the given path is an URL</returns>
   public static bool IsURL(string? path) //NUnit
   {
      return NetworkHelper.IsURL(path);
   }

   /// <summary>
   /// Validates a given path and add missing slash.
   /// </summary>
   /// <param name="path">Path to validate</param>
   /// <param name="addEndDelimiter">Add delimiter at the end of the path (optional, default: true)</param>
   /// <param name="preserveFile">Preserves a given file in the path (optional, default: true)</param>
   /// <param name="removeInvalidChars">Removes invalid characters in the path name (optional default: true)</param>
   /// <returns>Valid path (if possible)</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static string ValidatePath(string path, bool addEndDelimiter = true, bool preserveFile = true, bool removeInvalidChars = true) //NUnit
   {
      ArgumentNullException.ThrowIfNullOrEmpty(path);

      if (IsURL(path))
      {
         if (addEndDelimiter && !path.BNEndsWith(Constants.PATH_DELIMITER_UNIX))
            path += Constants.PATH_DELIMITER_UNIX;

         return path;
      }

      if (path.BNStartsWith("~"))
         path = replaceTilde(path);

      string pathTemp = !preserveFile && ExistsFile(path.Trim()) ? GetDirectoryName(path.Trim()) : path.Trim();

      string result;

      if (IsWindowsPath(pathTemp) || IsUNCPath(pathTemp))
      {
         //if (!isUNCPath(pathTemp))
         result = pathTemp.Replace('/', '\\');

         if (addEndDelimiter && !result.BNEndsWith(Constants.PATH_DELIMITER_WINDOWS))
            result += Constants.PATH_DELIMITER_WINDOWS;
      }
      else
      {
         result = pathTemp.Replace('\\', '/');

         if (addEndDelimiter && !result.BNEndsWith(Constants.PATH_DELIMITER_UNIX))
            result += Constants.PATH_DELIMITER_UNIX;
      }

      if (removeInvalidChars && HasPathInvalidChars(result))
         result = string.Join(string.Empty, result.Split(_invalidPathChars));

      return result;
   }

   /// <summary>
   /// Validates a given file.
   /// </summary>
   /// <param name="path">File to validate</param>
   /// <param name="removeInvalidChars">Removes invalid characters in the file name (optional, default: true)</param>
   /// <returns>Valid file path</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static string ValidateFile(string path, bool removeInvalidChars = true) //NUnit
   {
      ArgumentNullException.ThrowIfNullOrEmpty(path);

      if (IsURL(path))
         return path;

      bool isWin = IsWindowsPath(path);
      bool isUNC = IsUNCPath(path);

      string result = ValidatePath(path, false, true, removeInvalidChars);

      if (result.BNEndsWith(Constants.PATH_DELIMITER_WINDOWS) ||
          result.BNEndsWith(Constants.PATH_DELIMITER_UNIX))
         result = result.Substring(0, result.Length - 1);

      string fileName;
      if (isWin || isUNC)
      {
         fileName = result.Substring(result.BNLastIndexOf(Constants.PATH_DELIMITER_WINDOWS) + 1);
      }
      else
      {
         fileName = result.Substring(result.BNLastIndexOf(Constants.PATH_DELIMITER_UNIX) + 1);
      }

      string newName = fileName;

      if (removeInvalidChars && HasFileInvalidChars(fileName))
         newName = string.Join(string.Empty, fileName.Split(_invalidFilenameChars)); //.Replace(BaseConstants.PATH_DELIMITER_WINDOWS, string.Empty).Replace(BaseConstants.PATH_DELIMITER_UNIX, string.Empty);

      if ((isWin || isUNC) && newName.EndsWith('.')) //file under Windows/UNC can not end with .
         newName = newName.Substring(0, fileName.Length - 1);

      return result.Substring(0, result.Length - fileName.Length) + newName; //this is correct!
   }

   /// <summary>
   /// Checks a given path for invalid characters.
   /// </summary>
   /// <param name="path">Path to check for invalid characters</param>
   /// <param name="ignoreNullOrEmpty">If set to true, return false for null or empty paths (optional, default: true)</param>
   /// <returns>Returns true if the path contains invalid chars, otherwise it's false</returns>
   public static bool HasPathInvalidChars(string? path, bool ignoreNullOrEmpty = true) //NUnit
   {
      if (string.IsNullOrEmpty(path))
         return !ignoreNullOrEmpty;

      return path.IndexOfAny(_invalidPathChars) >= 0;
   }

   /// <summary>
   /// Checks a given file for invalid characters.
   /// </summary>
   /// <param name="file">File to check for invalid characters</param>
   /// <param name="ignoreNullOrEmpty">If set to true, return false for null or empty paths (optional, default: true)</param>
   /// <returns>Returns true if the file contains invalid chars, otherwise it's false</returns>
   public static bool HasFileInvalidChars(string? file, bool ignoreNullOrEmpty = true) //NUnit
   {
      if (file == null || string.IsNullOrEmpty(file))
         return !ignoreNullOrEmpty;

      return GetFileName(file, false).IndexOfAny(_invalidFilenameChars) >= 0 || file.EndsWith('.');
   }

   /// <summary>
   /// Find files inside a path.
   /// NOTE: for a non-blocking version, consider calling this method from a separate thread
   /// </summary>
   /// <param name="path">Path to find the files</param>
   /// <param name="isRecursive">Recursive search (optional, default: false)</param>
   /// <param name="filenames">Array of file names for the file search, e.g. "Image.png" (optional)</param>
   /// <returns>Returns array of the found files inside the path (alphabetically ordered). Zero length array when an error occured</returns>
   /// <exception cref="Exception"></exception>
   public static string[] GetFilesForName(string path, bool isRecursive = false, params string[]? filenames) //NUnit
   {
      ArgumentNullException.ThrowIfNullOrEmpty(path);

      try
      {
         string validPath = ValidatePath(path);

         if (ExistsDirectory(validPath))
         {
            if (filenames == null || filenames.Length == 0 || filenames.Any(extension => extension.Equals("*") || extension.Equals("*.*")))
            {
               return Directory.EnumerateFiles(validPath, "*", isRecursive
                  ? SearchOption.AllDirectories
                  : SearchOption.TopDirectoryOnly).ToArray();
            }

            List<string> files = [];

            foreach (string filename in filenames)
            {
               files.AddRange(Directory.EnumerateFiles(validPath, filename.BNStartsWith("*.") ? filename : $"*{filename}*", isRecursive
                  ? SearchOption.AllDirectories
                  : SearchOption.TopDirectoryOnly));
            }

            return files.OrderBy(q => q).ToArray();
         }
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"Could not scan the path '{path}' for files");
         throw;
      }

      return [];
   }

   /// <summary>
   /// Find files inside a path.
   /// NOTE: for a non-blocking version, consider calling this method from a separate thread
   /// </summary>
   /// <param name="path">Path to find the files</param>
   /// <param name="isRecursive">Recursive search (optional, default: false)</param>
   /// <param name="extensions">Extensions for the file search, e.g. "png" (optional)</param>
   /// <returns>Returns array of the found files inside the path (alphabetically ordered). Zero length array when an error occured</returns>
   /// <exception cref="Exception"></exception>
   public static string[] GetFiles(string path, bool isRecursive = false, params string[]? extensions) //NUnit
   {
      if (extensions?.Length > 0)
      {
         string[] wildcardExt = new string[extensions.Length];

         for (int ii = 0; ii < extensions.Length; ii++)
         {
            wildcardExt[ii] = $"*.{extensions[ii]}";
         }

         return GetFilesForName(path, isRecursive, wildcardExt);
      }

      return GetFilesForName(path, isRecursive, extensions);
   }

   /// <summary>
   /// Find directories inside a path.
   /// NOTE: for a non-blocking version, consider calling this method from a separate thread
   /// </summary>
   /// <param name="path">Path to find the directories</param>
   /// <param name="isRecursive">Recursive search (optional, default: false)</param>
   /// <returns>Returns array of the found directories inside the path. Zero length array when an error occured</returns>
   /// <exception cref="Exception"></exception>
   public static string[] GetDirectories(string path, bool isRecursive = false) //NUnit
   {
      ArgumentNullException.ThrowIfNullOrEmpty(path);

      try
      {
         string validPath = ValidatePath(path);

         if (ExistsDirectory(validPath))
         {
            return Directory.EnumerateDirectories(validPath, "*", isRecursive
               ? SearchOption.AllDirectories
               : SearchOption.TopDirectoryOnly).ToArray();
         }

         _logger.LogWarning($"Path does not exist: {path}");
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"Could not scan the path '{path}' for directorie");
         throw;
      }

      return [];
   }

   /// <summary>
   /// Find all logical drives.
   /// </summary>
   /// <returns>Returns array of the found drives. Zero length array when an error occured</returns>
   /// <exception cref="Exception"></exception>
   public static string[] GetDrives() //NUnit
   {
      try
      {
         return Directory.GetLogicalDrives();
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, "Could not scan for drives");
         throw;
      }
   }

   /// <summary>
   /// Gathers all information for all logical drives.
   /// </summary>
   /// <returns>Array with DriveInfo</returns>
   public static DriveInfo[] GetDriveInfo()
   {
      return DriveInfo.GetDrives();
   }

   /// <summary>
   /// Combines two paths together.
   /// </summary>
   /// <param name="path1">First path</param>
   /// <param name="path2">Second path</param>
   /// <returns>Combined path</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static string Combine(string path1, string path2)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(path1);
      ArgumentNullException.ThrowIfNullOrEmpty(path2);

      //path1 = ValidatePath(path1);
      //path2 = ValidatePath(path2);

      return Path.Combine(path1, path2);
   }

   /// <summary>
   /// Copy or move a file or directory.
   /// NOTE: for a non-blocking version, consider calling this method from a separate thread
   /// </summary>
   /// <param name="sourcePath">Source file/directory path</param>
   /// <param name="destPath">Destination file/directory path</param>
   /// <param name="move">Move file/directory instead of copy (optional, default: false)</param>
   /// <param name="moveSafe">Moves a file/directory in a safe, but slower way (optional, default: true)</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool Copy(string sourcePath, string destPath, bool move = false, bool moveSafe = true)
   {
      return IsFile(sourcePath) ? CopyFile(sourcePath, destPath, move, moveSafe) : CopyDirectory(sourcePath, destPath, move, moveSafe);
   }

   /// <summary>
   /// Copy or move a file.
   /// NOTE: for a non-blocking version, consider calling this method from a separate thread
   /// </summary>
   /// <param name="sourceFile">Source file path</param>
   /// <param name="destFile">Destination file path</param>
   /// <param name="move">Move file instead of copy (optional, default: false)</param>
   /// <param name="moveSafe">Moves a file in a safe, but slower way (optional, default: true)</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool CopyFile(string sourceFile, string destFile, bool move = false, bool moveSafe = true) //NUnit
   {
      ArgumentNullException.ThrowIfNullOrEmpty(sourceFile);
      ArgumentNullException.ThrowIfNullOrEmpty(destFile);

      bool success = false;

      try
      {
         sourceFile = ValidateFile(sourceFile);

         if (!ExistsFile(sourceFile))
         {
            _logger.LogWarning($"Source file does not exists: {sourceFile}");
         }
         else
         {
            string dest = ValidateFile(destFile);

            CreateDirectory(GetDirectoryName(dest));

            if (ExistsFile(dest))
            {
               _logger.LogInformation($"Overwrite destination file: {dest}");

               DeleteFile(dest);
            }

            if (move)
            {
               if (moveSafe)
               {
                  File.Copy(sourceFile, dest);
                  File.Delete(sourceFile);
               }
               else
               {
                  File.Move(sourceFile, dest);
               }
            }
            else
            {
               File.Copy(sourceFile, dest);
            }

            success = true;
         }
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"Could not {(move ? "move" : "copy")} file '{sourceFile}' to '{destFile}'");
         throw;
      }

      return success;
   }

   /// <summary>
   /// Copy or move a directory.
   /// NOTE: for a non-blocking version, consider calling this method from a separate thread
   /// </summary>
   /// <param name="sourceDir">Source directory path</param>
   /// <param name="destDir">Destination directory path</param>
   /// <param name="move">Move directory instead of copy (optional, default: false)</param>
   /// <param name="moveSafe">Moves a directory in a safe, but slower way (optional, default: true)</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool CopyDirectory(string sourceDir, string destDir, bool move = false, bool moveSafe = true) //NUnit
   {
      ArgumentNullException.ThrowIfNullOrEmpty(sourceDir);
      ArgumentNullException.ThrowIfNullOrEmpty(destDir);

      bool success = false;

      try
      {
         string src = ValidatePath(sourceDir);
         string dest = ValidatePath(destDir);

         if (!ExistsDirectory(src))
         {
            _logger.LogWarning($"Source directory does not exists: {src}");
         }
         else
         {
            if (ExistsDirectory(dest))
            {
               _logger.LogInformation($"Overwrite destination directory: {dest}");

               DeleteDirectory(dest);
            }

            if (move)
            {
               if (moveSafe)
               {
                  copyAll(new DirectoryInfo(src), new DirectoryInfo(dest));

                  DeleteDirectory(src);
               }
               else
               {
                  Directory.Move(src, dest); //Directory.Move sometimes fails, therefor the "moveSafe"-option is way better
               }
            }
            else
            {
               copyAll(new DirectoryInfo(src), new DirectoryInfo(dest));
            }

            success = true;
         }
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"Could not {(move ? "move" : "copy")} directory '{sourceDir}' to '{destDir}'");
         throw;
      }

      return success;
   }

   /// <summary>
   /// Move a file or directory.
   /// NOTE: for a non-blocking version, consider calling this method from a separate thread
   /// </summary>
   /// <param name="sourcePath">Source file/directory path</param>
   /// <param name="destPath">Destination file/directory path</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool Move(string sourcePath, string destPath)
   {
      return IsFile(sourcePath) ? MoveFile(sourcePath, destPath) : MoveDirectory(sourcePath, destPath);
   }

   /// <summary>
   /// Move a file.
   /// NOTE: for a non-blocking version, consider calling this method from a separate thread
   /// </summary>
   /// <param name="sourceFile">Source file path</param>
   /// <param name="destFile">Destination file path</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool MoveFile(string sourceFile, string destFile)
   {
      return CopyFile(sourceFile, destFile, true);
   }

   /// <summary>
   /// Move a directory.
   /// NOTE: for a non-blocking version, consider calling this method from a separate thread
   /// </summary>
   /// <param name="sourceDir">Source directory path</param>
   /// <param name="destDir">Destination directory path</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool MoveDirectory(string sourceDir, string destDir)
   {
      return CopyDirectory(sourceDir, destDir, true);
   }

   /// <summary>
   /// Renames a file or directory.
   /// </summary>
   /// <param name="path">Path to the file/directory</param>
   /// <param name="newName">New name for the file/directory</param>
   /// <returns>New path of the file/directory</returns>
   /// <exception cref="Exception"></exception>
   public static string Rename(string path, string newName)
   {
      return IsFile(path) ? RenameFile(path, newName) : RenameDirectory(path, newName);
   }

   /// <summary>
   /// Renames a file in a path.
   /// </summary>
   /// <param name="path">Path to the file</param>
   /// <param name="newName">New name for the file</param>
   /// <returns>New path of the file</returns>
   /// <exception cref="Exception"></exception>
   public static string RenameFile(string path, string newName) //NUnit
   {
      ArgumentNullException.ThrowIfNullOrEmpty(path);
      ArgumentNullException.ThrowIfNullOrEmpty(newName);

      try
      {
         string dir = GetDirectoryName(path, true);

         string newPath = Combine(dir, newName);
         File.Move(path, newPath);

         return newPath;
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"Could not rename file '{path}' to '{newName}'");
         throw;
      }
   }

   /// <summary>
   /// Renames a directory in a path.
   /// </summary>
   /// <param name="path">Path to the directory</param>
   /// <param name="newName">New name for the directory</param>
   /// <returns>New path of the directory</returns>
   /// <exception cref="Exception"></exception>
   public static string RenameDirectory(string path, string newName) //NUnit
   {
      ArgumentNullException.ThrowIfNullOrEmpty(path);
      ArgumentNullException.ThrowIfNullOrEmpty(newName);

      try
      {
         path = ValidatePath(path);

         DirectoryInfo di = new(path);
         DirectoryInfo? parent = di.Parent;

         if (parent != null)
         {
            string newPath = Combine(parent.FullName, newName);
            Directory.Move(path, newPath);

            return newPath;
         }
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"Could not rename directory '{path}' to '{newName}'");
         throw;
      }

      return path;
   }

   /// <summary>
   /// Delete a file or directory.
   /// </summary>
   /// <param name="path">Delete file/directory</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool Delete(string path)
   {
      return IsFile(path) ? DeleteFile(path) : DeleteDirectory(path);
   }

   /// <summary>
   /// Delete a file.
   /// </summary>
   /// <param name="file">File to delete</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool DeleteFile(string file) //NUnit
   {
      ArgumentNullException.ThrowIfNullOrEmpty(file);

      bool success = false;

      try
      {
         file = ValidateFile(file);

         if (!ExistsFile(file))
         {
            _logger.LogWarning($"File does not exists: {file}");
         }
         else
         {
            File.Delete(file);
            success = true;
         }
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"Could not delete file '{file}'");
         throw;
      }

      return success;
   }

   /// <summary>
   /// Delete a directory.
   /// NOTE: for a non-blocking version, consider calling this method from a separate thread
   /// </summary>
   /// <param name="dir">Directory to delete</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool DeleteDirectory(string dir) //NUnit
   {
      ArgumentNullException.ThrowIfNullOrEmpty(dir);

      bool success = false;

      try
      {
         dir = ValidatePath(dir);

         if (!ExistsDirectory(dir))
         {
            _logger.LogWarning($"Source directory does not exists: {dir}");
         }
         else
         {
            Directory.Delete(dir, true);
            success = true;
         }
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"Could not delete directory '{dir}'");
         throw;
      }

      return success;
   }

   /// <summary>
   /// Checks if a file or directory exists.
   /// </summary>
   /// <param name="path">Path to the file or directory</param>
   /// <returns>True if the file or directory exists</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static bool Exists(string path)
   {
      return IsFile(path) ? ExistsFile(path) : ExistsDirectory(path);
   }

   /// <summary>
   /// Checks if a file exists.
   /// </summary>
   /// <param name="path">Path to the file</param>
   /// <returns>True if the file exists</returns>
   public static bool ExistsFile(string path) //NUnit
   {
      ArgumentNullException.ThrowIfNull(path);

      path = replaceTilde(path);

      return File.Exists(path);
   }

   /// <summary>
   /// Checks if a directory exists.
   /// </summary>
   /// <param name="path">Path to the directory</param>
   /// <returns>True if the directory exists</returns>
   public static bool ExistsDirectory(string path) //NUnit
   {
      ArgumentNullException.ThrowIfNull(path);

      path = replaceTilde(path);

      return Directory.Exists(path);
   }

   /// <summary>
   /// Creates a file in a given path.
   /// </summary>
   /// <param name="path">Path for the file</param>
   /// <param name="fileName">New file</param>
   /// <exception cref="Exception"></exception>
   public static string CreateFile(string path, string fileName) //NUnit
   {
      ArgumentNullException.ThrowIfNullOrEmpty(path);
      ArgumentNullException.ThrowIfNullOrEmpty(fileName);

      try
      {
         path = ValidatePath(path);

         if (!ExistsDirectory(path))
            throw new Exception($"Path directory does not exists: {path}");

         string newPath = Combine(path, fileName);
         using (File.Create(newPath))
         {
         }

         return newPath;
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"Could not create file at '{path}' with name '{fileName}'");
         throw;
      }
   }

   /// <summary>
   /// Creates a directory in a given path.
   /// </summary>
   /// <param name="path">Path for the directory</param>
   /// <param name="folderName">New folder</param>
   /// <exception cref="Exception"></exception>
   public static string CreateDirectory(string path, string folderName) //NUnit
   {
      ArgumentNullException.ThrowIfNullOrEmpty(path);
      ArgumentNullException.ThrowIfNullOrEmpty(folderName);

      try
      {
         path = ValidatePath(path);

         if (!ExistsDirectory(path))
            throw new Exception($"Path directory does not exists: {path}");

         string newPath = Combine(path, folderName);
         Directory.CreateDirectory(newPath);
         return newPath;
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"Could not create directory at '{path}' with name '{folderName}'");
         throw;
      }
   }

   /// <summary>
   /// Creates a file.
   /// </summary>
   /// <param name="path">Path to the file to create</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool CreateFile(string path) //NUnit
   {
      ArgumentNullException.ThrowIfNullOrEmpty(path);

      bool success;

      try
      {
         path = ValidateFile(path);

         using (File.Create(path))
         {
         }

         success = true;
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"Could not create file '{path}'");
         throw;
      }

      return success;
   }

   /// <summary>
   /// Creates a directory.
   /// </summary>
   /// <param name="path">Path to the directory to create</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool CreateDirectory(string path) //NUnit
   {
      ArgumentNullException.ThrowIfNullOrEmpty(path);

      bool success;

      try
      {
         path = ValidatePath(path);

         Directory.CreateDirectory(path);
         success = true;
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"Could not create directory '{path}'");
         throw;
      }

      return success;
   }

   /// <summary>
   /// Checks if the path is a directory.
   /// </summary>
   /// <param name="path">Path to the directory</param>
   /// <param name="checkForExtensions">Check for extensions (optional, default: true)</param>
   /// <returns>True if the path is a directory</returns>
   public static bool IsDirectory(string? path, bool checkForExtensions = true) //NUnit
   {
      if (string.IsNullOrEmpty(path))
         return false;

      if (ExistsDirectory(path))
         return true;

      if (ExistsFile(path))
         return false;

      if (checkForExtensions)
      {
         string extension = GetExtension(path);
         return extension.Length <= 1; // extension includes '.'
      }

      return false;
   }

   /// <summary>
   /// Checks if the path is a file.
   /// </summary>
   /// <param name="path">Path to the file</param>
   /// <param name="checkForExtensions">Check for extensions (optional, default: true)</param>
   /// <returns>True if the path is a file</returns>
   public static bool IsFile(string? path, bool checkForExtensions = true) //NUnit
   {
      return !string.IsNullOrEmpty(path) && !IsDirectory(path, checkForExtensions);
   }

   /// <summary>
   /// Checks if the path is the root.
   /// </summary>
   /// <param name="path">Possible root</param>
   /// <returns>True if the path is the root</returns>
   public static bool IsRoot(string? path) //NUnit
   {
      return !string.IsNullOrEmpty(path) && (path.Equals("/") || (path.Length is > 1 and < 4 && Constants.REGEX_DRIVE_LETTERS.IsMatch(path)));
   }

   /// <summary>
   /// Returns the name for the file or directory.
   /// </summary>
   /// <param name="path">Path to the file/directory</param>
   /// <returns>File name for file/directory path</returns>
   public static string GetName(string path)
   {
      return IsFile(path) ? GetFileName(path) : GetDirectoryName(path);
   }

   /// <summary>
   /// Returns the file name for the path.
   /// </summary>
   /// <param name="path">Path to the file</param>
   /// <param name="removeInvalidChars">Removes invalid characters in the file name (optional, default: true)</param>
   /// <returns>File name for the path</returns>
   public static string GetFileName(string path, bool removeInvalidChars = true) //NUnit
   {
      ArgumentNullException.ThrowIfNullOrEmpty(path);

      string validPath = ValidatePath(path, false, removeInvalidChars);
      //string validPath = ValidateFile(path, removeInvalidChars);
      string fname = validPath;

      try
      {
         fname = Path.GetFileName(validPath);
      }
      catch (Exception)
      {
         //do nothing
      }

      if (string.IsNullOrEmpty(fname) || fname == validPath)
         fname = IsWindowsPath(validPath) ? validPath.Substring(validPath.BNLastIndexOf(Constants.PATH_DELIMITER_WINDOWS) + 1) : validPath.Substring(validPath.BNLastIndexOf(Constants.PATH_DELIMITER_UNIX) + 1);

      if (removeInvalidChars)
         fname = string.Join(string.Empty, fname.Split(_invalidFilenameChars));

      return fname;
   }

   /// <summary>
   /// Returns the directory name for the path.
   /// </summary>
   /// <param name="path">Path to the directory</param>
   /// <param name="fullPath">Return the full path to the directory (optional, default: false)</param>
   /// <returns>Directory name for the path</returns>
   /// <exception cref="Exception"></exception>
   public static string GetDirectoryName(string path, bool fullPath = false) //NUnit
   {
      ArgumentNullException.ThrowIfNullOrEmpty(path);

      string validPath = ValidatePath(path, false);
      string dname = validPath;

      try
      {
         if (fullPath)
            return Path.GetDirectoryName(validPath)!;

         dname = new DirectoryInfo(validPath).Name;
      }
      catch (Exception)
      {
         //do nothing
      }

      if (string.IsNullOrEmpty(dname) || dname == validPath)
      {
         dname = IsWindowsPath(validPath) ? validPath.Substring(validPath.BNLastIndexOf(Constants.PATH_DELIMITER_WINDOWS) + 1) : validPath.Substring(validPath.BNLastIndexOf(Constants.PATH_DELIMITER_UNIX) + 1);

         if (HasPathInvalidChars(dname))
            dname = string.Join(string.Empty, dname.Split(_invalidPathChars));
      }

      return dname;
   }

   /// <summary>
   /// Returns the size of a file or directory in bytes.
   /// </summary>
   /// <param name="path">Path of the file/directory</param>
   /// <returns>Size of the file/directory</returns>
   /// <exception cref="Exception"></exception>
   public static long GetSize(string path) //NUnit
   {
      return IsFile(path) ? GetFileSize(path) : GetDirectorySize(path);
   }

   /// <summary>
   /// Returns the size of a file in bytes.
   /// </summary>
   /// <param name="path">Path of the file</param>
   /// <returns>Size of the file</returns>
   /// <exception cref="Exception"></exception>
   public static long GetFileSize(string path) //NUnit
   {
      ArgumentNullException.ThrowIfNullOrEmpty(path);

      path = ValidateFile(path);

      if (ExistsFile(path))
      {
         try
         {
            return new FileInfo(path).Length;
         }
         catch (Exception ex)
         {
            _logger.LogError(ex, $"Could not get file size for '{path}'");
            throw;
         }
      }

      _logger.LogWarning($"File does not exists: {path}");

      return -1;
   }

   /// <summary>
   /// Returns the size of a directory in bytes.
   /// </summary>
   /// <param name="path">Path of the directory</param>
   /// <returns>Size of the directory</returns>
   /// <exception cref="Exception"></exception>
   public static long GetDirectorySize(string path)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(path);

      path = ValidatePath(path);

      if (ExistsDirectory(path))
      {
         try
         {
            string[] files = Directory.GetFiles(path, "*.*");

            return files.Select(name => new FileInfo(name)).Select(info => info.Length).Sum();
         }
         catch (Exception ex)
         {
            _logger.LogError(ex, $"Could not get directory size for '{path}'");
            throw;
         }
      }

      _logger.LogWarning($"Directory does not exists: {path}");

      return -1;
   }

   /// <summary>
   /// Returns the extension of a file.
   /// </summary>
   /// <param name="path">Path to the file</param>
   /// <returns>Extension of the file</returns>
   /// <exception cref="Exception"></exception>
   public static string GetExtension(string path) //NUnit
   {
      ArgumentNullException.ThrowIfNullOrEmpty(path);

      try
      {
         if (IsDirectory(path, false))
            return string.Empty;

         //path = ValidateFile(path);

         string ext = Path.GetExtension(path);

         string res = ext.Length > 0 ? ext[1..] : path;
         return res.ToLower();
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"Could not get extension for file '{path}'");
         throw;
      }
   }

   /// <summary>
   /// Returns the last write (=modified) timestamp of a file or directory.
   /// </summary>
   /// <param name="path">Path to the file/directory</param>
   /// <returns>Last write timestamp</returns>
   /// <exception cref="Exception"></exception>
   public static DateTime GetLastWriteTime(string path)
   {
      return IsFile(path) ? GetLastFileWriteTime(path) : GetLastDirectoryWriteTime(path);
   }

   /// <summary>
   /// Returns the last access (=read) timestamp of a file or directory.
   /// </summary>
   /// <param name="path">Path to the file/directory</param>
   /// <returns>Last access timestamp</returns>
   /// <exception cref="Exception"></exception>
   public static DateTime GetLastAccessTime(string path)
   {
      return IsFile(path) ? GetLastFileAccessTime(path) : GetLastDirectoryAccessTime(path);
   }

   /// <summary>
   /// Returns the creation timestamp of a file or directory.
   /// </summary>
   /// <param name="path">Path to the file/directory</param>
   /// <returns>Creation timestamp</returns>
   /// <exception cref="Exception"></exception>
   public static DateTime GetCreationTime(string path)
   {
      return IsFile(path) ? GetFileCreationTime(path) : GetDirectoryCreationTime(path);
   }

   /// <summary>
   /// Returns the last write (=modified) timestamp of a file.
   /// </summary>
   /// <param name="file">Path to the file</param>
   /// <returns>Last write timestamp</returns>
   /// <exception cref="Exception"></exception>
   public static DateTime GetLastFileWriteTime(string file) //NUnit
   {
      ArgumentNullException.ThrowIfNullOrEmpty(file);

      file = ValidateFile(file);

      if (ExistsFile(file))
      {
         try
         {
            return new FileInfo(file).LastWriteTime;
         }
         catch (Exception ex)
         {
            _logger.LogError(ex, $"Could not get last write timestamp for '{file}'");
            throw;
         }
      }

      _logger.LogWarning($"File does not exists: {file}");

      return DateTime.MinValue;
   }

   /// <summary>
   /// Returns the last access (=read) timestamp of a file.
   /// </summary>
   /// <param name="file">Path to the file</param>
   /// <returns>Last access timestamp</returns>
   /// <exception cref="Exception"></exception>
   public static DateTime GetLastFileAccessTime(string file) //NUnit
   {
      ArgumentNullException.ThrowIfNullOrEmpty(file);

      file = ValidateFile(file);

      if (ExistsFile(file))
      {
         try
         {
            return new FileInfo(file).LastAccessTime;
         }
         catch (Exception ex)
         {
            _logger.LogError(ex, $"Could not get last access timestamp for '{file}'");
            throw;
         }
      }

      _logger.LogWarning($"File does not exists: {file}");

      return DateTime.MinValue;
   }

   /// <summary>
   /// Returns the creation timestamp of a file.
   /// </summary>
   /// <param name="file">Path to the file</param>
   /// <returns>Creation timestamp</returns>
   /// <exception cref="Exception"></exception>
   public static DateTime GetFileCreationTime(string file) //NUnit
   {
      ArgumentNullException.ThrowIfNullOrEmpty(file);

      file = ValidateFile(file);

      if (ExistsFile(file))
      {
         try
         {
            return new FileInfo(file).CreationTime;
         }
         catch (Exception ex)
         {
            _logger.LogError(ex, $"Could not get creation timestamp for '{file}'");
            throw;
         }
      }

      _logger.LogWarning($"File does not exists: {file}");

      return DateTime.MinValue;
   }

   /// <summary>
   /// Returns the last write (=modified) timestamp of directory.
   /// </summary>
   /// <param name="path">Path to the directory</param>
   /// <returns>Last write timestamp</returns>
   /// <exception cref="Exception"></exception>
   public static DateTime GetLastDirectoryWriteTime(string path) //NUnit
   {
      ArgumentNullException.ThrowIfNullOrEmpty(path);

      path = ValidatePath(path);

      if (ExistsDirectory(path))
      {
         try
         {
            return new DirectoryInfo(path).LastWriteTime;
         }
         catch (Exception ex)
         {
            _logger.LogError(ex, $"Could not get last write timestamp for '{path}'");
            throw;
         }
      }

      _logger.LogWarning($"Directory does not exists: {path}");

      return DateTime.MinValue;
   }

   /// <summary>
   /// Returns the last access (=read) timestamp of a directory.
   /// </summary>
   /// <param name="path">Path to the directory</param>
   /// <returns>Last access timestamp</returns>
   /// <exception cref="Exception"></exception>
   public static DateTime GetLastDirectoryAccessTime(string path) //NUnit
   {
      ArgumentNullException.ThrowIfNullOrEmpty(path);

      path = ValidatePath(path);

      if (ExistsDirectory(path))
      {
         try
         {
            return new DirectoryInfo(path).LastAccessTime;
         }
         catch (Exception ex)
         {
            _logger.LogError(ex, $"Could not get last access timestamp for '{path}'");
            throw;
         }
      }

      _logger.LogWarning($"Directory does not exists: {path}");

      return DateTime.MinValue;
   }

   /// <summary>
   /// Returns the creation timestamp of a directory.
   /// </summary>
   /// <param name="path">Path to the directory</param>
   /// <returns>Creation timestamp</returns>
   /// <exception cref="Exception"></exception>
   public static DateTime GetDirectoryCreationTime(string path) //NUnit
   {
      ArgumentNullException.ThrowIfNullOrEmpty(path);

      path = ValidatePath(path);

      if (ExistsDirectory(path))
      {
         try
         {
            return new DirectoryInfo(path).CreationTime;
         }
         catch (Exception ex)
         {
            _logger.LogError(ex, $"Could not get creation timestamp for '{path}'");
            throw;
         }
      }

      _logger.LogWarning($"Directory does not exists: {path}");

      return DateTime.MinValue;
   }

   /// <summary>
   /// Reads the text of a file.
   /// </summary>
   /// <param name="path">Path to the file</param>
   /// <param name="encoding">Encoding of the text (optional, default: UTF8)</param>
   /// <returns>Text-content of the file</returns>
   /// <exception cref="Exception"></exception>
   public static string ReadAllText(string path, Encoding? encoding = null) //NUnit
   {
      ArgumentNullException.ThrowIfNullOrEmpty(path);

      try
      {
         path = ValidateFile(path);

         if (!ExistsFile(path))
            throw new Exception($"File does not exists: {path}");

         return File.ReadAllText(path, encoding ?? Encoding.UTF8);
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"Could not read file '{path}'");
         throw;
      }
   }

   /// <summary>
   /// Reads the text of a file asynchronously.
   /// </summary>
   /// <param name="path">Path to the file</param>
   /// <param name="encoding">Encoding of the text (optional, default: UTF8)</param>
   /// <returns>Text-content of the file</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<string> ReadAllTextAsync(string path, Encoding? encoding = null)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(path);

      try
      {
         path = ValidateFile(path);

         if (!ExistsFile(path))
            throw new Exception($"File does not exists: {path}");

         return await File.ReadAllTextAsync(path, encoding ?? Encoding.UTF8);
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"Could not read file '{path}'");
         throw;
      }
   }

   /// <summary>
   /// Reads all lines of text from a file.
   /// </summary>
   /// <param name="path">Path to the file</param>
   /// <param name="encoding">Encoding of the text (optional, default: UTF8)</param>
   /// <returns>Array of text lines from the file</returns>
   /// <exception cref="Exception"></exception>
   public static string[] ReadAllLines(string path, Encoding? encoding = null) //NUnit
   {
      ArgumentNullException.ThrowIfNullOrEmpty(path);

      try
      {
         path = ValidateFile(path);

         if (!ExistsFile(path))
            throw new Exception($"File does not exists: {path}");

         return File.ReadAllLines(path, encoding ?? Encoding.UTF8);
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"Could not read file '{path}'");
         throw;
      }
   }

   /// <summary>
   /// Reads all lines of text from a file asynchronously.
   /// </summary>
   /// <param name="path">Path to the file</param>
   /// <param name="encoding">Encoding of the text (optional, default: UTF8)</param>
   /// <returns>Array of text lines from the file</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<string[]> ReadAllLinesAsync(string path, Encoding? encoding = null)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(path);

      try
      {
         path = ValidateFile(path);

         if (!ExistsFile(path))
            throw new Exception($"File does not exists: {path}");

         return await File.ReadAllLinesAsync(path, encoding ?? Encoding.UTF8);
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"Could not read file '{path}'");
         throw;
      }
   }

   /// <summary>
   /// Reads the bytes of a file.
   /// </summary>
   /// <param name="path">Path to the file</param>
   /// <returns>Byte-content of the file</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] ReadAllBytes(string path) //NUnit
   {
      ArgumentNullException.ThrowIfNullOrEmpty(path);

      try
      {
         path = ValidateFile(path);

         if (!ExistsFile(path))
            throw new Exception($"File does not exists: {path}");

         return File.ReadAllBytes(path);
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"Could not read file '{path}'");
         throw;
      }
   }

   /// <summary>
   /// Reads the bytes of a file asynchronously.
   /// </summary>
   /// <param name="path">Path to the file</param>
   /// <returns>Byte-content of the file</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<byte[]> ReadAllBytesAsync(string path)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(path);

      try
      {
         path = ValidateFile(path);

         if (!ExistsFile(path))
            throw new Exception($"File does not exists: {path}");

         return await File.ReadAllBytesAsync(path);
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"Could not read file '{path}'");
         throw;
      }
   }

   /// <summary>
   /// Writes text to a file.
   /// </summary>
   /// <param name="destFile">Destination file path</param>
   /// <param name="text">Text-content to write</param>
   /// <param name="encoding">Encoding of the text (optional, default: UTF8)</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool WriteAllText(string destFile, string text, Encoding? encoding = null) //NUnit
   {
      ArgumentNullException.ThrowIfNullOrEmpty(destFile);
      ArgumentNullException.ThrowIfNull(text);

      bool success;

      try
      {
         destFile = ValidateFile(destFile);

         File.WriteAllText(destFile, text, encoding ?? Encoding.UTF8);
         success = true;
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"Could not write file '{destFile}'");
         throw;
      }

      return success;
   }

   /// <summary>
   /// Writes text to a file asynchronously.
   /// </summary>
   /// <param name="destFile">Destination file path</param>
   /// <param name="text">Text-content to write</param>
   /// <param name="encoding">Encoding of the text (optional, default: UTF8)</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<bool> WriteAllTextAsync(string destFile, string text, Encoding? encoding = null)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(destFile);
      ArgumentNullException.ThrowIfNull(text);

      bool success;

      try
      {
         destFile = ValidateFile(destFile);

         await File.WriteAllTextAsync(destFile, text, encoding ?? Encoding.UTF8);
         success = true;
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"Could not write file '{destFile}'");
         throw;
      }

      return success;
   }

   /// <summary>
   /// Writes all lines of text to a file.
   /// </summary>
   /// <param name="destFile">Destination file path</param>
   /// <param name="lines">Array of text lines to write</param>
   /// <param name="encoding">Encoding of the text (optional, default: UTF8)</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool WriteAllLines(string destFile, string[] lines, Encoding? encoding = null) //NUnit
   {
      ArgumentNullException.ThrowIfNullOrEmpty(destFile);
      ArgumentNullException.ThrowIfNull(lines);

      bool success;

      try
      {
         destFile = ValidateFile(destFile);

         File.WriteAllLines(destFile, lines, encoding ?? Encoding.UTF8);
         success = true;
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"Could not write file '{destFile}'");
         throw;
      }

      return success;
   }

   /// <summary>
   /// Writes all lines of text to a file asynchronously.
   /// </summary>
   /// <param name="destFile">Destination file path</param>
   /// <param name="lines">Array of text lines to write</param>
   /// <param name="encoding">Encoding of the text (optional, default: UTF8)</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<bool> WriteAllLinesAsync(string destFile, string[] lines, Encoding? encoding = null) //NUnit
   {
      ArgumentNullException.ThrowIfNullOrEmpty(destFile);
      ArgumentNullException.ThrowIfNull(lines);

      bool success;

      try
      {
         destFile = ValidateFile(destFile);

         await File.WriteAllLinesAsync(destFile, lines, encoding ?? Encoding.UTF8);
         success = true;
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"Could not write file '{destFile}'");
         throw;
      }

      return success;
   }

   /// <summary>
   /// Writes bytes to a file.
   /// </summary>
   /// <param name="destFile">Destination file path</param>
   /// <param name="data">Byte-content to write</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool WriteAllBytes(string destFile, byte[] data) //NUnit
   {
      ArgumentNullException.ThrowIfNullOrEmpty(destFile);
      ArgumentNullException.ThrowIfNull(data);

      bool success;

      try
      {
         destFile = ValidateFile(destFile);

         File.WriteAllBytes(destFile, data);
         success = true;
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"Could not write file '{destFile}'");
         throw;
      }

      return success;
   }

   /// <summary>
   /// Writes bytes to a file asynchronously.
   /// </summary>
   /// <param name="destFile">Destination file path</param>
   /// <param name="data">Byte-content to write</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<bool> WriteAllBytesAsync(string destFile, byte[] data) //NUnit
   {
      ArgumentNullException.ThrowIfNullOrEmpty(destFile);
      ArgumentNullException.ThrowIfNull(data);

      bool success;

      try
      {
         destFile = ValidateFile(destFile);

         await File.WriteAllBytesAsync(destFile, data);
         success = true;
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"Could not write file '{destFile}'");
         throw;
      }

      return success;
   }

   /// <summary>
   /// Shows the location of a file or directory in OS file explorer.
   /// NOTE: only works on standalone platforms
   /// </summary>
   /// <param name="path">Path to the file/directory</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool Show(string? path)
   {
      if (!Constants.IsPC)
      {
         _logger.LogWarning("Method is not supported under the current platform!");
         return false;
      }

      bool success = false;

      string? usedPath;

      if (string.IsNullOrEmpty(path) || path.Equals("."))
      {
         usedPath = ".";
      }
      else if (Constants.IsWindows && path.Length < 4)
      {
         usedPath = path; //root directory
      }
      else
      {
         usedPath = ValidatePath(GetDirectoryName(path));
      }

      try
      {
         if (ExistsDirectory(usedPath))
         {
            using Process process = new();
            process.StartInfo.Arguments = $"\"{usedPath}\"";

            if (Constants.IsWindows)
            {
               process.StartInfo.FileName = "explorer.exe";
               process.StartInfo.CreateNoWindow = true;
            }
            else if (Constants.IsOSX)
            {
               process.StartInfo.FileName = "open";
            }
            else
            {
               process.StartInfo.FileName = "xdg-open";
            }

            process.Start();

            success = true;
         }

         else
         {
            _logger.LogWarning($"Path to file doesn't exist: {usedPath}");
         }
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"Could not show file location '{path}'");
         throw;
      }

      return success;
   }

   /// <summary>
   /// Opens a file with the OS default application.
   /// NOTE: only works for standalone platforms
   /// </summary>
   /// <param name="file">File path</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool OpenFile(string? file)
   {
      if (!Constants.IsPC)
      {
         _logger.LogWarning("Method is not supported under the current platform!");
         return false;
      }

      bool success = false;

      try
      {
         if (file != null && ExistsFile(file))
         {
            using (Process process = new())
            {
               if (Constants.IsWindows)
               {
                  process.StartInfo.FileName = "explorer";
                  process.StartInfo.Arguments = $"\"{file}\"";
               }
               else if (Constants.IsOSX)
               {
                  process.StartInfo.FileName = "open";

                  if (file != null)
                  {
                     process.StartInfo.WorkingDirectory = GetDirectoryName(file) + Constants.PATH_DELIMITER_UNIX;
                     process.StartInfo.Arguments = $"-t \"{GetFileName(file)}\"";
                  }
               }
               else
               {
                  process.StartInfo.FileName = "xdg-open";

                  if (file != null)
                  {
                     process.StartInfo.WorkingDirectory = GetDirectoryName(file) + Constants.PATH_DELIMITER_UNIX;
                     process.StartInfo.Arguments = GetFileName(file);
                  }
               }

               process.Start();
            }

            success = true;
         }
         else
         {
            _logger.LogWarning($"File doesn't exist: {file}");
         }
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"Could not open file '{file}'");
         throw;
      }


      return success;
   }

   #endregion

   #region Private methods

   private static string replaceTilde(string path)
   {
      return path.Replace("~", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)).Replace("//", "/");
   }

   private static void copyAll(DirectoryInfo source, DirectoryInfo target)
   {
      CreateDirectory(target.FullName);

      foreach (FileInfo fi in source.GetFiles())
      {
         fi.CopyTo(Combine(target.FullName, fi.Name), true);
      }

      // Copy each subdirectory using recursion.
      foreach (DirectoryInfo sourceSubDir in source.GetDirectories())
      {
         DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(sourceSubDir.Name);
         copyAll(sourceSubDir, nextTargetSubDir);
      }
   }

   #endregion
}