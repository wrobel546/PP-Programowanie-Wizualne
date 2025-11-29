using System.Windows;
using NowakowskaWrobel.Smartphones.BLC;

namespace NowakowskaWrobel.Smartphones.UI.Desktop
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var dataAccess = DataAccessLoader.Load();
            var catalogService = new CatalogService(dataAccess.ProducerRepository, dataAccess.SmartphoneRepository);

            DataContext = new MainViewModel(catalogService, dataAccess.EntityFactory);
        }
    }
}
