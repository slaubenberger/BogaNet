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
      Assert.Multiple(() =>
      {
         Assert.That(FileHelper.IsUnixPath(_testDirectoryWin), Is.False);
         Assert.That(FileHelper.IsUnixPath(_testDirectoryUnix), Is.True);
         Assert.That(FileHelper.IsUnixPath(_testDirectoryUrl), Is.False);
         Assert.That(FileHelper.IsUnixPath(_testDirectoryUNC), Is.False);
         Assert.That(FileHelper.IsUnixPath(""), Is.False);
         Assert.That(FileHelper.IsUnixPath(null), Is.False);
      });
   }

   [Test]
   public void isWindowsPath_Test()
   {
      Assert.Multiple(() =>
      {
         Assert.That(FileHelper.IsWindowsPath(_testDirectoryWin), Is.True);
         Assert.That(FileHelper.IsWindowsPath(_testDirectoryUnix), Is.False);
         Assert.That(FileHelper.IsWindowsPath(_testDirectoryUrl), Is.False);
         Assert.That(FileHelper.IsWindowsPath(_testDirectoryUNC), Is.False);
         Assert.That(FileHelper.IsWindowsPath(""), Is.False);
         Assert.That(FileHelper.IsWindowsPath(null), Is.False);
      });
   }

   [Test]
   public void isUNCPath_Test()
   {
      Assert.Multiple(() =>
      {
         Assert.That(FileHelper.IsUNCPath(_testDirectoryWin), Is.False);
         Assert.That(FileHelper.IsUNCPath(_testDirectoryUnix), Is.False);
         Assert.That(FileHelper.IsUNCPath(_testDirectoryUrl), Is.False);
         Assert.That(FileHelper.IsUNCPath(_testDirectoryUNC), Is.True);
         Assert.That(FileHelper.IsUNCPath(""), Is.False);
         Assert.That(FileHelper.IsUNCPath(null), Is.False);
      });
   }

   [Test]
   public void isURL_Test()
   {
      Assert.Multiple(() =>
      {
         Assert.That(FileHelper.IsURL(_testDirectoryWin), Is.False);
         Assert.That(FileHelper.IsURL(_testDirectoryUnix), Is.False);
         Assert.That(FileHelper.IsURL(_testDirectoryUrl), Is.True);
         Assert.That(FileHelper.IsURL(_testDirectoryUNC), Is.False);
         Assert.That(FileHelper.IsURL(""), Is.False);
         Assert.That(FileHelper.IsURL(null), Is.False);
      });
   }

   [Test]
   public void ValidatePath_Test()
   {
      string path = FileHelper.ValidatePath(_testTempPath);
      Assert.That(path, Is.EqualTo(_testTempPath));

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

      path = FileHelper.ValidatePath(_testDirectoryWin + "*", false);
      Assert.That(path, Is.EqualTo(_testDirectoryWin));
   }

   [Test]
   public void ValidateFile_Test()
   {
      string path = FileHelper.ValidateFile(_testTempFile);
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

      path = FileHelper.ValidateFile(_testFilePathWin + "*");
      Assert.That(path, Is.EqualTo(_testFilePathWin));

      path = FileHelper.ValidateFile(_testFilePathWin + ".");
      Assert.That(path, Is.EqualTo(_testFilePathWin));
   }

   [Test]
   public void HasPathInvalidChars_Test()
   {
      Assert.Multiple(() =>
      {
         Assert.That(FileHelper.HasPathInvalidChars(_testTempPath), Is.False);
         Assert.That(FileHelper.HasPathInvalidChars(_testDirectoryWin), Is.False);
         Assert.That(FileHelper.HasPathInvalidChars(_testDirectoryUnix), Is.False);
         Assert.That(FileHelper.HasPathInvalidChars(_testDirectoryUrl), Is.False);
         Assert.That(FileHelper.HasPathInvalidChars(_testDirectoryUNC), Is.False);
         Assert.That(FileHelper.HasPathInvalidChars(""), Is.False);
         Assert.That(FileHelper.HasPathInvalidChars("", false), Is.True);
         Assert.That(FileHelper.HasPathInvalidChars(null), Is.False);
         Assert.That(FileHelper.HasPathInvalidChars(null, false), Is.True);
         Assert.That(FileHelper.HasPathInvalidChars(_testTempPath + "*"), Is.True);
      });
   }

   [Test]
   public void HasFileInvalidChars_Test()
   {
      Assert.Multiple(() =>
      {
         Assert.That(FileHelper.HasFileInvalidChars(_testTempFile), Is.False);
         Assert.That(FileHelper.HasFileInvalidChars(_testFilePathWin), Is.False);
         Assert.That(FileHelper.HasFileInvalidChars(_testFilePathUnix), Is.False);
         Assert.That(FileHelper.HasFileInvalidChars(_testFilePathUrl), Is.False);
         //         Assert.False(FileHelper.HasFileInvalidChars(testFilePathUNC));
         Assert.That(FileHelper.HasFileInvalidChars(_testFileName), Is.False);
         Assert.That(FileHelper.HasFileInvalidChars(""), Is.False);
         Assert.That(FileHelper.HasFileInvalidChars("", false), Is.True);
         Assert.That(FileHelper.HasFileInvalidChars(null), Is.False);
         Assert.That(FileHelper.HasFileInvalidChars(null, false), Is.True);
         Assert.That(FileHelper.HasFileInvalidChars("*" + _testFileName), Is.False); //System.IO.Path.GetFileName corrects the path
         Assert.That(FileHelper.HasFileInvalidChars(_testFileName + "."), Is.True);
      });
   }

   [Test]
   public void GetFilesForName_Test()
   {
      string[] result = FileHelper.GetFilesForName(_testDirectory);
      Assert.That(result, Has.Length.EqualTo(1)); //README.md

      result = FileHelper.GetFilesForName(_testDirectory, true);
      Assert.That(result, Has.Length.EqualTo(9)); //9 files

      result = FileHelper.GetFilesForName(_testDirectory, true, "*");
      Assert.That(result, Has.Length.EqualTo(9)); //9 files

      result = FileHelper.GetFilesForName(_testDirectory, true, "logo_");
      Assert.That(result, Has.Length.EqualTo(2)); //2 files

      result = FileHelper.GetFilesForName(_testDirectory, true, "MUUUUUUH");
      Assert.That(result, Is.Empty);

      result = FileHelper.GetFilesForName(_testDirectory, true, null);
      Assert.That(result, Has.Length.EqualTo(9)); //9 files

      result = FileHelper.GetFilesForName(_testDirectory, true, []);
      Assert.That(result, Has.Length.EqualTo(9)); //9 files
   }

   [Test]
   public void GetFiles_Test()
   {
      string[] result = FileHelper.GetFiles(_testDirectory);
      Assert.That(result, Has.Length.EqualTo(1)); //README.md

      result = FileHelper.GetFiles(_testDirectory, true);
      Assert.That(result, Has.Length.EqualTo(9)); //9 files

      result = FileHelper.GetFiles(_testDirectory, true, "*");
      Assert.That(result, Has.Length.EqualTo(9)); //9 files

      result = FileHelper.GetFiles(_testDirectory, true, "png");
      Assert.That(result, Has.Length.EqualTo(2)); //2 files

      result = FileHelper.GetFiles(_testDirectory, true, "MUUUUUUH");
      Assert.That(result, Is.Empty);

      result = FileHelper.GetFiles(_testDirectory, true, null);
      Assert.That(result, Has.Length.EqualTo(9)); //9 files

      result = FileHelper.GetFiles(_testDirectory, true, []);
      Assert.That(result, Has.Length.EqualTo(9)); //9 files
   }

   [Test]
   public void GetDirectories_Test()
   {
      string[] result = FileHelper.GetDirectories(_testDirectory);
      Assert.That(result, Has.Length.EqualTo(4));

      result = FileHelper.GetDirectories(_testDirectory, true);
      Assert.That(result, Has.Length.EqualTo(7));
   }

   [Test]
   public void GetDrives_Test()
   {
      string[] result = FileHelper.GetDrives();
      Assert.That(result, Is.Not.Empty);
   }

   [Test]
   public void CopyDirectory_Test()
   {
      string newPath = FileHelper.Combine(_testTempPath, FileHelper.GetDirectoryName(_testDirectory));

      Assert.Multiple(() =>
      {
         Assert.That(FileHelper.CopyDirectory(_testDirectory, newPath), Is.True);
         Assert.That(FileHelper.ExistsDirectory(newPath), Is.True);
      });

      string movePath = FileHelper.CreateDirectory(_testTempPath, "BN_TEST");

      Assert.Multiple(() =>
      {
         Assert.That(FileHelper.CopyDirectory(newPath, movePath, true), Is.True);
         Assert.That(FileHelper.ExistsDirectory(newPath), Is.False);
      });
   }

   [Test]
   public void RenameDirectory_Test()
   {
      string fname = FileHelper.CreateDirectory(FileHelper.TempPath, System.Guid.NewGuid().ToString());
      Assert.That(FileHelper.ExistsDirectory(fname), Is.True);

      string fnameNew = FileHelper.RenameDirectory(fname, System.Guid.NewGuid().ToString());

      Assert.Multiple(() =>
      {
         Assert.That(FileHelper.ExistsDirectory(fname), Is.False);
         Assert.That(FileHelper.ExistsDirectory(fnameNew), Is.True);
      });
   }

   [Test]
   public void CopyFile_Test()
   {
      string newPath = FileHelper.Combine(_testTempPath, FileHelper.GetFileName(_testFile));

      Assert.Multiple(() =>
      {
         Assert.That(FileHelper.CopyFile(_testFile, newPath), Is.True);
         Assert.That(FileHelper.ExistsFile(newPath), Is.True);
         Assert.That(FileHelper.DeleteFile(newPath), Is.True);
      });
   }

   [Test]
   public void RenameFile_Test()
   {
      string fname = FileHelper.CreateFile(FileHelper.TempPath, Guid.NewGuid().ToString());
      Assert.That(FileHelper.ExistsFile(fname), Is.True);

      string fnameNew = FileHelper.RenameFile(fname, Guid.NewGuid().ToString());

      Assert.Multiple(() =>
      {
         Assert.That(FileHelper.ExistsFile(fname), Is.False);
         Assert.That(FileHelper.ExistsFile(fnameNew), Is.True);
      });
   }

   [Test]
   public void DeleteFile_Test()
   {
      string tempFile = FileHelper.TempFile;

      Assert.Multiple(() =>
      {
         Assert.That(FileHelper.ExistsFile(tempFile), Is.True);
         Assert.That(FileHelper.DeleteFile(tempFile), Is.True);
         Assert.That(!FileHelper.ExistsFile(tempFile), Is.True);
         Assert.That(FileHelper.DeleteFile(tempFile), Is.False);
      });
   }

   [Test]
   public void DeleteDirectory_Test()
   {
      string folderName = Guid.NewGuid().ToString();
      string newPath = FileHelper.CreateDirectory(FileHelper.TempPath, folderName);

      Assert.Multiple(() =>
      {
         Assert.That(FileHelper.ExistsDirectory(newPath), Is.True);
         Assert.That(FileHelper.DeleteDirectory(newPath), Is.True);
         Assert.That(!FileHelper.ExistsDirectory(newPath), Is.True);
         Assert.That(FileHelper.DeleteDirectory(newPath), Is.False);
      });
   }

   [Test]
   public void ExistsFile_Test()
   {
      Assert.Multiple(() =>
      {
         Assert.That(FileHelper.ExistsFile(FileHelper.TempFile), Is.True);
         //Assert.That(FileHelper.ExistsFile("~/Desktop/Locales.csv"), Is.True);
         Assert.That(FileHelper.ExistsFile(_testFile), Is.True);
         Assert.That(FileHelper.ExistsFile(_testFilePathWin), Is.False);
         Assert.That(FileHelper.ExistsFile(""), Is.False);
         //Assert.That(FileHelper.ExistsFile(null), Is.False);
      });
   }

   [Test]
   public void ExistsDirectory_Test()
   {
      Assert.Multiple(() =>
      {
         Assert.That(FileHelper.ExistsDirectory(FileHelper.TempPath), Is.True);
         //Assert.That(FileHelper.ExistsDirectory("~/Desktop/"), Is.True);
         Assert.That(FileHelper.ExistsDirectory(_testDirectory), Is.True);
         Assert.That(FileHelper.ExistsDirectory(_testDirectoryWin), Is.False);
         Assert.That(FileHelper.ExistsDirectory(""), Is.False);
         //Assert.That(FileHelper.ExistsDirectory(null), Is.False);
      });
   }

   [Test]
   public void CreateDirectory_Test()
   {
      string path = FileHelper.CreateDirectory(FileHelper.TempPath, System.Guid.NewGuid().ToString());

      Assert.Multiple(() =>
      {
         Assert.That(FileHelper.ExistsDirectory(path), Is.True);
         Assert.That(FileHelper.DeleteDirectory(path), Is.True);
         Assert.That(FileHelper.ExistsDirectory(path), Is.False);
         Assert.That(FileHelper.CreateDirectory(path), Is.True);
         Assert.That(FileHelper.ExistsDirectory(path), Is.True);
         Assert.That(FileHelper.DeleteDirectory(path), Is.True);
      });
   }

   [Test]
   public void CreateFile_Test()
   {
      string fname = FileHelper.CreateFile(FileHelper.TempPath, $"{System.Guid.NewGuid()}.tmp");

      Assert.Multiple(() =>
      {
         Assert.That(FileHelper.ExistsFile(fname), Is.True);
         Assert.That(FileHelper.DeleteFile(fname), Is.True);
         Assert.That(FileHelper.ExistsFile(fname), Is.False);
         Assert.That(FileHelper.CreateFile(fname), Is.True);
         Assert.That(FileHelper.ExistsFile(fname), Is.True);
         Assert.That(FileHelper.DeleteFile(fname), Is.True);
      });
   }

   [Test]
   public void isDirectory_Test()
   {
      Assert.Multiple(() =>
      {
         Assert.That(FileHelper.IsDirectory(FileHelper.TempPath), Is.True);
         Assert.That(FileHelper.IsDirectory(FileHelper.TempFile), Is.False);
         Assert.That(FileHelper.IsDirectory(""), Is.False);
         Assert.That(FileHelper.IsDirectory(null), Is.False);
      });
   }

   [Test]
   public void isFile_Test()
   {
      Assert.Multiple(() =>
      {
         Assert.That(FileHelper.IsFile(FileHelper.TempPath), Is.False);
         Assert.That(FileHelper.IsFile(FileHelper.TempFile), Is.True);
         Assert.That(FileHelper.IsFile(""), Is.False);
         Assert.That(FileHelper.IsFile(null), Is.False);
      });
   }

   [Test]
   public void isRoot_Test()
   {
      Assert.Multiple(() =>
      {
         Assert.That(FileHelper.IsRoot(FileHelper.TempPath), Is.False);
         Assert.That(FileHelper.IsRoot(@"C:\"), Is.True);
         Assert.That(FileHelper.IsRoot("C:/"), Is.True);
         Assert.That(FileHelper.IsRoot("D:"), Is.True);
         Assert.That(FileHelper.IsRoot("D:/"), Is.True);
         Assert.That(FileHelper.IsRoot("Ö:"), Is.False);
         Assert.That(FileHelper.IsRoot("/"), Is.True);
         Assert.That(FileHelper.IsRoot("//"), Is.False);
         Assert.That(FileHelper.IsRoot(""), Is.False);
         Assert.That(FileHelper.IsRoot(null), Is.False);
      });
   }

   [Test]
   public void GetFileName_Test()
   {
      string fname = FileHelper.GetFileName(_testTempFile);
      Assert.That(string.IsNullOrEmpty(fname), Is.False);

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
      fname = FileHelper.GetFileName("*" + _testFileName);
      Assert.That(fname, Is.EqualTo(_testFileName));
   }

   [Test]
   public void GetDirectoryName_Test()
   {
      string fname = FileHelper.GetDirectoryName(_testTempPath);
      Assert.That(string.IsNullOrEmpty(fname), Is.False);

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
   }

   [Test]
   public void GetFilesize_Test()
   {
      Assert.Multiple(() =>
      {
         Assert.That(FileHelper.GetFileSize(FileHelper.TempFile), Is.GreaterThanOrEqualTo(0));
         Assert.That(FileHelper.GetFileSize(_testFileName), Is.EqualTo(-1));
         Assert.That(FileHelper.GetFileSize(FileHelper.TempPath), Is.EqualTo(-1));
      });
   }

   [Test]
   public void GetExtension_Test()
   {
      Assert.Multiple(() =>
      {
         Assert.That(FileHelper.GetExtension(FileHelper.TempFile), Is.EqualTo("tmp"));
         Assert.That(FileHelper.GetExtension(_testFileName), Is.EqualTo("png"));
         Assert.That(FileHelper.GetExtension("mp3"), Is.EqualTo("mp3"));
         Assert.That(FileHelper.GetExtension(".mp3"), Is.EqualTo("mp3"));
         Assert.That(FileHelper.GetExtension(FileHelper.TempPath), Is.EqualTo(string.Empty));
      });
   }

   [Test]
   public void GetLastWriteTime_Test()
   {
      Assert.Multiple(() =>
      {
         Assert.That(FileHelper.GetLastFileWriteTime(FileHelper.TempFile), Is.GreaterThanOrEqualTo(System.DateTime.Today));
         Assert.That(FileHelper.GetLastFileWriteTime(_testFileName), Is.EqualTo(System.DateTime.MinValue));
         Assert.That(FileHelper.GetLastFileWriteTime(FileHelper.TempPath), Is.EqualTo(System.DateTime.MinValue));
      });
   }

   [Test]
   public void GetLastAccessTime_Test()
   {
      Assert.Multiple(() =>
      {
         Assert.That(FileHelper.GetLastFileAccessTime(FileHelper.TempFile), Is.GreaterThanOrEqualTo(System.DateTime.Today));
         Assert.That(FileHelper.GetLastFileAccessTime(_testFileName), Is.EqualTo(System.DateTime.MinValue));
         Assert.That(FileHelper.GetLastFileAccessTime(FileHelper.TempPath), Is.EqualTo(System.DateTime.MinValue));
      });
   }

   [Test]
   public void GetCreationTime_Test()
   {
      Assert.Multiple(() =>
      {
         Assert.That(FileHelper.GetFileCreationTime(FileHelper.TempFile), Is.GreaterThanOrEqualTo(System.DateTime.Today));
         Assert.That(FileHelper.GetFileCreationTime(_testFileName), Is.EqualTo(System.DateTime.MinValue));
         Assert.That(FileHelper.GetFileCreationTime(FileHelper.TempPath), Is.EqualTo(System.DateTime.MinValue));
      });
   }

   [Test]
   public void GetDirWriteTime_Test()
   {
      Assert.That(FileHelper.GetLastDirectoryWriteTime(FileHelper.TempDirectory), Is.GreaterThanOrEqualTo(System.DateTime.Today));
   }

   [Test]
   public void GetDirAccessTime_Test()
   {
      Assert.That(FileHelper.GetLastDirectoryAccessTime(FileHelper.TempDirectory), Is.GreaterThanOrEqualTo(System.DateTime.Today));
   }

   [Test]
   public void GetDirCreationTime_Test()
   {
      Assert.That(FileHelper.GetDirectoryCreationTime(FileHelper.TempDirectory), Is.GreaterThanOrEqualTo(System.DateTime.Today));

//      Assert.That(FileHelper.GetDirectoryCreationTime(""), Is.EqualTo(System.DateTime.MinValue));
      //Assert.That(FileHelper.GetDirectoryCreationTime(null), Is.EqualTo(System.DateTime.MinValue));
   }

   [Test]
   public void WriteAndReadAllText_Test()
   {
      string fname = FileHelper.TempFile;
      const string text = Constants.SIGNS;

      Assert.Multiple(() =>
      {
         Assert.That(FileHelper.WriteAllText(fname, text), Is.True);
         Assert.That(FileHelper.ReadAllText(fname), Is.EqualTo(text));
      });
   }

   [Test]
   public void WriteAndReadAllLines_Test()
   {
      string fname = FileHelper.TempFile;
      string[] text = [Constants.SIGNS, Constants.SIGNS, Constants.SIGNS];

      Assert.Multiple(() =>
      {
         Assert.That(FileHelper.WriteAllLines(fname, text), Is.True);
         Assert.That(FileHelper.ReadAllLines(fname), Is.EqualTo(text));
         Assert.That(FileHelper.WriteAllLines(fname, []), Is.True);
      });
   }

   [Test]
   public void WriteAndReadAllBytes_Test()
   {
      string fname = FileHelper.TempFile;
      byte[] data = Constants.SIGNS.BNToByteArray();

      Assert.Multiple(() =>
      {
         Assert.That(FileHelper.WriteAllBytes(fname, data), Is.True);
         Assert.That(FileHelper.ReadAllBytes(fname), Is.EqualTo(data));
         Assert.That(FileHelper.WriteAllBytes(fname, []), Is.True);
      });
   }

   #endregion

   #region Cleanup

   [TearDown]
   public void CleanUp()
   {
      string movePath = FileHelper.CreateDirectory(_testTempPath, "CT_TEST");

      FileHelper.DeleteDirectory(movePath);
   }

   #endregion
}