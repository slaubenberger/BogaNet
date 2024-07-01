using BogaNet.Helper;
using BogaNet.Extension;

namespace BogaNet.Test.Helper;

public class FileHelperTest
{
   #region Variables

   private static readonly string _testTempPath = FileHelper.TempPath;
   private static readonly string _testTempFile = FileHelper.TempFile;

   private static readonly string _testDirectory = $"{FileHelper.CurrentDirectory}Testfiles/";
   private static readonly string _testFile = $"{_testDirectory}Images/logo_ct.png";

   private const string _testDirectoryName = "crosstales LLC";
   private const string _testDirectoryWin = $@"C:\Windows\System64\{_testDirectoryName}";
   private const string _testDirectoryUnix = $"/usr/bin/{_testDirectoryName}";
   private const string _testDirectoryUrl = $"https://crosstales.com/media/{_testDirectoryName}";
   private const string _testDirectoryUNC = $@"\\MyComputer\MyShare\{_testDirectoryName}";

   private const string _testDirectoryWinNotok = $@"C:\Windows/System64\{_testDirectoryName}";
   private const string _testDirectoryUnixNotok = $@"/usr\bin/{_testDirectoryName}";
   private const string _testDirectoryUrlNotok = $@"https:/\crosstales.com\media/{_testDirectoryName}";
   private const string _testDirectoryUNCNotok = $@"\\MyComputer/MyShare/{_testDirectoryName}";

   private const string _testFileName = "1st ct.logo.png";
   private const string _testFilePathWin = $"{_testDirectoryWin}\\{_testFileName}";
   private const string _testFilePathUnix = $"{_testDirectoryUnix}/{_testFileName}";
   private const string _testFilePathUrl = $"{_testDirectoryUrl}/{_testFileName}";
   private const string _testFilePathUNC = $"{_testDirectoryUNC}\\{_testFileName}";

   private const string _testFilePathWinNotok = $"{_testDirectoryWinNotok}\\{_testFileName}";
   private const string _testFilePathUnixNotok = $"{_testDirectoryUnixNotok}/{_testFileName}";
   private const string _testFilePathUrlNotok = $"{_testDirectoryUrlNotok}/{_testFileName}";
   private const string _testFilePathUNCNotok = $"{_testDirectoryUNC}\\{_testFileName}";

   #endregion

   #region Tests

   [Test]
   public void isUnixPath_Test()
   {
      Assert.False(FileHelper.IsUnixPath(_testDirectoryWin));

      Assert.True(FileHelper.IsUnixPath(_testDirectoryUnix));

      Assert.False(FileHelper.IsUnixPath(_testDirectoryUrl));

      Assert.False(FileHelper.IsUnixPath(_testDirectoryUNC));

      Assert.False(FileHelper.IsUnixPath(""));

      Assert.False(FileHelper.IsUnixPath(null));
   }

   [Test]
   public void isWindowsPath_Test()
   {
      Assert.True(FileHelper.IsWindowsPath(_testDirectoryWin));

      Assert.False(FileHelper.IsWindowsPath(_testDirectoryUnix));

      Assert.False(FileHelper.IsWindowsPath(_testDirectoryUrl));

      Assert.False(FileHelper.IsWindowsPath(_testDirectoryUNC));

      Assert.False(FileHelper.IsWindowsPath(""));

      Assert.False(FileHelper.IsWindowsPath(null));
   }

   [Test]
   public void isUNCPath_Test()
   {
      Assert.False(FileHelper.IsUNCPath(_testDirectoryWin));

      Assert.False(FileHelper.IsUNCPath(_testDirectoryUnix));

      Assert.False(FileHelper.IsUNCPath(_testDirectoryUrl));

      Assert.True(FileHelper.IsUNCPath(_testDirectoryUNC));

      Assert.False(FileHelper.IsUNCPath(""));

      Assert.False(FileHelper.IsUNCPath(null));
   }

   [Test]
   public void isURL_Test()
   {
      Assert.False(FileHelper.IsURL(_testDirectoryWin));

      Assert.False(FileHelper.IsURL(_testDirectoryUnix));

      Assert.True(FileHelper.IsURL(_testDirectoryUrl));

      Assert.False(FileHelper.IsURL(_testDirectoryUNC));

      Assert.False(FileHelper.IsURL(""));

      Assert.False(FileHelper.IsURL(null));
   }

