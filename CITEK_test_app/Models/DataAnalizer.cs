using CITEK_test_app.Models;
using CITEK_test_app.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace CITEK_test_app
{
    internal static class DataAnalizer
    {
        private static void UnpackArchive()
        {
            Logger.UpdateLog("Распаковка архива...");

            using (var archive = new ZipArchive(new FileStream(Configurations.garPackedFiles, FileMode.Open, FileAccess.Read)))
            {
                archive.ExtractToDirectory(Configurations.garUnpackedFilesDirectory);
            }

            Logger.UpdateLog("Архив распакован");
        }

        private static ObservableDictionary<int, AddressObjectTable> ReadData()
        {

            Logger.UpdateLog("Загрузка данных об уровнях объектов...");

            var dataTable = new ObservableDictionary<int, AddressObjectTable>();

            var levelsDocument = new XmlDocument();

            string levelDocumentAddress = Directory.GetFiles(Configurations.garUnpackedFilesDirectory, "AS_OBJECT_LEVELS*.XML")[0];

            levelsDocument.Load(levelDocumentAddress);

            var xmlRootElement = levelsDocument.DocumentElement;

            foreach (XmlNode node in xmlRootElement.ChildNodes)
            {
                XmlNode? isActiveAttr = node.Attributes.GetNamedItem("ISACTIVE");

                if (isActiveAttr.Value != "true") continue;

                XmlNode? levelAttr = node.Attributes.GetNamedItem("LEVEL");
                int level = Int32.Parse(levelAttr.Value);

                XmlNode? nameAttr = node.Attributes.GetNamedItem("NAME");
                string name = nameAttr.Value;

                dataTable.Add(level, new AddressObjectTable(name));
            }

            Logger.UpdateLog($"Загрузка об уровнях объектов завершена. Обнаружено {dataTable.Keys.Count} уровней");

            string[] dataFiles = Directory.GetFiles(Configurations.garUnpackedFilesDirectory, "AS_ADDR_OBJ_2*.XML", SearchOption.AllDirectories);

            Logger.UpdateLog("Загрузка данных об объектах...");

            foreach (var file in dataFiles)
            {
                var addressObjectDocument = new XmlDocument();

                addressObjectDocument.Load(file);

                var xmlRootObjectElement = addressObjectDocument.DocumentElement;

                if (xmlRootObjectElement == null)
                    throw new Exception("XML file \"AS_ADDR_OBJ\" corrupted!");

                foreach (XmlNode node in xmlRootObjectElement.ChildNodes)
                {
                    XmlNode? isActiveAttr = node.Attributes.GetNamedItem("ISACTIVE");

                    if (isActiveAttr.Value != "1") continue;

                    XmlNode? levelAttr = node.Attributes.GetNamedItem("LEVEL");
                    int level = Int32.Parse(levelAttr.Value);

                    XmlNode? nameAttr = node.Attributes.GetNamedItem("NAME");
                    string name = nameAttr.Value;

                    XmlNode? typeNameAttr = node.Attributes.GetNamedItem("TYPENAME");
                    string typeName = typeNameAttr.Value;

                    var addressObject = new AddressObjectInfoPair(typeName, name);

                    dataTable[level].AddressObjects.Add(addressObject);
                }
            }

            Logger.UpdateLog("Информация об объектах загружена");

            foreach (var key in dataTable.Keys)
            {
                dataTable[key].SortByName();
            }

            Logger.UpdateLog("Информация об объектах отсортирована. Теперь возможно создание отчёта");

            return dataTable;
        }

        public static DateTime GetDate()
        {
            string[] dateFileInfoStrings = File.ReadAllLines(Configurations.garVersions);
            DateTime date = new DateTime();
            var parsingResult = DateTime.TryParse(dateFileInfoStrings[0], out date);

            if (!parsingResult)
            {
                throw new Exception("Файл \"version.txt\"повреждён!");
            }

            Logger.UpdateLog("Данные о дате создания архива получены");

            return date;
        }

        public static ObservableDictionary<int, AddressObjectTable>  AnalizeData()
        {
            ObservableDictionary<int, AddressObjectTable>? result = null;

            try
            {
                UnpackArchive();

                result = ReadData();
            }
            catch (Exception ex)
            {
                Logger.UpdateLog($"Неизвестная ошибка! Подробности ошибки: {ex.Message}");
                MessageBox.Show($"Неизвестная ошибка! Подробности ошибки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return result;
        }
    }


}
