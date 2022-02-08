﻿using WorkingWithFiles.Shared;
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
            DirectoryInfo dir = new DirectoryInfo(path);

            List<FileModel> filesList = new List<FileModel>();

            try
            {
                foreach (var files in dir.GetFiles())
                {
                    filesList.Add(new FileModel
                    {
                        FileName = files.Name,
                        FullPath = files.FullName
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Не удалось открыть файл: " + ex.Message);
            }

            return filesList;
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
        public async Task<List<string?>> UploadFiles(string path, SaveFile saveFile)
        {
            path = Cripto.EncodeDecrypt(path);

            path = path[^1] == '\\' ? path : path + '\\';

            // Список в который собираются файлы уже существующие на сервере
            List<string?> fileExists = new List<string?>();

            foreach (var file in saveFile.Files)
            {
                // Проверяет существует ли файл, если да, то добавляет имя файла в список, иначе сохраняет на сервер
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
