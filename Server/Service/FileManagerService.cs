using WorkingWithFiles.Shared;

namespace WorkingWithFiles.Server.Service
{
    public class FileManagerService
    {
        public Dictionary<string, List<string>> GetFiles(string path)
        {
            path = Cripto.EncodeDecrypt(path);
            DirectoryInfo dir = new DirectoryInfo(path);


            Dictionary<string, List<string>> typeAll = new Dictionary<string, List<string>>();

            try
            {

                foreach (var directory in dir.GetDirectories())
                {
                    if (!typeAll.ContainsKey("directory"))
                        typeAll["directory"] = new List<string>();

                    typeAll["directory"].Add(directory.FullName);
                }

                foreach (var file in dir.GetFiles())
                {
                    if (!typeAll.ContainsKey("file"))
                        typeAll["file"] = new List<string>();

                    typeAll["file"].Add(file.FullName);
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return typeAll;
        }
    }
}
