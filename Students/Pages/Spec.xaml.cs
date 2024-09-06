using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static Students.Pages.Subject;

namespace Students.Pages
{
    /// <summary>
    /// Логика взаимодействия для Spec.xaml
    /// </summary>
    public partial class Spec : Page
    {
        public Spec()
        {
            InitializeComponent();
            LoadData();
            DisplaySpec(specDataList[currentIndex]);
        }

        private int currentIndex = 0;

        private void LoadData()
        {
            string connectionString = "Data Source=PC1;Initial Catalog=students;Integrated Security=True";
            string query = "SELECT TOP (1000) [Код специальности], [Наименование специальности], [Описание специальности] FROM [dbo].[Специальности]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string code = reader["Код специальности"].ToString();
                    string name = reader["Наименование специальности"].ToString();
                    string description = reader["Описание специальности"].ToString();

                    // Create a new data object to hold the loaded data
                    SpecData data = new SpecData { Code = code, Name = name, Description = description };

                    // Add the data object to a list or collection
                    specDataList.Add(data);
                }
            }
        }

        private List<SpecData> specDataList = new List<SpecData>();

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected data object from the list
            SpecData selectedData = specDataList.FirstOrDefault(d => d.Code == txtCode.Text);

            if (selectedData != null)
            {
                // Update the data object with the new values
                selectedData.Name = txtName.Text;
                selectedData.Description = txtOpic.Text;

                // Update the database with the new values
                UpdateData(selectedData);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected data object from the list
            SpecData selectedData = specDataList.FirstOrDefault(d => d.Code == txtCode.Text);

            if (selectedData != null)
            {
                // Remove the data object from the list
                specDataList.Remove(selectedData);

                // Delete the data from the database
                DeleteData(selectedData.Code);
            }
        }

        private void UpdateData(SpecData data)
        {
            string connectionString = "Data Source=PC1;Initial Catalog=students;Integrated Security=True";
            string query = "UPDATE [dbo].[Специальности] SET [Наименование специальности] = @Name, [Описание специальности] = @Description WHERE [Код специальности] = @Code";

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

        private void DeleteData(string code)
        {
            string connectionString = "Data Source=PC1;Initial Catalog=students;Integrated Security=True";
            string query = "DELETE FROM [dbo].[Специальности] WHERE [Код специальности] = @Code";

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
            if (currentIndex < specDataList.Count - 1)
            {
                currentIndex++;
                DisplaySpec(specDataList[currentIndex]);
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentIndex > 0)
            {
                currentIndex--;
                DisplaySpec(specDataList[currentIndex]);
            }
        }

        private void DisplaySpec(SpecData spec)
        {
            txtCode.Text = spec.Code;
            txtName.Text = spec.Name;
            txtOpic.Text = spec.Description;
        }

        public class SpecData
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}


