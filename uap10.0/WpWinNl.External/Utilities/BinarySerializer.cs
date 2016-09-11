using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace WpWinNl.Utilities
{
  public class BinarySerializer : IStorage
  {
    public byte[] Data { get; private set; }
    private MemoryStream _myStream;


    /// <summary>
    /// Used when serializing
    /// </summary>
    public BinarySerializer()
    {

    }
    /// <summary>
    /// Used when deserializaing
    /// </summary>
    /// <param name="data"></param>
    public BinarySerializer(byte[] data)
    {
      Data = data;
    }


    #region writing

    private BinaryWriter _writer;
    private void EncodeType(object item, Type storedType)
    {

      if (item == null)
      {
        WriteSimpleValue((ushort)0xFFFE);
        return;
      }

      var itemType = item.GetType().TypeHandle;

      //If this isn't a simple type, then this might be a subclass so we need to
      //store the type 
      if (storedType == null || !storedType.Equals(item.GetType()) || SilverlightSerializer.Verbose)
      {
        //Write the type identifier
        var tpId = SilverlightSerializer.GetTypeId(itemType);
        WriteSimpleValue(tpId);
      }
      else
        //Write a dummy identifier
        WriteSimpleValue((ushort)0xFFFF);
    }

    public bool StartSerializing(Entry entry, int id)
    {
      if (entry.MustHaveName)
      {
        ushort nameID = SilverlightSerializer.GetPropertyDefinitionId(entry.Name);
        WriteSimpleValue(nameID);
      }
      var item = entry.Value ?? new SilverlightSerializer.Nuller();
      EncodeType(item, entry.StoredType);
      return false;
    }

    public void StartSerializing()
    {
      _myStream = new MemoryStream();
      _writer = new BinaryWriter(_myStream);
      SilverlightSerializer.KtStack.Push(SilverlightSerializer.KnownTypes);
      SilverlightSerializer.PiStack.Push(SilverlightSerializer.PropertyIds);
      SilverlightSerializer.KnownTypes = new List<RuntimeTypeHandle>();
      SilverlightSerializer.PropertyIds = new List<string>();
    }

    public void FinishedSerializing()
    {
      _writer.Flush();
      _writer.Dispose();
      _myStream.Flush();
      var data = _myStream.ToArray();
      _myStream.Dispose();
      _myStream = null;

      var stream = new MemoryStream();
      var outputWr = new BinaryWriter(stream);
      outputWr.Write("SerV7");
      //New, store the verbose property
      outputWr.Write(SilverlightSerializer.Verbose);
      outputWr.Write(SilverlightSerializer.KnownTypes.Count);
      foreach (var kt in SilverlightSerializer.KnownTypes.Select(Type.GetTypeFromHandle))
      {
        outputWr.Write(kt.AssemblyQualifiedName);
      }
      outputWr.Write(SilverlightSerializer.PropertyIds.Count);
      foreach (var pi in SilverlightSerializer.PropertyIds)
      {
        outputWr.Write(pi);
      }

      outputWr.Write(data.Length);
      outputWr.Write(data);
      outputWr.Flush();
      outputWr.Dispose();
      stream.Flush();

      Data = stream.ToArray();
      stream.Dispose();
      _writer = null;
      _reader = null;

      SilverlightSerializer.KnownTypes = SilverlightSerializer.KtStack.Pop();
      SilverlightSerializer.PropertyIds = SilverlightSerializer.PiStack.Pop();

    }



    public bool SupportsOnDemand
    {
      get { return false; }
    }
    public void BeginOnDemand(int id) { }
    public void EndOnDemand() { }



    public void BeginWriteObject(int id, Type objectType, bool wasSeen)
    {
      if (wasSeen)
      {
        WriteSimpleValue('S');
        WriteSimpleValue(id);
      }
      else
      {
        WriteSimpleValue('O');
      }
    }






    public void BeginWriteProperties(int count)
    {
      WriteSimpleValue((byte)count);
    }
    public void BeginWriteFields(int count)
    {
      WriteSimpleValue((byte)count);
    }
    public void WriteSimpleValue(object value)
    {
      SilverlightSerializer.WriteValue(_writer, value);
    }
    public void BeginWriteList(int count, Type listType)
    {
      WriteSimpleValue(count);
    }
    public void BeginWriteDictionary(int count, Type dictionaryType)
    {
      WriteSimpleValue(count);
    }



    public void WriteSimpleArray(int count, Array array)
    {
      WriteSimpleValue(count);

      var elementType = array.GetType().GetElementType();
      if (elementType == typeof(byte))
      {
        WriteSimpleValue((byte[])array);
      }
      else if (elementType.GetTypeInfo().IsPrimitive)
      {
        var ba = new byte[Buffer.ByteLength(array)];
        Buffer.BlockCopy(array, 0, ba, 0, ba.Length);
        WriteSimpleValue(ba);
      }
      else
      {
        for (int i = 0; i < count; i++)
        {
          WriteSimpleValue(array.GetValue(i));
        }
      }
    }


    public void BeginMultiDimensionArray(Type arrayType, int dimensions, int count)
    {
      WriteSimpleValue(-1);
      WriteSimpleValue(dimensions);
      WriteSimpleValue(count);
    }
    public void WriteArrayDimension(int dimension, int count)
    {
      WriteSimpleValue(count);
    }
    public void BeginWriteObjectArray(int count, Type arrayType)
    {
      WriteSimpleValue(count);
    }

    public Entry[] ShouldWriteFields(Entry[] fields) { return fields; }
    public Entry[] ShouldWriteProperties(Entry[] properties) { return properties; }




    #endregion writing



    #region reading

    private BinaryReader _reader;
    private Type DecodeType(Type storedType)
    {
      ushort tid = this.ReadSimpleValue<ushort>();
      if (tid == 0xFFFE)
      {
        return null;
      }
      if (tid != 0xffff)
      {
        storedType = Type.GetTypeFromHandle(SilverlightSerializer.KnownTypes[tid]);
      }
      return storedType;

    }

    public void FinishedDeserializing()
    {
      _reader.Dispose();
      _myStream.Dispose();
      _reader = null;
      _myStream = null;
      _writer = null;
      SilverlightSerializer.KnownTypes = SilverlightSerializer.KtStack.Pop();
      SilverlightSerializer.PropertyIds = SilverlightSerializer.PiStack.Pop();
    }

    //Gets the name from the stream
    public void DeserializeGetName(Entry entry)
    {
      if (entry.MustHaveName)
      {
        ushort id = this.ReadSimpleValue<ushort>();
        entry.Name = SilverlightSerializer.PropertyIds[id];
      }
    }

    /// <summary>
    /// Starts to deserialize the object
    /// </summary>
    /// <param name="entry"></param>
    /// <returns></returns>
    public object StartDeserializing(Entry entry)
    {
      var itemType = DecodeType(entry.StoredType);
      entry.StoredType = itemType;
      return null;
    }


    public Entry BeginReadProperty(Entry entry)
    {
      return entry;
    }
    public void EndReadProperty()
    {

    }
    public Entry BeginReadField(Entry entry)
    {
      return entry;
    }
    public void EndReadField()
    {

    }

    public void StartDeserializing()
    {
      SilverlightSerializer.KtStack.Push(SilverlightSerializer.KnownTypes);
      SilverlightSerializer.PiStack.Push(SilverlightSerializer.PropertyIds);

      var stream = new MemoryStream(Data);
      var reader = new BinaryReader(stream);
      var version = reader.ReadString();
      SilverlightSerializer.currentVersion = int.Parse(version.Substring(4));
      if (SilverlightSerializer.currentVersion >= 3)
        SilverlightSerializer.Verbose = reader.ReadBoolean();

      SilverlightSerializer.PropertyIds = new List<string>();
      SilverlightSerializer.KnownTypes = new List<RuntimeTypeHandle>();
      var count = reader.ReadInt32();
      for (var i = 0; i < count; i++)
      {
        var typeName = reader.ReadString();
        var tp = Type.GetType(typeName);
        if (tp == null)
        {
          var map = new SilverlightSerializer.TypeMappingEventArgs
          {
            TypeName = typeName
          };
          SilverlightSerializer.InvokeMapMissingType(map);
          tp = map.UseType;
        }
        if (tp == null)
          throw new ArgumentException(string.Format("Cannot reference type {0} in this context", typeName));
        SilverlightSerializer.KnownTypes.Add(tp.TypeHandle);
      }
      count = reader.ReadInt32();
      for (var i = 0; i < count; i++)
      {
        SilverlightSerializer.PropertyIds.Add(reader.ReadString());
      }

      var data = reader.ReadBytes(reader.ReadInt32());

      _myStream = new MemoryStream(data);
      _reader = new BinaryReader(_myStream);
      reader.Dispose();
      stream.Dispose();
    }

    public void FinishDeserializing(Entry entry) { }





    public Array ReadSimpleArray(Type elementType, int count)
    {
      if (count == -1)
      {
        count = ReadSimpleValue<int>();
      }

      if (elementType == typeof(byte))
      {
        return ReadSimpleValue<byte[]>();
      }
      if (elementType.GetTypeInfo().IsPrimitive && SilverlightSerializer.currentVersion >= 6)
      {
        var ba = ReadSimpleValue<byte[]>();
        var a = Array.CreateInstance(elementType, count);
        Buffer.BlockCopy(ba, 0, a, 0, ba.Length);
        return a;
      }
      var result = Array.CreateInstance(elementType, count);
      for (var l = 0; l < count; l++)
      {
        result.SetValue(this.ReadSimpleValue(elementType), l);
      }
      return result;
    }

    public int BeginReadProperties()
    {
      return this.ReadSimpleValue<byte>();
    }
    public int BeginReadFields()
    {
      return this.ReadSimpleValue<byte>();
    }


    public T ReadSimpleValue<T>()
    {
      return (T)ReadSimpleValue(typeof(T));
    }
    public object ReadSimpleValue(Type type)
    {
      SilverlightSerializer.ReadAValue read;
      if (!SilverlightSerializer.Readers.TryGetValue(type, out read))
      {
        return _reader.ReadInt32();
      }
      return read(_reader);

    }

    public bool IsMultiDimensionalArray(out int length)
    {
      var count = ReadSimpleValue<int>();
      if (count == -1)
      {
        length = -1;
        return true;
      }
      length = count;
      return false;
    }

    public int BeginReadDictionary()
    {
      return ReadSimpleValue<int>(); ;
    }
    public void EndReadDictionary() { }

    public int BeginReadObjectArray()
    {
      return ReadSimpleValue<int>();
    }
    public void EndReadObjectArray() { }


    public void BeginReadMultiDimensionalArray(out int dimension, out int count)
    {
      //
      //var dimensions = storage.ReadValue<int>("dimensions");
      //var totalLength = storage.ReadValue<int>("length");
      dimension = ReadSimpleValue<int>();
      count = ReadSimpleValue<int>();
    }
    public void EndReadMultiDimensionalArray() { }

    public int ReadArrayDimension(int index)
    {
      // //.ReadValue<int>("dim_len" + item);
      return ReadSimpleValue<int>();
    }


    public int BeginReadList()
    {
      return ReadSimpleValue<int>();
    }
    public void EndReadList() { }

    private int _currentObjectID = 0;
    public int BeginReadObject(out bool isReference)
    {
      int result;
      char knownType = this.ReadSimpleValue<char>();
      if (knownType == 'O')
      {
        result = _currentObjectID;
        _currentObjectID++;
        isReference = false;
      }
      else
      {
        result = this.ReadSimpleValue<int>();
        isReference = true;
      }

      return result;
    }


    #endregion reading



    #region do nothing methods

    public void EndWriteObjectArray() { }
    public void EndWriteList() { }
    public void EndWriteDictionary() { }
    public void BeginWriteDictionaryKey(int id) { }
    public void EndWriteDictionaryKey() { }
    public void BeginWriteDictionaryValue(int id) { }
    public void EndWriteDictionaryValue() { }
    public void EndMultiDimensionArray() { }
    public void EndReadObject() { }
    public void BeginWriteListItem(int index) { }
    public void EndWriteListItem() { }
    public void BeginWriteObjectArrayItem(int index) { }
    public void EndWriteObjectArrayItem() { }
    public void EndReadProperties() { }
    public void EndReadFields() { }
    public void BeginReadListItem(int index) { }
    public void EndReadListItem() { }
    public void BeginReadDictionaryKeyItem(int index) { }
    public void EndReadDictionaryKeyItem() { }
    public void BeginReadDictionaryValueItem(int index) { }
    public void EndReadDictionaryValueItem() { }
    public void BeginReadObjectArrayItem(int index) { }
    public void EndReadObjectArrayItem() { }
    public void EndWriteObject() { }
    public void BeginWriteProperty(string name, Type type) { }
    public void EndWriteProperty() { }
    public void BeginWriteField(string name, Type type) { }
    public void EndWriteField() { }
    public void EndWriteProperties() { }
    public void EndWriteFields() { }
    public void FinishSerializing(Entry entry) { }


    #endregion do nothing methods



  }
}