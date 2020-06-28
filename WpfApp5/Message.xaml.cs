using System.Collections.ObjectModel;
using System.Windows;

namespace WpfApp5
{
    public partial class Message : Window
    {
        public Model.Group Group { get; set; }
        public ObservableCollection<Model.Group> Groups { get; set; }
               
        public Message()
        {
            InitializeComponent();

            this.Groups = db.Groups.Local;
            this.Group = this.Groups[0];
            this.DataContext = this;
        }

        private Model.EFContext db = (Application.Current as App).db;

        public void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}