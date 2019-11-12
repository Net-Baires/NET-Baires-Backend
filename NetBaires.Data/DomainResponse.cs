using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetBaires.Data
{
    public class DomainResponse
    {
        public bool SuccessResult { get; }
        public List<string> Errors { get; set; } = new List<string>();

        public DomainResponse Success(Func<Task> callback)
        {
            if (SuccessResult)
                callback().Wait();
            return this;
        }

        public DomainResponse Error(Func<List<string>, Task> callback)
        {
            if (!SuccessResult)
                callback(Errors).Wait(); ;
            return this;
        }

        public static DomainResponse Error(List<string> errors)
        {
            return new DomainResponse(errors);
        }

        public static DomainResponse Ok()
        {
            return new DomainResponse();
        }
        public static DomainResponse Error(string error)
        {
            return new DomainResponse(new List<string> { error });
        }
        public static DomainResponse Error()
        {
            return new DomainResponse(new List<string>());
        }

        protected DomainResponse()
        {
            SuccessResult = true;
        }


        protected DomainResponse(List<string> errors)
        {
            SuccessResult = false;
            Errors = new List<string>();
            Errors.AddRange(errors);
        }
    }
    public class DomainResult<TResponse>
    {
        public bool SuccessResult { get; }
        public List<string> Errors { get; set; } = new List<string>();
        public TResponse Response { get; set; }

        public DomainResult<TResponse> Success(Func<TResponse, Task> callback)
        {
            if (SuccessResult)
                callback(Response).Wait();
            return this;
        }
        public TResult Success<TResult>(Func<TResponse, Task<TResult>> callback)
        {
            if (SuccessResult)
                return callback(Response).Result;
            return default(TResult);
        }

        public DomainResult<TResponse> Error(Func<List<string>, Task> callback)
        {
            if (!SuccessResult)
                callback(Errors).Wait();
            return this;
        }

        public static DomainResult<TResponse> Error(List<string> errors) =>
            new DomainResult<TResponse>(errors);
        public static DomainResult<TResponse> Error() =>
            new DomainResult<TResponse>(new List<string>());

        public static DomainResult<TResponse> Ok(TResponse response) =>
            new DomainResult<TResponse>(response);
        public static DomainResult<TResponse> Error(string error) =>
            new DomainResult<TResponse>(new List<string> { error });

        protected DomainResult(TResponse response)
        {
            SuccessResult = true;
            Response = response;
        }


        protected DomainResult(List<string> errors)
        {
            SuccessResult = false;
            Errors = new List<string>();
            Errors.AddRange(errors);
        }

    }
}