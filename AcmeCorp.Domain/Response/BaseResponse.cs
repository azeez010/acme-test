using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcmeCorp.Domain.Response
{
    public class BaseResponse
    {
        public object Data { get; set; }
        public string Message { get; set; }
        public bool Status { get; set; }

        public BaseResponse()
        {

        }
        public BaseResponse(object data, string message, bool status)
        {
            Data = data;
            Message = message;
            Status = status;
        }

        public static BaseResponse Success(object data, string message = "Operation succeeded")
        {
            return new BaseResponse(data, message, true);
        }

        public static BaseResponse Error(object data = null, string message = "Operation failed")
        {
            return new BaseResponse(data, message, false);
        }
    }
}
