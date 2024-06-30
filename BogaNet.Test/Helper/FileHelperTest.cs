using BogaNet.Helper;

namespace BogaNet.Test.Helper;

public class FileHelperTest
{
   #region Variables

   private static readonly string testTempPath = FileHelper.TempPath;
   private static readonly string testTempFile = FileHelper.TempFile;

   private static readonly string testDirectory = $"{FileHelper.CurrentDirectory}Testfiles/";
   private static readonly string testFile = $"{testDirectory}Images/logo_ct.png";

   private const string testDirectoryName = "crosstales LLC";
   private const string testDirectoryWin = $@"C:\Windows\System64\{testDirectoryName}";
   private const string testDirectoryUnix = $"/usr/bin/{testDirectoryName}";
   private const string testDirectoryUrl = $"https://crosstales.com/media/{testDirectoryName}";
   private const string testDirectoryUNC = $@"\\MyComputer\MyShare\{testDirectoryName}";

   private const string testDirectoryWinNotok = $@"C:\Windows/System64\{testDirectoryName}";
   private const string testDirectoryUnixNotok = $@"/usr\bin/{testDirectoryName}";
   private const string testDirectoryUrlNotok = $@"https:/\crosstales.com\media/{testDirectoryName}";
   private const string testDirectoryUNCNotok = $@"\\MyComputer/MyShare/{testDirectoryName}";

   private const string testFileName = "1st ct.logo.png";
   private const string testFilePathWin = $"{testDirectoryWin}\\{testFileName}";
   private const string testFilePathUnix = $"{testDirectoryUnix}/{testFileName}";
   private const string testFilePathUrl = $"{testDirectoryUrl}/{testFileName}";
   private const string testFilePathUNC = $"{testDirectoryUNC}\\{testFileName}";

   private const string testFilePathWinNotok = $"{testDirectoryWinNotok}\\{testFileName}";
   private const string testFilePathUnixNotok = $"{testDirectoryUnixNotok}/{testFileName}";
   private const string testFilePathUrlNotok = $"{testDirectoryUrlNotok}/{testFileName}";
   private const string testFilePathUNCNotok = $"{testDirectoryUNC}\\{testFileName}";

   #endregion

   #region Tests

   [Test]
   public void isUnixPath_Test()
   {
      Assert.False(FileHelper.IsUnixPath(testDirectoryWin));

      Assert.True(FileHelper.IsUnixPath(testDirectoryUnix));

      Assert.False(FileHelper.IsUnixPath(testDirectoryUrl));

      Assert.False(FileHelper.IsUnixPath(testDirectoryUNC));

      Assert.False(FileHelper.IsUnixPath(""));

      Assert.False(FileHelper.IsUnixPath(null));
   }

   [Test]
   public void isWindowsPath_Test()
   {
      Assert.True(FileHelper.IsWindowsPath(testDirectoryWin));

      Assert.False(FileHelper.IsWindowsPath(testDirectoryUnix));

      Assert.False(FileHelper.IsWindowsPath(testDirectoryUrl));

      Assert.False(FileHelper.IsWindowsPath(testDirectoryUNC));

      Assert.False(FileHelper.IsWindowsPath(""));

      Assert.False(FileHelper.IsWindowsPath(null));
   }

   [Test]
   public void isUNCPath_Test()
   {
      Assert.False(FileHelper.IsUNCPath(testDirectoryWin));

      Assert.False(FileHelper.IsUNCPath(testDirectoryUnix));

      Assert.False(FileHelper.IsUNCPath(testDirectoryUrl));

      Assert.True(FileHelper.IsUNCPath(testDirectoryUNC));

      Assert.False(FileHelper.IsUNCPath(""));

      Assert.False(FileHelper.IsUNCPath(null));
   }

   [Test]
   public void isURL_Test()
   {
      Assert.False(FileHelper.IsURL(testDirectoryWin));

      Assert.False(FileHelper.IsURL(testDirectoryUnix));

      Assert.True(FileHelper.IsURL(testDirectoryUrl));

      Assert.False(FileHelper.IsURL(testDirectoryUNC));

      Assert.False(FileHelper.IsURL(""));

      Assert.False(FileHelper.IsURL(null));
   }

