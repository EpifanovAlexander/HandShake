using System.IO;

namespace EpdApp.Services.XmlsService
{
    public interface IXmlService
    {
        string GetXmlDirectory();
        string SaveXmlFile(Stream stream, string name);
        void ConfigureXmlFolders();
    }
}
