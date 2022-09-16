namespace EpdApp.Services.XmlsService
{
    /// <summary>
    /// Класс электронного перевозочного документа (ЭПД)
    /// </summary>
    public class Epd
    {
        /// <summary>
        /// Номер ЭПД
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Дата ЭПД
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Грузоотправитель
        /// </summary>
        public string Shipper { get; set; }

        /// <summary>
        /// Грузополучатель
        /// </summary>
        public string Сonsignee { get; set; }

        /// <summary>
        /// Товары
        /// </summary>
        public string Goods { get; set; }

        /// <summary>
        /// Водители
        /// </summary>
        public string Drivers { get; set; }

        /// <summary>
        /// Транспортное средство
        /// </summary>
        public string Transport { get; set; }

        public Epd(string number, string date, string shipper, string сonsignee, string goods, string drivers, string transport)
        {
            Number = number;
            Date = date;
            Shipper = shipper;
            Сonsignee = сonsignee;
            Goods = goods;
            Drivers = drivers;
            Transport = transport;
        }

        public Epd(string epdString)
        {
            var props = epdString.Split('|');
            Number = props[0];
            Date = props[1];
            Shipper = props[2];
            Сonsignee = props[3];
            Goods = props[4];
            Drivers = props[5];
            Transport = props[6];
        }

        public override string ToString()
        {
            return $"{Number}|{Date}|{Shipper}|{Сonsignee}|{Goods}|{Drivers}|{Transport}";
        }
    }
}
