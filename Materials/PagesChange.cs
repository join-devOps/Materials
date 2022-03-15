using System.ComponentModel;

namespace Materials
{
    class PagesChange : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static byte CountItems = 5;
        public byte[] NumberPage { get; set; } = new byte[CountItems];
        public string[] Visible { get; set; } = new string[CountItems];
        public string[] Bold { get; set; } = new string[CountItems];
        
        private byte _CountPages;
        public byte CountPages                            /*Cвойство в котором хранится общее кол-во страц, при изменении данного свойства будет определяться, скрыт будет номер той или итой страницы или нет (в зависимости об общего кол-ва записей в списке) */
        {
            get => _CountPages;
            set
            {
                _CountPages = value;
                for (byte i = 1; i < CountItems; i++)
                {
                    if (CountPages <= i)
                        Visible[i] = "Hidden";
                    else 
                        Visible[i] = "Visible";
                }
            }
        }

        private byte _CountPage; //количество записей на странице
        public byte CountPage    //свойство, в котором хранится количество записей на странице, при изменении данного свойства будет изменяться общее количесво страниц для отображения
        {
            get => _CountPage;
            set
            {
                _CountPage = value;
                if (Countlist % value == 0)
                    CountPages = (byte)(Countlist / value);
                else
                    CountPages = (byte)(Countlist / value + 1);
            }
        }

        private byte _CountList; // количество записей в списке
        public byte Countlist    //свойство, в котором хранится общее количество записей в списке, при изменении данного свойства будет изменяться общее количесво страниц для отображения
        {
            get => _CountList;
            set
            {
                _CountList = value;
                if (value % CountPage == 0)
                {
                    CountPages = (byte)(value / CountPage);//определение количества страниц
                }
                else
                {
                    CountPages = (byte)(1 + value / CountPage);
                }
            }
        }

        private byte _CurrentPage;
        public byte CurrentPage
        {
            get => _CurrentPage;
            set
            {
                _CurrentPage = value;
                if (_CurrentPage < 1)
                {
                    _CurrentPage = 1;
                }
                if (_CurrentPage >= CountPages)
                {
                    _CurrentPage = CountPages;
                }

                //отрисовка меню с номерами страниц, рассмотрим три возможных случая                            
                for (int i = 0; i < CountItems; i++)
                {
                    if (_CurrentPage < (1 + CountItems / 2) || CountPages < CountItems) NumberPage[i] = (byte)(i + 1);//если страница в начале списка
                    else if (_CurrentPage > CountPages - (CountItems / 2 + 1)) NumberPage[i] = (byte)(CountPages - (CountItems - 1) + i);//если страница в конце списка
                    else NumberPage[i] = (byte)(_CurrentPage + i - (CountItems / 2));//если страница в середине списка
                }

                for (int i = 0; i < CountItems; i++)//выделяем активную страницу жирным
                {
                    if (NumberPage[i] == _CurrentPage) Bold[i] = "ExtraBold";
                    else Bold[i] = "Regular";
                }

                PropertyChanged(this, new PropertyChangedEventArgs("NumberPage"));
                PropertyChanged(this, new PropertyChangedEventArgs("Visible"));
                PropertyChanged(this, new PropertyChangedEventArgs("Bold"));
            }
        }
        public PagesChange() // контруктор
        {
            for (int i = 0; i < CountItems; i++)  // показываем исходное меню ( 1 2 3 4 5)
            {
                Visible[i] = "Visible";
                NumberPage[i] = (byte)(i + 1);
                Bold[i] = "Regular";
            }
            _CurrentPage = 1;  // по умолчанию 1-ая страница будет текущей
            _CountPage = 1;  // по умолчанию все записи будут отображаться на одной странице
            _CountList = 1;  // по умолчанию в общес списке будет только одна запись
        }
    }
}