   [Test]
   public void ValidatePath_Test()
   {
      string? path = FileHelper.ValidatePath(_testTempPath);
      Assert.That(path, Is.EqualTo(_testTempPath));

      //Assert.That(FileHelper.ValidatePath("D:/Builds"), Is.EqualTo(@"D:\Builds\"));

      //path = FileHelper.ValidatePath(testDirectory, false);
      //Assert.That(path, Is.EqualTo(testDirectory));


      string tempDir = _testTempPath + _testDirectoryName;

      FileHelper.CreateDirectory(tempDir);
      path = FileHelper.ValidatePath(tempDir, false);
      Assert.That(path, Is.EqualTo(tempDir));
      FileHelper.DeleteDirectory(tempDir);

      path = FileHelper.ValidatePath(_testDirectoryWin, false);
      Assert.That(path, Is.EqualTo(_testDirectoryWin));

      path = FileHelper.ValidatePath(_testDirectoryUnix, false);
      Assert.That(path, Is.EqualTo(_testDirectoryUnix));

      path = FileHelper.ValidatePath(_testDirectoryUrl, false);
      Assert.That(path, Is.EqualTo(_testDirectoryUrl));

      path = FileHelper.ValidatePath(_testDirectoryUNC, false);
      Assert.That(path, Is.EqualTo(_testDirectoryUNC));

      path = FileHelper.ValidatePath(_testDirectoryWinNotok, false);
      Assert.That(path, Is.EqualTo(_testDirectoryWin));

      path = FileHelper.ValidatePath(_testDirectoryUnixNotok, false);
      Assert.That(path, Is.EqualTo(_testDirectoryUnix));

      path = FileHelper.ValidatePath(_testDirectoryUrlNotok, false);
      Assert.That(path, Is.EqualTo(_testDirectoryUrl));

      path = FileHelper.ValidatePath(_testDirectoryUNCNotok, false);
      Assert.That(path, Is.EqualTo(_testDirectoryUNC));

      path = FileHelper.ValidatePath("");
      Assert.True(path == "");

      path = FileHelper.ValidatePath(null);
      Assert.True(path == null);

      path = FileHelper.ValidatePath(_testDirectoryWin + "*", false);
      Assert.That(path, Is.EqualTo(_testDirectoryWin));
   }

   [Test]
   public void ValidateFile_Test()
   {
      string? path = FileHelper.ValidateFile(_testTempFile);
      Assert.That(path, Is.EqualTo(_testTempFile));

      path = FileHelper.ValidateFile(_testFilePathWin);
      Assert.That(path, Is.EqualTo(_testFilePathWin));

      path = FileHelper.ValidateFile(_testFilePathUnix);
      Assert.That(path, Is.EqualTo(_testFilePathUnix));

      path = FileHelper.ValidateFile(_testFilePathUrl);
      Assert.That(path, Is.EqualTo(_testFilePathUrl));

      path = FileHelper.ValidateFile(_testFilePathUNC);
      Assert.That(path, Is.EqualTo(_testFilePathUNC));

      path = FileHelper.ValidateFile(_testFilePathWinNotok);
      Assert.That(path, Is.EqualTo(_testFilePathWin));

      path = FileHelper.ValidateFile(_testFilePathUnixNotok);
      Assert.That(path, Is.EqualTo(_testFilePathUnix));

      path = FileHelper.ValidateFile(_testFilePathUrlNotok);
      Assert.That(path, Is.EqualTo(_testFilePathUrl));

      path = FileHelper.ValidateFile(_testFilePathUNCNotok);
      Assert.That(path, Is.EqualTo(_testFilePathUNC));

      path = FileHelper.ValidateFile("");
      Assert.True(path == "");

      path = FileHelper.ValidateFile(null);
      Assert.True(path == null);

      path = FileHelper.ValidateFile(_testFilePathWin + "*");
      Assert.That(path, Is.EqualTo(_testFilePathWin));

      path = FileHelper.ValidateFile(_testFilePathWin + ".");
      Assert.That(path, Is.EqualTo(_testFilePathWin));
   }

