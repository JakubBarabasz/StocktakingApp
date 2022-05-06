using Microsoft.EntityFrameworkCore;
using StocktakingApp.Classes;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace StocktakingApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ProductContext _context =
            new ProductContext();

        private CollectionViewSource categoryViewSource;

        public MainWindow()
        {
            InitializeComponent();
            categoryViewSource =
                (CollectionViewSource)FindResource(nameof(categoryViewSource));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _context.Database.EnsureCreated();
        }


        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string whatItem = SearchTermTextBox.Text;

            if (whatItem == null || whatItem == "")
            {
                MessageBox.Show("Podaj kod produktu");
            }
            else
            {
                try
                {

                    Category category;
                    // load the entities into EF Core


                    category = _context.Categories.Where(c => c.SerialCode == whatItem).FirstOrDefault();



                    currentCode.Text = category.SerialCode;


                    currentAmount.Text = category.Amount.ToString();
                }
                catch (Exception)
                {
                    MessageBox.Show("Wprowadzony kod nie istnieje");
                }
            }
        }

        private void ShowAll_Click(object sender, RoutedEventArgs e)
        {

            // load the entities into EF Core
            categoryDataGrid.Items.Refresh();

            _context.Categories.Load();
            categoryDataGrid.Items.Refresh();


            // bind to the source
            categoryViewSource.Source =
            _context.Categories.Local.ToObservableCollection();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Category category;
                // load the entities into EF Core
                categoryDataGrid.Items.Refresh();
                string whatItem = SearchTermTextBox.Text;
                int amount = Int32.Parse(SelectedRow.Text);



                category = _context.Categories.Where(c => c.SerialCode == whatItem).FirstOrDefault();
                category.Amount += amount;
                categoryDataGrid.Items.Refresh();
                currentAmount.Text = category.Amount.ToString();


                // bind to the source
                categoryViewSource.Source =
                _context.Categories.Local.ToObservableCollection();
            }
            catch (Exception)
            {
                MessageBox.Show("Wprowadź poprawny kod produktu");
            }
        }

        private void Sub_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Category category;
                // load the entities into EF Core
                categoryDataGrid.Items.Refresh();
                string whatItem = SearchTermTextBox.Text;
                int amount = Int32.Parse(SelectedRow.Text);

                category = _context.Categories.Where(c => c.SerialCode == whatItem).FirstOrDefault();

                category.Amount -= amount;
                if (category.Amount < 0)
                {
                    MessageBox.Show("Nie możesz mieć ujemnej ilości produktów");
                    category.Amount = 0;
                }
                categoryDataGrid.Items.Refresh();

                currentAmount.Text = category.Amount.ToString();
                // bind to the source
                categoryViewSource.Source =
                _context.Categories.Local.ToObservableCollection();
            }
            catch (Exception)
            {
                MessageBox.Show("Wprowadź poprawny kod produktu");
            }
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            Category category;
            string strPath = Environment.GetFolderPath(
                                   System.Environment.SpecialFolder.DesktopDirectory);

            FileStream fs = new FileStream(strPath + "\\" + "Renament-" + DateTime.Today.Day.ToString() + "-" + DateTime.Today.Month + "-" + DateTime.Today.Year + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.BaseStream.Seek(0, SeekOrigin.End);
            sw.WriteLine("Renament");

            category = _context.Categories.Where(c => c.SerialCode == "ABC123").FirstOrDefault();
            sw.WriteLine(category);



            sw.Flush();
            sw.Close();
            MessageBox.Show("Zapisano:" + strPath);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // all changes are automatically tracked, including
            // deletes!
            MessageBox.Show("Zapisano zmiany");
            _context.SaveChanges();

            // this forces the grid to refresh to latest values
            categoryDataGrid.Items.Refresh();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _context.SaveChanges();
            // clean up database connections
            _context.Dispose();
            base.OnClosing(e);
        }
    }
}