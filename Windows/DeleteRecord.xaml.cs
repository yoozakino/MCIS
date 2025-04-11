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
                    object recordToDelete = null;
                    string successMessage = "";

                    switch (tableName)
                    {
                        case "Пациенты":
                            var patients = context.Patients.ToList();
                            if (recordIndex > patients.Count)
                            {
                                MessageBox.Show("Запись с таким номером не найдена");
                                return;
                            }

                            recordToDelete = patients[recordIndex - 1];

                            if (context.Appointments.Any(a => a.PatientName == ((Patients)recordToDelete).FullName) ||
                                context.MedicalRecords.Any(mr => mr.PatientName == ((Patients)recordToDelete).FullName) ||
                                context.Invoices.Any(i => i.PatientName == ((Patients)recordToDelete).FullName))
                            {
                                MessageBox.Show("Невозможно удалить пациента, так как он связан с другими записями.");
                                return;
                            }

                            context.Patients.Remove((Patients)recordToDelete);
                            successMessage = "Запись о пациенте успешно удалена";
                            break;

                        case "Врачи":
                            var doctors = context.Doctors.ToList();
                            if (recordIndex > doctors.Count)
                            {
                                MessageBox.Show("Запись с таким номером не найдена");
                                return;
                            }

                            recordToDelete = doctors[recordIndex - 1];

                            if (context.Appointments.Any(a => a.DoctorName == ((Doctors)recordToDelete).FullName))
                            {
                                MessageBox.Show("Невозможно удалить врача, так как на него есть записи на приём.");
                                return;
                            }

                            context.Doctors.Remove((Doctors)recordToDelete);
                            successMessage = "Запись о враче успешно удалена";
                            break;

                        case "Записи на приём":
                            var appointments = context.Appointments.ToList();
                            if (recordIndex > appointments.Count)
                            {
                                MessageBox.Show("Запись с таким номером не найдена");
                                return;
                            }

                            context.Appointments.Remove(appointments[recordIndex - 1]);
                            successMessage = "Запись о приёме успешно удалена";
                            break;

                        case "Услуги":
                            var services = context.Servicess.ToList();
                            if (recordIndex > services.Count)
                            {
                                MessageBox.Show("Запись с таким номером не найдена");
                                return;
                            }

                            recordToDelete = services[recordIndex - 1];

                            if (context.Invoices.Any(i => i.ServiceName == ((Servicess)recordToDelete).ServiceName))
                            {
                                MessageBox.Show("Невозможно удалить услугу, так как она используется в выставленных счетах.");
                                return;
                            }

                            context.Servicess.Remove((Servicess)recordToDelete);
                            successMessage = "Запись об услуге успешно удалена";
                            break;

                        case "Медицинские карты":
                            var medicalRecords = context.MedicalRecords.ToList();
                            if (recordIndex > medicalRecords.Count)
                            {
                                MessageBox.Show("Запись с таким номером не найдена");
                                return;
                            }

                            recordToDelete = medicalRecords[recordIndex - 1];

                            if (context.Prescriptions.Any(p => p.RecordID == ((MedicalRecords)recordToDelete).RecordID))
                            {
                                MessageBox.Show("Невозможно удалить медицинскую карту, так как на неё есть назначенные лекарства.");
                                return;
                            }

                            context.MedicalRecords.Remove((MedicalRecords)recordToDelete);
                            successMessage = "Запись о медицинской карте успешно удалена";
                            break;

                        case "Расписание врачей":
                            var schedules = context.Schedules.ToList();
                            if (recordIndex > schedules.Count)
                            {
                                MessageBox.Show("Запись с таким номером не найдена");
                                return;
                            }

                            context.Schedules.Remove(schedules[recordIndex - 1]);
                            successMessage = "Запись из расписания успешно удалена";
                            break;

                        case "Кабинеты":
                            var rooms = context.Rooms.ToList();
                            if (recordIndex > rooms.Count)
                            {
                                MessageBox.Show("Запись с таким номером не найдена");
                                return;
                            }

                            context.Rooms.Remove(rooms[recordIndex - 1]);
                            successMessage = "Запись о кабинете успешно удалена";
                            break;

                        case "Счета":
                            var invoices = context.Invoices.ToList();
                            if (recordIndex > invoices.Count)
                            {
                                MessageBox.Show("Запись с таким номером не найдена");
                                return;
                            }

                            context.Invoices.Remove(invoices[recordIndex - 1]);
                            successMessage = "Счёт успешно удалён";
                            break;

                        case "Лекарства":
                            var meds = context.Medications.ToList();
                            if (recordIndex > meds.Count)
                            {
                                MessageBox.Show("Запись с таким номером не найдена");
                                return;
                            }

                            context.Medications.Remove(meds[recordIndex - 1]);
                            successMessage = "Лекарство успешно удалено";
                            break;

                        case "Инструкции":
                            var prescriptions = context.Prescriptions.ToList();
                            if (recordIndex > prescriptions.Count)
                            {
                                MessageBox.Show("Запись с таким номером не найдена");
                                return;
                            }

                            context.Prescriptions.Remove(prescriptions[recordIndex - 1]);
                            successMessage = "Инструкция успешно удалена";
                            break;

                        default:
                            MessageBox.Show("Выберите корректную таблицу.");
                            return;
                    }

                    context.SaveChanges();
                    MessageBox.Show(successMessage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}