   [Test]
   public void HasPathInvalidChars_Test()
   {
      Assert.False(FileHelper.HasPathInvalidChars(_testTempPath));

      Assert.False(FileHelper.HasPathInvalidChars(_testDirectoryWin));
      Assert.False(FileHelper.HasPathInvalidChars(_testDirectoryUnix));
      Assert.False(FileHelper.HasPathInvalidChars(_testDirectoryUrl));
      Assert.False(FileHelper.HasPathInvalidChars(_testDirectoryUNC));

      Assert.False(FileHelper.HasPathInvalidChars(""));
      Assert.True(FileHelper.HasPathInvalidChars("", false));
      Assert.False(FileHelper.HasPathInvalidChars(null));
      Assert.True(FileHelper.HasPathInvalidChars(null, false));

      Assert.True(FileHelper.HasPathInvalidChars(_testTempPath + "*"));
   }

   [Test]
   public void HasFileInvalidChars_Test()
   {
      Assert.False(FileHelper.HasFileInvalidChars(_testTempFile));

      Assert.False(FileHelper.HasFileInvalidChars(_testFilePathWin));
      Assert.False(FileHelper.HasFileInvalidChars(_testFilePathUnix));
      Assert.False(FileHelper.HasFileInvalidChars(_testFilePathUrl));
//         Assert.False(FileHelper.HasFileInvalidChars(testFilePathUNC));

      Assert.False(FileHelper.HasFileInvalidChars(_testFileName));

      Assert.False(FileHelper.HasFileInvalidChars(""));
      Assert.True(FileHelper.HasFileInvalidChars("", false));
      Assert.False(FileHelper.HasFileInvalidChars(null));
      Assert.True(FileHelper.HasFileInvalidChars(null, false));

      Assert.False(FileHelper.HasFileInvalidChars("*" + _testFileName)); //System.IO.Path.GetFileName corrects the path

      Assert.True(FileHelper.HasFileInvalidChars(_testFileName + "."));
   }

   [Test]
   public void GetFilesForName_Test()
   {
      string[] result = FileHelper.GetFilesForName(_testDirectory);
      Assert.That(result.Length, Is.EqualTo(1)); //README.md

      result = FileHelper.GetFilesForName(_testDirectory, true);
      Assert.That(result.Length, Is.EqualTo(9)); //9 files

      result = FileHelper.GetFilesForName(_testDirectory, true, "*");
      Assert.That(result.Length, Is.EqualTo(9)); //9 files

      result = FileHelper.GetFilesForName(_testDirectory, true, "logo_");
      Assert.That(result.Length, Is.EqualTo(2)); //2 files

      result = FileHelper.GetFilesForName(_testDirectory, true, "MUUUUUUH");
      Assert.That(result.Length, Is.EqualTo(0));

      result = FileHelper.GetFilesForName(_testDirectory, true, null);
      Assert.That(result.Length, Is.EqualTo(9)); //9 files

      result = FileHelper.GetFilesForName(_testDirectory, true, []);
      Assert.That(result.Length, Is.EqualTo(9)); //9 files

      result = FileHelper.GetFilesForName("");
      Assert.That(result.Length, Is.EqualTo(0));

      result = FileHelper.GetFilesForName(null);
      Assert.That(result.Length, Is.EqualTo(0));
   }

   [Test]
   public void GetFiles_Test()
   {
      string[] result = FileHelper.GetFiles(_testDirectory);
      Assert.That(result.Length, Is.EqualTo(1)); //README.md

      result = FileHelper.GetFiles(_testDirectory, true);
      Assert.That(result.Length, Is.EqualTo(9)); //9 files

      result = FileHelper.GetFiles(_testDirectory, true, "*");
      Assert.That(result.Length, Is.EqualTo(9)); //9 files

      result = FileHelper.GetFiles(_testDirectory, true, "png");
      Assert.That(result.Length, Is.EqualTo(2)); //2 files

      result = FileHelper.GetFiles(_testDirectory, true, "MUUUUUUH");
      Assert.That(result.Length, Is.EqualTo(0));

      result = FileHelper.GetFiles(_testDirectory, true, null);
      Assert.That(result.Length, Is.EqualTo(9)); //9 files

      result = FileHelper.GetFiles(_testDirectory, true, []);
      Assert.That(result.Length, Is.EqualTo(9)); //9 files

      result = FileHelper.GetFiles("");
      Assert.That(result.Length, Is.EqualTo(0));

      result = FileHelper.GetFiles(null);
      Assert.That(result.Length, Is.EqualTo(0));
   }

