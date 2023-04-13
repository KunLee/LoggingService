namespace LoggingServiceAPI.Exceptions
{
    /// <summary>
    /// An exception to alert that an attempt was made to access something, where the current security context is not authorised to do so
    /// </summary>
    public class AuthorisationException : CustomizedException
    {
        public AuthorisationException()
            : base(nameof(AuthorisationException))
        {
        }

        public AuthorisationException(Exception innerException)
            : base(nameof(AuthorisationException), innerException)
        {
        }
    }
}
