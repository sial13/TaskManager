using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using TaskManager.Data;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            /*Tasks = new ObservableCollection<TaskSampleDb>();
            lstTasks.ItemsSource = Tasks;*/
            
            InitializeComponent();
            RefreshDataGrid();
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new AppDbContext())
            {
                var task = new TaskSampleDb
                {
                    Title = txtTitle.Text,
                    Description = txtDescription.Text,
                    IsImportant = chkCompleted.IsChecked ?? false
                };

                context.TaskSamples.Add(task);
                context.SaveChanges();

                RefreshDataGrid();

                txtTitle.Text = "Title";
                txtTitle.Foreground = SystemColors.GrayTextBrush;
                txtDescription.Text = "Description";
                txtDescription.Foreground = SystemColors.GrayTextBrush;
                chkCompleted.IsChecked = false;

            }
        }

        private void RefreshDataGrid()
        {
            using (var context = new AppDbContext())
            {
                var query = $"SELECT * FROM public.\"TaskSamples\"\r\nORDER BY\r\n\tCASE WHEN \"TaskSamples\".\"IsImportant\" = true THEN 0 ELSE 1 END;";

                tasksListBox.ItemsSource = context.TaskSamples.FromSqlRaw(query).ToList();
            }
        }
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (!string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = string.Empty;
                textBox.Foreground = SystemColors.WindowTextBrush;
            }
        }

        private void txtTitle_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "Title";
                textBox.Foreground = SystemColors.GrayTextBrush;
            }
        }

        private void txtDesc_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "Description";
                textBox.Foreground = SystemColors.GrayTextBrush;
            }
        }

        private void tasksListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tasksListBox.SelectedItem is TaskSampleDb selectedTask)
            {
                // Здесь вы можете редактировать данные выбранной задачи.
                // Например, отобразить данные в других элементах управления.
                txtTitle.Text = selectedTask.Title;
                txtDescription.Text = selectedTask.Description;
                chkCompleted.IsChecked = selectedTask.IsImportant;
            }
            else
            {
                // Очистка данных, если ничего не выбрано.
                txtTitle.Text = string.Empty;
                txtDescription.Text = string.Empty;
                chkCompleted.IsChecked = false;
            }
        }

        private void saveEditedTask(object sender, RoutedEventArgs e)
        {
            if (tasksListBox.SelectedItem is TaskSampleDb selecteedTask)
            {
                selecteedTask.Title = txtTitle.Text;
                selecteedTask.Description = txtDescription.Text;
                selecteedTask.IsImportant = chkCompleted.IsChecked ?? false;

                using (var context = new AppDbContext())
                {
                    context.Entry(selecteedTask).State = EntityState.Modified;
                    context.SaveChanges();

                }
            }

            RefreshDataGrid();
        }
    }
}
