using EpdApp.Services.UsersService;
using EpdApp.Services.XmlsService;
using System.Collections.ObjectModel;
using System.Linq;

namespace EpdApp.ViewModels
{
    /// <summary>
    /// ViewModel для пользователя
    /// </summary>
    public class UserViewModel : BaseViewModel
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Роль пользователя
        /// </summary>
        public UserRoles Role { get; set; }

        /// <summary>
        /// Является ли пользователь водителем
        /// </summary>
        public bool IsDriver { get => Role == UserRoles.Driver; }

        /// <summary>
        /// Является ли пользователь инспектором
        /// </summary>
        public bool IsPolice { get => Role == UserRoles.Police; }

        /// <summary>
        /// Список Xml-документов, сохарнённых на устройстве водителя
        /// </summary>
        public ObservableCollection<XmlDoc> XmlDocuments { get; set; } = new ObservableCollection<XmlDoc>();

        /// <summary>
        /// Флаг того, существуют ли сохранённые документы на устройстве водителя
        /// </summary>
        public bool IsExistDocuments { get => XmlDocuments.Any(); }

        /// <summary>
        /// Флаг того, отсутствуют ли сохранённые документы на устройстве водителя
        /// </summary>
        public bool IsNonExistDocuments { get => !IsExistDocuments; }


        public UserViewModel(User user)
        {
            Name = user.Name;
            Role = user.Role;
        }

        public UserViewModel(UserRoles role)
        {
            Role = role;
        }

        /// <summary>
        /// Обновляем флаги, если изменилось количество Xml-документов на устройстве водителя
        /// </summary>
        public void UpdateXmlDocumentsIsExist() 
        {
            OnPropertyChanged("IsExistDocuments");
            OnPropertyChanged("IsNonExistDocuments");
        }

        /// <summary>
        /// Текущий ЭПД документ, отображаемый у инспектора
        /// </summary>
        private Epd epd;

        /// <summary>
        /// Текущий ЭПД документ, отображаемый у инспектора
        /// </summary>
        public Epd CurrentEpd 
        {
            get => epd;
            set
            { 
                epd = value;
                OnPropertyChanged("CurrentEpd");
            }
        }
    }
}
