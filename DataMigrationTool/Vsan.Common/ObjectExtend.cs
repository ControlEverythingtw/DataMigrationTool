using Vsan.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.Common
{
    /// <summary>
    /// 对Object的扩展
    /// </summary>
    public static class ObjectExtend
    {
     

        /// <summary>
        /// 复制属性值(属性名称一样类型一样且不为空才复制)
        /// </summary>
        /// <typeparam name="Tin"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="tin"></param>
        /// <param name="tout"></param>
        public static void CopyPropValue<Tin,TOut>(this Tin tin,TOut tout)
        {
            var props = tin.GetType().GetProperties();
            var props2 = tout.GetType().GetProperties();

            for (int i = 0; i < props.Length; i++)
            {
                var item = props[i];
                var value = item.GetValue(tin);
                if (value != null)
                {
                    var prop = props2.FirstOrDefault(a => a.Name == item.Name);
                    if (prop != null)
                    {
                        prop.SetValue(tout, value);
                    }
                }
            }

        }

        /// <summary>
        /// 在指定枚举中检索具有指定值的常数的名称。
        /// </summary>
        /// <returns></returns>
        public static string GetName(this Enum enumValue)
        {
            return Enum.GetName(enumValue.GetType(),enumValue);
        }

        private const string NOT_DESC = "无枚举描述";

        /// <summary>
        /// 获取枚举的描述
        /// </summary>
        /// <returns></returns>
        public static string GetDescription(this Enum enumValue)
        {
            var value = enumValue.ToString();
            var field = enumValue.GetType().GetField(value);
            if (field == null)
            {
                return NOT_DESC;
            }
            var descriptionAttribute = field.GetCustomAttribute<DescriptionAttribute>();
            return descriptionAttribute == null ? NOT_DESC : descriptionAttribute.Description;
        }
        /// <summary>
        /// 获取枚举的描述
        /// </summary>
        /// <returns></returns>
        public static T GetCustomAttribute<T>(this Enum enumValue)where T:Attribute
        {
            var value = enumValue.ToString();
            var field = enumValue.GetType().GetField(value);
            if (field == null)
            {
                return default(T);
            }
            var customAttribute = field.GetCustomAttribute<T>();
            return customAttribute;
        }

        /// <summary>
        /// 时间格式转换
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToGwyString(this DateTime source)
        {
            return source.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 获取日期字符串
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string GetDateString(this DateTime source)
        {
            return source.ToString("yyyy-MM-dd");
        }
        


        /// <summary>
        /// 映射
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="tSource">数据源</param>
        /// <param name="tOut">映射到</param>
        /// <returns></returns>
        public static TOut Map<TSource, TOut>(this TSource tSource, TOut tOut) where TOut : new()
        {
            var modelProps = typeof(TOut).GetType().GetProperties();
            var props = tSource.GetType().GetProperties();
            foreach (var item in props)
            {
                var modelP = modelProps.FirstOrDefault(a => a.Name.Equals(item.Name));
                if (modelP != null)
                {
                    var value = item.GetValue(tSource);
                    if (value != null)
                    {
                        modelP.SetValue(tOut, value);
                    }
                }
            }
            return tOut;
        }

        /// <summary>
        /// 转换成Sql字段串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToSqlField<T>(this T source) where T : class
        {
            var type = source.GetType();
            var fields = string.Join(",", type.GetProperties().Select(a => a.Name));
            return fields;
        }
        /// <summary>
        /// 转换成Sql字段串(Query)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string GetFields<T>()
        {
            var type = typeof(T);
            var fields = string.Join(",", type.GetProperties().Select(a => a.Name));
            return fields;
        }

        /// <summary>
        /// 获取查询SQL
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">字段刷选</param>
        /// <param name="tableName">表名</param>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public static string GetSelectSql<T>(Func<PropertyInfo, bool> predicate = null, string tableName = null, string where = " 1=1 ")
        {
            var type = typeof(T);
            IEnumerable<PropertyInfo> elist = type.GetProperties();
            if (predicate != null)
            {
                elist = elist.Where(predicate);
            }
            var fields = string.Join(",", elist.Select(a => $"[{a.Name}]"));
            var sql = $"SELECT {fields} FROM {tableName ?? type.Name} WHERE {where}";
            return sql;
        }

    }

    /// <summary>
    /// 生成表达式目录树  泛型缓存
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    public class ExpressionGenericMapper<TIn, TOut>
    {
        private static Func<TIn, TOut> _FUNC = null;
        static ExpressionGenericMapper()
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(TIn), "p");
            List<MemberBinding> memberBindingList = new List<MemberBinding>();
            foreach (var item in typeof(TOut).GetProperties())
            {
                MemberExpression property = Expression.Property(parameterExpression, typeof(TIn).GetProperty(item.Name));
                MemberBinding memberBinding = Expression.Bind(item, property);
                memberBindingList.Add(memberBinding);
            }
            foreach (var item in typeof(TOut).GetFields())
            {
                MemberExpression property = Expression.Field(parameterExpression, typeof(TIn).GetField(item.Name));
                MemberBinding memberBinding = Expression.Bind(item, property);
                memberBindingList.Add(memberBinding);
            }
            MemberInitExpression memberInitExpression = Expression.MemberInit(Expression.New(typeof(TOut)), memberBindingList.ToArray());
            Expression<Func<TIn, TOut>> lambda = Expression.Lambda<Func<TIn, TOut>>(memberInitExpression, new ParameterExpression[]
            {
                    parameterExpression
            });
            _FUNC = lambda.Compile();//拼装是一次性的
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static TOut Trans(TIn t)
        {
            return _FUNC(t);
        }
    }

}
