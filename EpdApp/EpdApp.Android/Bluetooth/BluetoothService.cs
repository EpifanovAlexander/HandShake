using Android.Bluetooth;
using EpdApp.Services;
using Java.Util;
using System.Linq;
using System.Text;

namespace EpdApp.Droid.Bluetooth
{
    /// <summary>
    /// Реализация bluetooth сервиса для андроида
    /// </summary>
    public class BluetoothService : IBluetoothService
    {
        /// <summary>
        /// Отправляет сообщение по Bluetooth
        /// </summary>
        /// <param name="message"> Текст сообщения </param>
        public void SendMessage(string message)
        {
            var deviceName = "Some device name";
            BluetoothDevice device = (from bd in BluetoothAdapter.DefaultAdapter?.BondedDevices
                                      where bd?.Name == deviceName
                                      select bd).FirstOrDefault();
            try
            {
                using (BluetoothSocket bluetoothSocket = device?.
                    CreateRfcommSocketToServiceRecord(
                    UUID.FromString("00001101-0000-1000-8000-00805f9b34fb")))
                {
                    bluetoothSocket?.Connect();
                    byte[] buffer = Encoding.UTF8.GetBytes(message);
                    bluetoothSocket?.OutputStream.Write(buffer, 0, buffer.Length);
                    bluetoothSocket.Close();
                }
            }
            catch (System.Exception exp)
            {
                System.Console.WriteLine(exp.Message);
            }
        }
    }
}