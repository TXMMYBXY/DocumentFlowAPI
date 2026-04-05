using DocumentFlowAPI.Interfaces.Services;

namespace DocumentFlowAPI.Services.FileStorage;

public class FileStorageService : IFileStorageService
{
    private readonly string _rootPath = "/app/storage"; 

    public async Task<string> SaveFileAsync(
        Stream fileStream,
        string fileName,
        string projectFolder)
    {
        var normalizedFolder = projectFolder
            .Replace("\\", "/")
            .Replace("//", "/");

        var normalizedFileName = fileName
            .Replace("\\", "/")
            .Replace("//", "/");

        normalizedFolder = normalizedFolder.Replace("..", "");
        normalizedFileName = Path.GetFileName(normalizedFileName);

        var projectPath = Path.Combine(_rootPath, normalizedFolder);

        Directory.CreateDirectory(projectPath);

        var fullPath = Path.Combine(projectPath, normalizedFileName);

        using var file = new FileStream(fullPath, FileMode.Create);
        
        await fileStream.CopyToAsync(file);

        var normalizedFullPath = fullPath.Replace("\\", "/");

        return normalizedFullPath;
    }

    public Task DeleteFileAsync(string filePath)
    {
        if (File.Exists(filePath))
            File.Delete(filePath);

        return Task.CompletedTask;
    }
}
