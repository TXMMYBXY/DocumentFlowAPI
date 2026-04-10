namespace DocumentFlowAPI.Interfaces.Services;

public interface IFileStorageService
{
    Task<string> SaveFileAsync(Stream fileStream, string fileName, string projectFolder);
    Task DeleteFileAsync(string filePath);
}
