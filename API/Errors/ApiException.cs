using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Errors
{
    public class ApiException : ApiResponse
    {
        public ApiException(int StatusCode, string message=null, string details = null) : base(StatusCode,message)
        {
            Details =  details;
        }

        public string Details { get; set; }
    }
}