namespace RailwayProgrammingExample.Common
{
	using System;

	// Result class can also be called Monad
	// db level result or domain
	// it might also have nullable enum type error for managing error messages
	// and catch it in the service with switch statement then return custom error message

	/// <exception cref="InvalidOperationException">Thrown when success is true and message is not empty or success is false and message is empty.</exception>
	public class Result
	{
		protected Result(bool success, string errorMsg)
		{
			if (success && !string.IsNullOrEmpty(errorMsg))
			{
				throw new InvalidOperationException(nameof(Result));
			}

			if (!success && string.IsNullOrEmpty(errorMsg))
			{
				throw new InvalidOperationException(nameof(Result));
			}

			this.IsSuccess = success;
			this.ErrorMsg = errorMsg;
		}

		public bool IsSuccess { get; }

		public bool IsFailure => !IsSuccess;

		public string ErrorMsg { get; }

		public static Result Fail(string errorMsg)
			=> new Result(false, errorMsg);

		public static Result<T> Fail<T>(string errorMsg)
			=> new Result<T>(default, false, errorMsg);

		public static Result Ok()
			=> new Result(true, string.Empty);

		public static Result<T> Ok<T>(T value)
			=> new Result<T>(value, true, string.Empty);

		// combines only success results
		public static Result Combine(params Result[] results)
		{
			foreach (var result in results)
			{
				if (result.IsFailure)
				{
					return result;
				}
			}

			return Ok();
		}
	}

	public class Result<T> : Result
	{
		private readonly T value;

		protected internal Result(T value, bool success, string errorMsg)
			: base(success, errorMsg)
			=> this.value = value;

		public T Value
		{
			get
			{
				if (base.IsFailure)
				{
					throw new InvalidOperationException("Cannot set the value if the Success state is false!");
				}

				return this.value;
			}
		}
	}
}