   [Test]
   public void ValidatePath_Test()
   {
      string? path = FileHelper.ValidatePath(testTempPath);
      Assert.That(path, Is.EqualTo(testTempPath));

      //Assert.That(FileHelper.ValidatePath("D:/Builds"), Is.EqualTo(@"D:\Builds\"));

      //path = FileHelper.ValidatePath(testDirectory, false);
      //Assert.That(path, Is.EqualTo(testDirectory));


      string tempDir = testTempPath + testDirectoryName;

      FileHelper.CreateDirectory(tempDir);
      path = FileHelper.ValidatePath(tempDir, false);
      Assert.That(path, Is.EqualTo(tempDir));
      FileHelper.DeleteDirectory(tempDir);

      path = FileHelper.ValidatePath(testDirectoryWin, false);
      Assert.That(path, Is.EqualTo(testDirectoryWin));

      path = FileHelper.ValidatePath(testDirectoryUnix, false);
      Assert.That(path, Is.EqualTo(testDirectoryUnix));

      path = FileHelper.ValidatePath(testDirectoryUrl, false);
      Assert.That(path, Is.EqualTo(testDirectoryUrl));

      path = FileHelper.ValidatePath(testDirectoryUNC, false);
      Assert.That(path, Is.EqualTo(testDirectoryUNC));

      path = FileHelper.ValidatePath(testDirectoryWinNotok, false);
      Assert.That(path, Is.EqualTo(testDirectoryWin));

      path = FileHelper.ValidatePath(testDirectoryUnixNotok, false);
      Assert.That(path, Is.EqualTo(testDirectoryUnix));

      path = FileHelper.ValidatePath(testDirectoryUrlNotok, false);
      Assert.That(path, Is.EqualTo(testDirectoryUrl));

      path = FileHelper.ValidatePath(testDirectoryUNCNotok, false);
      Assert.That(path, Is.EqualTo(testDirectoryUNC));

      path = FileHelper.ValidatePath("");
      Assert.True(path == "");

      path = FileHelper.ValidatePath(null);
      Assert.True(path == null);

      path = FileHelper.ValidatePath(testDirectoryWin + "*", false);
      Assert.That(path, Is.EqualTo(testDirectoryWin));
   }

   [Test]
   public void ValidateFile_Test()
   {
      string? path = FileHelper.ValidateFile(testTempFile);
      Assert.That(path, Is.EqualTo(testTempFile));

      path = FileHelper.ValidateFile(testFilePathWin);
      Assert.That(path, Is.EqualTo(testFilePathWin));

      path = FileHelper.ValidateFile(testFilePathUnix);
      Assert.That(path, Is.EqualTo(testFilePathUnix));

      path = FileHelper.ValidateFile(testFilePathUrl);
      Assert.That(path, Is.EqualTo(testFilePathUrl));

      path = FileHelper.ValidateFile(testFilePathUNC);
      Assert.That(path, Is.EqualTo(testFilePathUNC));

      path = FileHelper.ValidateFile(testFilePathWinNotok);
      Assert.That(path, Is.EqualTo(testFilePathWin));

      path = FileHelper.ValidateFile(testFilePathUnixNotok);
      Assert.That(path, Is.EqualTo(testFilePathUnix));

      path = FileHelper.ValidateFile(testFilePathUrlNotok);
      Assert.That(path, Is.EqualTo(testFilePathUrl));

      path = FileHelper.ValidateFile(testFilePathUNCNotok);
      Assert.That(path, Is.EqualTo(testFilePathUNC));

      path = FileHelper.ValidateFile("");
      Assert.True(path == "");

      path = FileHelper.ValidateFile(null);
      Assert.True(path == null);

      path = FileHelper.ValidateFile(testFilePathWin + "*");
      Assert.That(path, Is.EqualTo(testFilePathWin));

      path = FileHelper.ValidateFile(testFilePathWin + ".");
      Assert.That(path, Is.EqualTo(testFilePathWin));
   }

