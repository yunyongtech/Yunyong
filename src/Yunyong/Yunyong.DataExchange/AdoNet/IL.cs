using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using Yunyong.DataExchange.Core;
using Yunyong.DataExchange.Core.Helper;

namespace Yunyong.DataExchange.AdoNet
{
    internal struct IL
    {
        private static void EmitInt32(ILGenerator il, int value)
        {
            switch (value)
            {
                case -1: il.Emit(OpCodes.Ldc_I4_M1); break;
                case 0: il.Emit(OpCodes.Ldc_I4_0); break;
                case 1: il.Emit(OpCodes.Ldc_I4_1); break;
                case 2: il.Emit(OpCodes.Ldc_I4_2); break;
                case 3: il.Emit(OpCodes.Ldc_I4_3); break;
                case 4: il.Emit(OpCodes.Ldc_I4_4); break;
                case 5: il.Emit(OpCodes.Ldc_I4_5); break;
                case 6: il.Emit(OpCodes.Ldc_I4_6); break;
                case 7: il.Emit(OpCodes.Ldc_I4_7); break;
                case 8: il.Emit(OpCodes.Ldc_I4_8); break;
                default:
                    if (value >= -128 && value <= 127)
                    {
                        il.Emit(OpCodes.Ldc_I4_S, (sbyte)value);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldc_I4, value);
                    }
                    break;
            }
        }
        private static void StoreLocal(ILGenerator il, int index)
        {
            if (index < 0 || index >= short.MaxValue) throw new ArgumentNullException(nameof(index));
            switch (index)
            {
                case 0: il.Emit(OpCodes.Stloc_0); break;
                case 1: il.Emit(OpCodes.Stloc_1); break;
                case 2: il.Emit(OpCodes.Stloc_2); break;
                case 3: il.Emit(OpCodes.Stloc_3); break;
                default:
                    if (index <= 255)
                    {
                        il.Emit(OpCodes.Stloc_S, (byte)index);
                    }
                    else
                    {
                        il.Emit(OpCodes.Stloc, (short)index);
                    }
                    break;
            }
        }
        private static Func<IDataReader, object> SetFunc(Type mType, IDataReader reader)
        {
            // 
            var dm = new DynamicMethod("MyDAL" + Guid.NewGuid().ToString(), mType, new[] { typeof(IDataReader) }, mType, true);
            var il = dm.GetILGenerator();
            il.DeclareLocal(typeof(int));    // 定义 loc0  int
            il.DeclareLocal(mType);    //  定义 loc1 M
            il.Emit(OpCodes.Ldc_I4_0);
            il.Emit(OpCodes.Stloc_0);   //  赋值 loc0 = 0

            //
            var length = reader.FieldCount;
            var names = Enumerable.Range(0, length).Select(i => reader.GetName(i)).ToArray();
            var typeMap = AdoNetHelper.GetTypeMap(mType);
            int index = 0;
            var ctor = typeMap.DefaultConstructor();

            //
            il.Emit(OpCodes.Newobj, ctor);
            il.Emit(OpCodes.Stloc_1);   //  赋值 loc1 = new M();
            il.BeginExceptionBlock();    //  try begin 
            il.Emit(OpCodes.Ldloc_1);    //   加载 loc1 

            //
            var members = names.Select(n => typeMap.GetMember(n)).ToList();
            bool first = true;
            var allDone = il.DefineLabel();
            int enumDeclareLocal = -1;
            int valueCopyLocal = il.DeclareLocal(typeof(object)).LocalIndex;    // 定义 loc_object 变量, 然后返回此本地变量的index值, 其实就是截止目前, 定义了本地变量的个数            
            foreach (var item in members)
            {
                if (item != null)
                {
                    il.Emit(OpCodes.Dup); 
                    Label isDbNullLabel = il.DefineLabel();
                    Label finishLabel = il.DefineLabel();

                    il.Emit(OpCodes.Ldarg_0); 
                    EmitInt32(il, index); 
                    il.Emit(OpCodes.Dup);
                    il.Emit(OpCodes.Stloc_0);
                    il.Emit(OpCodes.Callvirt, XConfig.GetItem);   //  获取reader读取的值, reader[index]
                    il.Emit(OpCodes.Dup); 
                    StoreLocal(il, valueCopyLocal);   //  将 reader[index]的值, 存放到本地变量 loc_object 中
                    Type colType = reader.GetFieldType(index);    // reader[index] 的列的类型  source
                    Type memberType = item.MemberType();   //  M[item] 的类型   

                    if (memberType == typeof(char) || memberType == typeof(char?))
                    {
                        il.EmitCall(
                            OpCodes.Call, 
                            typeof(AdoNetHelper).GetMethod(
                                memberType == typeof(char) 
                                ? nameof(AdoNetHelper.ReadChar) 
                                : nameof(AdoNetHelper.ReadNullableChar), BindingFlags.Static | BindingFlags.NonPublic),
                            null); 
                    }
                    else
                    {
                        il.Emit(OpCodes.Dup); 
                        il.Emit(OpCodes.Isinst, typeof(DBNull));    //  判断是否为DBNull类型, 如果是, 则跳转到 标签isDbNullLabel 
                        il.Emit(OpCodes.Brtrue_S, isDbNullLabel); 
                        
                        var nullUnderlyingType = Nullable.GetUnderlyingType(memberType);
                        var unboxType = nullUnderlyingType?.IsEnum == true ? nullUnderlyingType : memberType;

                        if (unboxType.IsEnum)
                        {
                            Type numericType = Enum.GetUnderlyingType(unboxType);
                            if (colType == typeof(string))
                            {
                                if (enumDeclareLocal == -1)
                                {
                                    enumDeclareLocal = il.DeclareLocal(typeof(string)).LocalIndex;
                                }
                                il.Emit(OpCodes.Castclass, typeof(string)); // stack is now [target][target][string]
                                StoreLocal(il, enumDeclareLocal); // stack is now [target][target]
                                il.Emit(OpCodes.Ldtoken, unboxType); // stack is now [target][target][enum-type-token]
                                il.EmitCall(OpCodes.Call, typeof(Type).GetMethod(nameof(Type.GetTypeFromHandle)), null);// stack is now [target][target][enum-type]
                                AdoNetHelper.LoadLocal(il, enumDeclareLocal); // stack is now [target][target][enum-type][string]
                                il.Emit(OpCodes.Ldc_I4_1); // stack is now [target][target][enum-type][string][true]
                                il.EmitCall(OpCodes.Call, XConfig.EnumParse, null); // stack is now [target][target][enum-as-object]
                                il.Emit(OpCodes.Unbox_Any, unboxType); // stack is now [target][target][typed-value]
                            }
                            else
                            {
                                AdoNetHelper.FlexibleConvertBoxedFromHeadOfStack(il, colType, unboxType, numericType);
                            }

                            if (nullUnderlyingType != null)
                            {
                                il.Emit(OpCodes.Newobj, memberType.GetConstructor(new[] { nullUnderlyingType })); // stack is now [target][target][typed-value]
                            }
                        }
                        else if (memberType.FullName == XConfig.LinqBinary)
                        {
                            il.Emit(OpCodes.Unbox_Any, typeof(byte[])); // stack is now [target][target][byte-array]
                            il.Emit(OpCodes.Newobj, memberType.GetConstructor(new Type[] { typeof(byte[]) }));// stack is now [target][target][binary]
                        }
                        else
                        {
                            TypeCode dataTypeCode = Type.GetTypeCode(colType), unboxTypeCode = Type.GetTypeCode(unboxType);
                            if (colType == unboxType || dataTypeCode == unboxTypeCode || dataTypeCode == Type.GetTypeCode(nullUnderlyingType))
                            {
                                il.Emit(OpCodes.Unbox_Any, unboxType); // stack is now [target][target][typed-value]
                            }
                            else
                            {
                                // not a direct match; need to tweak the unbox
                                AdoNetHelper.FlexibleConvertBoxedFromHeadOfStack(il, colType, nullUnderlyingType ?? unboxType, null);
                                if (nullUnderlyingType != null)
                                {
                                    il.Emit(OpCodes.Newobj, unboxType.GetConstructor(new[] { nullUnderlyingType })); // stack is now [target][target][typed-value]
                                }
                            }
                        }
                    }
                    // Store the value in the property/field
                    if (item.Property != null)
                    {
                        il.Emit(OpCodes.Callvirt, AdoNetHelper.GetPropertySetter(item.Property, mType));
                    }
                    else
                    {
                        il.Emit(OpCodes.Stfld, item.Field); // stack is now [target]
                    }

                    il.Emit(OpCodes.Br_S, finishLabel); // stack is now [target]
                    il.MarkLabel(isDbNullLabel); // incoming stack: [target][target][value]
                    il.Emit(OpCodes.Pop); // stack is now [target][target]
                    il.Emit(OpCodes.Pop); // stack is now [target]
                    il.MarkLabel(finishLabel);
                }
                first = false;
                index++;
            }

            il.Emit(OpCodes.Stloc_1); // stack is empty
            il.MarkLabel(allDone);
            il.BeginCatchBlock(typeof(Exception)); // stack is Exception
            il.Emit(OpCodes.Ldloc_0); // stack is Exception, index
            il.Emit(OpCodes.Ldarg_0); // stack is Exception, index, reader
            AdoNetHelper.LoadLocal(il, valueCopyLocal); // stack is Exception, index, reader, value
            il.EmitCall(OpCodes.Call, typeof(AdoNetHelper).GetMethod(nameof(AdoNetHelper.ThrowDataException)), null);
            il.EndExceptionBlock();

            il.Emit(OpCodes.Ldloc_1); // stack is [rval]
            il.Emit(OpCodes.Ret);

            var funcType = Expression.GetFuncType(typeof(IDataReader), mType);
            return (Func<IDataReader, object>)dm.CreateDelegate(funcType);
        }

        internal static Row Row(Type mType, IDataReader reader)
        {
            return new Row
            {
                Handle = SetFunc(mType, reader)
            };
        }
    }
}
