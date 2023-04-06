namespace LogginServiceAPI.Exceptions
{
    /// <summary>
    /// An exception used to alert to request validation errors.  It is assumed that exceptions of this
    /// nature are human-readable and safe to report back to the end user.
    /// </summary>
    public class ValidationException : Exception
    {
        public ValidationException()
            : base(nameof(BadRequestException))
        {
        }

        public ValidationException(Exception innerException)
            : base(nameof(BadRequestException), innerException)
        {
        }
    }
}