   [Test]
   public void HasPathInvalidChars_Test()
   {
      Assert.False(FileHelper.HasPathInvalidChars(testTempPath));

      Assert.False(FileHelper.HasPathInvalidChars(testDirectoryWin));
      Assert.False(FileHelper.HasPathInvalidChars(testDirectoryUnix));
      Assert.False(FileHelper.HasPathInvalidChars(testDirectoryUrl));
      Assert.False(FileHelper.HasPathInvalidChars(testDirectoryUNC));

      Assert.False(FileHelper.HasPathInvalidChars(""));
      Assert.True(FileHelper.HasPathInvalidChars("", false));
      Assert.False(FileHelper.HasPathInvalidChars(null));
      Assert.True(FileHelper.HasPathInvalidChars(null, false));

      Assert.True(FileHelper.HasPathInvalidChars(testTempPath + "*"));
   }

   [Test]
   public void HasFileInvalidChars_Test()
   {
      Assert.False(FileHelper.HasFileInvalidChars(testTempFile));

      Assert.False(FileHelper.HasFileInvalidChars(testFilePathWin));
      Assert.False(FileHelper.HasFileInvalidChars(testFilePathUnix));
      Assert.False(FileHelper.HasFileInvalidChars(testFilePathUrl));
//         Assert.False(FileHelper.HasFileInvalidChars(testFilePathUNC));

      Assert.False(FileHelper.HasFileInvalidChars(testFileName));

      Assert.False(FileHelper.HasFileInvalidChars(""));
      Assert.True(FileHelper.HasFileInvalidChars("", false));
      Assert.False(FileHelper.HasFileInvalidChars(null));
      Assert.True(FileHelper.HasFileInvalidChars(null, false));

      Assert.False(FileHelper.HasFileInvalidChars("*" + testFileName)); //System.IO.Path.GetFileName corrects the path

      Assert.True(FileHelper.HasFileInvalidChars(testFileName + "."));
   }

   [Test]
   public void GetFilesForName_Test()
   {
      string[] result = FileHelper.GetFilesForName(testDirectory);
      Assert.That(result.Length, Is.EqualTo(1)); //README.md

      result = FileHelper.GetFilesForName(testDirectory, true);
      Assert.That(result.Length, Is.EqualTo(9)); //9 files

      result = FileHelper.GetFilesForName(testDirectory, true, "*");
      Assert.That(result.Length, Is.EqualTo(9)); //9 files

      result = FileHelper.GetFilesForName(testDirectory, true, "logo_");
      Assert.That(result.Length, Is.EqualTo(2)); //2 files

      result = FileHelper.GetFilesForName(testDirectory, true, "MUUUUUUH");
      Assert.That(result.Length, Is.EqualTo(0));

      result = FileHelper.GetFilesForName(testDirectory, true, null);
      Assert.That(result.Length, Is.EqualTo(9)); //9 files

      result = FileHelper.GetFilesForName(testDirectory, true, []);
      Assert.That(result.Length, Is.EqualTo(9)); //9 files

      result = FileHelper.GetFilesForName("");
      Assert.That(result.Length, Is.EqualTo(0));

      result = FileHelper.GetFilesForName(null);
      Assert.That(result.Length, Is.EqualTo(0));
   }

   [Test]
   public void GetFiles_Test()
   {
      string[] result = FileHelper.GetFiles(testDirectory);
      Assert.That(result.Length, Is.EqualTo(1)); //README.md

      result = FileHelper.GetFiles(testDirectory, true);
      Assert.That(result.Length, Is.EqualTo(9)); //9 files

      result = FileHelper.GetFiles(testDirectory, true, "*");
      Assert.That(result.Length, Is.EqualTo(9)); //9 files

      result = FileHelper.GetFiles(testDirectory, true, "png");
      Assert.That(result.Length, Is.EqualTo(2)); //2 files

      result = FileHelper.GetFiles(testDirectory, true, "MUUUUUUH");
      Assert.That(result.Length, Is.EqualTo(0));

      result = FileHelper.GetFiles(testDirectory, true, null);
      Assert.That(result.Length, Is.EqualTo(9)); //9 files

      result = FileHelper.GetFiles(testDirectory, true, []);
      Assert.That(result.Length, Is.EqualTo(9)); //9 files

      result = FileHelper.GetFiles("");
      Assert.That(result.Length, Is.EqualTo(0));

      result = FileHelper.GetFiles(null);
      Assert.That(result.Length, Is.EqualTo(0));
   }

