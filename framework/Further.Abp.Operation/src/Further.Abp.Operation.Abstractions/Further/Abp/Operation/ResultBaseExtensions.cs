using FluentResults;
using System;
using System.Collections.Generic;
using System.Text;

namespace Further.Abp.Operation
{
    public static class ResultBaseExtensions
    {
        public static Result WithReason(this IResultBase result, IReason reason)
        {
            var fluentResult = result as Result;
            return fluentResult!.WithReason(reason);
        }

        public static Result WithReasons(this IResultBase result, IEnumerable<IReason> reason)
        {
            var fluentResult = result as Result;
            return fluentResult!.WithReasons(reason);
        }

        public static Result WithError(this IResultBase result, string errorMessage)
        {
            var fluentResult = result as Result;
            return fluentResult!.WithError(errorMessage);
        }

        public static Result WithError(this IResultBase result, IError error)
        {
            var fluentResult = result as Result;
            return fluentResult!.WithError(error);
        }

        public static Result WithErrors(this IResultBase result, IEnumerable<IError> errors)
        {
            var fluentResult = result as Result;
            return fluentResult!.WithErrors(errors);
        }

        public static Result WithErrors(this IResultBase result, IEnumerable<string> errors)
        {
            var fluentResult = result as Result;
            return fluentResult!.WithErrors(errors);
        }

        public static Result WithSuccess(this IResultBase result, string successMessage)
        {
            var fluentResult = result as Result;
            return fluentResult!.WithSuccess(successMessage);
        }

        public static Result WithSuccess(this IResultBase result, ISuccess success)
        {
            var fluentResult = result as Result;
            return fluentResult!.WithSuccess(success);
        }

        public static Result WithSuccesses(this IResultBase result, IEnumerable<ISuccess> successes)
        {
            var fluentResult = result as Result;
            return fluentResult!.WithSuccesses(successes);
        }

        public static Result WithSuccess<TSuccess>(this IResultBase result) where TSuccess : Success, new()
        {
            var fluentResult = result as Result;
            return fluentResult!.WithSuccess<TSuccess>();
        }
    }
}
