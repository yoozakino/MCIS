using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
                            string patientName = values[0];
                            string doctorName = values[1];
                            DateTime appointmentDate = DateTime.Parse(values[2]);
                            string status = values[3];

                            bool patientExists = context.Patients.Any(p => p.FullName == patientName);
                            bool doctorExists = context.Doctors.Any(d => d.FullName == doctorName);

                            if (!patientExists || !doctorExists)
                            {
                                MessageBox.Show("Указанный пациент или врач не существует.");
                                return;
                            }

                            var appointment = new Appointments
                            {
                                AppointmentID = context.Appointments.Any() ? context.Appointments.Max(a => a.AppointmentID) + 1 : 1,
                                PatientName = patientName,
                                DoctorName = doctorName,
                                AppointmentDate = appointmentDate,
                                Statuss = status
                            };
                            context.Appointments.Add(appointment);
                            break;

                        case "Медицинские карты":
                            string medPatient = values[0];
                            string medDoctor = values[1];
                            DateTime visitDate = DateTime.Parse(values[2]);
                            string diagnosis = values[3];
                            string prescriptions = values[4];
                            string comments = values[5];

                            if (!context.Patients.Any(p => p.FullName == medPatient) || !context.Doctors.Any(d => d.FullName == medDoctor))
                            {
                                MessageBox.Show("Пациент или врач не найдены.");
                                return;
                            }

                            var medicalRecord = new MedicalRecords
                            {
                                RecordID = context.MedicalRecords.Any() ? context.MedicalRecords.Max(r => r.RecordID) + 1 : 1,
                                PatientName = medPatient,
                                DoctorName = medDoctor,
                                VisitDate = visitDate,
                                Diagnosis = diagnosis,
                                Prescriptions = prescriptions,
                                Comments = comments
                            };
                            context.MedicalRecords.Add(medicalRecord);
                            break;

                        case "Услуги":
                            string serviceName = values[0];
                            string description = values[1];
                            decimal price = decimal.Parse(values[2]);

                            var service = new Servicess
                            {
                                ServiceName = serviceName,
                                Descriptionn = description,
                                Price = price
                            };
                            context.Servicess.Add(service);
                            break;

                        case "Расписание врачей":
                            string scheduleDoctor = values[0];
                            string dayOfWeek = values[1];
                            TimeSpan startTime = TimeSpan.Parse(values[2]);
                            TimeSpan endTime = TimeSpan.Parse(values[3]);

                            if (!context.Doctors.Any(d => d.FullName == scheduleDoctor))
                            {
                                MessageBox.Show("Указанный врач не найден.");
                                return;
                            }

                            var schedule = new Schedules
                            {
                                ScheduleID = context.Schedules.Any() ? context.Schedules.Max(s => s.ScheduleID) + 1 : 1,
                                DoctorName = scheduleDoctor,
                                Day_of_week = dayOfWeek,
                                StartTime = startTime,
                                EndTime = endTime
                            };
                            context.Schedules.Add(schedule);
                            break;

                        case "Кабинеты":
                            string roomNumber = values[0];
                            string roomDescription = values[1];

                            if (context.Rooms.Any(r => r.RoomNumber == roomNumber))
                            {
                                MessageBox.Show("Кабинет с таким номером уже существует.");
                                return;
                            }

                            var room = new Rooms
                            {
                                RoomNumber = roomNumber,
                                Descriptionn = roomDescription
                            };
                            context.Rooms.Add(room);
                            break;

                        case "Счета":
                            string invoicePatient = values[0];
                            string invoiceService = values[1];
                            DateTime invoiceDate = DateTime.Parse(values[2]);
                            decimal amount = decimal.Parse(values[3]);
                            string invoiceStatus = values[4];

                            bool patientOk = context.Patients.Any(p => p.FullName == invoicePatient);
                            bool serviceOk = context.Servicess.Any(s => s.ServiceName == invoiceService);

                            if (!patientOk || !serviceOk)
                            {
                                MessageBox.Show("Пациент или услуга не найдены.");
                                return;
                            }

                            var invoice = new Invoices
                            {
                                InvoiceID = context.Invoices.Any() ? context.Invoices.Max(i => i.InvoiceID) + 1 : 1,
                                PatientName = invoicePatient,
                                ServiceName = invoiceService,
                                InvoiceDate = invoiceDate,
                                Amount = amount,
                                Statuss = invoiceStatus
                            };
                            context.Invoices.Add(invoice);
                            break;

                        case "Лекарства":
                            string medName = values[0];
                            string medDesc = values[1];
                            string dosage = values[2];
                            decimal medPrice = decimal.Parse(values[3]);

                            var medication = new Medications
                            {
                                MedicationName = medName,
                                Descriptionn = medDesc,
                                Dosage = dosage,
                                Price = medPrice
                            };
                            context.Medications.Add(medication);
                            break;

                        case "Инструкции":
                            int recordId = int.Parse(values[0]);
                            string medicationName = values[1];
                            string instructions = values[2];

                            bool recordExists = context.MedicalRecords.Any(mr => mr.RecordID == recordId);
                            bool medicationExists = context.Medications.Any(m => m.MedicationName == medicationName);

                            if (!recordExists || !medicationExists)
                            {
                                MessageBox.Show("Медицинская карта или лекарство не найдены.");
                                return;
                            }

                            var prescription = new Prescriptions
                            {
                                PrescriptionID = context.Prescriptions.Any() ? context.Prescriptions.Max(p => p.PrescriptionID) + 1 : 1,
                                RecordID = recordId,
                                MedicationName = medicationName,
                                DosageInstructions = instructions
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
