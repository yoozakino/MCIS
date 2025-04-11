using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Информационная_система_медицинской_клиники.Pages;

namespace Информационная_система_медицинской_клиники.Windows
{
    public partial class AddRecord : Window
    {
        public AddRecord()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string tableName = (tablename.SelectedItem as ComboBoxItem)?.Content.ToString();
            string recordData = recorddata.Text.Trim();

            if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(recordData))
            {
                MessageBox.Show("Пожалуйста, выберите таблицу и введите данные.");
                return;
            }

            try
            {
                var values = recordData.Split(',');
                values = values.Select(v => v.Trim()).ToArray();

                using (var context = new Medical_ClinicEntities())
                {
                    if (tableName == "Пациенты")
                    {
                        var newRecord = new Patients
                        {
                            FullName = values[0],
                            DateOfBirth = DateTime.Parse(values[1]),
                            Gender = values[2],
                            Addresss = values[3],
                            Phone = values[4],
                            Email = values[5],
                            InsuranceNumber = values[6]
                        };
                        context.Patients.Add(newRecord);
                    }
                    
                    else if (tableName == "Врачи")
                    {
                        var newRecord = new Doctors
                        {
                            FullName = values[0],
                            Specialization = values[1],
                            Phone = values[2],
                            Email = values[3],
                            HireDate = DateTime.Parse(values[4])
                        };
                        context.Doctors.Add(newRecord);
                        context.SaveChanges();
                    }

                    else if (tableName == "Записи на приём")
                    {
                        var newRecord = new Appointments
                        {
                            AppointmentID = context.Appointments.Any() ? context.Appointments.Max(a => a.AppointmentID) + 1 : 1, 
                            PatientName = values[0],
                            DoctorName = values[1],
                            AppointmentDate = DateTime.Parse(values[2]),
                            Statuss = values[3]
                        };
                        context.Appointments.Add(newRecord);
                    }

                    else if (tableName == "Медицинские карты")
                    {
                        var newRecord = new MedicalRecords
                        {
                            RecordID = context.MedicalRecords.Any() ? context.MedicalRecords.Max(a => a.RecordID) + 1 : 1,
                            PatientName = values[0],
                            DoctorName = values[1],
                            VisitDate = DateTime.Parse(values[2]),
                            Diagnosis = values[3],
                            Prescriptions = values[4],
                            Comments = values[5]
                        };
                        context.MedicalRecords.Add(newRecord);
                    }

                    else if (tableName == "Услуги")
                    {
                        if (decimal.TryParse(values[2], out decimal price))
                        {
                            var newRecord = new Servicess
                            {
                                ServiceName = values[0],
                                Descriptionn = values[1],
                                Price = price
                            };

                            context.Servicess.Add(newRecord);
                            context.SaveChanges();
                        }

                        else
                        {
                            MessageBox.Show("Ошибка: Неверный формат цены.");
                        }
                    }

                    else if (tableName == "Расписание врачей")
                    {
                        if (TimeSpan.TryParse(values[2], out TimeSpan startTime) &&
                            TimeSpan.TryParse(values[3], out TimeSpan endTime))
                        {
                            var newRecord = new Schedules
                            {
                                ScheduleID = context.Schedules.Any() ? context.Schedules.Max(a => a.ScheduleID) + 1 : 1,
                                DoctorName = values[0],
                                Day_of_week = values[1],
                                StartTime = startTime,
                                EndTime = endTime
                            };
                            context.Schedules.Add(newRecord);
                        }
                        else
                        {
                            MessageBox.Show("Ошибка: Неверный формат времени (используйте ЧЧ:ММ)");
                            return;
                        }
                    }

                    else if (tableName == "Кабинеты")
                    {
                        if (values.Length < 2)
                        {
                            MessageBox.Show("Пожалуйста, введите номер кабинета и его описание, через запятую.");
                            return;
                        }

                        var roomNumber = values[0];
                        var description = values[1];

                        // Проверка на уникальность номера кабинета
                        if (context.Rooms.Any(r => r.RoomNumber == roomNumber))
                        {
                            MessageBox.Show("Кабинет с таким номером уже существует.");
                            return;
                        }

                        var newRecord = new Rooms
                        {
                            RoomNumber = roomNumber,
                            Descriptionn = description
                        };

                        context.Rooms.Add(newRecord);
                    }

                    else if (tableName == "Счета")
                    {
                        if (values.Length < 5)
                        {
                            MessageBox.Show("Пожалуйста, введите данные: Имя пациента, Название услуги, Дата (ГГГГ-ММ-ДД), Сумма, Статус");
                            return;
                        }

                        if (!DateTime.TryParse(values[2], out DateTime invoiceDate))
                        {
                            MessageBox.Show("Ошибка: Неверный формат даты.");
                            return;
                        }

                        if (!decimal.TryParse(values[3], out decimal amount))
                        {
                            MessageBox.Show("Ошибка: Неверный формат суммы.");
                            return;
                        }

                        var newInvoice = new Invoices
                        {
                            InvoiceID = context.Invoices.Any() ? context.Invoices.Max(i => i.InvoiceID) + 1 : 1,
                            PatientName = values[0],
                            ServiceName = values[1],
                            InvoiceDate = invoiceDate,
                            Amount = amount,
                            Statuss = values[4]
                        };

                        context.Invoices.Add(newInvoice);
                    }


                    context.SaveChanges();
                    MessageBox.Show("Запись успешно добавлена!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}
