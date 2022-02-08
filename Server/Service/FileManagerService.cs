using WorkingWithFiles.Shared;
using WorkingWithFiles.Shared.Model;

namespace WorkingWithFiles.Server.Service
{
    public class FileManagerService
    {
        /// <summary>
        /// Получить все файлы с папки
        /// </summary>
        public List<FileModel> GetFiles(string path)
        {
            path = Cripto.EncodeDecrypt(path);

            List<FileModel> fileList = new List<FileModel>();

            DirectoryInfo dir = new DirectoryInfo(path);
            foreach (var files in dir.GetFiles())
            {
                fileList.Add(new FileModel
                {
                    FileName = files.Name,
                    FullPath = files.FullName
                });
            }

            return fileList;
        }


        /// <summary>
        /// Удаление файла из папки
        /// </summary>
        public void DeleteFile(FileModel file)
        {
            if (File.Exists(file.FullPath))
            {
                File.Delete(file.FullPath);
            }
        }


        /// <summary>
        /// Загрузить файл в папку
        /// </summary>
        public async Task<List<string?>> UploadFiles(string path, List<FileData> saveFile)
        {
            path = Cripto.EncodeDecrypt(path);

            path = path[^1] == '\\' ? path : path + '\\';

            List<string?> fileExists = new List<string?>();

            foreach (var file in saveFile)
            {
                if (File.Exists($"{path}{file.FileName}"))
                {
                    fileExists.Add(file.FileName);
                }
                else
                {
                    using (var fileStream = File.Create($"{path}{file.FileName}"))
                    {
                        await fileStream.WriteAsync(file.Data);
                    }
                }
            }
            return fileExists;
        }
    }
}
