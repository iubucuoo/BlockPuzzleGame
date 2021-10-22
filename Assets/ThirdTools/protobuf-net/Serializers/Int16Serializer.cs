﻿#if !NO_RUNTIME
using CustomDataStruct;
using System;

#if FEAT_IKVM
using Type = IKVM.Reflection.Type;
using IKVM.Reflection;
#else
using System.Reflection;
#endif

namespace ProtoBuf.Serializers
{
    sealed class Int16Serializer : IProtoSerializer
    {
#if FEAT_IKVM
        readonly Type expectedType;
#else
        static readonly Type expectedType = typeof(short);
#endif
        public Int16Serializer(ProtoBuf.Meta.TypeModel model)
        {
#if FEAT_IKVM
            expectedType = model.MapType(typeof(short));
#endif
        }
        public Type ExpectedType { get { return expectedType; } }

        bool IProtoSerializer.RequiresOldValue { get { return false; } }
        bool IProtoSerializer.ReturnsValue { get { return true; } }
#if !FEAT_IKVM
        public object Read(object value, ProtoReader source, bool isNv = false)
        {
            Helpers.DebugAssert(value == null); // since replaces            
            return ValueObject.Get(source.ReadInt16());
        }
        public void Write(object value, ProtoWriter dest)
        {
            ProtoWriter.WriteInt16(ValueObject.Value<short>(value), dest);
        }
#endif
#if FEAT_COMPILER
        void IProtoSerializer.EmitWrite(Compiler.CompilerContext ctx, Compiler.Local valueFrom)
        {
            ctx.EmitBasicWrite("WriteInt16", valueFrom);
        }
        void IProtoSerializer.EmitRead(Compiler.CompilerContext ctx, Compiler.Local valueFrom)
        {
            ctx.EmitBasicRead("ReadInt16", ExpectedType);
        }
#endif

    }
}
#endif