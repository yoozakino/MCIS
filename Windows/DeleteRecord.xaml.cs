using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Информационная_система_медицинской_клиники.Windows
{
    public partial class DeleteRecord : Window
    {
        public Program MainProgramWindow { get; set; }

        public DeleteRecord()
        {
            InitializeComponent();
            tableComboBox.SelectedIndex = 0;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            string idText = recordId.Text.Trim();

            if (string.IsNullOrEmpty(idText))
            {
                MessageBox.Show("Введите ID записи!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(idText, out int recordId))
            {
                MessageBox.Show("ID должен быть числом!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var context = Medical_ClinicEntities.GetContext();
                bool recordFound = false;

                switch (tableComboBox.SelectedIndex)
                {
                    case 0: // Пациенты
                        var patient = context.Patients.Find(recordId);
                        if (patient != null)
                        {
                            context.Patients.Remove(patient);
                            recordFound = true;
                        }
                        break;

                    case 1: // Врачи
                        var doctor = context.Doctors.Find(recordId);
                        if (doctor != null)
                        {
                            context.Doctors.Remove(doctor);
                            recordFound = true;
                        }
                        break;

                    case 2: // Записи на прием
                        var appointment = context.Appointments.Find(recordId);
                        if (appointment != null)
                        {
                            context.Appointments.Remove(appointment);
                            recordFound = true;
                        }
                        break;

                    case 3: // Медицинские карты
                        var medicalRecord = context.MedicalRecords.Find(recordId);
                        if (medicalRecord != null)
                        {
                            context.MedicalRecords.Remove(medicalRecord);
                            recordFound = true;
                        }
                        break;

                    case 4: // Лекарства
                        var medication = context.Medications.Find(recordId);
                        if (medication != null)
                        {
                            context.Medications.Remove(medication);
                            recordFound = true;
                        }
                        break;

                    case 5: // Назначения лекарств
                        var prescription = context.Prescriptions.Find(recordId);
                        if (prescription != null)
                        {
                            context.Prescriptions.Remove(prescription);
                            recordFound = true;
                        }
                        break;

                    case 6: // Кабинеты
                        var room = context.Rooms.Find(recordId);
                        if (room != null)
                        {
                            context.Rooms.Remove(room);
                            recordFound = true;
                        }
                        break;

                    case 7: // Расписание врачей
                        var schedule = context.Schedules.Find(recordId);
                        if (schedule != null)
                        {
                            context.Schedules.Remove(schedule);
                            recordFound = true;
                        }
                        break;

                    case 8: // Услуги
                        var service = context.Servicess.Find(recordId);
                        if (service != null)
                        {
                            context.Servicess.Remove(service);
                            recordFound = true;
                        }
                        break;

                    case 9: // Счета
                        var invoice = context.Invoices.Find(recordId);
                        if (invoice != null)
                        {
                            context.Invoices.Remove(invoice);
                            recordFound = true;
                        }
                        break;
                }

                if (!recordFound)
                {
                    MessageBox.Show("Запись с указанным ID не найдена!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                context.SaveChanges();
                MessageBox.Show("Запись успешно удалена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                // Обновляем данные в текущей открытой странице Program.xaml
                if (MainProgramWindow != null)
                {
                    MainProgramWindow.RefreshCurrentPage();
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении записи: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Medical_ClinicEntities.ResetContext();
            }
        }
    }
}