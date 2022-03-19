using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using class_library;
using Microsoft.Win32;

namespace WPF_APP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        ViewData Item = new ViewData();
        public MainWindow()
        {
            InitializeComponent();
            MainGrid.DataContext = Item;
            For_Times.DataContext = Item.BM.Time_Coll;
            For_Accuracy.DataContext = Item.BM.Accur_Coll;
            ForInput.DataContext = Item.Grid_V;
            var enumDataSource = System.Enum.GetValues(typeof(System.Windows.HorizontalAlignment));
        }

        private void Add_Time_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Item.AddVMTime(new VMGrid(Item.Grid_V.Length, Item.Grid_V.Begin, Item.Grid_V.End), Item.MFun);
                Item.WasChanged = true;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }
        private void Add_Accur_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Item.AddVMAccur(new VMGrid(Item.Grid_V.Length, Item.Grid_V.Begin, Item.Grid_V.End), Item.MFun);
                Item.WasChanged = true;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }
        private void New_Click(object sender, RoutedEventArgs e)
        {
            if (Item.WasChanged)
            {
                var res = MessageBox.Show("Данные не были сохранены\nСохранить данные?", "Предупреждение", 
                    MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (res == MessageBoxResult.Yes)
                {
                    try
                    {
                        SaveFileDialog dialog = new SaveFileDialog();
                        dialog.Filter = "Text Files (*.txt) | *.txt";
                        dialog.ShowDialog();
                        if (dialog.FileName != "")
                        {
                            Item.Save(dialog.FileName);
                            Item.WasChanged = false;
                        }
                    }
                    catch (Exception Ex)
                    {
                        MessageBox.Show(Ex.Message);
                    }
                    return;
                }
                Item.WasChanged = false;
            }
            try
            {
                Item = new ViewData();
                MainGrid.DataContext = Item;
                For_Times.DataContext = Item.BM.Time_Coll;
                For_Accuracy.DataContext = Item.BM.Accur_Coll;
                ForInput.DataContext = Item.Grid_V;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            if (Item.WasChanged)
            {
                var res = MessageBox.Show("Данные не были сохранены\nСохранить данные?", "Предупреждение", 
                    MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (res == MessageBoxResult.Yes)
                {
                    try
                    {
                        SaveFileDialog dialog = new SaveFileDialog();
                        dialog.Filter = "Text Files (*.txt) | *.txt";
                        dialog.ShowDialog();
                        if (dialog.FileName != "")
                        {
                            Item.Save(dialog.FileName);
                            Item.WasChanged = false;
                        }
                    }
                    catch (Exception Ex)
                    {
                        MessageBox.Show(Ex.Message);
                    }
                    Item.WasChanged = false;
                    return;
                }
            }
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.ShowDialog();
                if (dialog.FileName != "")
                {
                    Item = new ViewData();
                    Item.Load(dialog.FileName);
                    MainGrid.DataContext = Item;
                    For_Times.DataContext = Item.BM.Time_Coll;
                    For_Accuracy.DataContext = Item.BM.Accur_Coll;
                    ForInput.DataContext = Item.Grid_V;
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "Text Files (*.txt) | *.txt" ;
                dialog.ShowDialog();
                if (dialog.FileName != "")
                {
                    Item.Save(dialog.FileName);
                    Item.WasChanged = false;
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void Window_Closing(object sender, EventArgs e)
        {
            if (Item.WasChanged)
            {
                var res = MessageBox.Show("Данные не были сохранены\nСохранить данные?", "Предупреждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (res == MessageBoxResult.Yes)
                {
                    try
                    {
                        SaveFileDialog dialog = new SaveFileDialog();
                        dialog.Filter = "Text Files (*.txt) | *.txt";
                        dialog.ShowDialog();
                        if (dialog.FileName != "")
                        {
                            Item.Save(dialog.FileName);
                            Item.WasChanged = false;
                        }
                    }
                    catch (Exception Ex)
                    {
                        MessageBox.Show(Ex.Message);
                    }
                    return;
                }
                Item.WasChanged = false;
            }
        }
    }
}
