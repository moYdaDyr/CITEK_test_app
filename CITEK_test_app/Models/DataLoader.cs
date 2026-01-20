using CITEK_test_app.Models;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Linq;

namespace CITEK_test_app
{
    internal static class DataLoader
    {

        private static async Task<string?> LoadDataAddressAsync()
        {
            Logger.UpdateLog("Получение информации об архиве...");

            DownloadFileInfo? fileInfo = new DownloadFileInfo();
            string? rawFileInfo;

            HttpClient client = new HttpClient();

            HttpResponseMessage? response = await client.GetAsync(Configuration.fiasNalogFilesInfoAddress, HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode();

            rawFileInfo = await response.Content.ReadAsStringAsync();

            fileInfo = JsonSerializer.Deserialize<DownloadFileInfo>(rawFileInfo);

            Logger.UpdateLog("Информация об архиве получена");

            return fileInfo.GarXMLDeltaURL;
        }

        private static async Task LoadDataAsync(string archiveAddress)
        {
            ClearGarDirectory();

            ObservableDictionary<int, AddressObjectTable>? result = null;

            HttpClient client = new HttpClient();

            Logger.UpdateLog("Загрузка архива...");

            HttpResponseMessage? response = await client.GetAsync(archiveAddress, HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode();

            long totalBytesToLoad, totalBytesLoaded;

            int bufferSize = 8192;
            byte[] buffer = new byte[bufferSize];

            if (response.Content.Headers.ContentLength != null)
            {
                totalBytesToLoad = (long)response.Content.Headers.ContentLength;
            }
            else throw new HttpRequestException();

            SetProgressPercentEvent(0.0);

            using (var archiveFileStream = new FileStream(Configuration.garPackedFiles, FileMode.Create, FileAccess.Write))
            {
                using (var archiveInternetStream = await response.Content.ReadAsStreamAsync())
                {
                    for (totalBytesLoaded = 0; totalBytesLoaded < totalBytesToLoad;) // передаём так, чтобы было удобнее отслеживть
                    {
                        int bytesLoadedThisStep = await archiveInternetStream.ReadAsync(buffer, 0, bufferSize);

                        await archiveFileStream.WriteAsync(buffer, 0, bytesLoadedThisStep);

                        totalBytesLoaded += bytesLoadedThisStep;

                        SetProgressPercentEvent((double)(totalBytesLoaded) / totalBytesToLoad);
                    }
                }
            }

            Logger.UpdateLog("Загрузка архива завершена");
        }

        private static void ClearGarDirectory()
        {
            if (Directory.Exists(Configuration.garUnpackedFilesDirectory))
                Directory.Delete(Configuration.garUnpackedFilesDirectory, true);
            
            Directory.CreateDirectory(Configuration.garUnpackedFilesDirectory);

            Logger.UpdateLog("Директория под архив расчищена");
        }

        public static async Task UpdateDataAsync()
        {
            string dataArchiveUrl;

            try
            {
                dataArchiveUrl = await LoadDataAddressAsync();

                await LoadDataAsync(dataArchiveUrl);
            }
            catch (HttpRequestException httpEx)
            {
                Logger.UpdateLog("Ошибка при загрузке данных!");
                MessageBox.Show("Ошибка при загрузке данных!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                Logger.UpdateLog($"Неизвестная ошибка! Подробности ошибки: {ex.Message}");
                MessageBox.Show($"Неизвестная ошибка! Подробности ошибки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public delegate void SetProgressPercent(double percent);
        public static event SetProgressPercent SetProgressPercentEvent;
    }
}
