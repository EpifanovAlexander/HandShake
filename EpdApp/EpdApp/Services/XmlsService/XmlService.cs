using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Xamarin.Forms;

namespace EpdApp.Services.XmlsService
{
    /// <summary>
    /// Сервис взаимодействия с Xml документами
    /// </summary>
    public class XmlService : IXmlService
    {
        private static XmlService instance = null;
        public static XmlService Instance
        {
            get
            {
                if (instance == null) 
                { 
                    instance = new XmlService();
                }
                return instance;
            }
        }

        public XmlService()
        {
        }

        /// <summary>
        /// Возвращает ЭПД из xml текста
        /// </summary>
        /// <param name="text"> Xml текст, из которого надо получить информацию об ЭПД </param>
        public static Epd GetEpdFromXml(string text)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(text.Replace("windows-1251", "utf-8"));
            XmlElement xRoot = xDoc.DocumentElement;

            if (xRoot == null)
                return null;

            XmlElement xDocumentNode = GetNodeByName("Документ", xRoot);
            XmlElement xSodInfGo = GetNodeByName("СодИнфГО", xDocumentNode);

            // Номер и дата накладной
            var number = xSodInfGo.GetAttribute("НомерТрН");
            var date = xSodInfGo.GetAttribute("ДатаТрН");

            // Информация о товарах
            XmlElement xGoodsInfo = GetNodeByName("СвГруз", xSodInfGo);
            List<XmlElement> xGoods = GetMultipleNodesByName("ОпГруз", xGoodsInfo);
            var goodsResult = "";
            foreach (var good in xGoods)
            {
                goodsResult += good.GetAttribute("НаимГруз") + Environment.NewLine;
            }
            var goods = goodsResult.Substring(0, goodsResult.Length - 1);

            // Информация о водителях
            List<XmlElement> xDrivers = GetMultipleNodesByName("СвВодит", xSodInfGo);
            var driversResult = "";
            foreach (var xDriver in xDrivers)
            {
                XmlElement driver = GetNodeByName("ФИО", xDriver);
                driversResult += $"{driver.GetAttribute("Фамилия")} {driver.GetAttribute("Имя")} {driver.GetAttribute("Отчество")}" + Environment.NewLine;
            }
            var drivers = driversResult.Substring(0, driversResult.Length - 1);

            // Информация о транспортном средстве
            XmlElement xTransportInfo = GetNodeByName("СвТС", xSodInfGo);
            XmlElement xTransport = GetNodeByName("ТС", xTransportInfo);
            var transport = $"Номер: {xTransport.GetAttribute("РегНомер")}" + Environment.NewLine;
            XmlElement parTransport = GetNodeByName("ПарТС", xTransport);
            transport += $"Марка: {parTransport.GetAttribute("Марка")}";

            // Информация о грузоотправителе и грузополучателе
            var shipper = GetUserInfo("СвГО", "РекИдентГО", xSodInfGo);
            var сonsignee = GetUserInfo("СвГП", "РекИдентГП", xSodInfGo);

            return new Epd(number, date, shipper, сonsignee, goods, drivers, transport);
        }

