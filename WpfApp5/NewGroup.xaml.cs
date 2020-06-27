using System.Windows;

namespace WpfApp5
{
    public partial class NewGroup : Window
    {
        public NewGroup()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}