using BogaNet.Util;
using BogaNet.Test.Testfiles;

namespace BogaNet.Test.Util;

public class MimeTypeMapTest
{
   [Test]
   public void GetMimeType_Test()
   {
      string mime = MimeTypeMap.GetMimeType(".mp3");
      const string refMime = "audio/mpeg";
      Assert.That(mime, Is.EqualTo(refMime));

      mime = MimeTypeMap.GetMimeType("stefan.mp3");
      Assert.That(mime, Is.EqualTo(refMime));

      mime = MimeTypeMap.GetMimeType("http://www.crosstales.com/stefan.mp3");
      Assert.That(mime, Is.EqualTo(refMime));

      mime = MimeTypeMap.GetMimeType("mp3");
      Assert.That(mime, Is.EqualTo(refMime));

      mime = MimeTypeMap.GetMimeType("MP3");
      Assert.That(mime, Is.EqualTo(refMime));

      mime = MimeTypeMap.GetMimeType("boganet");
      Assert.That(mime, Is.EqualTo("text/html"));
   }

   [Test]
   public void GetExtension_Test()
   {
      string ext = MimeTypeMap.GetExtension("audio/mpeg");
      const string refExt = "mp3";
      Assert.That(ext, Is.EqualTo(refExt));

      ext = MimeTypeMap.GetExtension("AUDIO/mpeg");
      Assert.That(ext, Is.EqualTo(refExt));

      ext = MimeTypeMap.GetExtension("AUDIO/boganet");
      Assert.That(ext, Is.EqualTo("html"));
   }
}