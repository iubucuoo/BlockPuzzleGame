
using ProtoBuf;
using ProtoBuf.Meta;
using ProtoBuf.Serializers;
using System.IO;
using CustomDataStruct;

public class ProtobufTools:Singleton<ProtobufTools>
{ 
    public ProtobufTools()
    { 
        InitMgr();
    }
    public void InitMgr()
    {
        Init();
        AddProtoPool();
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
    public object Deserialize(System.Type type, byte[] message)
    {
        using (var stream = new MemoryStream(message))
        {
            return RuntimeTypeModel.Default.Deserialize(stream, null, type);
        }
    }





    public static T Deserialize<T>(byte[] message)
    {
        using (var stream = new MemoryStream(message))
        {
            return Serializer.Deserialize<T>(stream);
        }
    }
    public static void SerializeToFile<T>(string path,T msg)
    {
        using (Stream stream =File.Create(path))
        {
            Serializer.Serialize(stream, msg);
            stream.Close();
        }
    }
    public static void SerializeToFile(string path, object msg)
    {
        using (Stream stream = File.Create(path))
        {
            Serializer.Serialize(stream, msg);
            stream.Close();
        }
    }
    public static byte[] Serialize(object msg)
    {
        using(var stream=new MemoryStream())
        {
            Serializer.Serialize(stream, msg);
            return stream.ToArray();
        } 
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
            case 3: return data.type;
            case 4: return data.quality;
            case 5: return data.drop_icon;
            //case 18: return data.drop_light;
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
            case 3: data.type = ValueObject.Value<short>(value); break;
            case 4: data.quality = ValueObject.Value<short>(value); break;
            case 5: data.drop_icon = value as string; break;
            //case 18: data.drop_light = ValueObject.Value<int>(value); break;
        }
    }
}