using LibGit2Sharp;

namespace MultilingualJournal.Services
{
    public class GitService : IGitService
    {
        private readonly string _repositoryPath;
        private readonly ILogger<GitService> _logger;

        public GitService(ILogger<GitService> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            // Use the project root directory as the Git repository path
            _repositoryPath = FindGitRepositoryRoot(environment.ContentRootPath);
        }

        public async Task<bool> CreateMilestoneTagAsync(string tagName, string message)
        {
            try
            {
                if (!IsGitRepositoryInitialized())
                {
                    _logger.LogWarning("Git repository not initialized at path: {Path}", _repositoryPath);
                    return false;
                }

                await Task.Run(() =>
                {
                    using var repo = new Repository(_repositoryPath);
                    
                    // Sanitize tag name to be Git-compatible (replace spaces with hyphens, lowercase)
                    var gitTagName = $"milestone-{SanitizeTagName(tagName)}";
                    
                    // Check if tag already exists
                    if (repo.Tags[gitTagName] != null)
                    {
                        _logger.LogInformation("Git tag {TagName} already exists", gitTagName);
                        return;
                    }

                    // Get the current HEAD commit
                    var headCommit = repo.Head.Tip;
                    if (headCommit == null)
                    {
                        _logger.LogWarning("No commits found in repository");
                        return;
                    }

                    // Create annotated tag
                    var signature = GetGitSignature();
                    var tag = repo.ApplyTag(gitTagName, headCommit, signature, message);
                    
                    _logger.LogInformation("Created Git tag: {TagName} at commit {CommitSha}", gitTagName, headCommit.Sha[..8]);
                });

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create Git tag: {TagName}", tagName);
                return false;
            }
        }

        public async Task<IEnumerable<string>> GetAllTagsAsync()
        {
            try
            {
                if (!IsGitRepositoryInitialized())
                {
                    return Enumerable.Empty<string>();
                }

                return await Task.Run(() =>
                {
                    using var repo = new Repository(_repositoryPath);
                    return repo.Tags
                        .Where(t => t.FriendlyName.StartsWith("milestone-"))
                        .Select(t => t.FriendlyName)
                        .OrderByDescending(t => t)
                        .ToList();
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get Git tags");
                return Enumerable.Empty<string>();
            }
        }

        public async Task<bool> DeleteTagAsync(string tagName)
        {
            try
            {
                if (!IsGitRepositoryInitialized())
                {
                    return false;
                }

                await Task.Run(() =>
                {
                    using var repo = new Repository(_repositoryPath);
                    var gitTagName = $"milestone-{SanitizeTagName(tagName)}";
                    
                    var tag = repo.Tags[gitTagName];
                    if (tag != null)
                    {
                        repo.Tags.Remove(tag);
                        _logger.LogInformation("Deleted Git tag: {TagName}", gitTagName);
                    }
                });

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete Git tag: {TagName}", tagName);
                return false;
            }
        }

        public bool IsGitRepositoryInitialized()
        {
            try
            {
                return !string.IsNullOrEmpty(_repositoryPath) && Repository.IsValid(_repositoryPath);
            }
            catch
            {
                return false;
            }
        }

        private string FindGitRepositoryRoot(string startPath)
        {
            var currentPath = startPath;
            
            while (!string.IsNullOrEmpty(currentPath))
            {
                if (Directory.Exists(Path.Combine(currentPath, ".git")))
                {
                    return currentPath;
                }
                
                var parentPath = Directory.GetParent(currentPath)?.FullName;
                if (parentPath == currentPath) // Reached root directory
                    break;
                    
                currentPath = parentPath;
            }
            
            return startPath; // Fallback to start path if no Git repo found
        }

        private string SanitizeTagName(string tagName)
        {
            // Convert to lowercase and replace invalid characters with hyphens
            return tagName.ToLowerInvariant()
                .Replace(" ", "-")
                .Replace("_", "-")
                .Replace("/", "-")
                .Replace("\\", "-")
                .Replace(":", "-")
                .Replace("*", "")
                .Replace("?", "")
                .Replace("\"", "")
                .Replace("<", "")
                .Replace(">", "")
                .Replace("|", "")
                .Trim('-');
        }

        private Signature GetGitSignature()
        {
            // Try to get user info from Git config, fallback to default values
            try
            {
                using var repo = new Repository(_repositoryPath);
                var config = repo.Config;
                
                var name = config.Get<string>("user.name")?.Value ?? "Multilingual Journal App";
                var email = config.Get<string>("user.email")?.Value ?? "journal@localhost";
                
                return new Signature(name, email, DateTimeOffset.Now);
            }
            catch
            {
                return new Signature("Multilingual Journal App", "journal@localhost", DateTimeOffset.Now);
            }
        }
    }
}