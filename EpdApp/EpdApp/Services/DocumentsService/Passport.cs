using System;
using System.Collections.Generic;
using System.Text;

namespace EpdApp.Services.DocumentsService
{
    internal class Passport : Document
    {
        private string _snum;
        private string _number;
        private string _name;
        private string _middlename;
        private string _surname;
        private int _sex;
        private DateTime _birthday;

        public string Snum
        {
            get => _snum;
            set => _snum = value;
        }

        public string Number
        {
            get => _number;
            set => _number = value;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public string Middlename
        {
            get => _middlename;
            set => _middlename = value;
        }

        public string Surname
        {
            get => _surname;
            set => _surname = value;
        }

        public int Sex
        {
            get => _sex;
            set => _sex = value;
        }

        public DateTime Birthday
        {
            get => _birthday;
            set => _birthday = value;
        }

        public string stringBirthday
        {
            get => _birthday.ToString("d.MM.yyyy");

        }

        public override string ToString()
        {
            return $"{Snum}|{Number}|{Name}|{Middlename}|{Surname}|{Birthday.Date.ToString("yyyy-M-dd")}|{Sex}";
        }

        public Passport(string snum, string number, string name, string middlename, string surname, int sex, DateTime birthday)
        {
            _snum = snum;
            _number = number;
            _name = name;
            _middlename = middlename;
            _surname = surname;
            _sex = sex;
            _birthday = birthday;
        }

        public Passport(string passport)
        {
            var props = passport.Split("|");
            Snum = props[0];
            Number = props[1];
            Name = props[2];
            Middlename = props[3];
            Surname = props[4];
            Birthday = DateTime.Parse(props[5]);
            Sex = Int32.Parse(props[6]);
        }
    }
}
