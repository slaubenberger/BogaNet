using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text;
using BogaNet.Util;

namespace BogaNet.IO;

/// <summary>
/// Various helper functions for filesystem operations.
/// </summary>
public abstract class FileHelper
{
   #region Variables

   private static readonly ILogger _logger = GlobalLogging.CreateLogger("FileHelper");

   private static char[] _invalidFilenameChars = [];
   private static char[] _invalidPathChars = [];

   #endregion

   #region Properties

   /// <summary>Returns a temporary file.</summary>
   /// <returns>Temporary file</returns>
   public static string TempFile => Path.GetTempFileName();

   /// <summary>Returns the temporary directory path.</summary>
   /// <returns>Temporary directory path of the device</returns>
   public static string TempPath => ValidatePath(Path.GetTempPath()) ?? string.Empty;

   /// <summary>Returns a temporary directory.</summary>
   /// <returns>Temporary directory</returns>
   public static string TempDirectory
   {
      get
      {
         string name = ValidatePath($"{TempPath}{ShortUID.NewShortUID()}") ?? string.Empty;

         CreateDirectory(name);

         return name;
      }
   }

   /// <summary>Returns the current directory.</summary>
   /// <returns>Current directory</returns>
   public static string CurrentDirectory => ValidatePath(Environment.CurrentDirectory) ?? string.Empty;

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
   /// Combine two paths together
   /// </summary>
   /// <param name="path1">First path</param>
   /// <param name="path2">Second path</param>
   /// <returns>Combined path</returns>
   public static string? Combine(string? path1, string? path2)
   {
      if (path1 == null)
         return path2;

      if (path2 == null)
         return path1;

      return Path.Combine(path1, path2);
   }

   /// <summary>Checks if the given path is from a Unix-device</summary>
   /// <param name="path">Path to check</param>
   /// <returns>True if the given path is from a Unix-device</returns>
   public static bool isUnixPath(string? path) //NUnit
   {
      return !string.IsNullOrEmpty(path) && path.StartsWith('/');
   }

   /// <summary>Checks if the given path is from a Windows-device</summary>
   /// <param name="path">Path to check</param>
   /// <returns>True if the given path is from a Windows-device</returns>
   public static bool isWindowsPath(string? path) //NUnit
   {
      return !string.IsNullOrEmpty(path) && Constants.REGEX_DRIVE_LETTERS.IsMatch(path);
   }

   /// <summary>Checks if the given path is UNC</summary>
   /// <param name="path">Path to check</param>
   /// <returns>True if the given path is UNC</returns>
   public static bool isUNCPath(string? path) //NUnit
   {
      return !string.IsNullOrEmpty(path) && path.StartsWith(@"\\");
   }

   /// <summary>Checks if the given path is an URL</summary>
   /// <param name="path">Path to check</param>
   /// <returns>True if the given path is an URL</returns>
   public static bool isURL(string? path) //NUnit
   {
      return NetworkHelper.isURL(path);
   }

   /// <summary>Validates a given path and add missing slash.</summary>
   /// <param name="path">Path to validate</param>
   /// <param name="addEndDelimiter">Add delimiter at the end of the path (optional, default: true)</param>
   /// <param name="preserveFile">Preserves a given file in the path (optional, default: true)</param>
   /// <param name="removeInvalidChars">Removes invalid characters in the path name (optional default: true)</param>
   /// <returns>Valid path</returns>
   public static string? ValidatePath(string? path, bool addEndDelimiter = true, bool preserveFile = true, bool removeInvalidChars = true) //NUnit
   {
      if (string.IsNullOrEmpty(path))
         return path;

      if (isURL(path))
      {
         if (addEndDelimiter && !path.EndsWith(Constants.PATH_DELIMITER_UNIX))
            path += Constants.PATH_DELIMITER_UNIX;

         return path;
      }

      string? pathTemp = !preserveFile && ExistsFile(path.Trim()) ? GetDirectoryName(path.Trim()) : path.Trim();

      if (pathTemp != null)
      {
         string result;

         if (isWindowsPath(pathTemp) || isUNCPath(pathTemp))
         {
            //if (!isUNCPath(pathTemp))
            result = pathTemp.Replace('/', '\\');

            if (addEndDelimiter && !result.EndsWith(Constants.PATH_DELIMITER_WINDOWS))
               result += Constants.PATH_DELIMITER_WINDOWS;
         }
         else
         {
            result = pathTemp.Replace('\\', '/');

            if (addEndDelimiter && !result.EndsWith(Constants.PATH_DELIMITER_UNIX))
               result += Constants.PATH_DELIMITER_UNIX;
         }

         if (removeInvalidChars && HasPathInvalidChars(result))
            result = string.Join(string.Empty, result.Split(_invalidPathChars));

         return result;
      }

      return null;
   }

