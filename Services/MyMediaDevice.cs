using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using MediaDevices;

namespace SmartfonManager.Services
{
    public class MyMediaDevice : IDisposable
    {
        public MediaDevice Device { get; set; }
        public string DirectoryToLoad { get; set; }
        public string[] Directories { get; set; }
        public string Description { get; set; }

        private bool isConnect = false;
        public MyMediaDevice() : this(null) { }
        public MyMediaDevice(MediaDevice selectedDevice)
        {
            this.Device = selectedDevice;
            this.Description = selectedDevice.Description;

            // Проверка на подключение телефона в другом потоке
            this.CheckErrorDeviceConnect();
            // Если следующая команда зависнит, то второй поток завершит процесс.
            this.Device.Connect();
            isConnect = true;

            var drives = Device.GetDrives();
            if (drives.Length != 0)
            {
                // В качестве директории для загрузки используется директория с наибольшим свободным пространством
                // Полное название всех диррексторий сохраняется в массив Directories
                Directories = new string[drives.Length];
                MediaDriveInfo rootDrive = drives[0];
                Directories[0] = drives[0].RootDirectory.FullName;
                for (int i = 1; i < drives.Length; i++)
                {
                    Directories[i] = drives[i].RootDirectory.FullName;
                    if (drives[i].TotalFreeSpace > rootDrive.TotalFreeSpace)
                    {
                        rootDrive = drives[i];
                    }
                }
                this.DirectoryToLoad = rootDrive.RootDirectory.FullName;
            }
            else
            {
                Device.Disconnect();
                throw new Exception("Некорректное подключение телефона к компьютеру");
            }

            this.Device.DeviceRemoved += Device_DeviceRemoved;
        }

        private void Device_DeviceRemoved(object sender, MediaDeviceEventArgs e)
        {
            // Телефон отключен от компьютера
            MessageBox.Show("Нарушено подключение телефона с компьютером. Будет осуществлен выход", 
                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

            // Завершить процесс из этого потока
            Environment.Exit(0);
        }

        public void Dispose()
        {
            Device.Dispose();
        }

        private void CheckErrorDeviceConnect()
        {
            Thread thrd = new Thread(() =>
            {
                Thread.Sleep(4000);
                if (!this.isConnect)
                {
                    MessageBox.Show("Телефон не отвечает. Попробуйте переподключить Ваш телефон к компьютеру",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Environment.Exit(0);
                }
            });
            thrd.IsBackground = true;
            thrd.Start();
        }
        
    }
}
