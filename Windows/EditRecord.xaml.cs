using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Информационная_система_медицинской_клиники.Windows
{
    public partial class EditRecord : Window
    {
        public Program MainProgramWindow { get; set; }

        public EditRecord()
        {
            InitializeComponent();
            tableComboBox.SelectedIndex = 0;
            tableComboBox.SelectionChanged += TableComboBox_SelectionChanged;
            UpdateHintText();
        }

        private void TableComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateHintText();
        }

        private void UpdateHintText()
        {
            switch (tableComboBox.SelectedIndex)
            {
                case 0: // Пациенты
                    hintText.Text = "Формат: ФИО, Дата рождения, Пол, Адрес, Телефон, Email, Номер страховки\nПример: Иванов Иван Иванович, -, -, ул. Новая 15, -, -, -\n(используйте '-' для пропуска поля)";
                    break;
                case 1: // Врачи
                    hintText.Text = "Формат: ФИО, Специализация, Телефон, Email, Дата приема\nПример: -, Кардиолог, +79123456789, -, -\n(используйте '-' для пропуска поля)";
                    break;
                case 2: // Записи на прием
                    hintText.Text = "Формат: ID пациента, ID врача, Дата приема, Статус\nПример: -, -, 2023-11-20 15:30, Подтвержден\n(используйте '-' для пропуска поля)";
                    break;
                case 3: // Медицинские карты
                    hintText.Text = "Формат: ID пациента, ID врача, Дата посещения, Диагноз, Назначения, Комментарии\nПример: -, -, -, ОРВИ, -, Температура в норме\n(используйте '-' для пропуска поля)";
                    break;
                case 4: // Лекарства
                    hintText.Text = "Формат: Название, Описание, Дозировка, Цена\nПример: Парацетамол, -, 500 мг, 60.50\n(используйте '-' для пропуска поля)";
                    break;
                case 5: // Назначения лекарств
                    hintText.Text = "Формат: ID карты, ID лекарства, Инструкция\nПример: -, -, Принимать после еды\n(используйте '-' для пропуска поля)";
                    break;
                case 6: // Кабинеты
                    hintText.Text = "Формат: Номер кабинета, Описание\nПример: -, Кабинет кардиолога\n(используйте '-' для пропуска поля)";
                    break;
                case 7: // Расписание врачей
                    hintText.Text = "Формат: ID врача, День недели, Время начала, Время окончания, Статус\nПример: -, Пятница, 10:00, 18:00, -\n(используйте '-' для пропуска поля)";
                    break;
                case 8: // Услуги
                    hintText.Text = "Формат: Название услуги, Описание, Цена\nПример: Консультация, -, 2000.00\n(используйте '-' для пропуска поля)";
                    break;
                case 9: // Счета
                    hintText.Text = "Формат: ID пациента, ID услуги, Дата счета, Сумма, Статус, Описание\nПример: -, -, -, -, Оплачен, -\n(используйте '-' для пропуска поля)";
                    break;
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            string idText = recordId.Text.Trim();
            string dataText = updateData.Text.Trim();

            if (string.IsNullOrEmpty(idText) || string.IsNullOrEmpty(dataText))
            {
                MessageBox.Show("Все поля должны быть заполнены!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
                var values = dataText.Split(',').Select(v => v.Trim()).ToArray();
                bool recordFound = false;

                switch (tableComboBox.SelectedIndex)
                {
                    case 0: // Пациенты
                        var patient = context.Patients.Find(recordId);
                        if (patient == null) break;
                        recordFound = true;

                        if (values.Length != 7)
                        {
                            MessageBox.Show("Неверное количество данных. Требуется 7 значений.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        patient.FullName = values[0] == "-" ? patient.FullName : values[0];
                        patient.DateOfBirth = values[1] == "-" ? patient.DateOfBirth : DateTime.Parse(values[1]);
                        patient.Gender = values[2] == "-" ? patient.Gender : values[2];
                        patient.Addresss = values[3] == "-" ? patient.Addresss : values[3];
                        patient.Phone = values[4] == "-" ? patient.Phone : values[4];
                        patient.Email = values[5] == "-" ? patient.Email : values[5];
                        patient.InsuranceNumber = values[6] == "-" ? patient.InsuranceNumber : values[6];
                        break;

                    case 1: // Врачи
                        var doctor = context.Doctors.Find(recordId);
                        if (doctor == null) break;
                        recordFound = true;

                        if (values.Length != 5)
                        {
                            MessageBox.Show("Неверное количество данных. Требуется 5 значений.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        doctor.FullName = values[0] == "-" ? doctor.FullName : values[0];
                        doctor.Specialization = values[1] == "-" ? doctor.Specialization : values[1];
                        doctor.Phone = values[2] == "-" ? doctor.Phone : values[2];
                        doctor.Email = values[3] == "-" ? doctor.Email : values[3];
                        doctor.HireDate = values[4] == "-" ? doctor.HireDate : DateTime.Parse(values[4]);
                        break;

                    case 2: // Записи на прием
                        var appointment = context.Appointments.Find(recordId);
                        if (appointment == null) break;
                        recordFound = true;

                        if (values.Length != 4)
                        {
                            MessageBox.Show("Неверное количество данных. Требуется 4 значения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        appointment.PatientID = values[0] == "-" ? appointment.PatientID : int.Parse(values[0]);
                        appointment.DoctorID = values[1] == "-" ? appointment.DoctorID : int.Parse(values[1]);
                        appointment.AppointmentDate = values[2] == "-" ? appointment.AppointmentDate : DateTime.Parse(values[2]);
                        appointment.Statuss = values[3] == "-" ? appointment.Statuss : values[3];
                        break;

                    case 3: // Медицинские карты
                        var medicalRecord = context.MedicalRecords.Find(recordId);
                        if (medicalRecord == null) break;
                        recordFound = true;

                        if (values.Length != 6)
                        {
                            MessageBox.Show("Неверное количество данных. Требуется 6 значений.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        medicalRecord.PatientID = values[0] == "-" ? medicalRecord.PatientID : int.Parse(values[0]);
                        medicalRecord.DoctorID = values[1] == "-" ? medicalRecord.DoctorID : int.Parse(values[1]);
                        medicalRecord.VisitDate = values[2] == "-" ? medicalRecord.VisitDate : DateTime.Parse(values[2]);
                        medicalRecord.Diagnosis = values[3] == "-" ? medicalRecord.Diagnosis : values[3];
                        medicalRecord.Prescriptions = values[4] == "-" ? medicalRecord.Prescriptions : values[4];
                        medicalRecord.Comments = values[5] == "-" ? medicalRecord.Comments : values[5];
                        break;

                    case 4: // Лекарства
                        var medication = context.Medications.Find(recordId);
                        if (medication == null) break;
                        recordFound = true;

                        if (values.Length != 4)
                        {
                            MessageBox.Show("Неверное количество данных. Требуется 4 значения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        medication.MedicationName = values[0] == "-" ? medication.MedicationName : values[0];
                        medication.Descriptionn = values[1] == "-" ? medication.Descriptionn : values[1];
                        medication.Dosage = values[2] == "-" ? medication.Dosage : values[2];
                        medication.Price = values[3] == "-" ? medication.Price : decimal.Parse(values[3]);
                        break;

                    case 5: // Назначения лекарств
                        var prescription = context.Prescriptions.Find(recordId);
                        if (prescription == null) break;
                        recordFound = true;

                        if (values.Length != 3)
                        {
                            MessageBox.Show("Неверное количество данных. Требуется 3 значения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        prescription.RecordID = values[0] == "-" ? prescription.RecordID : int.Parse(values[0]);
                        prescription.MedicationID = values[1] == "-" ? prescription.MedicationID : int.Parse(values[1]);
                        prescription.DosageInstructions = values[2] == "-" ? prescription.DosageInstructions : values[2];
                        break;

                    case 6: // Кабинеты
                        var room = context.Rooms.Find(recordId);
                        if (room == null) break;
                        recordFound = true;

                        if (values.Length != 2)
                        {
                            MessageBox.Show("Неверное количество данных. Требуется 2 значения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        room.RoomNumber = values[0] == "-" ? room.RoomNumber : values[0];
                        room.Descriptionn = values[1] == "-" ? room.Descriptionn : values[1];
                        break;

                    case 7: // Расписание врачей
                        var schedule = context.Schedules.Find(recordId);
                        if (schedule == null) break;
                        recordFound = true;

                        if (values.Length != 5)
                        {
                            MessageBox.Show("Неверное количество данных. Требуется 5 значений.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        schedule.DoctorID = values[0] == "-" ? schedule.DoctorID : int.Parse(values[0]);
                        schedule.Day_of_week = values[1] == "-" ? schedule.Day_of_week : values[1];
                        schedule.StartTime = values[2] == "-" ? schedule.StartTime : TimeSpan.Parse(values[2]);
                        schedule.EndTime = values[3] == "-" ? schedule.EndTime : TimeSpan.Parse(values[3]);
                        schedule.Statuss = values[4] == "-" ? schedule.Statuss : values[4];
                        break;

                    case 8: // Услуги
                        var service = context.Servicess.Find(recordId);
                        if (service == null) break;
                        recordFound = true;

                        if (values.Length != 3)
                        {
                            MessageBox.Show("Неверное количество данных. Требуется 3 значения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        service.ServiceName = values[0] == "-" ? service.ServiceName : values[0];
                        service.Descriptionn = values[1] == "-" ? service.Descriptionn : values[1];
                        service.Price = values[2] == "-" ? service.Price : decimal.Parse(values[2]);
                        break;

                    case 9: // Счета
                        var invoice = context.Invoices.Find(recordId);
                        if (invoice == null) break;
                        recordFound = true;

                        if (values.Length != 6)
                        {
                            MessageBox.Show("Неверное количество данных. Требуется 6 значений.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        invoice.PatientID = values[0] == "-" ? invoice.PatientID : int.Parse(values[0]);
                        invoice.ServiceID = values[1] == "-" ? invoice.ServiceID : int.Parse(values[1]);
                        invoice.InvoiceDate = values[2] == "-" ? invoice.InvoiceDate : DateTime.Parse(values[2]);
                        invoice.Amount = values[3] == "-" ? invoice.Amount : decimal.Parse(values[3]);
                        invoice.Statuss = values[4] == "-" ? invoice.Statuss : values[4];
                        invoice.Descriptionn = values[5] == "-" ? invoice.Descriptionn : values[5];
                        break;
                }

                if (!recordFound)
                {
                    MessageBox.Show("Запись с указанным ID не найдена!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                context.SaveChanges();
                MessageBox.Show("Запись успешно обновлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                // Обновляем данные в текущей открытой странице Program.xaml
                if (MainProgramWindow != null)
                {
                    MainProgramWindow.RefreshCurrentPage();
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении записи: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Medical_ClinicEntities.ResetContext();
            }
        }
    }
}