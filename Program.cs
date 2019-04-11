using System;
using System.IO;
using System.Runtime.Serialization.Json;
namespace Backup
{
    class Program
    {
        private static void DirectoryCopy(string sourceDirName, string targetDirName)
        {
            DirectoryInfo directory = new DirectoryInfo(sourceDirName);
            if (!directory.Exists)
            {
                throw new DirectoryNotFoundException("Папка не найдена: " + sourceDirName);
            }
            DirectoryInfo[] directories = directory.GetDirectories();

            if (!Directory.Exists(targetDirName))
            {
                Directory.CreateDirectory(targetDirName);
            }

            FileInfo[] files = directory.GetFiles();
            foreach (FileInfo file in files)
            {
                file.CopyTo(Path.Combine(targetDirName, file.Name));
            }
            foreach (DirectoryInfo dir in directories)
            {
                DirectoryCopy(dir.FullName, Path.Combine(targetDirName, dir.Name));
            }
        }
        private static Configuration DeserObj()
        {
            Configuration config;
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(Configuration));
            using (FileStream fs = new FileStream("config.json", FileMode.Open))
            {
                config = (Configuration)jsonFormatter.ReadObject(fs);
            }
            return config;
        }

        static void Main(string[] args)
        {
            
            DateTime dateTime = DateTime.Now;
            string date = @"\" + dateTime.ToString().Replace(':', '_');

            Configuration config = DeserObj();
            DirectoryInfo dir;
            try
            {
                dir = Directory.CreateDirectory(config.TargetDir + date);
                dir.CreationTimeUtc = DateTime.UtcNow;
                Console.WriteLine(dir.FullName + " установленное время создания " + dir.CreationTimeUtc);
                Console.WriteLine("Идет копирование файлов...");
                foreach(string str in config.SourceDir)
                {
                    DirectoryCopy(str, dir.FullName);
                }
                
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.WriteLine("Для завершения работы программы нажмите любую клавишу.");
                Console.ReadKey();
            }
        }
    }
}
