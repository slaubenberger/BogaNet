using System.Collections.Generic;
using System.Linq;
using BogaNet.BWF.Filter;
using BogaNet.Util;

namespace BogaNet.BWF;

/// <summary>
/// Combines all filters into one.
/// </summary>
public class Pacifier : Singleton<Pacifier>, IFilter
{
   #region Properties

   public virtual IBadWordFilter BadWordFilter { get; set; } = BogaNet.BWF.Filter.BadWordFilter.Instance;
   public virtual ICapitalizationFilter CapitalizationFilter { get; set; } = BogaNet.BWF.Filter.CapitalizationFilter.Instance;
   public virtual IDomainFilter DomainFilter { get; set; } = BogaNet.BWF.Filter.DomainFilter.Instance;
   public virtual IPunctuationFilter PunctuationFilter { get; set; } = BogaNet.BWF.Filter.PunctuationFilter.Instance;

   #endregion

   #region Constructor

   private Pacifier()
   {
   }

   #endregion

   #region Public methods

   public virtual bool Contains(string text, params string[]? sourceNames)
   {
      bool res = CapitalizationFilter.Contains(text);

      if (res)
         return res;

      res = PunctuationFilter.Contains(text);

      if (res)
         return res;

      res = BadWordFilter.Contains(text, sourceNames);

      if (res)
         return res;

      //return DomainFilter.Contains(text, sourceNames);
      return DomainFilter.Contains(text, null);
   }

   public virtual List<string> GetAll(string text, params string[]? sourceNames)
   {
      List<string> result = CapitalizationFilter.GetAll(text);
      result.AddRange(PunctuationFilter.GetAll(text));
      result.AddRange(BadWordFilter.GetAll(text, sourceNames));
      //result.AddRange(DomainFilter.GetAll(text, sourceNames));
      result.AddRange(DomainFilter.GetAll(text, null));

      return result.Distinct().OrderBy(x => x).ToList();
   }

   public virtual string ReplaceAll(string text, params string[]? sourceNames)
   {
      string removedCapitalization = CapitalizationFilter.ReplaceAll(text);
      string removedPunctuation = PunctuationFilter.ReplaceAll(removedCapitalization);
      string removedProfanity = BadWordFilter.ReplaceAll(removedPunctuation, sourceNames);
      //return DomainFilter.ReplaceAll(removedProfanity, sourceNames);
      return DomainFilter.ReplaceAll(removedProfanity, null);
   }

   #endregion
}