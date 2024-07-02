namespace UserMSDev.Services
{
    public interface ICommonService
    {
        string UploadedFile(IFormFile ProfilePicture);
        string UploadedFile(IFormFile _IFormFile, string CreatedDirName);
        string GetContentPath(string DBFilePath);
        string GetServerFileDir();
    }
}
