using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.Common
{
    /// <summary>
    /// 数组集合操作
    /// </summary>
    public static class ArrayHelper
    {


        /// <summary>
        /// 切分成二维数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> arr, int size = 1000)
        {

            if (arr == null)
            {
                throw new CheckException(ResultCode.ArgumentException, "参数异常 arr");
            }

            var count = arr.Count();

            if (size < 0 || size >= count)
            {
                throw new CheckException(ResultCode.ArgumentException, "参数异常 size");
            }
            var tCount = count % size == 0 ? count / size : (count / size) + 1;

            for (int i = 0; i < tCount; i++)
            {
                var pageList = arr.Skip(i * size).Take(size);

                yield return pageList;
            }

        }


        /// <summary>
        /// 切分成二维数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static IEnumerable<List<T>> Split<T>(List<T> arr, int size = 1000)
        {

            if (arr == null)
            {
                throw new CheckException(ResultCode.ArgumentException, "参数异常 arr");
            }
            if (size < 0 || size >= arr.Count)
            {
                throw new CheckException(ResultCode.ArgumentException, "参数异常 size");
            }
            var tCount = arr.Count % size == 0 ? arr.Count / size : (arr.Count / size) + 1;

            for (int i = 0; i < tCount; i++)
            {
                var pageList = arr.Skip(i*size).Take(size).ToList();

                yield return pageList;
            }

        }


        /// <summary>
        /// 切分成二维数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [Obsolete]
        public static List<List<T>> Split2<T>(List<T> arr,int size=1000)
        {

            if (arr==null)
            {
                throw new CheckException(ResultCode.ArgumentException,"参数异常 arr");
            }
            if (size<0||size>=arr.Count)
            {
                throw new CheckException(ResultCode.ArgumentException, "参数异常 size");
            }

            List<List<T>> two_Dimensional_Array = new List<List<T>>();

            var tCount = arr.Count % size == 0 ? arr.Count / size : (arr.Count / size) + 1;

            for (int i = 0; i < tCount; i++)
            {
                var pageList = arr.Skip(i * size).Take(size).ToList();

                two_Dimensional_Array.Add(pageList);
            }
            return two_Dimensional_Array;

        }

    }
}
