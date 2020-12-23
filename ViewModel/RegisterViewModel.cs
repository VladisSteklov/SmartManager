using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaDevices;
using SmartfonManager.Commands;
using System.Windows.Input;

namespace SmartfonManager.ViewModel
{
    public class RegisterViewModel
    {
        private IList<MediaDevice> _devices = new ObservableCollection<MediaDevice>();

        public IList<MediaDevice> Devices
        {
            get => _devices;
            set => _devices = value;
        }

        public RegisterViewModel()
        {
            //var telephones = from tel in MediaDevice.GetDevices()
            //                 where tel.Manufacturer == "Xiaomi" || tel.Manufacturer == "Samsung" ||
            //                 tel.Manufacturer == "LGE"
            //                 select tel;
            var telephones = MediaDevice.GetDevices();
            foreach (var device in telephones)
            {
                _devices.Add(device);
            }          
        }

        private ICommand _reloadDevices = null;
        public ICommand ReloadDevice => _reloadDevices ?? (_reloadDevices = new ReloadDevicesCommand());
    }
}
