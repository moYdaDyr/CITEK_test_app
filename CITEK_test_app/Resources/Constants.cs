using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CITEK_test_app
{
    internal static class Configurations
    {
        static public readonly string fiasNalogFilesInfoAddress = "https://fias.nalog.ru/WebServices/Public/GetLastDownloadFileInfo";
        static public readonly string garUnpackedFilesDirectory = Environment.CurrentDirectory + "\\FIAS_GAR\\Unpacked";
        static public readonly string garPackedFiles = Environment.CurrentDirectory + "\\FIAS_GAR\\gar.zip";
        static public readonly string garVersions = garUnpackedFilesDirectory + "\\" + "version.txt";
        static public readonly string fontPath = "Resources\\times.ttf";
    }
}
