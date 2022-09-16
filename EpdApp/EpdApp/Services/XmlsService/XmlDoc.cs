namespace EpdApp.Services.XmlsService
{
    /// <summary>
    /// Класс Xml документа, сохраняемого в память устройства
    /// </summary>
    public class XmlDoc
    {
        /// <summary>
        /// Имя файла для отображения
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Полный путь к файлу
        /// </summary>
        public string FullName { get; set; }

        public XmlDoc(string name, string fullName)
        {
            Name = name;
            FullName = fullName;
        }
    }
}
