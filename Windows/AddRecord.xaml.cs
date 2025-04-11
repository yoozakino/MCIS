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
                var values = recordData.Split(',').Select(v => v.Trim()).ToArray();

                using (var context = new Medical_ClinicEntities())
                {
                    switch (tableName)
                    {
                        case "Пациенты":
                            var patient = new Patients
                            {
                                FullName = values[0],
                                DateOfBirth = DateTime.Parse(values[1]),
                                Gender = values[2],
                                Addresss = values[3],
                                Phone = values[4],
                                Email = values[5],
                                InsuranceNumber = values[6]
                            };
                            context.Patients.Add(patient);
                            break;

                        case "Врачи":
                            var doctor = new Doctors
                            {
                                FullName = values[0],
                                Specialization = values[1],
                                Phone = values[2],
                                Email = values[3],
                                HireDate = DateTime.Parse(values[4])
                            };
                            context.Doctors.Add(doctor);
                            break;

                        case "Записи на приём":
                            if (!context.Patients.Any(p => p.FullName == values[0]) || !context.Doctors.Any(d => d.FullName == values[1]))
                            {
                                MessageBox.Show("Указанный пациент или врач не существует.");
                                return;
                            }

                            var appointment = new Appointments
                            {
                                AppointmentID = context.Appointments.Any() ? context.Appointments.Max(a => a.AppointmentID) + 1 : 1,
                                PatientName = values[0],
                                DoctorName = values[1],
                                AppointmentDate = DateTime.Parse(values[2]),
                                Statuss = values[3]
                            };
                            context.Appointments.Add(appointment);
                            break;

                        case "Медицинские карты":
                            var medicalRecord = new MedicalRecords
                            {
                                RecordID = context.MedicalRecords.Any() ? context.MedicalRecords.Max(r => r.RecordID) + 1 : 1,
                                PatientName = values[0],
                                DoctorName = values[1],
                                VisitDate = DateTime.Parse(values[2]),
                                Diagnosis = values[3],
                                Prescriptions = values[4],
                                Comments = values[5]
                            };
                            context.MedicalRecords.Add(medicalRecord);
                            break;

                        case "Услуги":
                            var service = new Servicess
                            {
                                ServiceName = values[0],
                                Descriptionn = values[1],
                                Price = decimal.Parse(values[2])
                            };
                            context.Servicess.Add(service);
                            break;

                        case "Расписание врачей":
                            var schedule = new Schedules
                            {
                                ScheduleID = context.Schedules.Any() ? context.Schedules.Max(s => s.ScheduleID) + 1 : 1,
                                DoctorName = values[0],
                                Day_of_week = values[1],
                                StartTime = TimeSpan.Parse(values[2]),
                                EndTime = TimeSpan.Parse(values[3])
                            };
                            context.Schedules.Add(schedule);
                            break;

                        case "Кабинеты":
                            if (context.Rooms.Any(r => r.RoomNumber == values[0]))
                            {
                                MessageBox.Show("Кабинет с таким номером уже существует.");
                                return;
                            }

                            var room = new Rooms
                            {
                                RoomNumber = values[0],
                                Descriptionn = values[1]
                            };
                            context.Rooms.Add(room);
                            break;

                        case "Счета":
                            if (!context.Patients.Any(p => p.FullName == values[0]) || !context.Servicess.Any(s => s.ServiceName == values[1]))
                            {
                                MessageBox.Show("Указанный пациент или услуга не найдены.");
                                return;
                            }

                            var invoice = new Invoices
                            {
                                InvoiceID = context.Invoices.Any() ? context.Invoices.Max(i => i.InvoiceID) + 1 : 1,
                                PatientName = values[0],
                                ServiceName = values[1],
                                InvoiceDate = DateTime.Parse(values[2]),
                                Amount = decimal.Parse(values[3]),
                                Statuss = values[4]
                            };
                            context.Invoices.Add(invoice);
                            break;

                        case "Лекарства":
                            var medication = new Medications
                            {
                                MedicationName = values[0],
                                Descriptionn = values[1],
                                Dosage = values[2],
                                Price = decimal.Parse(values[3])
                            };
                            context.Medications.Add(medication);
                            break;

                        case "Инструкции":
                            int recordId = int.Parse(values[0]);
                            if (!context.MedicalRecords.Any(mr => mr.RecordID == recordId) || !context.Medications.Any(m => m.MedicationName == values[1]))
                            {
                                MessageBox.Show("Медицинская карта или лекарство не найдены.");
                                return;
                            }

                            var prescription = new Prescriptions
                            {
                                PrescriptionID = context.Prescriptions.Any() ? context.Prescriptions.Max(p => p.PrescriptionID) + 1 : 1,
                                RecordID = recordId,
                                MedicationName = values[1],
                                DosageInstructions = values[2]
                            };
                            context.Prescriptions.Add(prescription);
                            break;

                        default:
                            MessageBox.Show("Неизвестная таблица.");
                            return;
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
