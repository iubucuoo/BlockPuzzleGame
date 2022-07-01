using CustomDataStruct;

using ProtoBuf.Serializers;
using System.Collections.Generic;

[ProtoBuf.ProtoContract]
public class ArtMgr
{
    [ProtoBuf.ProtoMember(1)]
    public string version;
    [ProtoBuf.ProtoMember(2)]
    public ushort versionNum;
    [ProtoBuf.ProtoMember(3)]
    public ResData[] _ArtsData;
    public List<ResData> ArtsData;
    #region ctor
    public ArtMgr(string _version,ushort num)
    {
        version = _version;
        versionNum = num;
        ArtsData = new List<ResData>();
    }
    public ArtMgr()
    {

    }
    public bool IsNull()
    {
        return version == null || versionNum == 0 || _ArtsData == null || _ArtsData.Length == 0;
    }

    #endregion
}

public class ArtMgrDecorator : ICustomProtoSerializer
{
    public object GetValue(object target, int fieldNumber)
    {
        ArtMgr data = target as ArtMgr;
        if (data == null)
        {
            return null;
        }

        switch (fieldNumber)
        {
            case 1:
                return data.version;
            case 2:
                return data.versionNum;
            case 3:
                return data._ArtsData;            
        }
        return null;
    }


    public void SetValue(object target, object value, int fieldNumber)
    {
        ArtMgr data = target as ArtMgr;
        if (data == null)
        {
            return;
        }

        switch (fieldNumber)
        {
            case 1:
                data.version = value as string;
                break;
            case 2:
                data.versionNum = ValueObject.Value<ushort>(value);
                break;
            case 3:
                data._ArtsData = value as ResData[];
                break;
            default:
                break;
        }
    }
}

public class ResDataDecorator : ICustomProtoSerializer
{
    public object GetValue(object target, int fieldNumber)
    {
        ResData data = target as ResData;
        if (data == null)
        {
            return null;
        }

        switch (fieldNumber)
        {
            case 1:return data.md5;
            case 2:return data.url;
            case 3:return data.name;
            case 4:return data.isFinish;
            case 5:return data.root;
            //case 6:return data.isUI;
            case 7:return data.mapID;
            case 8:return data.type;
            case 9:return data.kb;
        }
        return null;
    }


    public void SetValue(object target, object value, int fieldNumber)
    {
        ResData data = target as ResData;
        if (data == null)
        {
            return;
        }

        switch (fieldNumber)
        {
            case 1:data.md5 = value as string;break;
            case 2:data.url = value as string;break;
            case 3:data.name = value as string;break;
            case 4: data.isFinish= ValueObject.Value<bool>(value);break;
            case 5: data.root= ValueObject.Value<FileRoot>(value);break;
            //case 6: data.isUI=ValueObject.Value<bool>(value);break;
            case 7:data.mapID=ValueObject.Value<ushort>(value);break;
            case 8: data.type=ValueObject.Value<byte>(value);break;
            case 9: data.kb=ValueObject.Value<ushort>(value);break;
        }       
    }
}
