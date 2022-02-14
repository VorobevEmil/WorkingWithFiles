using WorkingWithFiles.Shared;
using WorkingWithFiles.Shared.Model;

namespace WorkingWithFiles.Server.Service
{
    public class FileManagerService
    {
        /// <summary>
        /// Получить все файлы с папки
        /// </summary>
        public List<FileModel> GetFiles()
        {
            string path = PathSelect.FOLDER_PATH;

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
        public async Task<List<string?>> UploadFiles(List<FileData> saveFile)
        {
            string path = PathSelect.FOLDER_PATH;

            path = path[^1] == '\\' ? path : path + '\\';

            List<string?> fileExists = new List<string?>();

            foreach (var file in saveFile)
            {
                var acceptFormat = new string[] { "application/msword", "application/vnd.ms-excel", "application/vnd.ms-powerpoint", "text/plain", "application/pdf" };
                if (file.FileName.Split('.')[^1] == "docx" || file.ContentType.Split('/')[0] == "image" || acceptFormat.Contains(file.ContentType))
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
            }
            return fileExists;
        }
    }
}
