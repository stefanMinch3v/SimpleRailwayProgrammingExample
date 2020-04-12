namespace RailwayProgrammingExample.Common
{
    using System;

    public static class ResultExtensions
    {
        public static Result<T> ToResult<T>(this Maybe<T> maybe, string errorMsg)
            where T : class
        {
            if (maybe.HasNoValue)
            {
                return Result.Fail<T>(errorMsg);
            }

            return Result.Ok(maybe.Value);
        }

        public static Result<int> ToResult(this int value)
            => Result.Ok(value);

        public static Result OnSuccess(this Result result, Action action)
        {
            if (result.IsFailure)
            {
                return result;
            }

            action();

            return Result.Ok();
        }

        public static Result<T> OnSuccess<T>(this Result<T> result, Action<T> action)
        {
            if (result.IsSuccess)
            {
                action(result.Value);
            }

            return result;
        }

        public static Result OnSuccess(this Result result, Func<Result> func)
        {
            if (result.IsFailure)
            {
                return result;
            }

            return func();
        }

        public static Result<K> OnSuccess<T, K>(this Result<T> result, Func<T, K> func)
        {
            if (result.IsFailure)
            {
                return Result.Fail<K>(result.ErrorMsg);
            }

            return Result.Ok(func(result.Value));
        }

        public static Result OnFailure(this Result result, Action action)
        {
            if (result.IsFailure)
            {
                action();
            }

            return result;
        }

        public static Result OnBoth(this Result result, Action<Result> action)
        {
            action(result);

            return result;
        }

        public static T OnBoth<T>(this Result result, Func<Result, T> func)
            => func(result);

        public static Result<T> Ensure<T>(this Result<T> result, Func<T, bool> predicate, string errorMsg)
        {
            if (result.IsFailure)
            {
                return result;
            }

            if (!predicate(result.Value))
            {
                return Result.Fail<T>(errorMsg);
            }

            return result;
        }

        public static Result<K> Map<T, K>(this Result<T> result, Func<T, K> func)
        {
            if (result.IsFailure)
            {
                return Result.Fail<K>(result.ErrorMsg);
            }

            return Result.Ok(func(result.Value));
        }
    }
}