   [Test]
   public void GetDirectories_Test()
   {
      string[] result = FileHelper.GetDirectories(_testDirectory);
      Assert.That(result.Length, Is.EqualTo(4));

      result = FileHelper.GetDirectories(_testDirectory, true);
      Assert.That(result.Length, Is.EqualTo(7));

      result = FileHelper.GetDirectories("");
      Assert.That(result.Length, Is.EqualTo(0));

      result = FileHelper.GetDirectories(null);
      Assert.That(result.Length, Is.EqualTo(0));
   }

   [Test]
   public void GetDrives_Test()
   {
      string[] result = FileHelper.GetDrives();
      Assert.That(result.Length, Is.GreaterThan(0));
   }

   [Test]
   public void CopyDirectory_Test()
   {
      string? newPath = FileHelper.Combine(_testTempPath, FileHelper.GetDirectoryName(_testDirectory));

      Assert.True(FileHelper.CopyDirectory(_testDirectory, newPath));

      Assert.True(FileHelper.ExistsDirectory(newPath));

      string? movePath = FileHelper.CreateDirectory(_testTempPath, "BN_TEST");

      Assert.True(FileHelper.CopyDirectory(newPath, movePath, true));

      Assert.False(FileHelper.ExistsDirectory(newPath));

      Assert.False(FileHelper.CopyDirectory("", newPath));
      Assert.False(FileHelper.CopyDirectory(null, newPath));
      Assert.False(FileHelper.CopyDirectory(_testDirectory, ""));
      Assert.False(FileHelper.CopyDirectory(_testDirectory, null));
      /*
      Assert.False(FileHelper.CopyDirectory(testDirectory, "Y:UELI"));
      Assert.False(FileHelper.CopyDirectory("UELI", newPath));
      */
   }

   [Test]
   public void RenameDirectory_Test()
   {
      string? fname = FileHelper.CreateDirectory(FileHelper.TempPath, System.Guid.NewGuid().ToString());
      Assert.True(FileHelper.ExistsDirectory(fname));

      string? fnameNew = FileHelper.RenameDirectory(fname, System.Guid.NewGuid().ToString());

      Assert.False(FileHelper.ExistsDirectory(fname));
      Assert.True(FileHelper.ExistsDirectory(fnameNew));

      Assert.True(FileHelper.RenameDirectory(fname, "") == fname);
      Assert.True(FileHelper.RenameDirectory(fname, null) == fname);

      Assert.True(FileHelper.RenameDirectory("", Guid.NewGuid().ToString()) == "");
      Assert.True(FileHelper.RenameDirectory(null, Guid.NewGuid().ToString()) == null);
   }

   [Test]
   public void CopyFile_Test()
   {
      string? newPath = FileHelper.Combine(_testTempPath, FileHelper.GetFileName(_testFile));

      Assert.True(FileHelper.CopyFile(_testFile, newPath));

      Assert.True(FileHelper.ExistsFile(newPath));

      Assert.True(FileHelper.DeleteFile(newPath));

      Assert.False(FileHelper.CopyFile("", newPath));
      Assert.False(FileHelper.CopyFile(null, newPath));
      Assert.False(FileHelper.CopyFile(_testFile, ""));
      Assert.False(FileHelper.CopyFile(_testFile, null));
   }

   [Test]
   public void RenameFile_Test()
   {
      string? fname = FileHelper.CreateFile(FileHelper.TempPath, Guid.NewGuid().ToString());
      Assert.True(FileHelper.ExistsFile(fname));

      string? fnameNew = FileHelper.RenameFile(fname, Guid.NewGuid().ToString());

      Assert.False(FileHelper.ExistsFile(fname));
      Assert.True(FileHelper.ExistsFile(fnameNew));

      Assert.True(FileHelper.RenameFile(fname, "") == fname);
      Assert.True(FileHelper.RenameFile(fname, null) == fname);

      Assert.True(FileHelper.RenameFile("", System.Guid.NewGuid().ToString()) == "");
      Assert.True(FileHelper.RenameFile(null, System.Guid.NewGuid().ToString()) == null);
   }

