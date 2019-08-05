namespace Yunyong.Core
{
    /// <summary>
    ///     AsyncTaskTResult
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AsyncTaskTResult<T> : AsyncTaskResult
    {
        //public static readonly AsyncTaskResult<T> Success = new AsyncTaskResult<T>(AsyncTaskStatus.Success, null);

        public AsyncTaskTResult():this(AsyncTaskStatus.Success, default(T))
        {
            
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AsyncTaskTResult{T}" /> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public AsyncTaskTResult(T data) : this(AsyncTaskStatus.Success, null, data)
        {
        }

        /// <summary>
        ///     Parameterized constructor.
        /// </summary>
        public AsyncTaskTResult(AsyncTaskStatus status, T data)
            : this(status, null, data)
        {
        }

        /// <summary>
        ///     Parameterized constructor.
        /// </summary>
        public AsyncTaskTResult(AsyncTaskStatus status, string errorMessage)
            : this(status, errorMessage, default(T))
        {
        }

        /// <summary>
        ///     Parameterized constructor.
        /// </summary>
        public AsyncTaskTResult(AsyncTaskStatus status, string errorMessage = null, T data = default(T))
            : base(status, errorMessage)
        {
            Data = data;
        }

        /// <summary>
        ///     Represents the async task result data.
        /// </summary>
        public T Data { get; }

        /// <summary>
        ///     Successes the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static AsyncTaskTResult<T> Success(T data)
        {
            return new AsyncTaskTResult<T>(data);
        }

        /// <summary>
        ///     Faileds the specified error MSG.
        /// </summary>
        /// <param name="errorMsg">The error MSG.</param>
        /// <returns></returns>
        public static AsyncTaskTResult<T> Failed(string errorMsg)
        {
            return new AsyncTaskTResult<T>(AsyncTaskStatus.Failed, errorMsg);
        }
    }
}