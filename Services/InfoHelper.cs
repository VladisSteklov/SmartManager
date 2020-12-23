using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartfonManager.Services
{
    public class InfoHelper : INotifyPropertyChanged
    {

        private string _textInfo;
        public string TextInfo
        {
            get => _textInfo;
            set
            {
                if (_textInfo == value) return;    
                _textInfo = value;
                OnPropertyChanged(nameof(TextInfo));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string nameProperty)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameProperty));
        }

        public enum Info
        {
            MAIN_WINDOW_INFO,
            LOAD_TO_TELEPHONE_INFO,
            LOAD_TO_COMPUTER_INFO,
            ENTER_GO_LOAD_TO_TELEPHONE_INFO,
            ENTER_GO_LOAD_TO_COMPUTER_INFO,
            ENTER_GO_TO_MENU_INFO,
            ENTER_GO_TO_EXIT_INFO,
            ENTER_GO_INFORMATION_INFO,
            LOAD_FILE_BUTTON_INFO,
            LOAD_TO_BUTTON_INFO
        }
        public void SetStateInfo(Info info)
        {
            switch (info)
            {
                case Info.MAIN_WINDOW_INFO:
                    TextInfo = "Для начала синхронизации данных компьютера и телефона нажмите на одну из представленнных кнопок";
                    break;
                case Info.LOAD_TO_TELEPHONE_INFO:
                    TextInfo = "Для загрузки файлов в телефон укажите путь и нажмите кнопку загрузить. " +
                        "Вы можете отправить несколько файлов одновременно";
                    break;
                case Info.LOAD_TO_COMPUTER_INFO:
                    TextInfo = "";
                    break;
                case Info.ENTER_GO_LOAD_TO_TELEPHONE_INFO:
                    TextInfo = "Перейти к загрузки файлов с компьютера в телефон";
                    break;
                case Info.ENTER_GO_LOAD_TO_COMPUTER_INFO:
                    TextInfo = "Перейти к загрузки файлов с телефона в компьютер";
                    break;
                case Info.ENTER_GO_TO_MENU_INFO:
                    TextInfo = "Перейти обратно в меню";
                    break;
                case Info.ENTER_GO_TO_EXIT_INFO:
                    TextInfo = "Выйти из программы";
                    break;
                case Info.ENTER_GO_INFORMATION_INFO:
                    TextInfo = "Информация о программе";
                    break;
                case Info.LOAD_TO_BUTTON_INFO:
                    TextInfo = "Загрузить файлы!";
                    break;
                case Info.LOAD_FILE_BUTTON_INFO:
                    TextInfo = "Открыть диалоговое окно выбора файлов";
                    break;
            }
        }
    }
}