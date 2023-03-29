namespace LogginServiceAPI.Models.Utilities
{
    /// <summary>
    /// Utilities to assist with interaction, encryption and manipulation of message objects
    /// </summary>
    public interface IMessageUtilities<T> 
            where T : class
    {
        bool Validate(T request);
        T Encrypt(T request);
    }
}
