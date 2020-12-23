using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartfonManager.ViewModel;
using MediaDevices;

namespace SmartfonManager.Commands
{
    public class ReloadDevicesCommand : BaseCommand
    {
        // Команда перезагрузки списка внешних устройств
        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            // Перезаписать список медиа устройств из RegisterViewModel
            IList<MediaDevice> devices = parameter as IList<MediaDevice>;
            if (devices != null)
            {
                devices.Clear();
                //var telephones = from tel in MediaDevice.GetDevices()
                //                 where tel.Manufacturer == "Xiaomi" || tel.Manufacturer == "LGE"
                //                 select tel;
                var telephones = MediaDevice.GetDevices();
                foreach (var d in telephones)
                {
                    devices.Add(d);
                }
            }
        }
    }
}
