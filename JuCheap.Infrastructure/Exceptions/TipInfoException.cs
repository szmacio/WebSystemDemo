using System;

namespace JuCheap.Infrastructure.Exceptions
{
    /// <summary>
    /// 业务异常
    /// </summary>
    [Serializable]
    public  class TipInfoException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tipMsg">是用于显示给用户看的首要信息</param>
        /// <param name="errorInfo">所有需要附加给用户知晓的，附加给调用方进行处理的额外信息，通常通过约定进行内容获取的处理</param>
        public TipInfoException(string tipMsg, object errorInfo = null) : base(tipMsg)
        {
            InitErrorInfo(errorInfo);
        }

        public TipInfoException(string tipMsg, Exception innerException, object errorInfo = null) : base(tipMsg, innerException)
        {
            InitErrorInfo(errorInfo);
        }

        private void InitErrorInfo(object errorInfo)
        {
            Data["TipForUI"] = errorInfo;
        }
    }
}
