using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Students.Pages
{
    public partial class Ocenki : Page
    {
        private DataTable studentsTable;
        private int currentStudentIndex = 0;

        // Подключение к базе данных
        private string connectionString = "Data Source=PC1;Initial Catalog=students;Integrated Security=True";

        public Ocenki()
        {
            InitializeComponent();
            LoadStudentsFromDatabase();
            DisplayCurrentStudent();
        }

        // Метод загрузки всех данных студентов из базы данных
        private void LoadStudentsFromDatabase()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT [Код студента], [Дата экзамена 1], [Код предмета 1], [Оценка 1], " +
                                   "[Дата экзамена 2], [Код предмета 2], [Оценка 2], [Дата экзамена 3], [Код предмета 3], [Оценка 3] " +
                                   "FROM [students].[dbo].[Оценки]";
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

        // Метод отображения текущего студента на основе индекса
        private void DisplayCurrentStudent()
        {
            if (studentsTable.Rows.Count > 0 && currentStudentIndex >= 0 && currentStudentIndex < studentsTable.Rows.Count)
            {
                DataRow student = studentsTable.Rows[currentStudentIndex];

                studentCodeTextBox.Text = student["Код студента"].ToString();
                examDate1DatePicker.SelectedDate = Convert.ToDateTime(student["Дата экзамена 1"]);
                subjectCode1TextBox.Text = student["Код предмета 1"].ToString();
                grade1TextBox.Text = student["Оценка 1"].ToString();

                examDate2DatePicker.SelectedDate = Convert.ToDateTime(student["Дата экзамена 2"]);
                subjectCode2TextBox.Text = student["Код предмета 2"].ToString();
                grade2TextBox.Text = student["Оценка 2"].ToString();

                examDate3DatePicker.SelectedDate = Convert.ToDateTime(student["Дата экзамена 3"]);
                subjectCode3TextBox.Text = student["Код предмета 3"].ToString();
                grade3TextBox.Text = student["Оценка 3"].ToString();
            }
        }

        // Метод вычисления среднего балла
        private void CalculateAverageGrade()
        {
            try
            {
                double grade1 = double.Parse(grade1TextBox.Text);
                double grade2 = double.Parse(grade2TextBox.Text);
                double grade3 = double.Parse(grade3TextBox.Text);

                double averageGrade = (grade1 + grade2 + grade3) / 3;
                averageGradeTextBox.Text = averageGrade.ToString("F2");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при расчете среднего балла: " + ex.Message);
            }
        }

        // Обработчик для кнопки "Вычислить средний балл"
        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            CalculateAverageGrade();
        }

        // Обработчик для кнопки "Следующий"
        private void nextButton_Click(object sender, RoutedEventArgs e)
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

        // Обработчик для кнопки "Предыдущий"
        private void backButton_Click(object sender, RoutedEventArgs e)
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
    }
}
