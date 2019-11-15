using System;

namespace Rundeck
{
    public class RundeckException : Exception
    {
        public ErrorResponse ErrorResponse;
        
        public RundeckException(ErrorResponse errorResponse): base(errorResponse.Message)
        {
            this.ErrorResponse = errorResponse;
        }
    }
}