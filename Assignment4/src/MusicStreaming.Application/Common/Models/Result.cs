using System.Collections.Generic;
using System.Linq;

namespace MusicStreaming.Application.Common.Models
{
    public class Result
    {
        public bool Succeeded { get; protected set; }
        public IEnumerable<string> Errors { get; protected set; }

        protected Result(bool succeeded, IEnumerable<string> errors)
        {
            Succeeded = succeeded;
            Errors = errors ?? new List<string>();
        }

        public static Result Success()
        {
            return new Result(true, Array.Empty<string>());
        }

        public static Result Failure(IEnumerable<string> errors)
        {
            return new Result(false, errors);
        }
    }

    public class Result<T> : Result
    {
        public T Data { get; set; }

        protected Result(T data, bool succeeded, IEnumerable<string> errors) 
            : base(succeeded, errors)
        {
            Data = data;
        }

        public static Result<T> Success(T data)
        {
            return new Result<T>(data, true, Array.Empty<string>());
        }

        public new static Result<T> Failure(IEnumerable<string> errors)
        {
            return new Result<T>(default, false, errors);
        }
    }
}