using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todoy.Web
{
    public static class ExceptionExtensions
    {
        public static object AsLoggable(this Exception exception)
        {
            return new
            {
                Type = exception.GetType().Name,
                Message = exception.Message,
                InnerException =
                    exception.InnerException != null ? 
                        exception.InnerException.AsLoggable() : 
                        null,
                StackTrace = exception.StackTrace,
                Data = exception.Data
            };
        }
    }
}
