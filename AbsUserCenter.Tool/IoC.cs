using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbsUserCenter.Tool
{
    public class IoC
    {
        /// <summary>
        /// 容器
        /// </summary>
        private static IContainer Container;
        /// <summary>
        /// 设置容器
        /// </summary>
        /// <param name="_Container"></param>
        public static void SetContainer(IContainer _Container)
        {
            Container = _Container;
        }
        /// <summary>
        /// 解析组件，用完之后必须调用组件的dispose函数释放内存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }
    }
}
