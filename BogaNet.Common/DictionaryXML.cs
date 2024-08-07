using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml;
using System.Collections.Generic;

namespace BogaNet;

/// <summary>
/// Serializable Dictionary-class for XML.
/// </summary>
public class DictionaryXML<TKey, TVal> : Dictionary<TKey, TVal>, IXmlSerializable, ISerializable where TKey : notnull
{
   #region Variables

   //private const string DictionaryNodeName = "Dictionary";
   private const string ITEM_NODE_NAME = "Item";
   private const string KEY_NODE_NAME = "Key";
   private const string VALUE_NODE_NAME = "Value";

   private XmlSerializer? _keySerializer;
   private XmlSerializer? _valueSerializer;

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
         KeyValuePair<TKey, TVal> kvp = (KeyValuePair<TKey, TVal>)info.GetValue($"Item{ii}", typeof(KeyValuePair<TKey, TVal>))!;
         Add(kvp.Key, kvp.Value);
      }
   }

   void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
   {
      info.AddValue("ItemCount", Count);
      int itemIdx = 0;
      foreach (KeyValuePair<TKey, TVal> kvp in this)
      {
         info.AddValue($"Item{itemIdx}", kvp, typeof(KeyValuePair<TKey, TVal>));
         itemIdx++;
      }
   }

   #endregion

   #region IXmlSerializable Members

   void IXmlSerializable.WriteXml(XmlWriter writer)
   {
      foreach (KeyValuePair<TKey, TVal> kvp in this)
      {
         writer.WriteStartElement(ITEM_NODE_NAME);
         writer.WriteStartElement(KEY_NODE_NAME);
         KeySerializer.Serialize(writer, kvp.Key);
         writer.WriteEndElement();
         writer.WriteStartElement(VALUE_NODE_NAME);
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

      while (reader.NodeType != XmlNodeType.EndElement)
      {
         reader.ReadStartElement(ITEM_NODE_NAME);
         reader.ReadStartElement(KEY_NODE_NAME);
         if (KeySerializer != null)
         {
            TKey key = (TKey)KeySerializer.Deserialize(reader)!;
            reader.ReadEndElement();
            reader.ReadStartElement(VALUE_NODE_NAME);
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

   private XmlSerializer ValueSerializer => _valueSerializer ??= new XmlSerializer(typeof(TVal));

   private XmlSerializer KeySerializer => _keySerializer ??= new XmlSerializer(typeof(TKey));

   #endregion
}