   [Test]
   public void GetDirectories_Test()
   {
      string[] result = FileHelper.GetDirectories(testDirectory);
      Assert.That(result.Length, Is.EqualTo(4));

      result = FileHelper.GetDirectories(testDirectory, true);
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
      string? newPath = FileHelper.Combine(testTempPath, FileHelper.GetDirectoryName(testDirectory));

      Assert.True(FileHelper.CopyDirectory(testDirectory, newPath));

      Assert.True(FileHelper.ExistsDirectory(newPath));

      string? movePath = FileHelper.CreateDirectory(testTempPath, "BN_TEST");

      Assert.True(FileHelper.CopyDirectory(newPath, movePath, true));

      Assert.False(FileHelper.ExistsDirectory(newPath));

      Assert.False(FileHelper.CopyDirectory("", newPath));
      Assert.False(FileHelper.CopyDirectory(null, newPath));
      Assert.False(FileHelper.CopyDirectory(testDirectory, ""));
      Assert.False(FileHelper.CopyDirectory(testDirectory, null));
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
      string? newPath = FileHelper.Combine(testTempPath, FileHelper.GetFileName(testFile));

      Assert.True(FileHelper.CopyFile(testFile, newPath));

      Assert.True(FileHelper.ExistsFile(newPath));

      Assert.True(FileHelper.DeleteFile(newPath));

      Assert.False(FileHelper.CopyFile("", newPath));
      Assert.False(FileHelper.CopyFile(null, newPath));
      Assert.False(FileHelper.CopyFile(testFile, ""));
      Assert.False(FileHelper.CopyFile(testFile, null));
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
      Assert.True(FileHelper.ExistsFile(testFile));

      Assert.False(FileHelper.ExistsFile(testFilePathWin));

      Assert.False(FileHelper.ExistsFile(""));
      Assert.False(FileHelper.ExistsFile(null));
   }

   [Test]
   public void ExistsDirectory_Test()
   {
      Assert.True(FileHelper.ExistsDirectory(FileHelper.TempPath));
      Assert.True(FileHelper.ExistsDirectory(testDirectory));

      Assert.False(FileHelper.ExistsDirectory(testDirectoryWin));

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
      string? fname = FileHelper.GetFileName(testTempFile);
      Assert.False(string.IsNullOrEmpty(fname));

      Console.WriteLine(fname);

      fname = FileHelper.GetFileName(testFileName);
      Assert.That(fname, Is.EqualTo(testFileName));

      Console.WriteLine(fname);

      fname = FileHelper.GetFileName(testFilePathWin);
      Assert.That(fname, Is.EqualTo(testFileName));

      fname = FileHelper.GetFileName(testFilePathUnix);
      Assert.That(fname, Is.EqualTo(testFileName));

      fname = FileHelper.GetFileName(testFilePathUrl);
      Assert.That(fname, Is.EqualTo(testFileName));
/*
         fname = FileHelper.GetFileName(testFilePathUNC);
         Assert.That(fname, Is.EqualTo(testFileName));
*/
      fname = FileHelper.GetFileName(testFilePathWinNotok);
      Assert.That(fname, Is.EqualTo(testFileName));

      fname = FileHelper.GetFileName(testFilePathUnixNotok);
      Assert.That(fname, Is.EqualTo(testFileName));

      fname = FileHelper.GetFileName(testFilePathUrlNotok);
      Assert.That(fname, Is.EqualTo(testFileName));
/*
         fname = FileHelper.GetFileName(testFilePathUNCNotok);
         Assert.That(fname, Is.EqualTo(testFileName));
*/
      fname = FileHelper.GetFileName("");
      Assert.True(fname == "");

      fname = FileHelper.GetFileName(null);
      Assert.True(fname == null);

      fname = FileHelper.GetFileName("*" + testFileName);
      Assert.That(fname, Is.EqualTo(testFileName));
   }

