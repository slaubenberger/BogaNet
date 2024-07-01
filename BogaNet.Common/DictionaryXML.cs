using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml;
using System.Collections.Generic;

namespace BogaNet;

/// <summary>
/// Serializable Dictionary-class for XML.
/// </summary>
[System.Serializable]
public class DictionaryXML<TKey, TVal> : Dictionary<TKey, TVal>, IXmlSerializable, ISerializable where TKey : notnull
{
   #region Variables

   //private const string DictionaryNodeName = "Dictionary";
   private const string ItemNodeName = "Item";
   private const string KeyNodeName = "Key";
   private const string ValueNodeName = "Value";

   private XmlSerializer? keySerializer;
   private XmlSerializer? valueSerializer;

   #endregion

   #region Constructors

   public DictionaryXML()
   {
      //empty
   }

   public DictionaryXML(IDictionary<TKey, TVal> dictionary) : base(dictionary)
   {
      //empty
   }

   public DictionaryXML(IEqualityComparer<TKey> comparer) : base(comparer)
   {
      //empty
   }

   public DictionaryXML(int capacity) : base(capacity)
   {
      //empty
   }

   public DictionaryXML(IDictionary<TKey, TVal> dictionary, IEqualityComparer<TKey> comparer) : base(dictionary, comparer)
   {
      //empty
   }

   public DictionaryXML(int capacity, IEqualityComparer<TKey> comparer) : base(capacity, comparer)
   {
      //empty
   }

   #endregion

   #region ISerializable Members

   protected DictionaryXML(SerializationInfo info, StreamingContext context)
   {
      int itemCount = info.GetInt32("ItemCount");
      for (int ii = 0; ii < itemCount; ii++)
      {
         System.Collections.Generic.KeyValuePair<TKey, TVal> kvp = (KeyValuePair<TKey, TVal>)info.GetValue($"Item{ii}", typeof(KeyValuePair<TKey, TVal>))!;
         Add(kvp.Key, kvp.Value);
      }
   }

   void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
   {
      info.AddValue("ItemCount", Count);
      int itemIdx = 0;
      foreach (KeyValuePair<TKey, TVal> kvp in this)
      {
         info.AddValue($"Item{itemIdx}", kvp, typeof(System.Collections.Generic.KeyValuePair<TKey, TVal>));
         itemIdx++;
      }
   }

   #endregion

   #region IXmlSerializable Members

   void IXmlSerializable.WriteXml(XmlWriter writer)
   {
      foreach (KeyValuePair<TKey, TVal> kvp in this)
      {
         writer.WriteStartElement(ItemNodeName);
         writer.WriteStartElement(KeyNodeName);
         KeySerializer.Serialize(writer, kvp.Key);
         writer.WriteEndElement();
         writer.WriteStartElement(ValueNodeName);
         ValueSerializer.Serialize(writer, kvp.Value);
         writer.WriteEndElement();
         writer.WriteEndElement();
      }
   }

   void IXmlSerializable.ReadXml(XmlReader? reader)
   {
      if (reader == null || reader.IsEmptyElement)
         return;

      if (!reader.Read())
         throw new XmlException("Error in Deserialization of Dictionary");

      while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
      {
         reader.ReadStartElement(ItemNodeName);
         reader.ReadStartElement(KeyNodeName);
         if (KeySerializer != null)
         {
            TKey key = (TKey)KeySerializer.Deserialize(reader)!;
            reader.ReadEndElement();
            reader.ReadStartElement(ValueNodeName);
            TVal value = (TVal)ValueSerializer.Deserialize(reader)!;
            reader.ReadEndElement();
            reader.ReadEndElement();
            Add(key, value);
         }

         reader.MoveToContent();
      }

      reader.ReadEndElement();
   }

   XmlSchema? IXmlSerializable.GetSchema()
   {
      return null;
   }

   #endregion

   #region Private Properties

   private XmlSerializer ValueSerializer => valueSerializer ??= new XmlSerializer(typeof(TVal));

   private XmlSerializer KeySerializer => keySerializer ??= new XmlSerializer(typeof(TKey));

   #endregion
}