   /// <summary>Validates a given file.</summary>
   /// <param name="path">File to validate</param>
   /// <param name="removeInvalidChars">Removes invalid characters in the file name (optional, default: true)</param>
   /// <returns>Valid file path</returns>
   public static string? ValidateFile(string? path, bool removeInvalidChars = true) //NUnit
   {
      if (string.IsNullOrEmpty(path))
         return path;

      if (isURL(path))
         return path;

      bool isWin = isWindowsPath(path);
      bool isUNC = isUNCPath(path);

      string? result = ValidatePath(path, false, true, removeInvalidChars);

      if (result != null)
      {
         if (result.EndsWith(Constants.PATH_DELIMITER_WINDOWS) ||
             result.EndsWith(Constants.PATH_DELIMITER_UNIX))
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

         result = result.Substring(0, result.Length - fileName.Length) + newName; //this is correct!
      }

      return result;
   }

   /// <summary>
   /// Checks a given path for invalid characters
   /// </summary>
   /// <param name="path">Path to check for invalid characters</param>
   /// <param name="ignoreNullOrEmpty">If set to true, return false for null or empty paths (optional, default: true)</param>
   /// <returns>Returns true if the path contains invalid chars, otherwise it's false.</returns>
   public static bool HasPathInvalidChars(string? path, bool ignoreNullOrEmpty = true) //NUnit
   {
      if (string.IsNullOrEmpty(path))
         return !ignoreNullOrEmpty;

      return path.IndexOfAny(_invalidPathChars) >= 0;
   }

   /// <summary>
   /// Checks a given file for invalid characters
   /// </summary>
   /// <param name="file">File to check for invalid characters</param>
   /// <param name="ignoreNullOrEmpty">If set to true, return false for null or empty paths (optional, default: true)</param>
   /// <returns>Returns true if the file contains invalid chars, otherwise it's false.</returns>
   public static bool HasFileInvalidChars(string? file, bool ignoreNullOrEmpty = true) //NUnit
   {
      if (file == null || string.IsNullOrEmpty(file))
         return !ignoreNullOrEmpty;

      return GetFileName(file, false)?.IndexOfAny(_invalidFilenameChars) >= 0 || file.EndsWith('.');
   }

   /// <summary>
   /// Find files inside a path.
   /// NOTE: for an non-blocking version, consider calling this method from a separate thread
   /// </summary>
   /// <param name="path">Path to find the files</param>
   /// <param name="isRecursive">Recursive search (optional, default: false)</param>
   /// <param name="filenames">Array of file names for the file search, e.g. "Image.png" (optional)</param>
   /// <returns>Returns array of the found files inside the path (alphabetically ordered). Zero length array when an error occured.</returns>
   /// <exception cref="Exception"></exception>
   public static string[] GetFilesForName(string? path, bool isRecursive = false, params string[]? filenames) //NUnit
   {
      if (!string.IsNullOrEmpty(path))
      {
         try
         {
            string? _path = ValidatePath(path);

            if (_path != null)
            {
               if (ExistsDirectory(_path))
               {
                  if (filenames == null || filenames.Length == 0 || filenames.Any(extension => extension.Equals("*") || extension.Equals("*.*")))
                  {
                     return Directory.EnumerateFiles(_path, "*", isRecursive
                        ? SearchOption.AllDirectories
                        : SearchOption.TopDirectoryOnly).ToArray();
                  }

                  List<string> files = [];

                  foreach (string filename in filenames)
                  {
                     files.AddRange(Directory.EnumerateFiles(_path, filename.StartsWith("*.") ? filename : $"*{filename}*", isRecursive
                        ? SearchOption.AllDirectories
                        : SearchOption.TopDirectoryOnly));
                  }

                  return files.OrderBy(q => q).ToArray();
               }
            }
            else
            {
               _logger.LogError($"Path does not exist: {path}");
            }
         }
         catch (Exception ex)
         {
            _logger.LogError(ex, $"Could not scan the path '{path}' for files");
            throw;
         }
      }

      return [];
   }

