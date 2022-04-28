using System;

namespace HireMe.Core.Helpers
{
    public class OperationResult
    {
        protected OperationResult(int? id)
        {
            this.Success = true;
            this.Id = id;
        }
        protected OperationResult(string message)
        {
            this.Success = false;
            this.FailureMessage = message;
        }
        protected OperationResult(Exception ex)
        {
            this.Success = false;
            this.Exception = ex;
        }
        public bool Success { get; protected set; }
        public string FailureMessage { get; protected set; }
        public int? Id { get; set; }
        public Exception Exception { get; protected set; }
        public static OperationResult SuccessResult(int? id)
        {
            return new OperationResult(id);
        }
        public static OperationResult FailureResult(string message)
        {
            return new OperationResult(message);
        }
        public static OperationResult ExceptionResult(Exception ex)
        {
            return new OperationResult(ex);
        }
        public bool IsException()
        {
            return this.Exception != null;
        }

    }
}
