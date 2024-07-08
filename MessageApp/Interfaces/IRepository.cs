namespace MessageApp.Interfaces
{
    public interface IRepository
    {
        Task<bool> IsEmailUniqueAsync(string email);
    }
}
