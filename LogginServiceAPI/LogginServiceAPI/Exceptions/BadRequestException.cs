namespace LogginServiceAPI.Exceptions
{
    /// <summary>
    /// Used to signify that the request was badly formatted.  It is assumed that exceptions of this nature are humar-readable
    /// and safe to report back to the end user
    /// </summary>
    public class BadRequestException : Exception
    {
        public BadRequestException() : base(nameof(BadRequestException))
        {
        }

        public BadRequestException(Exception innerException)
            : base(nameof(BadRequestException), innerException)
        {
        }
    }
}
