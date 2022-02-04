using WorkingWithFiles.Shared;
using WorkingWithFiles.Shared.Model;

namespace WorkingWithFiles.Server.Service
{
    public class FileManagerService
    {
        public List<FileModel> GetFiles(string path)
        {
            //path = Cripto.EncodeDecrypt(path);
            DirectoryInfo dir = new DirectoryInfo(path);

            List<FileModel> filesList = new List<FileModel>();

            try
            {
                foreach (var files in dir.GetFiles())
                {
                    filesList.Add(new FileModel
                    {
                        FileName = files.Name,
                        FullName = files.FullName
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Не удалось открыть файл: " + ex.Message);
            }

            return filesList;
        }

        public void DeleteFile(FileModel fileModel)
        {
            if (File.Exists(fileModel.FullName))
            {
                File.Delete(fileModel.FullName);
            }
        }

        public async Task<List<string?>> UploadFile(SaveFile saveFile)
        {
            List<string?> fileExists = new List<string?>();
            foreach (var file in saveFile.Files)
            {
                if (File.Exists($"Data\\{file.FileName}"))
                {
                    fileExists.Add(file.FileName);
                }
                else
                {
                    using (var fileStream = File.Create($"Data\\{file.FileName}"))
                    {
                        await fileStream.WriteAsync(file.Data);
                    }
                }
            }
            return fileExists;
        }
    }
}
