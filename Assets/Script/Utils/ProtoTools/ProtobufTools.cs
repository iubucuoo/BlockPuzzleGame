
using ProtoBuf;
using ProtoBuf.Meta;
using ProtoBuf.Serializers;
using System.IO;
using System;
using CustomDataStruct;
public interface IMgr
{
    void InitMgr();
    void ResetMgr();
}
public class ProtobufTools:IMgr
{
    static ProtobufTools inst;
    public ProtobufTools()
    {
        inst = this;
        InitMgr();
    }
    public void Init()
    {
        CustomSetting.AddCustomSerializer(typeof(Item), new ItemDecorator());
    }
    void AddProtoPool()
    {        
        //ProtoFactory.AddProtoPool(typeof(BuffData), new BuffDataPool());
        //ProtoFactory.AddProtoPool(typeof(EffectModel), new EffectModelPool());
    }

    public static void SerializeToFile<T>(string path,T msg)
    {
        if (inst==null)
        {
            new ProtobufTools();
        }
        using (Stream stream =File.Create(path))
        {
            Serializer.Serialize(stream, msg);
            stream.Close();
            stream.Dispose();
        }
    }

    public static byte[] Serialize(object msg)
    {
        byte[] result;
        using(var stream=new MemoryStream())
        {
            Serializer.Serialize(stream, msg);
            result = stream.ToArray();
        }
        return result;
    }

    public static T Deserialize<T>(byte[] message)
    {
        if (inst == null)
        {
            new ProtobufTools();
        }
        T result;
        using(var stream=new MemoryStream(message))
        {
            result = Serializer.Deserialize<T>(stream);
        }
        return result;
    }
    public static object Deserialize(System.Type type, byte[] message)
    {
        object result;
        using (var stream = new MemoryStream(message))
        {
            result = RuntimeTypeModel.Default.Deserialize(stream, null, type);
        }
        return result;
    }

    ProtoBuf.Meta.RuntimeTypeModel model;
    public void InitMgr()
    {
        model = ProtoBuf.Meta.RuntimeTypeModel.Default;
        Init();
        AddProtoPool();
        model.netDataPoolDelegate = ProtoFactory.Get;
        model.bufferPoolDelegate = StreamBufferPool.GetBuffer;
    }

    public void ResetMgr()
    {
        throw new NotImplementedException();
    }
}


public class ItemDecorator : ICustomProtoSerializer
{
    public object GetValue(object target, int fieldNumber)
    {
        Item data = target as Item;
        if (data == null)
        {
            return null;
        }


        switch (fieldNumber)
        {
            case 1: return data.id;
            case 2: return data.name;
            case 8: return data.type;
            case 15: return data.quality;
            case 17: return data.drop_icon;
            case 18: return data.drop_light;
        }
        return null;
    }

    public void SetValue(object target, object value, int fieldNumber)
    {
        Item data = target as Item;
        if (data == null)
        {
            return;
        }
        switch (fieldNumber)
        {
            case 1: data.id = ValueObject.Value<long>(value); break;
            case 2: data.name = value as string; break;
            case 8: data.type = ValueObject.Value<short>(value); break;
            case 15: data.quality = ValueObject.Value<short>(value); break;
            case 17: data.drop_icon = value as string; break;
            case 18: data.drop_light = ValueObject.Value<int>(value); break;
        }
    }
}