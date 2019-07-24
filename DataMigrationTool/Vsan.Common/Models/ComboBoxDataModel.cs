using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Vsan.Common.Models
{

    /// <summary>
    /// 下拉框数据模型
    /// </summary>
    public class ComboBoxData
    {
        /// <summary>
        /// 数据编号
        /// </summary>
        [JsonProperty(PropertyName = "Id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        /// <summary>
        /// 数据名称
        /// </summary>
        [JsonProperty(PropertyName = "Name",NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// 显示的文本
        /// </summary>
        [JsonProperty(PropertyName = "Text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }

        /// <summary>
        /// 转Json字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{{\"Id\":\"{Id}\",\"Text\":\"{Text}\"}}";
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="memberTypes">类的成员类型</param>
        /// <param name="startIndex">从startIndex开始</param>
        /// <returns></returns>
        public static ComboBoxData FirstOrDefault<T>(Func<ComboBoxData, bool> predicate, MemberTypes memberTypes = MemberTypes.All, int startIndex = 0)
        {
            return Get<T>(memberTypes, startIndex).FirstOrDefault(predicate);
        }

        /// <summary>
        /// 获取下拉框数据(枚举值必须不为自定义)  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memberTypes">类的成员类型</param>
        /// <param name="startIndex">从startIndex开始</param>
        /// <returns></returns>
        public static IEnumerable<ComboBoxData> Get<T>(MemberTypes memberTypes= MemberTypes.Field, int startIndex=1)
        {
            Type type = typeof(T);
            MemberInfo[] members =null;
            var id = -1;
            var desc = string.Empty;

            switch (memberTypes)
            {
                case MemberTypes.Constructor:
                    members = type.GetConstructors();
                    break;
                case MemberTypes.Event:
                    members = type.GetEvents();
                    break;
                case MemberTypes.Field:
                    members = type.GetFields();
                    break;
                case MemberTypes.Method:
                    members = type.GetMethods();
                    break;
                case MemberTypes.Property:
                    members = type.GetProperties();
                    break;
                case MemberTypes.All:
                    members = type.GetMembers();
                    break;
                default:
                    break;
            }

            for (int i = startIndex; i < members.Length; i++)
            {
                DescriptionAttribute descriptionAttribute = members[i].GetCustomAttribute<DescriptionAttribute>();
                if (descriptionAttribute != null)
                {
                    desc = descriptionAttribute.Description;
                }
                id++;
                yield return new ComboBoxData
                {
                    Id = id.ToString(),
                    Name = members[i].Name,
                    Text = desc
                };
            }
        }


        /// <summary>
        /// 获取下拉框数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memberTypes"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>

        public static IEnumerableData<ComboBoxData> GetIEnumerableData<T>(MemberTypes memberTypes = MemberTypes.Field, int startIndex = 1)
        {
           return  new IEnumerableData<ComboBoxData>(Get<T>(memberTypes, startIndex));
        }
       
    }
    /// <summary>
    /// 枚举的字符串值
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class ComboBoxDataAttribute : Attribute
    {

        /// <summary>
        /// 构造
        /// </summary>
        public ComboBoxDataAttribute(string id,string name,string text)
        {
            ComboBoxData = new ComboBoxData
            {
                Id = id,
                Name = name,
                Text = text
            };
        }
        /// <summary>
        /// 下拉框数据
        /// </summary>
        public ComboBoxData ComboBoxData { get; set; }


       

    }
}
