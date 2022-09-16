using Xamarin.Forms;

namespace EpdApp.Services
{
    /// <summary>
    /// Планируемый сервис для взаимодействия через Bluetooth
    /// </summary>
    public class BluetoothService : IBluetoothService
    {
        public void SendMessage(string message)
        {
            DependencyService.Get<IBluetoothService>().SendMessage(message);
        }
    }
}
