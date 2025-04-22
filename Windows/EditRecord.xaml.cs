using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Информационная_система_медицинской_клиники.Windows
{
    public partial class EditRecord : Window
    {
        public EditRecord()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string tableName = (tablename.SelectedItem as ComboBoxItem)?.Content.ToString();
            string recordId = recordnum.Text.Trim();
            string updateData = updaterecord.Text.Trim();

            if (string.IsNullOrWhiteSpace(tableName) || string.IsNullOrWhiteSpace(recordId) || string.IsNullOrWhiteSpace(updateData))
            {
                MessageBox.Show("Пожалуйста, выберите таблицу, введите номер записи и новые данные.");
                return;
            }

            try
            {
                var values = updateData.Split(',').Select(v => v.Trim()).ToArray();
                int skipCount = int.Parse(recordId) - 1;

                using (var context = new Medical_ClinicEntities())
                {
                    object recordToUpdate = null;
                    string successMessage = null;

                    switch (tableName)
                    {
                        case "Пациенты":
                            var patient = context.Patients.OrderBy(p => p.FullName).Skip(skipCount).FirstOrDefault();
                            if (patient != null)
                            {
                                if (values[0] != "-") patient.DateOfBirth = DateTime.Parse(values[0]);
                                if (values[1] != "-") patient.Gender = values[1];
                                if (values[2] != "-") patient.Addresss = values[2];
                                if (values[3] != "-") patient.Phone = values[3];
                                if (values[4] != "-") patient.Email = values[4];
                                if (values[5] != "-") patient.InsuranceNumber = values[5];
                                recordToUpdate = patient;
                                successMessage = "Запись пациента успешно обновлена!";
                            }
                            break;

                        case "Врачи":
                            var doctor = context.Doctors.OrderBy(d => d.FullName).Skip(skipCount).FirstOrDefault();
                            if (doctor != null)
                            {
                                if (values[0] != "-") doctor.Specialization = values[0];
                                if (values[1] != "-") doctor.Phone = values[1];
                                if (values[2] != "-") doctor.Email = values[2];
                                if (values[3] != "-") doctor.HireDate = DateTime.Parse(values[3]);
                                recordToUpdate = doctor;
                                successMessage = "Запись врача успешно обновлена!";
                            }
                            break;

                        case "Записи на приём":
                            var appointment = context.Appointments.OrderBy(a => a.AppointmentID).Skip(skipCount).FirstOrDefault();
                            if (appointment != null)
                            {
                                if (values[0] != "-") appointment.AppointmentDate = DateTime.Parse(values[0]);
                                if (values[1] != "-") appointment.Statuss = values[1];
                                recordToUpdate = appointment;
                                successMessage = "Запись на приём успешно обновлена!";
                            }
                            break;

                        case "Медицинские карты":
                            var record = context.MedicalRecords.OrderBy(m => m.RecordID).Skip(skipCount).FirstOrDefault();
                            if (record != null)
                            {
                                if (values[0] != "-") record.VisitDate = DateTime.Parse(values[0]);
                                if (values[1] != "-") record.Diagnosis = values[1];
                                if (values[2] != "-") record.Prescriptions = values[2];
                                if (values[3] != "-") record.Comments = values[3];
                                recordToUpdate = record;
                                successMessage = "Медицинская карта успешно обновлена!";
                            }
                            break;

                        case "Услуги":
                            var service = context.Servicess.OrderBy(s => s.ServiceName).Skip(skipCount).FirstOrDefault();
                            if (service != null)
                            {
                                if (values[0] != "-") service.Descriptionn = values[0];
                                if (values[1] != "-") service.Price = decimal.Parse(values[1]);
                                recordToUpdate = service;
                                successMessage = "Услуга успешно обновлена!";
                            }
                            break;

                        case "Расписание врачей":
                            var schedule = context.Schedules.OrderBy(s => s.ScheduleID).Skip(skipCount).FirstOrDefault();
                            if (schedule != null)
                            {
                                if (values[0] != "-") schedule.Day_of_week = values[0];
                                if (values[1] != "-") schedule.StartTime = TimeSpan.Parse(values[1]);
                                if (values[2] != "-") schedule.EndTime = TimeSpan.Parse(values[2]);
                                recordToUpdate = schedule;
                                successMessage = "Расписание успешно обновлено!";
                            }
                            break;

                        case "Кабинеты":
                            var room = context.Rooms.OrderBy(r => r.RoomNumber).Skip(skipCount).FirstOrDefault();
                            if (room != null)
                            {
                                if (values[0] != "-") room.Descriptionn = values[0];
                                recordToUpdate = room;
                                successMessage = "Описание кабинета успешно обновлено!";
                            }
                            break;

                        case "Счета":
                            var invoice = context.Invoices.OrderBy(i => i.InvoiceID).Skip(skipCount).FirstOrDefault();
                            if (invoice != null)
                            {
                                if (values[0] != "-") invoice.InvoiceDate = DateTime.Parse(values[0]);
                                if (values[1] != "-") invoice.Amount = decimal.Parse(values[1]);
                                if (values[2] != "-") invoice.Statuss = values[2];
                                recordToUpdate = invoice;
                                successMessage = "Счёт успешно обновлён!";
                            }
                            break;

                        case "Лекарства":
                            var medication = context.Medications.OrderBy(m => m.MedicationName).Skip(skipCount).FirstOrDefault();
                            if (medication != null)
                            {
                                if (values[0] != "-") medication.Descriptionn = values[0];
                                if (values[1] != "-") medication.Dosage = values[1];
                                if (values[2] != "-") medication.Price = decimal.Parse(values[2]);
                                recordToUpdate = medication;
                                successMessage = "Данные о лекарстве успешно обновлены!";
                            }
                            break;

                        case "Инструкции":
                            var prescription = context.Prescriptions.OrderBy(p => p.PrescriptionID).Skip(skipCount).FirstOrDefault();
                            if (prescription != null)
                            {
                                if (values[0] != "-")
                                {
                                    int newRecordId = int.Parse(values[0]);
                                    if (!context.MedicalRecords.Any(mr => mr.RecordID == newRecordId))
                                    {
                                        MessageBox.Show("Медицинская карта с таким RecordID не найдена!");
                                        return;
                                    }
                                    prescription.RecordID = newRecordId;
                                }
                                if (values[1] != "-")
                                {
                                    string newMedicationName = values[1];
                                    if (!context.Medications.Any(m => m.MedicationName == newMedicationName))
                                    {
                                        MessageBox.Show("Лекарство с таким названием не найдено!");
                                        return;
                                    }
                                    prescription.MedicationName = newMedicationName;
                                }
                                if (values[2] != "-") prescription.DosageInstructions = values[2];
                                recordToUpdate = prescription;
                                successMessage = "Инструкция успешно обновлена!";
                            }
                            break;

                        default:
                            MessageBox.Show("Выберите корректную таблицу.");
                            return;
                    }

                    if (recordToUpdate != null)
                    {
                        context.SaveChanges();
                        MessageBox.Show(successMessage);
                    }
                    else
                    {
                        MessageBox.Show("Запись не найдена!");
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
