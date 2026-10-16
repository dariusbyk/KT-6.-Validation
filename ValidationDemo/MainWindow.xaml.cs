using System.Windows;

namespace ValidationDemo
{
    public partial class MainWindow : Window
    {
        private User user = new User();

        public MainWindow()
        {
            InitializeComponent();

            DataContext = user;
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            user.ValidateAll();

            if (user.HasErrors == true)
            {
                MessageBox.Show(
                    "Исправьте ошибки в форме.",
                    "Ошибка"
                );

                return;
            }

            MessageBox.Show(
                "Все данные введены правильно.",
                "Проверка завершена"
            );
        }
    }
}