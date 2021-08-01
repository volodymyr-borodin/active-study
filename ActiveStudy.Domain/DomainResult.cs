using System.Collections.Generic;
using System.Linq;

namespace ActiveStudy.Domain
{
    public struct DomainResult
    {
        public IEnumerable<DomainError> Errors { get; }
        public bool Success => !Errors.Any();

        private DomainResult(IEnumerable<DomainError> errors = null) => Errors = errors ?? Enumerable.Empty<DomainError>();

        public static DomainResult Successed() => new DomainResult(new DomainError[] { });
        public static DomainResult Error(ErrorCode code) => Error(new DomainError(code.ToString(), string.Empty, code));
        public static DomainResult Error(DomainError error) => Error(new DomainError[] { error });
        public static DomainResult Error(IEnumerable<DomainError> errors) => new DomainResult(errors);
        public static DomainResult Error(string error) => Error(new string[] { error });
        public static DomainResult Error(string error, string internalError) => Error(new DomainError(error, internalError));
        public static DomainResult Error(IEnumerable<string> errors) => new DomainResult(errors.Select(e => new DomainError(e)));

        public static DomainResult<TRes> Successed<TRes>(TRes result) => DomainResult<TRes>.Successed(result);
    }

    public struct DomainResult<TRes>
    {
        public TRes Model { get; }
        public IEnumerable<DomainError> Errors { get; }
        public bool Success => !Errors.Any();

        private DomainResult(IEnumerable<DomainError> errors = null)
        {
            Model = default;
            Errors = errors ?? Enumerable.Empty<DomainError>();
        }

        private DomainResult(TRes result) : this()
        {
            Model = result;
            Errors = Enumerable.Empty<DomainError>();
        }

        public static DomainResult<TRes> Successed(TRes result) => new DomainResult<TRes>(result);
        public static DomainResult<TRes> Error(ErrorCode code) => Error(new DomainError(code.ToString(), string.Empty, code));
        public static DomainResult<TRes> Error(DomainError error) => Error(new DomainError[] { error });
        public static DomainResult<TRes> Error(IEnumerable<DomainError> errors) => new DomainResult<TRes>(errors);
        public static DomainResult<TRes> Error(string error) => Error(new string[] { error });
        public static DomainResult<TRes> Error(string error, string internalError) => Error(new DomainError(error, internalError));
        public static DomainResult<TRes> Error(IEnumerable<string> errors) => new DomainResult<TRes>(errors.Select(e => new DomainError(e)));

        public static implicit operator DomainResult<TRes>(DomainResult result)
        {
            return new DomainResult<TRes>(result.Errors);
        }
    }

    public static class DomainResultExtension
    {
        private const string ErrorMessageSeparator = ". ";

        public static string AsString(this IEnumerable<DomainError> errors)
        {
            if (!errors.Any())
            {
                return string.Empty;
            }

            return string.Join(ErrorMessageSeparator, errors.Select(e => e.Message));
        }
    }
}
