using System.Windows;

namespace WpfApp5
{
    public partial class NewStudent : Window
    {
        public NewStudent()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {                        
            System.Windows.Data.CollectionViewSource groupViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("groupViewSource")));
            groupViewSource.Source = (Application.Current as App).db.Groups.Local;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}