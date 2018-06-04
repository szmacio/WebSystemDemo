using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Web;

namespace JuCheap.Infrastructure.Exceptions
{
    public static class ExceptionExtensions
    {
        /// <summary>
        /// 是否是提示类的异常
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static bool IsTipOrRequestValidationException(this Exception exception)
        {
            return exception is TipInfoException || exception is HttpRequestValidationException;
        }

        /// <summary>
        /// 获取友好的错误信息
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static string GetFriendlyMessage(this Exception exception)
        {
            if (exception == null)
            {
                return string.Empty;
            }

            var friendlyMsg = exception.Message;

            var aggregateException = exception as AggregateException;
            if (aggregateException != null)
            {
                aggregateException = aggregateException.Flatten();
                if (aggregateException.InnerExceptions.Any())
                {
                    friendlyMsg = aggregateException.InnerExceptions
                                      .Aggregate(friendlyMsg, 
                                      (msgAccumulator, exNext) =>
                                        {
                                            msgAccumulator += $"{Environment.NewLine}{exNext.Message}";
                                            return msgAccumulator;
                                        });
                }
            }

            var entityValidationException = exception as DbEntityValidationException;
            if (entityValidationException != null)
            {
                var sb = new StringBuilder();
                sb.AppendLine();
                foreach (var entry in entityValidationException.EntityValidationErrors)
                {
                    foreach (var error in entry.ValidationErrors)
                    {
                        sb.AppendLine($"字段 {error.PropertyName} : {error.ErrorMessage}");
                    }
                }

                friendlyMsg += sb.ToString();
            }

            return friendlyMsg;
        }
    }
}
