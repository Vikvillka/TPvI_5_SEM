using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebDav
{
    internal class Program
    {
        private static readonly string WebDavUrl = "https://webdav.yandex.ru/";
        private static readonly string Username = "Vikvillka";
        private static readonly string Password = "ysiyiflpmrukvkdk";

        private static WebDavClient CreateWebDavClient()
        {
            var clientParams = new WebDavClientParams
            {
                BaseAddress = new Uri(WebDavUrl),
                Credentials = new System.Net.NetworkCredential(Username, Password)
            };

            return new WebDavClient(clientParams);
        }

        static async Task Main(string[] args)
        {
            var client = CreateWebDavClient();


            string sourceFolderPath = "/example_folder";
            string localFilePath = "example.txt";
            string remoteFilePath = $"{sourceFolderPath}/example.txt";
            string downloadPath = "downloaded_example.txt";         
            string destinationFolderPath = "/new_folder"; 
            string destinationFileName = "copy_example.txt"; 


            while (true)
            {
                Console.WriteLine("\nВыберите действие:");
                Console.WriteLine("1. Создать папку");
                Console.WriteLine("2. Выгрузить файл");
                Console.WriteLine("3. Загрузить файл");
                Console.WriteLine("4. Копировать файл");
                Console.WriteLine("5. Удалить файл");
                Console.WriteLine("6. Удалить папку");
                Console.WriteLine("0. Выход");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await CreateFolder(client, sourceFolderPath);
                        break;

                    case "2":
                        await UploadFile(client, localFilePath, remoteFilePath);
                        break;

                    case "3":
                        await DownloadFile(client, remoteFilePath, downloadPath); 
                        break;

                    case "4":
                        await CopyFile(client, remoteFilePath, destinationFolderPath, destinationFileName);
                        break;

                    case "5":
                        await DeleteFile(client, remoteFilePath);
                        break;

                    case "6":
                        await DeleteFolder(client, sourceFolderPath);
                        await DeleteFolder(client, destinationFolderPath);
                        break;

                    case "0":
                        Console.WriteLine("Выход из программы.");
                        return;

                    default:
                        Console.WriteLine("Неверный выбор, попробуйте снова.");
                        break;
                }
            }
        }

        private static async Task CreateFolder(WebDavClient client, string folderPath)
        {
            var response = await client.Mkcol(folderPath);
            if (response.IsSuccessful)
                Console.WriteLine($"Папка '{folderPath}' успешно создана.");
            else
                Console.WriteLine($"Ошибка при создании папки: {response.StatusCode}");
        }

        private static async Task UploadFile(WebDavClient client, string localFilePath, string remoteFilePath)
        {
            using var fileStream = File.OpenRead(localFilePath);
            var response = await client.PutFile(remoteFilePath, fileStream);
            if (response.IsSuccessful)
                Console.WriteLine($"Файл '{localFilePath}' успешно загружен в '{remoteFilePath}'.");
            else
                Console.WriteLine($"Ошибка при загрузке файла: {response.StatusCode}");
        }

        private static async Task DownloadFile(WebDavClient client, string remoteFilePath, string localFilePath)
        {
            var response = await client.GetRawFile(remoteFilePath);
            if (response.IsSuccessful)
            {
                using var fileStream = File.Create(localFilePath);
                await response.Stream.CopyToAsync(fileStream);
                Console.WriteLine($"Файл '{remoteFilePath}' успешно скачан в '{localFilePath}'.");
            }
            else
            {
                Console.WriteLine($"Ошибка при скачивании файла: {response.StatusCode}");
            }
        }

        private static async Task CopyFile(WebDavClient client, string sourcePath, string destinationFolderPath, string destinationFileName)
        {
            var folderResponse = await client.Mkcol(destinationFolderPath);
            if (!folderResponse.IsSuccessful)
            {
                Console.WriteLine($"Ошибка при создании папки: {folderResponse.StatusCode}");
                return;
            }

            string destinationPath = $"{destinationFolderPath}/{destinationFileName}";

            var copyResponse = await client.Copy(sourcePath, destinationPath);
            if (copyResponse.IsSuccessful)
                Console.WriteLine($"Файл '{sourcePath}' успешно скопирован в '{destinationPath}'.");
            else
                Console.WriteLine($"Ошибка при копировании файла: {copyResponse.StatusCode}");
        }

        private static async Task DeleteFile(WebDavClient client, string filePath)
        {
            var response = await client.Delete(filePath);
            if (response.IsSuccessful)
                Console.WriteLine($"Файл '{filePath}' успешно удален.");
            else
                Console.WriteLine($"Ошибка при удалении файла: {response.StatusCode}");
        }

        private static async Task DeleteFolder(WebDavClient client, string folderPath)
        {
            var response = await client.Delete(folderPath);
            if (response.IsSuccessful)
                Console.WriteLine($"Папка '{folderPath}' успешно удалена.");
            else
                Console.WriteLine($"Ошибка при удалении папки: {response.StatusCode}");
        }
    }
}