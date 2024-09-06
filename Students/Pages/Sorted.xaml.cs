using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Students.Pages
{
    public partial class Sorted : Page
    {
        private string connectionString = "Data Source=PC1;Initial Catalog=students;Integrated Security=True";

        public Sorted()
        {
            InitializeComponent();
            LoadStudentsData();
        }

        private void LoadStudentsData()
        {
            try
            {
                string query = @"SELECT TOP (1000) 
                                [Код студента], [ФИО], [Пол], [Дата рождения], 
                                [Родители], [Адрес], [Телефон], [Паспортные данные], 
                                [Номер зачетки], [Дата поступления], [Группа], 
                                [Курс], [Код специальности], [Очная форма обучения]
                                FROM [students].[dbo].[Студенты]";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    studentsDataGrid.ItemsSource = dataTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Обработчик события сортировки
        private void studentsDataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            e.Handled = true; // Отключаем стандартную сортировку

            // Определяем текущее направление сортировки
            ListSortDirection newDirection = (e.Column.SortDirection != ListSortDirection.Ascending)
                                             ? ListSortDirection.Ascending
                                             : ListSortDirection.Descending;

            // Присваиваем направление сортировки
            e.Column.SortDirection = newDirection;

            // Получаем коллекцию данных и сортируем по выбранной колонке
            var dataView = (sender as DataGrid).ItemsSource as DataView;
            if (dataView != null)
            {
                dataView.Sort = $"{e.Column.SortMemberPath} {(newDirection == ListSortDirection.Ascending ? "ASC" : "DESC")}";
            }

            // Обновляем заголовок с русским текстом
            foreach (var column in studentsDataGrid.Columns)
            {
                if (column != e.Column) // Убираем сортировку с остальных колонок
                {
                    column.Header = column.SortMemberPath;
                    column.SortDirection = null;
                }
            }

            e.Column.Header = $"{e.Column.SortMemberPath} {(newDirection == ListSortDirection.Ascending ? " (по возрастанию)" : " (по убыванию)")}";
        }
    }
}
