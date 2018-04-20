using System;
using System.Collections.Generic;
using System.Text;

namespace AbsUserCenter.Core.Model.Dto
{
    /// <summary>
    /// api的响应，执行结果操作
    /// </summary>
    public class ApiResponse
    {
        public ApiResponse() { }
        public ApiResponse(bool suc, string msg)
        {
            this.Success = suc; this.Message = msg;
        }
        public bool Success { get; set; }
        public string Message { get; set; }

        public object Data { get; set; }
    }
    /// <summary>
    /// api的响应，执行结果操作
    /// </summary>
    /// <typeparam name="T">响应数据类型</typeparam>
    public class ApiResponse<T> : ApiResponse
    {
        public ApiResponse() { }
        public ApiResponse(bool suc, string msg, T data)
            : base(suc, msg)
        {
            this.Data = data;
        }
        /// <summary>
        /// 返回额外对象
        /// </summary>
        public new T Data { get; set; }
    }
    /// <summary>
    /// 分页结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageList<T>
    {
        public PageList(long count, List<T> data)
        {
            this.Count = count;
            this.Data = data;
        }
        public long Count { get; set; }
        public List<T> Data { get; set; }
    }

    /// <summary>
    /// 分页查询api参数封装
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiReq_SplitPage<T> where T : new()
    {
        /// <summary>
        /// 分页查询api参数封装
        /// </summary>
        public ApiReq_SplitPage()
        {

        }
        /// <summary>
        /// 分页查询api参数封装
        /// </summary>
        /// <param name="ModelData"></param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        public ApiReq_SplitPage(T ModelData, int start, int limit)
        {
            this.ModelData = ModelData;
            this.start = start;
            this.limit = limit;
        }
        /// <summary>
        ///查询结构
        /// </summary>
        public T ModelData { get; set; }
        public int start { get; set; }
        public int limit { get; set; }

    }
}
