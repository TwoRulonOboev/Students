using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Students.Pages
{
    public partial class Student : Page
    {
        private DataTable studentsTable;
        private int currentStudentIndex = 0;

        // Connection string to the database
        private string connectionString = "Data Source=PC1;Initial Catalog=students;Integrated Security=True";

        public Student()
        {
            InitializeComponent();
            LoadStudentsFromDatabase();
            DisplayCurrentStudent();
        }

        // Method to load all student data from the database
        private void LoadStudentsFromDatabase()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT TOP (1000) [ФИО], [Пол], [Дата рождения], [Родители], [Адрес], [Телефон], [Паспортные данные], [Номер зачетки], [Дата поступления], [Группа], [Курс], [Код специальности], [Очная форма обучения] FROM [students].[dbo].[Студенты]";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    studentsTable = new DataTable();
                    adapter.Fill(studentsTable);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при подключении к базе данных: " + ex.Message);
                }
            }
        }

        // Method to display the current student based on the index
        private void DisplayCurrentStudent()
        {
            if (studentsTable.Rows.Count > 0 && currentStudentIndex >= 0 && currentStudentIndex < studentsTable.Rows.Count)
            {
                DataRow student = studentsTable.Rows[currentStudentIndex];

                txtFIO.Text = student["ФИО"].ToString();
                cmbPol.SelectedItem = student["Пол"].ToString() == "Мужской" ? cmbPol.Items[0] : cmbPol.Items[1];
                dpDateOfBirth.SelectedDate = Convert.ToDateTime(student["Дата рождения"]);
                txtRoditeli.Text = student["Родители"].ToString();
                txtAdress.Text = student["Адрес"].ToString();
                txtNum.Text = student["Телефон"].ToString();
                txtPassport.Text = student["Паспортные данные"].ToString();
                txtNumOfZach.Text = student["Номер зачетки"].ToString();
                dpDate.SelectedDate = Convert.ToDateTime(student["Дата поступления"]);
                txtGroup.Text = student["Группа"].ToString();
                txtKours.Text = student["Курс"].ToString();
                cmbKodSpec.SelectedItem = student["Код специальности"].ToString();
                chkOchFormaObuch.IsChecked = Convert.ToBoolean(student["Очная форма обучения"]);
            }
        }

        // Button click event handlers
        private void btnFirst_Click(object sender, RoutedEventArgs e)
        {
            currentStudentIndex = 0;
            DisplayCurrentStudent();
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (currentStudentIndex > 0)
            {
                currentStudentIndex--;
                DisplayCurrentStudent();
            }
            else
            {
                MessageBox.Show("Это первый студент.");
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (currentStudentIndex < studentsTable.Rows.Count - 1)
            {
                currentStudentIndex++;
                DisplayCurrentStudent();
            }
            else
            {
                MessageBox.Show("Это последний студент.");
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            // Create a new row in the studentsTable
            DataRow newRow = studentsTable.NewRow();

            // Get the values from the UI controls
            newRow["ФИО"] = txtFIO.Text;
            newRow["Пол"] = cmbPol.SelectedItem.ToString();
            newRow["Дата рождения"] = dpDateOfBirth.SelectedDate;
            newRow["Родители"] = txtRoditeli.Text;
            newRow["Адрес"] = txtAdress.Text;
            newRow["Телефон"] = txtNum.Text;
            newRow["Паспортные данные"] = txtPassport.Text;
            newRow["Номер зачетки"] = txtNumOfZach.Text;
            newRow["Дата поступления"] = dpDate.SelectedDate;
            newRow["Группа"] = txtGroup.Text;
            newRow["Курс"] = txtKours.Text;
            newRow["Код специальности"] = cmbKodSpec.SelectedItem.ToString();
            newRow["Очная форма обучения"] = chkOchFormaObuch.IsChecked;

            // Add the new row to the studentsTable
            studentsTable.Rows.Add(newRow);

            // Update the currentStudentIndex
            currentStudentIndex = studentsTable.Rows.Count - 1;

            // Display the new student
            DisplayCurrentStudent();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (currentStudentIndex >= 0 && currentStudentIndex < studentsTable.Rows.Count)
            {
                // Remove the current row from the studentsTable
                studentsTable.Rows.RemoveAt(currentStudentIndex);

                // Update the currentStudentIndex
                if (currentStudentIndex > 0)
                {
                    currentStudentIndex--;
                }
                else
                {
                    currentStudentIndex = 0;
                }

                // Display the previous student
                DisplayCurrentStudent();
            }
            else
            {
                MessageBox.Show("Нет студентов для удаления.");
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Create a SqlCommand to update the database
                    SqlCommand command = new SqlCommand("UPDATE [StudentsSIA].[dbo].[Студенты] SET [ФИО] = @FIO, [Пол] = @Pol, [Дата рождения] = @DateOfBirth, [Родители] = @Roditeli, [Адрес] = @Adress, [Телефон] = @Num, [Паспортные данные] = @Passport, [Номер зачетки] = @NumOfZach, [Дата поступления] = @Date, [Группа] = @Group, [Курс] = @Kours, [Код специальности] = @KodSpec, [Очная форма обучения] = @OchFormaObuch WHERE [Код студента] = @KodStudenta", connection);

                    // Add parameters to the command
                    command.Parameters.AddWithValue("@FIO", txtFIO.Text);
                    command.Parameters.AddWithValue("@Pol", cmbPol.SelectedItem.ToString());
                    command.Parameters.AddWithValue("@DateOfBirth", dpDateOfBirth.SelectedDate);
                    command.Parameters.AddWithValue("@Roditeli", txtRoditeli.Text);
                    command.Parameters.AddWithValue("@Adress", txtAdress.Text);
                    command.Parameters.AddWithValue("@Num", txtNum.Text);
                    command.Parameters.AddWithValue("@Passport", txtPassport.Text);
                    command.Parameters.AddWithValue("@NumOfZach", txtNumOfZach.Text);
                    command.Parameters.AddWithValue("@Date", dpDate.SelectedDate);
                    command.Parameters.AddWithValue("@Group", txtGroup.Text);
                    command.Parameters.AddWithValue("@Kours", txtKours.Text);
                    command.Parameters.AddWithValue("@KodSpec", cmbKodSpec.SelectedItem.ToString());
                    command.Parameters.AddWithValue("@OchFormaObuch", chkOchFormaObuch.IsChecked);

                    // Execute the command
                    command.ExecuteNonQuery();

                    MessageBox.Show("Изменения сохранены.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при сохранении изменений: " + ex.Message);
                }
            }
        }
    }
}