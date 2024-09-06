using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace Students.Pages
{
    /// <summary>
    /// Логика взаимодействия для Subject.xaml
    /// </summary>
    public partial class Subject : Page
    {
        private List<SubjectData> subjectDataList = new List<SubjectData>();
        private int currentIndex = 0;

        public Subject()
        {
            InitializeComponent();
            LoadData();
            DisplaySpec(subjectDataList[currentIndex]);
        }

        private void LoadData()
        {
            string connectionString = "Data Source=PC1;Initial Catalog=students;Integrated Security=True";
            string query = "SELECT TOP (1000) [Код предмета], [Нименования предмета], [Описание предмета] FROM [dbo].[Предметы]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string code = reader["Код предмета"].ToString();
                    string name = reader["Нименования предмета"].ToString();
                    string description = reader["Описание предмета"].ToString();

                    SubjectData data = new SubjectData { Code = code, Name = name, Description = description };

                    subjectDataList.Add(data);
                }
            }
        }




        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected data object from the list
            SubjectData selectedData = subjectDataList.FirstOrDefault(d => d.Code == txtCode.Text);

            if (selectedData != null)
            {
                // Update the data object with the new values
                selectedData.Name = txtName.Text;
                selectedData.Description = txtOpic.Text;

                // Update the database with the new values
                UpdateSubjectData(selectedData);
                UpdateSubjectInGrades(selectedData, selectedData.Code); // Pass the old code as a parameter
            }
        }

        private void UpdateSubjectData(SubjectData data)
        {
            string connectionString = "Data Source=PC1;Initial Catalog=students;Integrated Security=True";
            string query = "UPDATE [dbo].[Предметы] SET [Нименования предмета] = @Name, [Описание предмета] = @Description WHERE [Код предмета] = @Code";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", data.Name);
                command.Parameters.AddWithValue("@Description", data.Description);
                command.Parameters.AddWithValue("@Code", data.Code);

                command.ExecuteNonQuery();
            }
        }

        private void UpdateSubjectInGrades(SubjectData data, string oldCode)
        {
            string connectionString = "Data Source=PC1;Initial Catalog=students;Integrated Security=True";
            string query = "UPDATE [dbo].[Оценки] SET [Код предмета 1] = @Code WHERE [Код предмета 1] = @OldCode; UPDATE [dbo].[Оценки] SET [Код предмета 2] = @Code WHERE [Код предмета 2] = @OldCode; UPDATE [dbo].[Оценки] SET [Код предмета 3] = @Code WHERE [Код предмета 3] = @OldCode";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Code", data.Code);
                command.Parameters.AddWithValue("@OldCode", oldCode);

                command.ExecuteNonQuery();
            }
        }
        private void UpdateSubjectInGrades(SubjectData data)
        {
            string connectionString = "Data Source=PC1;Initial Catalog=students;Integrated Security=True";
            string query = "UPDATE [dbo].[Оценки] SET [Код предмета 1] = @Code WHERE [Код предмета 1] = @OldCode; UPDATE [dbo].[Оценки] SET [Код предмета 2] = @Code WHERE [Код предмета 2] = @OldCode; UPDATE [dbo].[Оценки] SET [Код предмета 3] = @Code WHERE [Код предмета 3] = @OldCode";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Code", data.Code);
                command.Parameters.AddWithValue("@OldCode", data.Code);

                command.ExecuteNonQuery();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected data object from the list
            SubjectData selectedData = subjectDataList.FirstOrDefault(d => d.Code == txtCode.Text);

            if (selectedData != null)
            {
                // Remove the data object from the list
                subjectDataList.Remove(selectedData);

                // Delete the data from the database
                DeleteSubjectData(selectedData.Code);
            }
        }

        private void DeleteSubjectData(string code)
        {
            string connectionString = "Data Source=PC1;Initial Catalog=students;Integrated Security=True";

            // Delete related rows from Оценки table
            string deleteGradesQuery = "DELETE FROM [dbo].[Оценки] WHERE [Код предмета 1] = @Code OR [Код предмета 2] = @Code OR [Код предмета 3] = @Code";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(deleteGradesQuery, connection);
                command.Parameters.AddWithValue("@Code", code);

                command.ExecuteNonQuery();
            }

            // Delete the subject from Предметы table
            string query = "DELETE FROM [dbo].[Предметы] WHERE [Код предмета] = @Code";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Code", code);

                command.ExecuteNonQuery();
            }
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentIndex < subjectDataList.Count - 1)
            {
                currentIndex++;
                DisplaySpec(subjectDataList[currentIndex]);
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentIndex > 0)
            {
                currentIndex--;
                DisplaySpec(subjectDataList[currentIndex]);
            }
        }

        private void DisplaySpec(SubjectData spec)
        {
            txtCode.Text = spec.Code;
            txtName.Text = spec.Name;
            txtOpic.Text = spec.Description;
        }

        public class SubjectData
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            // Создайте новый объект SubjectData
            SubjectData newSubject = new SubjectData();

            // Получите данные из текстовых полей
            newSubject.Code = txtCode.Text;
            newSubject.Name = txtName.Text;
            newSubject.Description = txtOpic.Text; // Используйте правильное имя текстового поля

            // Добавьте новый объект в коллекцию данных
            subjectDataList.Add(newSubject);

            // Очистите текстовые поля
            txtCode.Clear();
            txtName.Clear();
            txtOpic.Clear(); // Используйте правильное имя текстового поля
        }

    }
}