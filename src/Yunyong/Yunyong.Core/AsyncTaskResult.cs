namespace Yunyong.Core
{
    /// <summary>
    ///     AsyncTaskResult
    /// </summary>
    public class AsyncTaskResult
    {
        ///// <summary>
        ///// Success
        ///// </summary>
        //public static readonly AsyncTaskResult Success = new AsyncTaskResult(AsyncTaskStatus.Success, null);

        /// <summary>
        ///     Initializes a new instance of the <see cref="AsyncTaskResult" /> class.
        /// </summary>
        public AsyncTaskResult() : this(AsyncTaskStatus.Success, null)
        {
        }

        /// <summary>
        ///     AsyncTaskResult
        /// </summary>
        /// <param name="status"></param>
        /// <param name="errorMessage"></param>
        public AsyncTaskResult(AsyncTaskStatus status, string errorMessage)
        {
            Status = status;
            ErrorMessage = errorMessage;
            Status = status;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        ///     Status
        /// </summary>
        public AsyncTaskStatus Status { get; }

        /// <summary>
        ///     ErrorMessage
        /// </summary>
        public string ErrorMessage { get; }

        public static AsyncTaskTResult<T> Success<T>(T data)
        {
            return new AsyncTaskTResult<T>(data);
        }

        public static AsyncTaskTResult<T> Success<T>(string msg, T data)
        {
            return new AsyncTaskTResult<T>(AsyncTaskStatus.Success, msg, data);
        }

        /// <summary>
        ///     Faileds the specified error MSG.
        /// </summary>
        /// <param name="errorMsg">The error MSG.</param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static AsyncTaskTResult<T> Failed<T>(string errorMsg, T target = default(T))
        {
            return new AsyncTaskTResult<T>(AsyncTaskStatus.Failed, errorMsg, target);
        }
    }
}