using System;
using DFC.App.Account.Application.Common.Interfaces;

namespace DFC.App.Account.Application.Common
{
    public struct Result : IResult
    {
        private static readonly Result OkResult = new Result(false, null);

        public string Error { get; }
        public bool IsSuccess => !IsFailure;
        public bool IsFailure { get; }

        private Result(bool isFailure, string error)
        {
            if (isFailure && string.IsNullOrWhiteSpace(error))
                throw new ArgumentException($"For a failure {nameof(error)} cannot be null, empty or only whitespace.");

            IsFailure = isFailure;
            Error = error;
        }

        public static Result Ok()
        {
            return OkResult;
        }

        public static Result Fail(string error)
        {
            return new Result(true, error);
        }
    }
}
