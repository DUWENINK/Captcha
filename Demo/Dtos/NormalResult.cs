using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Dtos
{
    public class NormalResult
    {
        /// <summary>
        /// 是否成功
        /// 构造函数里默认为true了
        /// </summary>
        public bool Successful
        {
            get;
            set;
        }

        /// <summary>
        /// 返回的消息
        /// </summary>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// 用于前端根据不同的原因进行不同的处理
        /// </summary>
        public int Code
        {
            get;
            set;
        }

        /// <summary>
        /// 默认true
        /// </summary>
        public NormalResult()
        {
            Successful = true;
        }

        public NormalResult(bool success)
        {
            this.Successful = success;
        }

        /// <summary>
        /// Success = false;
        /// </summary>
        /// <param name="message"></param>
        public NormalResult(string message)
        {
            Successful = false;
            Message = message;
        }

        public NormalResult(bool success, string message)
        {
            Successful = success;
            Message = message;
        }
    }

    public class NormalResult<T> : NormalResult
    {
        public T Data
        {
            get;
            set;
        }

        public NormalResult()
        {

        }

        public NormalResult(bool success)
            : base(success)
        {

        }

        public NormalResult(string message)
            : base(message)
        {

        }
    }
}