        /// <summary>
        /// Внутренний метод. Возвращает информацию о грузоотправителе или грузополучаетеле
        /// </summary>
        /// <param name="name"> Имя параметра: грузоотправитель (СвГО) или грузополучатель (СвГП) </param>
        /// <param name="rec"> Имя реквизита: грузоотправитель (РекИдентГО) или грузополучатель (РекИдентГП) </param>
        /// <param name="root"> Узел Xml документа, из которого осуществлять поиск </param>
        private static string GetUserInfo(string name, string rec, XmlElement root)
        {
            XmlElement xShipperInfo = GetNodeByName(name, root);
            XmlElement RecIdentGo = GetNodeByName(rec, xShipperInfo);
            XmlElement IdSv = GetNodeByName("ИдСв", RecIdentGo);
            XmlElement xUser = GetNodeByName("СвФЛУчаст", IdSv);

            var user = "";
            if (xUser == null)
            {
                xUser = GetNodeByName("СвЮЛУч", IdSv);
                user += $"Организация: {xUser.GetAttribute("НаимОрг")}" + Environment.NewLine;
                var inn = xUser.GetAttribute("ИННЮЛ");
                user += $"ИНН: {inn}" + Environment.NewLine;
            }
            else
            {
                var xFio = GetNodeByName("ФИО", xUser);
                user += $"ФИО: {xFio.GetAttribute("Фамилия")} {xFio.GetAttribute("Имя")} {xFio.GetAttribute("Отчество")}" + Environment.NewLine;
                var inn = xUser.GetAttribute("ИННФЛ");
                user += $"ИНН: {inn}" + Environment.NewLine;
            }
            XmlElement xContact = GetNodeByName("Контакт", RecIdentGo);
            List<XmlElement> xPhones = GetMultipleNodesByName("Тлф", xContact);
            var phones = "Телефоны: ";
            foreach (var xPhone in xPhones)
            {
                phones += xPhone.InnerText + ", ";
            }
            user += phones.Substring(0, phones.Length - 2);
            return user;
        }

        /// <summary>
        /// Внутренний метод. Возвращает первый дочерний узел Xml документа, совпадающий по имени.
        /// Если узлов с заданным именем нет, возвращает null
        /// </summary>
        /// <param name="name"> Имя для поиска </param>
        /// <param name="element"> Узел, из которого осуществлять поиск </param>
        private static XmlElement GetNodeByName(string name, XmlElement element)
        {
            foreach (var xnode in element.ChildNodes)
            {
                XmlElement node = xnode as XmlElement;
                if ((node != null) && (node.Name == name))
                {
                    return node;
                }
            }
            return null;
        }

        /// <summary>
        /// Внутренний метод. Возвращает все дочерние узлы Xml документа, которые совпадают по имени.
        /// Если узлов с заданным именем нет, возвращает пустой список
        /// </summary>
        /// <param name="name"> Имя для поиска </param>
        /// <param name="element"> Узел, из которого осуществлять поиск </param>
        private static List<XmlElement> GetMultipleNodesByName(string name, XmlElement element)
        {
            List<XmlElement> result = new List<XmlElement>();
            foreach (var xnode in element.ChildNodes)
            {
                XmlElement node = xnode as XmlElement;
                if ((node != null) && (node.Name == name))
                {
                    result.Add(node);
                }
            }
            return result;
        }

        /// <summary>
        /// Сохраняет Xml документ в Shared-папку Android устройства
        /// </summary>
        /// <param name="stream"> Последовательность байтов с Xml-документом </param>
        /// <param name="name"> Имя, под которым сохраняется докумет </param>
        public string SaveXmlFile(Stream stream, string name)
        {
            return DependencyService.Get<IXmlService>().SaveXmlFile(stream, name+".xml");
        }

        /// <summary>
        /// Проверяет наличие папок с Xml-документами и создаёт их, если они отсутствуют. Вызывается при старте приложения
        /// </summary>
        public void ConfigureXmlFolders()
        {
            DependencyService.Get<IXmlService>().ConfigureXmlFolders();
        }

        /// <summary>
        /// Возвращает путь к Shared-папку Android устройства с Xml-документами
        /// </summary>
        public string GetXmlDirectory()
        {
           return DependencyService.Get<IXmlService>().GetXmlDirectory();
        }

        /// <summary>
        /// Возвращает Xml-документы из папки, сохранённой на устройстве
        /// </summary>
        public List<XmlDoc> GetDocuments() 
        { 
            List<XmlDoc> result = new List<XmlDoc>();
           /* foreach (var file in Directory.GetFiles(GetXmlDirectory()))
            { 
                var fileInfo = new FileInfo(file);
                result.Add(new XmlDoc(fileInfo.Name.Substring(0, fileInfo.Name.Length - 4), fileInfo.FullName));
            }*/
            return result;
        }
    }
}
