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

            if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(recordId) || string.IsNullOrEmpty(updateData))
            {
                MessageBox.Show("Пожалуйста, выберите таблицу, введите номер записи и новые данные.");
                return;
            }

            try
            {
                var values = updateData.Split(',').Select(v => v.Trim()).ToArray();

                using (var context = new Medical_ClinicEntities())
                {
                    int skipCount = int.Parse(recordId) - 1;

                    if (tableName == "Пациенты")
                    {
                        var record = context.Patients.OrderBy(p => p.FullName).Skip(skipCount).FirstOrDefault();
                        if (record != null)
                        {
                            if (values[0] != "-") record.DateOfBirth = DateTime.Parse(values[0]);
                            if (values[1] != "-") record.Gender = values[1];
                            if (values[2] != "-") record.Addresss = values[2];
                            if (values[3] != "-") record.Phone = values[3];
                            if (values[4] != "-") record.Email = values[4];
                            if (values[5] != "-") record.InsuranceNumber = values[5];

                            context.SaveChanges();
                            MessageBox.Show("Запись пациента успешно обновлена!");
                        }
                        else
                        {
                            MessageBox.Show("Запись пациента не найдена!");
                        }
                    }
                    else if (tableName == "Врачи")
                    {
                        var record = context.Doctors.OrderBy(d => d.FullName).Skip(skipCount).FirstOrDefault();
                        if (record != null)
                        {
                            if (values[0] != "-") record.Specialization = values[0];
                            if (values[1] != "-") record.Phone = values[1];
                            if (values[2] != "-") record.Email = values[2];
                            if (values[3] != "-") record.HireDate = DateTime.Parse(values[3]);

                            context.SaveChanges();
                            MessageBox.Show("Запись врача успешно обновлена!");
                        }
                        else
                        {
                            MessageBox.Show("Запись врача не найдена!");
                        }
                    }
                    else if (tableName == "Записи на прием")
                    {
                        var record = context.Appointments.OrderBy(a => a.AppointmentID).Skip(skipCount).FirstOrDefault();
                        if (record != null)
                        {
                            if (values[0] != "-") record.AppointmentDate = DateTime.Parse(values[0]);
                            if (values[1] != "-") record.Statuss = values[1];

                            context.SaveChanges();
                            MessageBox.Show("Запись на прием успешно обновлена!");
                        }
                        else
                        {
                            MessageBox.Show("Запись на прием не найдена!");
                        }
                    }
                    else if (tableName == "Медицинские карты")
                    {
                        var record = context.MedicalRecords.OrderBy(m => m.RecordID).Skip(skipCount).FirstOrDefault();
                        if (record != null)
                        {
                            if (values[0] != "-") record.VisitDate = DateTime.Parse(values[0]);
                            if (values[1] != "-") record.Diagnosis = values[1];
                            if (values[2] != "-") record.Prescriptions = values[2];
                            if (values[3] != "-") record.Comments = values[3];

                            context.SaveChanges();
                            MessageBox.Show("Медицинская карта успешно обновлена!");
                        }
                        else
                        {
                            MessageBox.Show("Медицинская карта не найдена!");
                        }
                    }
                    else if (tableName == "Услуги")
                    {
                        var record = context.Servicess.OrderBy(s => s.ServiceName).Skip(skipCount).FirstOrDefault();
                        if (record != null)
                        {
                            if (values[0] != "-") record.Descriptionn = values[0];
                            if (values[1] != "-") record.Price = decimal.Parse(values[1]);

                            context.SaveChanges();
                            MessageBox.Show("Услуга успешно обновлена!");
                        }
                        else
                        {
                            MessageBox.Show("Услуга не найдена!");
                        }
                    }

                    else if (tableName == "Расписание врачей")
                    {
                        var record = context.Schedules.OrderBy(s => s.ScheduleID).Skip(skipCount).FirstOrDefault();
                        if (record != null)
                        {
                            if (values[0] != "-") record.Day_of_week = values[0];
                            if (values[1] != "-") record.StartTime = TimeSpan.Parse(values[1]);
                            if (values[2] != "-") record.EndTime = TimeSpan.Parse(values[2]);

                            context.SaveChanges();
                            MessageBox.Show("Расписание успешно обновлено!");
                        }

                        else
                        {
                            MessageBox.Show("Расписание не найдено!");
                        }
                    }

                    else if (tableName == "Кабинеты")
                    {
                        var record = context.Rooms.OrderBy(s => s.RoomNumber).Skip(skipCount).FirstOrDefault();
                        if (record != null)
                        {
                            if (values[0] != "-") record.Descriptionn = values[0];

                            context.SaveChanges();
                            MessageBox.Show("Описание кабинета успешно обновлено!");
                        }

                        else
                        {
                            MessageBox.Show("Кабинет не найден!");
                        }
                    }

                    else if (tableName == "Счета")
                    {
                        var record = context.Invoices.OrderBy(s => s.InvoiceID).Skip(skipCount).FirstOrDefault();
                        if (record != null)
                        {
                            if (values[0] != "-") record.InvoiceDate = DateTime.Parse(values[0]);
                            if (values[1] != "-") record.Amount = decimal.Parse(values[1]);
                            if (values[2] != "-") record.Statuss = values[2];

                            context.SaveChanges();
                            MessageBox.Show("Счет успешно обновлен!");
                        }

                        else
                        {
                            MessageBox.Show("Счет не найден!");
                        }
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