   [Test]
   public void DeleteFile_Test()
   {
      string tempFile = FileHelper.TempFile;

      Assert.True(FileHelper.ExistsFile(tempFile));

      Assert.True(FileHelper.DeleteFile(tempFile));

      Assert.True(!FileHelper.ExistsFile(tempFile));

      Assert.False(FileHelper.DeleteFile(tempFile));
      Assert.False(FileHelper.DeleteFile(""));
      Assert.False(FileHelper.DeleteFile(null));
   }

   [Test]
   public void DeleteDirectory_Test()
   {
      string folderName = Guid.NewGuid().ToString();
      string? newPath = FileHelper.CreateDirectory(FileHelper.TempPath, folderName);

      Assert.True(FileHelper.ExistsDirectory(newPath));

      Assert.True(FileHelper.DeleteDirectory(newPath));

      Assert.True(!FileHelper.ExistsDirectory(newPath));

      Assert.False(FileHelper.DeleteDirectory(newPath));
      Assert.False(FileHelper.DeleteDirectory(""));
      Assert.False(FileHelper.DeleteDirectory(null));
   }

   [Test]
   public void ExistsFile_Test()
   {
      Assert.True(FileHelper.ExistsFile(FileHelper.TempFile));
      Assert.True(FileHelper.ExistsFile(_testFile));

      Assert.False(FileHelper.ExistsFile(_testFilePathWin));

      Assert.False(FileHelper.ExistsFile(""));
      Assert.False(FileHelper.ExistsFile(null));
   }

   [Test]
   public void ExistsDirectory_Test()
   {
      Assert.True(FileHelper.ExistsDirectory(FileHelper.TempPath));
      Assert.True(FileHelper.ExistsDirectory(_testDirectory));

      Assert.False(FileHelper.ExistsDirectory(_testDirectoryWin));

      Assert.False(FileHelper.ExistsDirectory(""));
      Assert.False(FileHelper.ExistsDirectory(null));
   }

   [Test]
   public void CreateDirectory_Test()
   {
      string? path = FileHelper.CreateDirectory(FileHelper.TempPath, System.Guid.NewGuid().ToString());

      Assert.True(FileHelper.ExistsDirectory(path));

      Assert.True(FileHelper.DeleteDirectory(path));

      Assert.False(FileHelper.ExistsDirectory(path));

      Assert.False(string.IsNullOrEmpty(FileHelper.CreateDirectory(FileHelper.TempPath, "")));
      Assert.False(string.IsNullOrEmpty(FileHelper.CreateDirectory(FileHelper.TempPath, null)));

      Assert.True(string.IsNullOrEmpty(FileHelper.CreateDirectory("", System.Guid.NewGuid().ToString())));
      Assert.True(string.IsNullOrEmpty(FileHelper.CreateDirectory(null, System.Guid.NewGuid().ToString())));

      Assert.True(FileHelper.CreateDirectory(path));

      Assert.True(FileHelper.ExistsDirectory(path));

      Assert.True(FileHelper.DeleteDirectory(path));

      Assert.False(FileHelper.CreateDirectory(""));
      Assert.False(FileHelper.CreateDirectory(null));
   }

   [Test]
   public void CreateFile_Test()
   {
      string? fname = FileHelper.CreateFile(FileHelper.TempPath, $"{System.Guid.NewGuid()}.tmp");

      Assert.True(FileHelper.ExistsFile(fname));

      Assert.True(FileHelper.DeleteFile(fname));

      Assert.False(FileHelper.ExistsFile(fname));

      Assert.False(string.IsNullOrEmpty(FileHelper.CreateFile(FileHelper.TempPath, "")));
      Assert.False(string.IsNullOrEmpty(FileHelper.CreateFile(FileHelper.TempPath, null)));

      Assert.True(string.IsNullOrEmpty(FileHelper.CreateFile("", $"{System.Guid.NewGuid()}.tmp")));
      Assert.True(string.IsNullOrEmpty(FileHelper.CreateFile(null, $"{System.Guid.NewGuid()}.tmp")));

      Assert.True(FileHelper.CreateFile(fname));

      Assert.True(FileHelper.ExistsFile(fname));

      Assert.True(FileHelper.DeleteFile(fname));

      Assert.False(FileHelper.CreateFile(""));
      Assert.False(FileHelper.CreateFile(null));
   }

