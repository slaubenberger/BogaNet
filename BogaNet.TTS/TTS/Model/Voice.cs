using BogaNet.TTS.Model.Enum;

namespace BogaNet.TTS.Model;

/// <summary>Model for a voice.</summary>
public class Voice
{
   #region Variables

   /// <summary>Name of the voice.</summary>
   public string Name = string.Empty;

   /// <summary>Culture of the voice.</summary>
   private string culture = string.Empty;

   /// <summary>Description of the voice.</summary>
   public string Description = string.Empty;

   /// <summary>Gender of the voice.</summary>
   public Gender Gender = Gender.UNKNOWN;

   /// <summary>Age of the voice.</summary>
   public string Age = "unknown";

   /// <summary>Identifier of the voice.</summary>
   public string Identifier = string.Empty;

   /// <summary>Vendor of the voice.</summary>
   public string Vendor = "unknown";

   /// <summary>Sample rate in Hz of the voice.</summary>
   public int SampleRate;

   /// <summary>Is the voice neural?</summary>
   public bool isNeural;

   #endregion

   #region Properties

   /// <summary>Culture of the voice (ISO 639-1).</summary>
   public string Culture
   {
      get => culture;

      set
      {
         if (value != null)
            culture = value.Trim().Replace('_', '-');
      }
   }

   /// <summary>Simplified culture of the voice.</summary>
   [System.Xml.Serialization.XmlIgnoreAttribute]
   public string SimplifiedCulture => culture.Replace("-", string.Empty);

   #endregion

   #region Constructors

   /// <summary>Default.</summary>
   public Voice()
   {
      //empty
   }

   /// <summary>Instantiate the class.</summary>
   /// <param name="name">Name of the voice.</param>
   /// <param name="description">Description of the voice.</param>
   /// <param name="gender">Gender of the voice.</param>
   /// <param name="age">Age of the voice.</param>
   /// <param name="culture">Culture of the voice.</param>
   /// <param name="id">Identifier of the voice (optional).</param>
   /// <param name="vendor">Vendor of the voice (optional).</param>
   /// <param name="sampleRate">Sample rate in Hz of the voice (optional).</param>
   /// <param name="neural">Is the voice neural (optional).</param>
   public Voice(string name, string description, Gender gender, string age, string culture, string id = "", string vendor = "unknown", int sampleRate = 0, bool neural = false)
   {
      Name = name;
      Description = description;
      Gender = gender;
      Age = age;
      Culture = culture;
      Identifier = id;
      Vendor = vendor;
      SampleRate = sampleRate;
      isNeural = neural;
   }

   #endregion

   #region Overridden methods

   public override bool Equals(object? obj)
   {
      if (obj == null || GetType() != obj.GetType())
         return false;

      Voice o = (Voice)obj;

      return Name == o.Name &&
             Culture == o.Culture &&
             Description == o.Description &&
             Gender == o.Gender &&
             Age == o.Age &&
             Identifier == o.Identifier &&
             Vendor == o.Vendor &&
             SampleRate == o.SampleRate &&
             isNeural == o.isNeural;
   }

   public override int GetHashCode()
   {
      return base.GetHashCode();
   }

   public override string ToString()
   {
      return $"{Name} ({Culture}, {Gender})";
   }

   #endregion
}