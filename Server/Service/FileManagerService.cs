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
                    filesList.Add(new FileModel(files.Name, files.FullName));
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

        public void UploadFile(UploadedFile uploadedFile)
        {
            var filePath = "Data\\";

            if (uploadedFile.FileContent.Length == default)
            {
                throw new Exception("Файл пуст");
            }

            if (!File.Exists(filePath + uploadedFile.FileName))
            {
                throw new Exception("Файл уже существует, сначала удалите предыдущую версию");
            }
            else
            {
                var fs = File.Create(filePath + uploadedFile.FileName);
                fs.Write(uploadedFile.FileContent, 0, uploadedFile.FileContent.Length);
                fs.Close();
            }
        }
    }
}
