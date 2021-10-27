using System;
[ProtoBuf.ProtoContract]
public class Item : TableBase
{
	[ProtoBuf.ProtoMember(1)]
	public long id;
	[ProtoBuf.ProtoMember(2)]
	public string name;
	[ProtoBuf.ProtoMember(3)]
	public short type;
	[ProtoBuf.ProtoMember(4)]
	public short quality;	
	[ProtoBuf.ProtoMember(5)]
	public string drop_icon;
	//[ProtoBuf.ProtoMember(6)]
 //   public int drop_light;

    public override int Key()
    {
        return 0;
    }

    public override long KeyLong()
    {
		return id;
    }
}
public class ItemManager: TableBaseManager
{
	protected override bool isLong { get { return true; } }
    public override bool Open
    {
        get
        {
            return true;
        }
    }
	public override string GetTableName()
	{
		return "Item";
	} 
#region Item
    static ItemManager mInstance;
static ItemManager instance
{
get
{
if (mInstance == null)
{
mInstance = new ItemManager();
}
	return (ItemManager)mInstance;
}
}
protected override Type[] GetTableType()
{
	return new Type[] { typeof(Item[]), typeof(Item) };
}
public static Item[] GetData()
{
	return (Item[])instance._GetData();
}
public static Item GetSingleData(long key)
{
	return (Item)instance._GetSingleData(key);
}
}
#endregion