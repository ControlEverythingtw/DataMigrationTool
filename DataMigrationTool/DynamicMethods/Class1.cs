using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicMethods
{



    public class 机构账号转移相关方法
    {
        public static object 获取Guid(object param)
        {
            return Guid.NewGuid().ToString();
        }
        public static object 转换性别(object param)
        {
            if (param == null)
            {
                return null;
            }
            var sex = param.ToString();

            if (sex == "女")
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }
        public static object 转换状态(object param)
        {
            if (param == null)
            {
                return "1";
            }
            var state = Convert.ToBoolean(param);
            return state ? "0" : "1";
        }
        public static object NotConvert(object param)
        {
            return param;
        }
    }
}
