using Android;
using EpdApp.Services.XmlsService;
using System.IO;

namespace EpdApp.Droid
{
    /// <summary>
    /// Реализация xml сервиса для андроида
    /// </summary>
    public class XmlService: IXmlService
    {
        /// <summary>
        /// Общая директория проекта HandShake
        /// </summary>
        private string handShakeDirectory = "";

        /// <summary>
        /// Директория с Xml-документами, находится внутри общей директории
        /// </summary>
        private string xmlsDirectory = "";

        /// <summary>
        /// Конфигурация папок проекта для андроида
        /// </summary>
        public void ConfigureXmlFolders()
        {
            if (MainActivity.Instance.CheckSelfPermission(Manifest.Permission.WriteExternalStorage) != Android.Content.PM.Permission.Granted)
                MainActivity.Instance.RequestPermissions(new string[] { Manifest.Permission.WriteExternalStorage }, 0);

            string path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments).AbsolutePath;
            handShakeDirectory = Path.Combine(path, "HandShake");
            xmlsDirectory = Path.Combine(handShakeDirectory, "XML documents");

            string[] directories = { handShakeDirectory, xmlsDirectory };
            foreach (var dir in directories)
            {
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
            }
        }

        /// <summary>
        /// Сохраняет Xml-файлы в память андроид устройства
        /// </summary>
        /// <param name="stream"> Последовательность байтов с Xml-документом </param>
        /// <param name="name"> Имя, под которым сохраняется докумет </param>
        public string SaveXmlFile(Stream stream, string name)
        {
            if (MainActivity.Instance.CheckSelfPermission(Manifest.Permission.WriteExternalStorage) != Android.Content.PM.Permission.Granted)
                MainActivity.Instance.RequestPermissions(new string[] { Manifest.Permission.WriteExternalStorage }, 0);

            if (MainActivity.Instance.CheckSelfPermission(Manifest.Permission.WriteExternalStorage) == Android.Content.PM.Permission.Granted)
            {
                string filePath = Path.Combine(xmlsDirectory, name);
                using (FileStream outputFileStream = new FileStream(filePath, FileMode.Create))
                {
                    stream.CopyTo(outputFileStream);
                }
                return filePath;
            }
            return "";
        }

        /// <summary>
        /// Удаляет Xml-файлы из памяти андроид устройства
        /// </summary>
        /// <param name="name"> Имя документа для удаления </param>
        public void DeleteXML(string name)
        {
            if (MainActivity.Instance.CheckSelfPermission(Manifest.Permission.WriteExternalStorage) != Android.Content.PM.Permission.Granted)
                MainActivity.Instance.RequestPermissions(new string[] { Manifest.Permission.WriteExternalStorage }, 0);

            string filePath = Path.Combine(xmlsDirectory, name);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        /// <summary>
        /// Возвращает директорию с Xml-документами 
        /// </summary>
        public string GetXmlDirectory()
        {
            return xmlsDirectory;
        }
    }
}