   [Test]
   public void GetDirectoryName_Test()
   {
      string? fname = FileHelper.GetDirectoryName(testTempPath);
      Assert.False(string.IsNullOrEmpty(fname));

      fname = FileHelper.GetDirectoryName(testDirectoryWin);
      Assert.That(fname, Is.EqualTo(testDirectoryName));

      fname = FileHelper.GetDirectoryName(testDirectoryUnix);
      Assert.That(fname, Is.EqualTo(testDirectoryName));

      fname = FileHelper.GetDirectoryName(testDirectoryUrl);
      Assert.That(fname, Is.EqualTo(testDirectoryName));

      fname = FileHelper.GetDirectoryName(testDirectoryWinNotok);
      Assert.That(fname, Is.EqualTo(testDirectoryName));

      fname = FileHelper.GetDirectoryName(testDirectoryUnixNotok);
      Assert.That(fname, Is.EqualTo(testDirectoryName));

      fname = FileHelper.GetDirectoryName(testDirectoryUrlNotok);
      Assert.That(fname, Is.EqualTo(testDirectoryName));

      fname = FileHelper.GetDirectoryName("");
      Assert.True(fname == "");

      fname = FileHelper.GetDirectoryName(null);
      Assert.True(fname == null);
   }

   [Test]
   public void GetFilesize_Test()
   {
      Assert.That(FileHelper.GetFileSize(FileHelper.TempFile), Is.GreaterThanOrEqualTo(0));

      Assert.That(FileHelper.GetFileSize(testFileName), Is.EqualTo(-1));

      Assert.That(FileHelper.GetFileSize(FileHelper.TempPath), Is.EqualTo(-1));
      Assert.That(FileHelper.GetFileSize(""), Is.EqualTo(-1));
      Assert.That(FileHelper.GetFileSize(null), Is.EqualTo(-1));
   }

   [Test]
   public void GetExtension_Test()
   {
      Assert.That(FileHelper.GetExtension(FileHelper.TempFile), Is.EqualTo("tmp"));

      Assert.That(FileHelper.GetExtension(testFileName), Is.EqualTo("png"));

      Assert.That(FileHelper.GetExtension("ct_logo"), Is.EqualTo(null));

      Assert.That(FileHelper.GetExtension(FileHelper.TempPath), Is.EqualTo(null));
      Assert.That(FileHelper.GetExtension(""), Is.EqualTo(null));
      Assert.That(FileHelper.GetExtension(null), Is.EqualTo(null));
   }

   [Test]
   public void GetLastWriteTime_Test()
   {
      Assert.That(FileHelper.GetLastFileWriteTime(FileHelper.TempFile), Is.GreaterThanOrEqualTo(System.DateTime.Today));

      Assert.That(FileHelper.GetLastFileWriteTime(testFileName), Is.EqualTo(System.DateTime.MinValue));

      Assert.That(FileHelper.GetLastFileWriteTime(FileHelper.TempPath), Is.EqualTo(System.DateTime.MinValue));
      Assert.That(FileHelper.GetLastFileWriteTime(""), Is.EqualTo(System.DateTime.MinValue));
      Assert.That(FileHelper.GetLastFileWriteTime(null), Is.EqualTo(System.DateTime.MinValue));
   }

   [Test]
   public void GetLastAccessTime_Test()
   {
      Assert.That(FileHelper.GetLastFileAccessTime(FileHelper.TempFile), Is.GreaterThanOrEqualTo(System.DateTime.Today));

      Assert.That(FileHelper.GetLastFileAccessTime(testFileName), Is.EqualTo(System.DateTime.MinValue));

      Assert.That(FileHelper.GetLastFileAccessTime(FileHelper.TempPath), Is.EqualTo(System.DateTime.MinValue));
      Assert.That(FileHelper.GetLastFileAccessTime(""), Is.EqualTo(System.DateTime.MinValue));
      Assert.That(FileHelper.GetLastFileAccessTime(null), Is.EqualTo(System.DateTime.MinValue));
   }

   [Test]
   public void GetCreationTime_Test()
   {
      Assert.That(FileHelper.GetFileCreationTime(FileHelper.TempFile), Is.GreaterThanOrEqualTo(System.DateTime.Today));

      Assert.That(FileHelper.GetFileCreationTime(testFileName), Is.EqualTo(System.DateTime.MinValue));

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
      string? movePath = FileHelper.CreateDirectory(testTempPath, "CT_TEST");

      FileHelper.DeleteDirectory(movePath);
   }

   #endregion
}