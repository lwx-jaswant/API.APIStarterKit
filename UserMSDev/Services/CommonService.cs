
namespace UserMSDev.Services
{
    public class CommonService : ICommonService
    {
        private readonly IWebHostEnvironment _iHostingEnvironment;
        public CommonService(IWebHostEnvironment iHostingEnvironment)
        {
            _iHostingEnvironment = iHostingEnvironment;
        }
        public string UploadedFile(IFormFile _IFormFile)
        {
            try
            {
                string FileName = null;
                if (_IFormFile != null)
                {
                    string _FileServerDir = Path.Combine(_iHostingEnvironment.ContentRootPath, "wwwroot/upload");
                    if (_FileServerDir.Contains("\n"))
                    {
                        _FileServerDir.Replace("\n", "/");
                    }

                    if (_IFormFile.FileName == null)
                        FileName = Guid.NewGuid().ToString() + "_" + "blank-person.png";
                    else
                        FileName = Guid.NewGuid().ToString() + "_" + _IFormFile.FileName;

                    string filePath = Path.Combine(_FileServerDir, FileName);
                    //using (var fileStream = new FileStream(filePath, FileMode.Create))
                    using (var fileStream = new FileStream(Path.Combine(_FileServerDir, FileName), FileMode.Create))
                    {
                        _IFormFile.CopyTo(fileStream);
                    }
                }
                return FileName;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string UploadedFile(IFormFile _IFormFile, string CreatedDirName)
        {
            string FileName = null;
            if (_IFormFile != null)
            {
                string _FileServerDir = Path.Combine(_iHostingEnvironment.ContentRootPath, "wwwroot/upload/" + CreatedDirName);
                if (_IFormFile.FileName == null)
                    FileName = Guid.NewGuid().ToString() + "_" + "blank-person.png";
                else
                    FileName = Guid.NewGuid().ToString() + "_" + _IFormFile.FileName;

                string filePath = Path.Combine(_FileServerDir, FileName);
                using (var _FileStream = new FileStream(filePath, FileMode.Create))
                {
                    _IFormFile.CopyTo(_FileStream);
                }
            }
            return FileName;
        }
        public string GetContentPath(string DBFilePath)
        {
            string _FileServerDir = Path.Combine(_iHostingEnvironment.ContentRootPath, "wwwroot");
            var _CombinePath = _FileServerDir + DBFilePath;
            return _CombinePath;
        }
        public string GetServerFileDir()
        {
            string _Path = Path.Combine(_iHostingEnvironment.ContentRootPath, "wwwroot/upload");
            return _Path;
        }
    }
}