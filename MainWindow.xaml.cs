using radaway_surcharge_calc_HUN.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace radaway_surcharge_calc_HUN
{
    public partial class MainWindow : Window
    {
        public bool IsSaveOn { get; set; } = false;
        public bool IsCrudMenuOn { get; set; } = true;
        public readonly dataContext _dbcontext;

        public MainWindow(dataContext dbcontext)
        {
            InitializeComponent();
            dataContext db = dbcontext;
            int asd = db.ProductFamilies.Count();
            MessageBox.Show($"{asd}");
        }
    }

}