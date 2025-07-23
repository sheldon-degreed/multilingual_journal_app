namespace MultilingualJournal.Services
{
    public interface IGitService
    {
        Task<bool> CreateMilestoneTagAsync(string tagName, string message);
        Task<IEnumerable<string>> GetAllTagsAsync();
        Task<bool> DeleteTagAsync(string tagName);
        bool IsGitRepositoryInitialized();
    }
}