   [Test]
   public void isDirectory_Test()
   {
      Assert.True(FileHelper.IsDirectory(FileHelper.TempPath));
      Assert.False(FileHelper.IsDirectory(FileHelper.TempFile));

      Assert.False(FileHelper.IsDirectory(""));
      Assert.False(FileHelper.IsDirectory(null));
   }

   [Test]
   public void isFile_Test()
   {
      Assert.False(FileHelper.IsFile(FileHelper.TempPath));
      Assert.True(FileHelper.IsFile(FileHelper.TempFile));

      Assert.False(FileHelper.IsFile(""));
      Assert.False(FileHelper.IsFile(null));
   }

   [Test]
   public void isRoot_Test()
   {
      Assert.False(FileHelper.IsRoot(FileHelper.TempPath));


      Assert.True(FileHelper.IsRoot(@"C:\"));
      Assert.True(FileHelper.IsRoot("C:/"));
      Assert.True(FileHelper.IsRoot("D:"));
      Assert.True(FileHelper.IsRoot("D:/"));
      Assert.False(FileHelper.IsRoot("Ö:"));

      Assert.True(FileHelper.IsRoot("/"));
      Assert.False(FileHelper.IsRoot("//"));

      Assert.False(FileHelper.IsRoot(""));
      Assert.False(FileHelper.IsRoot(null));
   }

   [Test]
   public void GetFileName_Test()
   {
      string? fname = FileHelper.GetFileName(_testTempFile);
      Assert.False(string.IsNullOrEmpty(fname));

      Console.WriteLine(fname);

      fname = FileHelper.GetFileName(_testFileName);
      Assert.That(fname, Is.EqualTo(_testFileName));

      Console.WriteLine(fname);

      fname = FileHelper.GetFileName(_testFilePathWin);
      Assert.That(fname, Is.EqualTo(_testFileName));

      fname = FileHelper.GetFileName(_testFilePathUnix);
      Assert.That(fname, Is.EqualTo(_testFileName));

      fname = FileHelper.GetFileName(_testFilePathUrl);
      Assert.That(fname, Is.EqualTo(_testFileName));
/*
         fname = FileHelper.GetFileName(testFilePathUNC);
         Assert.That(fname, Is.EqualTo(testFileName));
*/
      fname = FileHelper.GetFileName(_testFilePathWinNotok);
      Assert.That(fname, Is.EqualTo(_testFileName));

      fname = FileHelper.GetFileName(_testFilePathUnixNotok);
      Assert.That(fname, Is.EqualTo(_testFileName));

      fname = FileHelper.GetFileName(_testFilePathUrlNotok);
      Assert.That(fname, Is.EqualTo(_testFileName));
/*
         fname = FileHelper.GetFileName(testFilePathUNCNotok);
         Assert.That(fname, Is.EqualTo(testFileName));
*/
      fname = FileHelper.GetFileName("");
      Assert.True(fname == "");

      fname = FileHelper.GetFileName(null);
      Assert.True(fname == null);

      fname = FileHelper.GetFileName("*" + _testFileName);
      Assert.That(fname, Is.EqualTo(_testFileName));
   }

   [Test]
   public void GetDirectoryName_Test()
   {
      string? fname = FileHelper.GetDirectoryName(_testTempPath);
      Assert.False(string.IsNullOrEmpty(fname));

      fname = FileHelper.GetDirectoryName(_testDirectoryWin);
      Assert.That(fname, Is.EqualTo(_testDirectoryName));

      fname = FileHelper.GetDirectoryName(_testDirectoryUnix);
      Assert.That(fname, Is.EqualTo(_testDirectoryName));

      fname = FileHelper.GetDirectoryName(_testDirectoryUrl);
      Assert.That(fname, Is.EqualTo(_testDirectoryName));

      fname = FileHelper.GetDirectoryName(_testDirectoryWinNotok);
      Assert.That(fname, Is.EqualTo(_testDirectoryName));

      fname = FileHelper.GetDirectoryName(_testDirectoryUnixNotok);
      Assert.That(fname, Is.EqualTo(_testDirectoryName));

      fname = FileHelper.GetDirectoryName(_testDirectoryUrlNotok);
      Assert.That(fname, Is.EqualTo(_testDirectoryName));

      fname = FileHelper.GetDirectoryName("");
      Assert.True(fname == "");

      fname = FileHelper.GetDirectoryName(null);
      Assert.True(fname == null);
   }

   [Test]
   public void GetFilesize_Test()
   {
      Assert.That(FileHelper.GetFileSize(FileHelper.TempFile), Is.GreaterThanOrEqualTo(0));

      Assert.That(FileHelper.GetFileSize(_testFileName), Is.EqualTo(-1));

      Assert.That(FileHelper.GetFileSize(FileHelper.TempPath), Is.EqualTo(-1));
      Assert.That(FileHelper.GetFileSize(""), Is.EqualTo(-1));
      Assert.That(FileHelper.GetFileSize(null), Is.EqualTo(-1));
   }

   [Test]
   public void GetExtension_Test()
   {
      Assert.That(FileHelper.GetExtension(FileHelper.TempFile), Is.EqualTo("tmp"));

      Assert.That(FileHelper.GetExtension(_testFileName), Is.EqualTo("png"));

      Assert.That(FileHelper.GetExtension("ct_logo"), Is.EqualTo(null));

      Assert.That(FileHelper.GetExtension(FileHelper.TempPath), Is.EqualTo(null));
      Assert.That(FileHelper.GetExtension(""), Is.EqualTo(null));
      Assert.That(FileHelper.GetExtension(null), Is.EqualTo(null));
   }

   [Test]
   public void GetLastWriteTime_Test()
   {
      Assert.That(FileHelper.GetLastFileWriteTime(FileHelper.TempFile), Is.GreaterThanOrEqualTo(System.DateTime.Today));

      Assert.That(FileHelper.GetLastFileWriteTime(_testFileName), Is.EqualTo(System.DateTime.MinValue));

      Assert.That(FileHelper.GetLastFileWriteTime(FileHelper.TempPath), Is.EqualTo(System.DateTime.MinValue));
      Assert.That(FileHelper.GetLastFileWriteTime(""), Is.EqualTo(System.DateTime.MinValue));
      Assert.That(FileHelper.GetLastFileWriteTime(null), Is.EqualTo(System.DateTime.MinValue));
   }

   [Test]
   public void GetLastAccessTime_Test()
   {
      Assert.That(FileHelper.GetLastFileAccessTime(FileHelper.TempFile), Is.GreaterThanOrEqualTo(System.DateTime.Today));

      Assert.That(FileHelper.GetLastFileAccessTime(_testFileName), Is.EqualTo(System.DateTime.MinValue));

      Assert.That(FileHelper.GetLastFileAccessTime(FileHelper.TempPath), Is.EqualTo(System.DateTime.MinValue));
      Assert.That(FileHelper.GetLastFileAccessTime(""), Is.EqualTo(System.DateTime.MinValue));
      Assert.That(FileHelper.GetLastFileAccessTime(null), Is.EqualTo(System.DateTime.MinValue));
   }

   [Test]
   public void GetCreationTime_Test()
   {
      Assert.That(FileHelper.GetFileCreationTime(FileHelper.TempFile), Is.GreaterThanOrEqualTo(System.DateTime.Today));

      Assert.That(FileHelper.GetFileCreationTime(_testFileName), Is.EqualTo(System.DateTime.MinValue));

      Assert.That(FileHelper.GetFileCreationTime(FileHelper.TempPath), Is.EqualTo(System.DateTime.MinValue));
      Assert.That(FileHelper.GetFileCreationTime(""), Is.EqualTo(System.DateTime.MinValue));
      Assert.That(FileHelper.GetFileCreationTime(null), Is.EqualTo(System.DateTime.MinValue));
   }

   [Test]
   public void GetDirWriteTime_Test()
   {
      Assert.That(FileHelper.GetLastDirectoryWriteTime(FileHelper.TempDirectory), Is.GreaterThanOrEqualTo(System.DateTime.Today));

      Assert.That(FileHelper.GetLastDirectoryWriteTime(""), Is.EqualTo(System.DateTime.MinValue));
      Assert.That(FileHelper.GetLastDirectoryWriteTime(null), Is.EqualTo(System.DateTime.MinValue));
   }

   [Test]
   public void GetDirAccessTime_Test()
   {
      Assert.That(FileHelper.GetLastDirectoryAccessTime(FileHelper.TempDirectory), Is.GreaterThanOrEqualTo(System.DateTime.Today));

      Assert.That(FileHelper.GetLastDirectoryAccessTime(""), Is.EqualTo(System.DateTime.MinValue));
      Assert.That(FileHelper.GetLastDirectoryAccessTime(null), Is.EqualTo(System.DateTime.MinValue));
   }

   [Test]
   public void GetDirCreationTime_Test()
   {
      Assert.That(FileHelper.GetDirectoryCreationTime(FileHelper.TempDirectory), Is.GreaterThanOrEqualTo(System.DateTime.Today));

      Assert.That(FileHelper.GetDirectoryCreationTime(""), Is.EqualTo(System.DateTime.MinValue));
      Assert.That(FileHelper.GetDirectoryCreationTime(null), Is.EqualTo(System.DateTime.MinValue));
   }

   [Test]
   public void WriteAndReadAllText_Test()
   {
      string fname = FileHelper.TempFile;
      const string text = Constants.SIGNS;

      Assert.True(FileHelper.WriteAllText(fname, text));

      Assert.That(FileHelper.ReadAllText(fname), Is.EqualTo(text));

      Assert.True(FileHelper.WriteAllText(fname, null));

      Assert.False(FileHelper.WriteAllText("", text));
      Assert.False(FileHelper.WriteAllText(null, text));

      Assert.That(FileHelper.ReadAllText(""), Is.EqualTo(null));
      Assert.That(FileHelper.ReadAllText(null), Is.EqualTo(null));
   }

   [Test]
   public void WriteAndReadAllLines_Test()
   {
      string fname = FileHelper.TempFile;
      string[] text = [Constants.SIGNS, Constants.SIGNS, Constants.SIGNS];

      Assert.True(FileHelper.WriteAllLines(fname, text));

      Assert.That(FileHelper.ReadAllLines(fname), Is.EqualTo(text));

      Assert.False(FileHelper.WriteAllLines(fname, null));
      Assert.True(FileHelper.WriteAllLines(fname, []));

      Assert.False(FileHelper.WriteAllLines("", text));
      Assert.False(FileHelper.WriteAllLines(null, text));

      Assert.That(FileHelper.ReadAllLines(""), Is.EqualTo(null));
      Assert.That(FileHelper.ReadAllLines(null), Is.EqualTo(null));
   }

   [Test]
   public void WriteAndReadAllBytes_Test()
   {
      string fname = FileHelper.TempFile;
      byte[]? data = Constants.SIGNS.BNToByteArray();

      Assert.True(FileHelper.WriteAllBytes(fname, data));

      Assert.That(FileHelper.ReadAllBytes(fname), Is.EqualTo(data));

      Assert.False(FileHelper.WriteAllBytes(fname, null));
      Assert.True(FileHelper.WriteAllBytes(fname, []));

      Assert.False(FileHelper.WriteAllBytes("", data));
      Assert.False(FileHelper.WriteAllBytes(null, data));

      Assert.That(FileHelper.ReadAllBytes(""), Is.EqualTo(null));
      Assert.That(FileHelper.ReadAllBytes(null), Is.EqualTo(null));
   }

   #endregion

   #region Cleanup

   [TearDown]
   public void CleanUp()
   {
      string? movePath = FileHelper.CreateDirectory(_testTempPath, "CT_TEST");

      FileHelper.DeleteDirectory(movePath);
   }

   #endregion
}