   /// <summary>
   /// Find files inside a path.
   /// NOTE: for an non-blocking version, consider calling this method from a separate thread
   /// </summary>
   /// <param name="path">Path to find the files</param>
   /// <param name="isRecursive">Recursive search (optional, default: false)</param>
   /// <param name="extensions">Extensions for the file search, e.g. "png" (optional)</param>
   /// <returns>Returns array of the found files inside the path (alphabetically ordered). Zero length array when an error occured.</returns>
   /// <exception cref="Exception"></exception>
   public static string[] GetFiles(string? path, bool isRecursive = false, params string[]? extensions) //NUnit
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
   /// NOTE: for an non-blocking version, consider calling this method from a separate thread
   /// </summary>
   /// <param name="path">Path to find the directories</param>
   /// <param name="isRecursive">Recursive search (optional, default: false)</param>
   /// <returns>Returns array of the found directories inside the path. Zero length array when an error occured.</returns>
   /// <exception cref="Exception"></exception>
   public static string[] GetDirectories(string? path, bool isRecursive = false) //NUnit
   {
      if (!string.IsNullOrEmpty(path))
      {
         try
         {
            string? _path = ValidatePath(path);

            if (ExistsDirectory(_path))
            {
               if (_path != null)
                  return Directory.EnumerateDirectories(_path, "*", isRecursive
                     ? SearchOption.AllDirectories
                     : SearchOption.TopDirectoryOnly).ToArray();
            }
            else
            {
               _logger.LogError($"Path does not exist: {path}");
            }
         }
         catch (Exception ex)
         {
            _logger.LogError(ex, $"Could not scan the path '{path}' for directorie");
            throw;
         }
      }

      return [];
   }

   /// <summary>
   /// Find all logical drives.
   /// </summary>
   /// <returns>Returns array of the found drives. Zero length array when an error occured.</returns>
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

