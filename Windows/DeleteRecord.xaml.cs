using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Информационная_система_медицинской_клиники.Windows
{
    public partial class DeleteRecord : Window
    {
        public DeleteRecord()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string tableName = (tablename.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (!int.TryParse(recordnum.Text.Trim(), out int recordIndex) || recordIndex < 1)
            {
                MessageBox.Show("Введите корректный порядковый номер записи (начиная с 1)");
                return;
            }

            try
            {
                using (var context = new Medical_ClinicEntities())
                {
                    if (tableName == "Пациенты")
                    {
                        var records = context.Patients.ToList();

                        if (recordIndex > records.Count)
                        {
                            MessageBox.Show("Запись с таким номером не найдена");
                            return;
                        }

                        var recordToDelete = records[recordIndex - 1];

                        var appointments = context.Appointments.Where(a => a.PatientName == recordToDelete.FullName).ToList();
                        if (appointments.Any())
                        {
                            MessageBox.Show("Невозможно удалить пациента, так как на него есть записи на приём.");
                            return;
                        }

                        var medicalRecords = context.MedicalRecords.Where(mr => mr.PatientName == recordToDelete.FullName).ToList();
                        if (medicalRecords.Any())
                        {
                            MessageBox.Show("Невозможно удалить пациента, так как на него есть медицинские карты.");
                            return;
                        }

                        var invoices = context.Invoices.Where(i => i.PatientName == recordToDelete.FullName).ToList();
                        if (invoices.Any())
                        {
                            MessageBox.Show("Невозможно удалить пациента, так как на него есть счета.");
                            return;
                        }

                        context.Patients.Remove(recordToDelete);
                        context.SaveChanges();

                        MessageBox.Show("Запись о пациенте успешно удалена");
                    }
                    else if (tableName == "Врачи")
                    {
                        var records = context.Doctors.ToList();
                        if (recordIndex > records.Count)
                        {
                            MessageBox.Show("Запись с таким номером не найдена");
                            return;
                        }

                        var recordToDelete = records[recordIndex - 1];

                        var appointments = context.Appointments.Where(a => a.DoctorName == recordToDelete.FullName).ToList();
                        if (appointments.Any())
                        {
                            MessageBox.Show("Невозможно удалить врача, так как на него есть записи на приём.");
                            return;
                        }

                        context.Doctors.Remove(recordToDelete);
                        context.SaveChanges();

                        MessageBox.Show("Запись о враче успешно удалена");
                    }

                    else if (tableName == "Записи на приём")
                    {
                        var records = context.Appointments.ToList();

                        if (recordIndex > records.Count)
                        {
                            MessageBox.Show("Запись с таким номером не найдена");
                            return;
                        }

                        var recordToDelete = records[recordIndex - 1];
                        context.Appointments.Remove(recordToDelete);
                        context.SaveChanges();

                        MessageBox.Show("Запись о приёме успешно удалена");
                    }

                    else if (tableName == "Услуги")
                    {
                        var records = context.Servicess.ToList();

                        if (recordIndex > records.Count)
                        {
                            MessageBox.Show("Запись с таким номером не найдена");
                            return;
                        }

                        var recordToDelete = records[recordIndex - 1];

                        var invoices = context.Invoices.Where(i => i.ServiceName == recordToDelete.ServiceName).ToList();
                        if (invoices.Any())
                        {
                            MessageBox.Show("Невозможно удалить услугу, так как она используется в выставленных счетах.");
                            return;
                        }

                        context.Servicess.Remove(recordToDelete);
                        context.SaveChanges();

                        MessageBox.Show("Запись об услуге успешно удалена");
                    }

                    else if (tableName == "Медицинские карты")
                    {
                        var records = context.MedicalRecords.ToList();

                        if (recordIndex > records.Count)
                        {
                            MessageBox.Show("Запись с таким номером не найдена");
                            return;
                        }

                        var recordToDelete = records[recordIndex - 1];

                        var prescriptions = context.Prescriptions.Where(p => p.RecordID == recordToDelete.RecordID).ToList();
                        if (prescriptions.Any())
                        {
                            MessageBox.Show("Невозможно удалить медицинскую карту, так как на неё есть назначенные лекарства.");
                            return;
                        }

                        context.MedicalRecords.Remove(recordToDelete);
                        context.SaveChanges();

                        MessageBox.Show("Запись о медицинской карте успешно удалена");
                    }

                    else if (tableName == "Расписание врачей")
                    {
                        var records = context.Schedules.ToList();

                        if (recordIndex > records.Count)
                        {
                            MessageBox.Show("Запись с таким номером не найдена");
                            return;
                        }

                        var recordToDelete = records[recordIndex - 1];

                        context.Schedules.Remove(recordToDelete);
                        context.SaveChanges();

                        MessageBox.Show("Запись из расписания успешно удалена");
                    }

                    else if (tableName == "Кабинеты")
                    {
                        var records = context.Rooms.ToList();

                        if (recordIndex > records.Count)
                        {
                            MessageBox.Show("Запись с таким номером не найдена");
                            return;
                        }

                        var recordToDelete = records[recordIndex - 1];
                        context.Rooms.Remove(recordToDelete);
                        context.SaveChanges();

                        MessageBox.Show("Запись о кабинете успешно удалена");
                    }

                    else if (tableName == "Счета")
                    {
                        var records = context.Invoices.ToList();

                        if (recordIndex > records.Count)
                        {
                            MessageBox.Show("Запись с таким номером не найдена");
                            return;
                        }

                        var recordToDelete = records[recordIndex - 1];
                        context.Invoices.Remove(recordToDelete);
                        context.SaveChanges();

                        MessageBox.Show("Счёт успешно удалён");
                    }

                    else
                    {
                        MessageBox.Show("Выберите корректную таблицу.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}
