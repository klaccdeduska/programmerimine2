namespace KooliProjekt.WpfApplication
{
    public class AutoModel : NotifyPropertyChangedBase
    {
        private int _id;
        private string _tootja;
        private string _mudel;
        private string _numbrimark;

        public int Id
        {
            get => _id;
            set
            {
                if (_id == value)
                {
                    return;
                }

                _id = value;
                NotifyPropertyChanged();
            }
        }

        public string Tootja
        {
            get => _tootja;
            set
            {
                if (_tootja == value)
                {
                    return;
                }

                _tootja = value;
                NotifyPropertyChanged();
            }
        }

        public string Mudel
        {
            get => _mudel;
            set
            {
                if (_mudel == value)
                {
                    return;
                }

                _mudel = value;
                NotifyPropertyChanged();
            }
        }

        public string Numbrimark
        {
            get => _numbrimark;
            set
            {
                if (_numbrimark == value)
                {
                    return;
                }

                _numbrimark = value;
                NotifyPropertyChanged();
            }
        }
    }
}