      //return Array.Empty<string>();
   }

   /// <summary>
   /// Copy or move a directory.
   /// NOTE: for an non-blocking version, consider calling this method from a separate thread
   /// </summary>
   /// <param name="sourceDir">Source directory path</param>
   /// <param name="destDir">Destination directory path</param>
   /// <param name="move">Move directory instead of copy (optional, default: false)</param>
   /// <param name="moveSafe">Moves a directory in a safe, but slower way (optional, default: true)</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool CopyDirectory(string? sourceDir, string? destDir, bool move = false, bool moveSafe = true) //NUnit
   {
      if (string.IsNullOrEmpty(destDir))
         return false;

      bool success = false;

      try
      {
         string? src = ValidatePath(sourceDir);
         string? dest = ValidatePath(destDir);

         if (src != null && dest != null)
         {
            if (!ExistsDirectory(src))
            {
               _logger.LogError($"Source directory does not exists: {src}");
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
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"Could not {(move ? "move" : "copy")} directory '{sourceDir}' to '{destDir}'");
         throw;
      }

      return success;
   }

   /// <summary>
   /// Copy or move a file.
   /// NOTE: for an non-blocking version, consider calling this method from a separate thread
   /// </summary>
   /// <param name="sourceFile">Source file path</param>
   /// <param name="destFile">Destination file path</param>
   /// <param name="move">Move file instead of copy (optional, default: false)</param>
   /// <param name="moveSafe">Moves a file in a safe, but slower way (optional, default: true)</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool CopyFile(string? sourceFile, string? destFile, bool move = false, bool moveSafe = true) //NUnit
   {
      if (string.IsNullOrEmpty(destFile))
         return false;

      bool success = false;

      try
      {
         if (sourceFile == null || !ExistsFile(sourceFile))
         {
            _logger.LogError($"Source file does not exists: {sourceFile}");
         }
         else
         {
            string? dest = ValidateFile(destFile);

            if (dest != null)
            {
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
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"Could not {(move ? "move" : "copy")} file '{sourceFile}' to '{destFile}'");
         throw;
      }

      return success;
   }

   /// <summary>
   /// Move a directory.
   /// NOTE: for an non-blocking version, consider calling this method from a separate thread
   /// </summary>
   /// <param name="sourceDir">Source directory path</param>
   /// <param name="destDir">Destination directory path</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool MoveDirectory(string? sourceDir, string? destDir)
   {
      return CopyDirectory(sourceDir, destDir, true);
   }

   /// <summary>
   /// Move a file.
   /// NOTE: for an non-blocking version, consider calling this method from a separate thread
   /// </summary>
   /// <param name="sourceFile">Source file path</param>
   /// <param name="destFile">Destination file path</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool MoveFile(string? sourceFile, string? destFile)
   {
      return CopyFile(sourceFile, destFile, true);
   }

   /// <summary>Renames a directory in a path.</summary>
   /// <param name="path">Path to the directory</param>
   /// <param name="newName">New name for the directory</param>
   /// <returns>New path of the directory</returns>
   /// <exception cref="Exception"></exception>
   public static string? RenameDirectory(string? path, string? newName) //NUnit
   {
      if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(newName))
         return path;

      try
      {
         DirectoryInfo di = new(path);

         DirectoryInfo? parent = di.Parent;

         if (parent != null)
         {
            string newPath = Path.Combine(parent.FullName, newName);
            Directory.Move(path, newPath);

            return newPath;
         }
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"Could not rename directory '{path}' to '{newName}'");
         throw;
      }

      return null;
   }

   /// <summary>Renames a file in a path.</summary>
   /// <param name="path">Path to the file</param>
   /// <param name="newName">New name for the file</param>
   /// <returns>New path of the file</returns>
   /// <exception cref="Exception"></exception>
   public static string? RenameFile(string? path, string? newName) //NUnit
   {
      if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(newName))
         return path;

      try
      {
         string? dir = Path.GetDirectoryName(path);

         if (dir != null)
         {
            string newPath = Path.Combine(dir, newName);
            File.Move(path, newPath);

            return newPath;
         }
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"Could not rename file '{path}' to '{newName}'");
         throw;
      }

      return null;
   }

   /// <summary>Delete a file.</summary>
   /// <param name="file">File to delete</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool DeleteFile(string? file) //NUnit
   {
      bool success = false;

      if (file != null)
      {
         try
         {
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
      }

      return success;
   }

   /// <summary>
   /// Delete a directory.
   /// NOTE: for an non-blocking version, consider calling this method from a separate thread
   /// </summary>
   /// <param name="dir">Directory to delete</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool DeleteDirectory(string? dir) //NUnit
   {
      bool success = false;

      if (dir != null)
      {
         try
         {
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
      }

      return success;
   }

   /// <summary>Checks if the directory exists.</summary>
   /// <returns>True if the directory exists</returns>
   public static bool ExistsFile(string? file) //NUnit
   {
      return File.Exists(file);
   }

   /// <summary>Checks if the directory exists.</summary>
   /// <returns>True if the directory exists</returns>
   public static bool ExistsDirectory(string? path) //NUnit
   {
      return Directory.Exists(path);
   }

   /// <summary>Creates a directory in a given path.</summary>
   /// <param name="path">Path for the directory</param>
   /// <param name="folderName">New folder</param>
   /// <exception cref="Exception"></exception>
   public static string? CreateDirectory(string? path, string? folderName) //NUnit
   {
      if (string.IsNullOrEmpty(folderName))
         return path;

      if (path != null)
      {
         try
         {
            if (!ExistsDirectory(path))
            {
               _logger.LogError($"Path directory does not exists: {path}");
            }
            else
            {
               string newPath = Path.Combine(path, folderName);
               Directory.CreateDirectory(newPath);
               return newPath;
            }
         }
         catch (Exception ex)
         {
            _logger.LogError(ex, $"Could not create directory at '{path}' with name '{folderName}'");
            throw;
         }
      }

      return null;
   }

   /// <summary>Creates a directory.</summary>
   /// <param name="path">Path to the directory to create</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool CreateDirectory(string? path) //NUnit
   {
      if (string.IsNullOrEmpty(path))
         return false;

      bool success;

      try
      {
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

   /// <summary>Creates a file in a given path.</summary>
   /// <param name="path">Path for the file</param>
   /// <param name="fileName">New file</param>
   /// <exception cref="Exception"></exception>
   public static string? CreateFile(string? path, string? fileName) //NUnit
   {
      if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(fileName))
         return path;

      try
      {
         if (!ExistsDirectory(path))
         {
            _logger.LogError($"Path directory does not exists: {path}");
         }
         else
         {
            string newPath = Path.Combine(path, fileName);
            using (File.Create(newPath))
            {
            }

            return newPath;
         }
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"Could not create file at '{path}' with name '{fileName}'");
         throw;
      }

      return null;
   }

   /// <summary>Creates a file.</summary>
   /// <param name="path">Path to the file to create</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool CreateFile(string? path) //NUnit
   {
      if (string.IsNullOrEmpty(path))
         return false;

      bool success;

      try
      {
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

   /// <summary>Checks if the path is a directory.</summary>
   /// <param name="path">Path to the directory</param>
   /// <param name="checkForExtensions">Check for extensions (optional, default: true)</param>
   /// <returns>True if the path is a directory</returns>
   public static bool isDirectory(string? path, bool checkForExtensions = true) //NUnit
   {
      if (string.IsNullOrEmpty(path))
         return false;

      if (ExistsDirectory(path))
         return true;

      if (ExistsFile(path))
         return false;

      if (checkForExtensions)
      {
         string? extension = GetExtension(path);
         return extension == null || extension.Length <= 1; // extension includes '.'
      }

      return false;
   }

   /// <summary>Checks if the path is a file.</summary>
   /// <param name="path">Path to the file</param>
   /// <param name="checkForExtensions">Check for extensions (optional, default: true)</param>
   /// <returns>True if the path is a file</returns>
   public static bool isFile(string? path, bool checkForExtensions = true) //NUnit
   {
      return !string.IsNullOrEmpty(path) && !isDirectory(path, checkForExtensions);
   }

   /// <summary>Checks if the path is the root.</summary>
   /// <param name="path">Possible root</param>
   /// <returns>True if the path is the root</returns>
   public static bool isRoot(string? path) //NUnit
   {
      return !string.IsNullOrEmpty(path) && (path.Equals("/") || (path.Length is > 1 and < 4 && Constants.REGEX_DRIVE_LETTERS.IsMatch(path)));
   }

   /// <summary>Returns the file name for the path.</summary>
   /// <param name="path">Path to the file</param>
   /// <param name="removeInvalidChars">Removes invalid characters in the file name (optional, default: true)</param>
   /// <returns>File name for the path</returns>
   public static string? GetFileName(string? path, bool removeInvalidChars = true) //NUnit
   {
      string? _path = ValidatePath(path, false, removeInvalidChars);
      string? fname = _path;

      if (!string.IsNullOrEmpty(_path))
      {
         try
         {
            fname = Path.GetFileName(_path);
         }
         catch (Exception)
         {
            //do nothing
         }

         if (string.IsNullOrEmpty(fname) || fname == _path)
         {
            fname = isWindowsPath(_path) ? _path.Substring(_path.BNLastIndexOf(Constants.PATH_DELIMITER_WINDOWS) + 1) : _path.Substring(_path.BNLastIndexOf(Constants.PATH_DELIMITER_UNIX) + 1);
         }

         if (removeInvalidChars)
            fname = string.Join(string.Empty, fname.Split(_invalidFilenameChars));
      }

      return fname;
   }

   /// <summary>Returns the current directory name for the path.</summary>
   /// <param name="path">Path to the directory</param>
   /// <returns>Current directory name for the path</returns>
   public static string? GetCurrentDirectoryName(string? path) //NUnit
   {
      string? _path = ValidatePath(path, false);
      string? dname = _path;

      if (!string.IsNullOrEmpty(_path))
      {
         try
         {
            dname = new DirectoryInfo(_path).Name;
         }
         catch (Exception)
         {
            //do nothing
         }

         if (string.IsNullOrEmpty(dname) || dname == _path)
         {
            dname = isWindowsPath(_path) ? _path.Substring(_path.BNLastIndexOf(Constants.PATH_DELIMITER_WINDOWS) + 1) : _path.Substring(_path.BNLastIndexOf(Constants.PATH_DELIMITER_UNIX) + 1);

            if (HasPathInvalidChars(dname))
               dname = string.Join(string.Empty, dname.Split(_invalidPathChars));
         }
      }

      return dname;
   }

   /// <summary>Returns the directory name for the path.</summary>
   /// <param name="path">Path to the directory</param>
   /// <returns>Directory name for the path</returns>
   /// <exception cref="Exception"></exception>
   public static string? GetDirectoryName(string? path) //NUnit
   {
      string? dname = path;

      if (!string.IsNullOrEmpty(path))
      {
         bool isUNC = isUNCPath(path);
         bool isWin = isWindowsPath(path);

         try
         {
            bool hadEndDelimiter = !string.IsNullOrEmpty(path) && (path.EndsWith(Constants.PATH_DELIMITER_WINDOWS) ||
                                                                   path.EndsWith(Constants.PATH_DELIMITER_UNIX));

            string? _path = ValidatePath(isWin ? "/" + path : path, !Constants.REGEX_FILE.IsMatch(path));

            if (!isURL(_path))
               dname = Path.GetDirectoryName(_path);

            if (string.IsNullOrEmpty(dname))
               dname = _path;

            if (isWin)
               dname = dname?.Substring(1);

            dname = isWin || isUNC ? dname?.Replace('/', '\\') : dname?.Replace('\\', '/');

            bool hasEndDelimiter = !string.IsNullOrEmpty(dname) && (dname.EndsWith(Constants.PATH_DELIMITER_WINDOWS) ||
                                                                    dname.EndsWith(Constants.PATH_DELIMITER_UNIX));

            if (hadEndDelimiter && !hasEndDelimiter)
            {
               dname = ValidatePath(dname);
            }
            else if (!hadEndDelimiter && hasEndDelimiter)
            {
               dname = dname?.Substring(0, dname.Length - 1);
            }
         }
         catch (Exception ex)
         {
            _logger.LogError(ex, $"Could not get directory name for '{path}'");
            throw;
         }
      }

      return dname;
   }

   /// <summary>Returns the size of a file.</summary>
   /// <param name="path">Path of the file</param>
   /// <returns>Size for the file</returns>
   /// <exception cref="Exception"></exception>
   public static long GetFilesize(string? path) //NUnit
   {
      if (path != null)
      {
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
      }

      _logger.LogError($"File does not exists: {path}");

      return -1;
   }

   /// <summary>Returns the extension of a file.</summary>
   /// <param name="path">Path to the file</param>
   /// <returns>Extension of the file</returns>
   /// <exception cref="Exception"></exception>
   public static string? GetExtension(string? path) //NUnit
   {
      if (isFile(path, false))
      {
         try
         {
            string? ext = Path.GetExtension(path);

            return !string.IsNullOrEmpty(ext) ? ext.Substring(1) : null;
         }
         catch (Exception ex)
         {
            _logger.LogError(ex, $"Could not get extension for file '{path}'");
            throw;
         }
      }

      _logger.LogError($"File does not exists: {path}");

      return null;
   }

   /// <summary>Returns the size of a file.</summary>
   /// <param name="path">Path to the file</param>
   /// <returns>Size for the file</returns>
   /// <exception cref="Exception"></exception>
   public static DateTime GetLastModifiedDate(string? path) //NUnit
   {
      if (path != null)
      {
         if (ExistsFile(path))
         {
            try
            {
               return new FileInfo(path).LastWriteTime;
            }
            catch (Exception ex)
            {
               _logger.LogError(ex, $"Could not get last modify date for '{path}'");
               throw;
            }
         }
      }

      _logger.LogError($"File does not exists: {path}");

      return DateTime.MinValue;
   }

   /// <summary>Reads the text of a file.</summary>
   /// <param name="path">Path to the file</param>
   /// <param name="encoding">Encoding of the text (optional, default: UTF8)</param>
   /// <returns>Text-content of the file</returns>
   /// <exception cref="Exception"></exception>
   public static string? ReadAllText(string? path, Encoding? encoding = null) //NUnit
   {
      return ReadAllTextAsync(path, encoding).GetAwaiter().GetResult();
   }

   /// <summary>Reads the text of a file asynchronusly.</summary>
   /// <param name="path">Path to the file</param>
   /// <param name="encoding">Encoding of the text (optional, default: UTF8)</param>
   /// <returns>Text-content of the file</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<string?> ReadAllTextAsync(string? path, Encoding? encoding = null)
   {
      if (path != null)
      {
         try
         {
            if (ExistsFile(path))
               return await File.ReadAllTextAsync(path, encoding ?? Encoding.UTF8);
         }
         catch (Exception ex)
         {
            _logger.LogError(ex, $"Could not read file '{path}'");
            throw;
         }
      }

      _logger.LogError($"File does not exists: {path}");

      return null;
   }

   /// <summary>Reads all lines of text from a file.</summary>
   /// <param name="path">Path to the file</param>
   /// <param name="encoding">Encoding of the text (optional, default: UTF8)</param>
   /// <returns>Array of text lines from the file</returns>
   /// <exception cref="Exception"></exception>
   public static string[]? ReadAllLines(string? path, Encoding? encoding = null) //NUnit
   {
      return ReadAllLinesAsync(path, encoding).GetAwaiter().GetResult();
   }

   /// <summary>Reads all lines of text from a file asynchronusly.</summary>
   /// <param name="path">Path to the file</param>
   /// <param name="encoding">Encoding of the text (optional, default: UTF8)</param>
   /// <returns>Array of text lines from the file</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<string[]?> ReadAllLinesAsync(string? path, Encoding? encoding = null)
   {
      if (path != null)
      {
         try
         {
            if (ExistsFile(path))
               return await File.ReadAllLinesAsync(path, encoding ?? Encoding.UTF8);
         }
         catch (Exception ex)
         {
            _logger.LogError(ex, $"Could not read file '{path}'");
            throw;
         }
      }

      _logger.LogError($"File does not exists: {path}");

      return null;
   }

   /// <summary>Reads the bytes of a file.</summary>
   /// <param name="path">Path to the file</param>
   /// <returns>Byte-content of the file</returns>
   /// <exception cref="Exception"></exception>
   public static byte[]? ReadAllBytes(string? path) //NUnit
   {
      return ReadAllBytesAsync(path).GetAwaiter().GetResult();
   }

   /// <summary>Reads the bytes of a file asynchronusly.</summary>
   /// <param name="path">Path to the file</param>
   /// <returns>Byte-content of the file</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<byte[]?> ReadAllBytesAsync(string? path)
   {
      if (path != null)
      {
         try
         {
            if (ExistsFile(path))
               return await File.ReadAllBytesAsync(path);
         }
         catch (Exception ex)
         {
            _logger.LogError(ex, $"Could not read file '{path}'");
            throw;
         }
      }

      _logger.LogError($"File does not exists: {path}");

      return null;
   }

   /// <summary>Writes text to a file.</summary>
   /// <param name="destFile">Destination file path</param>
   /// <param name="text">Text-content to write</param>
   /// <param name="encoding">Encoding of the text (optional, default: UTF8)</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool WriteAllText(string? destFile, string? text, Encoding? encoding = null) //NUnit
   {
      return WriteAllTextAsync(destFile, text, encoding).GetAwaiter().GetResult();
   }

   /// <summary>Writes text to a file asynchronusly.</summary>
   /// <param name="destFile">Destination file path</param>
   /// <param name="text">Text-content to write</param>
   /// <param name="encoding">Encoding of the text (optional, default: UTF8)</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<bool> WriteAllTextAsync(string? destFile, string? text, Encoding? encoding = null)
   {
      if (string.IsNullOrEmpty(destFile))
         return false;

      bool success;

      try
      {
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

   /// <summary>Writes all lines of text to a file.</summary>
   /// <param name="destFile">Destination file path</param>
   /// <param name="lines">Array of text lines to write</param>
   /// <param name="encoding">Encoding of the text (optional, default: UTF8)</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool WriteAllLines(string? destFile, string[]? lines, Encoding? encoding = null) //NUnit
   {
      return WriteAllLinesAsync(destFile, lines, encoding).GetAwaiter().GetResult();
   }

   /// <summary>Writes all lines of text to a file asynchronusly.</summary>
   /// <param name="destFile">Destination file path</param>
   /// <param name="lines">Array of text lines to write</param>
   /// <param name="encoding">Encoding of the text (optional, default: UTF8)</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<bool> WriteAllLinesAsync(string? destFile, string[]? lines, Encoding? encoding = null) //NUnit
   {
      if (string.IsNullOrEmpty(destFile) || lines == null)
         return false;

      bool success;

      try
      {
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

   /// <summary>Writes bytes to a file.</summary>
   /// <param name="destFile">Destination file path</param>
   /// <param name="data">Byte-content to write</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool WriteAllBytes(string? destFile, byte[]? data) //NUnit
   {
      return WriteAllBytesAsync(destFile, data).GetAwaiter().GetResult();
   }

   /// <summary>Writes bytes to a file.</summary>
   /// <param name="destFile">Destination file path</param>
   /// <param name="data">Byte-content to write</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<bool> WriteAllBytesAsync(string? destFile, byte[]? data) //NUnit
   {
      if (string.IsNullOrEmpty(destFile) || data == null)
         return false;

      bool success;

      try
      {
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
   /// Shows the location of a path (or file) in OS file explorer.
   /// NOTE: only works on standalone platforms
   /// </summary>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool ShowPath(string? path)
   {
      return ShowFile(path);
   }

   /// <summary>
   /// Shows the location of a file (or path) in OS file explorer.
   /// NOTE: only works on standalone platforms
   /// </summary>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool ShowFile(string? file)
   {
      bool success = false;

      string? path;

      if (string.IsNullOrEmpty(file) || file.Equals("."))
      {
         path = ".";
      }
      /*
      else if ((BaseHelper.isWindowsPlatform || BaseHelper.isWindowsEditor) && file.Length < 4)
      {
         path = file; //root directory
      }
      */
      else
      {
         path = ValidatePath(GetDirectoryName(file));
      }

      try
      {
         if (ExistsDirectory(path))
         {
            using Process process = new();
            process.StartInfo.Arguments = $"\"{path}\"";

            if (Helper.isWindows)
            {
               process.StartInfo.FileName = "explorer.exe";
               process.StartInfo.CreateNoWindow = true;
            }
            else if (Helper.isOSX)
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
            _logger.LogWarning($"Path to file doesn't exist: {path}");
         }
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"Could not show file location '{file}'");
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
      bool success = false;

      try
      {
         if (ExistsFile(file))
         {
            using (Process process = new())
            {
               if (Helper.isWindows)
               {
                  process.StartInfo.FileName = "explorer";
                  process.StartInfo.Arguments = $"\"{file}\"";
               }
               else if (Helper.isOSX)
               {
                  process.StartInfo.FileName = "open";
                  process.StartInfo.WorkingDirectory = GetDirectoryName(file) + Constants.PATH_DELIMITER_UNIX;
                  process.StartInfo.Arguments = $"-t \"{GetFileName(file)}\"";
               }
               else
               {
                  process.StartInfo.FileName = "xdg-open";
                  process.StartInfo.WorkingDirectory = GetDirectoryName(file) + Constants.PATH_DELIMITER_UNIX;
                  process.StartInfo.Arguments = GetFileName(file);
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

   private static void copyAll(DirectoryInfo source, DirectoryInfo target)
   {
      CreateDirectory(target.FullName);

      foreach (FileInfo fi in source.GetFiles())
      {
         fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
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