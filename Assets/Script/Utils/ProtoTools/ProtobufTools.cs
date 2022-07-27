using ProtoBuf;
using ProtoBuf.Meta;
using System.IO;

public class ProtobufTools
{ 
    public static object Deserialize(System.Type type, byte[] message)
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