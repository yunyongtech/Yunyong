using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Yunyong.DataExchange.Core.ExpressionX
{
    internal class ValHandle
    {

        private Context DC { get; set; }

        private ValHandle() { }

        internal ValHandle(Context dc)
        {
            DC = dc;
        }

        /*******************************************************************************************************/

        // 03 04
        private bool IsListT(Type type)
        {
            if (type.IsGenericType
                && "System.Collections.Generic".Equals(type.Namespace, StringComparison.OrdinalIgnoreCase)
                && "List`1".Equals(type.Name, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        private bool IsSysType(Type type)
        {
            if (type == typeof(string)
                || type == typeof(ushort)
                || type == typeof(short)
                || type == typeof(uint)
                || type == typeof(int)
                || type == typeof(ulong)
                || type == typeof(long)
                || type == typeof(Guid))
            {
                return true;
            }

            return false;
        }
        // 03 04
        private string InValueForListT(Type type, object vals, bool isArray)
        {
            var str = string.Empty;

            //
            var valType = default(Type);
            var typeT = default(Type);
            if (isArray)
            {
                typeT = type.GetElementType();
            }
            else
            {
                typeT = type.GetGenericArguments()[0];
                if (typeT.IsGenericType
                    && typeT.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    typeT = typeT.GetGenericArguments()[0];
                }
            }

            valType = typeT;

            //
            var ds = vals as dynamic;
            var intVals = new List<object>();
            var num = default(int);
            if (isArray)
            {
                num = ds.Length;
            }
            else
            {
                num = ds.Count;
            }
            for (var i = 0; i < num; i++)
            {
                intVals.Add(DC.GH.GetTypeValue(ds[i]));
            }
            str = string.Join(",", intVals.Select(it => it.ToString()));

            //
            return str;
        }

        private object GetMemObj(Expression exprX,MemberInfo memX)
        {
            if (memX.MemberType == MemberTypes.Field)
            {
                var fInfo = memX as FieldInfo;
                var obj = default(object);
                if (exprX.NodeType == ExpressionType.Constant)
                {
                    var cExpr = exprX as ConstantExpression;
                    obj = cExpr.Value;
                }
                else if (exprX.NodeType == ExpressionType.MemberAccess)
                {
                    var expr = exprX as MemberExpression;
                    obj = GetMemObj(expr.Expression, expr.Member);
                }
                return fInfo.GetValue(obj);
            }
            else if(memX.MemberType== MemberTypes.Property)
            {
                var fInfo = memX as PropertyInfo;
                var obj = default(object);
                if (exprX.NodeType == ExpressionType.Constant)
                {
                    var cExpr = exprX as ConstantExpression;
                    obj = cExpr.Value;
                }
                else if (exprX.NodeType == ExpressionType.MemberAccess)
                {
                    var expr = exprX as MemberExpression;
                    obj = GetMemObj(expr.Expression, expr.Member);
                }
                return fInfo.GetValue(obj);
            }

            return null;
        }


        /*******************************************************************************************************/

        // -02-03-
        internal object GetMemExprVal(MemberExpression memExpr, string funcStr)
        {
            var objx = default(object);
            var fName = string.Empty;

            //
            if (memExpr.Expression == null                                                                                    //  null   Property   
                && memExpr.Member.MemberType == MemberTypes.Property)
            {
                var targetProp = memExpr.Member as PropertyInfo;
                var type = memExpr.Type as Type;
                var instance = Activator.CreateInstance(type);
                fName = targetProp.Name;
                objx = DC.GH.GetTypeValue(targetProp, instance); 
            }
            else if (memExpr.Expression.NodeType == ExpressionType.Constant                     //  Constant   Field 
                && memExpr.Member.MemberType == MemberTypes.Field)
            {
                var fInfo = memExpr.Member as FieldInfo;
                var obj = GetMemObj(memExpr.Expression, memExpr.Member);
                var fType = fInfo.FieldType;
                if (IsListT(fType)
                    || fType.IsArray)
                {
                    var type = memExpr.Type as Type;
                    fName = fInfo.Name;
                    objx = InValueForListT(type, obj, fType.IsArray);
                }
                else
                {
                    fName = fInfo.Name;
                    objx = obj;
                }
            }
            else if (memExpr.Expression.NodeType == ExpressionType.Constant                     //  Constant   Property
                && memExpr.Member.MemberType == MemberTypes.Property)
            {
                var pInfo = memExpr.Member as PropertyInfo;
                var obj = GetMemObj(memExpr.Expression, memExpr.Member);
                var valType = pInfo.PropertyType;
                if (IsListT(valType)
                    || valType.IsArray)
                {
                    fName = pInfo.Name;
                    objx = InValueForListT(valType, obj, valType.IsArray);
                }
                else
                {
                    fName = pInfo.Name;
                    objx = obj;  // DC.GH.GetTypeValue(pInfo, obj);    
                }
            }
            else if (memExpr.Expression.NodeType == ExpressionType.MemberAccess          //  MemberAccess   Property
                && memExpr.Member.MemberType == MemberTypes.Property)
            {
                var targetProp = memExpr.Member as PropertyInfo;
                MemberExpression innerMember = memExpr.Expression as MemberExpression;
                if (innerMember.Member.MemberType == MemberTypes.Property)
                {
                    var valObj = GetMemObj(innerMember.Expression, innerMember.Member);
                    var fType = targetProp.PropertyType;
                    if (IsListT(fType)
                        || fType.IsArray)
                    {
                        var type = memExpr.Type as Type;
                        var vals = targetProp.GetValue(valObj);
                        fName = targetProp.Name;
                        objx = InValueForListT(type, vals, fType.IsArray);
                    }
                    else
                    {
                        fName = targetProp.Name;
                        objx = DC.GH.GetTypeValue(targetProp, valObj);
                    }
                }
                else if (innerMember.Member.MemberType == MemberTypes.Field)
                {
                    var expr = innerMember.Expression;
                    var mem = innerMember.Member;
                    var objX = GetMemObj(expr, mem);
                   
                    fName = targetProp.Name;
                    objx = DC.GH.GetTypeValue(targetProp, objX);
                }
            }

            if (objx == null)
            {
                throw new Exception($"条件筛选表达式【{funcStr}】中,条件值【{fName}】不能为 Null !");
            }

            return objx;
        }
        // 01
        internal object GetCallVal(MethodCallExpression mcExpr, string funcStr)
        {
            var val = default(object);

            //
            var type = mcExpr.Type;
            var pExpr = mcExpr.Arguments[0];
            if (pExpr.NodeType == ExpressionType.Constant
                && type == typeof(DateTime))
            {
                if (mcExpr.Object == null)
                {
                    var con = pExpr as ConstantExpression;
                    val = GetConstantVal(con, con.Type);
                }
                else if (mcExpr.Object.NodeType == ExpressionType.MemberAccess)
                {
                    var obj = Convert.ToDateTime(GetMemExprVal(mcExpr.Object as MemberExpression, funcStr));
                    var method = mcExpr.Method.Name;
                    var args = new List<object>();
                    foreach (var arg in mcExpr.Arguments)
                    {
                        if (arg.NodeType == ExpressionType.Constant)
                        {
                            var carg = arg as ConstantExpression;
                            args.Add(carg.Value);
                        }
                    }
                    val = type.InvokeMember(method, BindingFlags.Default | BindingFlags.InvokeMethod, null, obj, args.ToArray());//.ToString();
                }
            }
            else if (pExpr.NodeType == ExpressionType.Constant)
            {
                var con = pExpr as ConstantExpression;
                val = GetConstantVal(con, con.Type);
            }
            else if (pExpr.NodeType == ExpressionType.MemberAccess)
            {
                val = GetMemExprVal(pExpr as MemberExpression, funcStr);
            }

            if (val != null)
            {
                return val;
            }
            else
            {
                throw new Exception();
            }
        }
        // 01
        internal object GetConstantVal(ConstantExpression con, Type valType)
        {
            if (valType.IsEnum)
            {
                return con.Value; 
            }
            else
            {
                return con.Value; // .ToString();
            }
        }

        // 02
        internal object GetConvertVal(UnaryExpression expr, string funcStr)
        {
            var result = default(object);
            if (expr.Operand.NodeType == ExpressionType.Convert)
            {
                var exprExpr = expr.Operand as UnaryExpression;
                var memExpr = exprExpr.Operand as MemberExpression;
                var memCon = memExpr.Expression as ConstantExpression;
                var memObj = memCon.Value;
                var memFiled = memExpr.Member as FieldInfo;
                result = memFiled.GetValue(memObj);//.ToString();
            }
            else if (expr.Operand.NodeType == ExpressionType.MemberAccess)
            {
                result = DC.VH.GetMemExprVal(expr.Operand as MemberExpression, funcStr);
            }
            else if (expr.Operand.NodeType == ExpressionType.Constant)
            {
                result = DC.VH.GetConstantVal(expr.Operand as ConstantExpression, expr.Operand.Type);
            }
            return result;
        }

        /*******************************************************************************************************/

    }
}
