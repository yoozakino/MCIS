using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Информационная_система_медицинской_клиники.Windows
{
    public partial class AddRecord : Window
    {
        public Program MainProgramWindow { get; set; }

        public AddRecord()
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
                    hintText.Text = "Формат: ФИО, Дата рождения (гггг-мм-дд), Пол, Адрес, Телефон, Email, Номер страховки\nПример: Иванов Иван Иванович, 1990-05-15, Мужской, ул. Ленина 10, +79123456789, ivanov@mail.ru, 123456789012";
                    break;
                case 1: // Врачи
                    hintText.Text = "Формат: ФИО, Специализация, Телефон, Email, Дата приема (гггг-мм-дд)\nПример: Петрова Мария Сергеевна, Терапевт, +79123456780, petrova@mail.ru, 2015-06-10";
                    break;
                case 2: // Записи на прием
                    hintText.Text = "Формат: ID пациента, ID врача, Дата приема (гггг-мм-дд чч:мм), Статус\nПример: 1, 2, 2023-10-15 14:30, Запланирован";
                    break;
                case 3: // Медицинские карты
                    hintText.Text = "Формат: ID пациента, ID врача, Дата посещения (гггг-мм-дд), Диагноз, Назначения, Комментарии\nПример: 1, 2, 2023-10-10, ОРВИ, Постельный режим, Температура 37.5";
                    break;
                case 4: // Лекарства
                    hintText.Text = "Формат: Название, Описание, Дозировка, Цена\nПример: Парацетамол, Жаропонижающее, 500 мг, 50.99";
                    break;
                case 5: // Назначения лекарств
                    hintText.Text = "Формат: ID карты, ID лекарства, Инструкция\nПример: 1, 1, По 1 таблетке 3 раза в день после еды";
                    break;
                case 6: // Кабинеты
                    hintText.Text = "Формат: Номер кабинета, Описание\nПример: 101, Кабинет терапевта";
                    break;
                case 7: // Расписание врачей
                    hintText.Text = "Формат: ID врача, День недели, Время начала (чч:мм), Время окончания (чч:мм), Статус\nПример: 1, Понедельник, 09:00, 18:00, Работает";
                    break;
                case 8: // Услуги
                    hintText.Text = "Формат: Название услуги, Описание, Цена\nПример: Консультация терапевта, Первичный прием, 1500.00";
                    break;
                case 9: // Счета
                    hintText.Text = "Формат: ID пациента, ID услуги, Дата счета (гггг-мм-дд), Сумма, Статус, Описание\nПример: 1, 1, 2023-10-10, 1500.00, Оплачен, Консультация";
                    break;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string inputData = recordData.Text.Trim();

            if (string.IsNullOrEmpty(inputData))
            {
                MessageBox.Show("Введите данные для добавления!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var context = Medical_ClinicEntities.GetContext();
                var values = inputData.Split(',').Select(v => v.Trim()).ToArray();

                switch (tableComboBox.SelectedIndex)
                {
                    case 0: // Пациенты
                        if (values.Length != 7)
                        {
                            MessageBox.Show("Неверное количество данных. Требуется 7 значений.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        context.Patients.Add(new Patients
                        {
                            FullName = values[0],
                            DateOfBirth = DateTime.Parse(values[1]),
                            Gender = values[2],
                            Addresss = values[3],
                            Phone = values[4],
                            Email = values[5],
                            InsuranceNumber = values[6]
                        });
                        break;

                    case 1: // Врачи
                        if (values.Length != 5)
                        {
                            MessageBox.Show("Неверное количество данных. Требуется 5 значений.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        context.Doctors.Add(new Doctors
                        {
                            FullName = values[0],
                            Specialization = values[1],
                            Phone = values[2],
                            Email = values[3],
                            HireDate = DateTime.Parse(values[4])
                        });
                        break;

                    case 2: // Записи на прием
                        if (values.Length != 4)
                        {
                            MessageBox.Show("Неверное количество данных. Требуется 4 значения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        context.Appointments.Add(new Appointments
                        {
                            PatientID = int.Parse(values[0]),
                            DoctorID = int.Parse(values[1]),
                            AppointmentDate = DateTime.Parse(values[2]),
                            Statuss = values[3]
                        });
                        break;

                    case 3: // Медицинские карты
                        if (values.Length != 6)
                        {
                            MessageBox.Show("Неверное количество данных. Требуется 6 значений.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        context.MedicalRecords.Add(new MedicalRecords
                        {
                            PatientID = int.Parse(values[0]),
                            DoctorID = int.Parse(values[1]),
                            VisitDate = DateTime.Parse(values[2]),
                            Diagnosis = values[3],
                            Prescriptions = values[4],
                            Comments = values[5]
                        });
                        break;

                    case 4: // Лекарства
                        if (values.Length != 4)
                        {
                            MessageBox.Show("Неверное количество данных. Требуется 4 значения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        context.Medications.Add(new Medications
                        {
                            MedicationName = values[0],
                            Descriptionn = values[1],
                            Dosage = values[2],
                            Price = decimal.Parse(values[3])
                        });
                        break;

                    case 5: // Назначения лекарств
                        if (values.Length != 3)
                        {
                            MessageBox.Show("Неверное количество данных. Требуется 3 значения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        context.Prescriptions.Add(new Prescriptions
                        {
                            RecordID = int.Parse(values[0]),
                            MedicationID = int.Parse(values[1]),
                            DosageInstructions = values[2]
                        });
                        break;

                    case 6: // Кабинеты
                        if (values.Length != 2)
                        {
                            MessageBox.Show("Неверное количество данных. Требуется 2 значения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        context.Rooms.Add(new Rooms
                        {
                            RoomNumber = values[0],
                            Descriptionn = values[1]
                        });
                        break;

                    case 7: // Расписание врачей
                        if (values.Length != 5)
                        {
                            MessageBox.Show("Неверное количество данных. Требуется 5 значений.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        context.Schedules.Add(new Schedules
                        {
                            DoctorID = int.Parse(values[0]),
                            Day_of_week = values[1],
                            StartTime = TimeSpan.Parse(values[2]),
                            EndTime = TimeSpan.Parse(values[3]),
                            Statuss = values[4]
                        });
                        break;

                    case 8: // Услуги
                        if (values.Length != 3)
                        {
                            MessageBox.Show("Неверное количество данных. Требуется 3 значения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        context.Servicess.Add(new Servicess
                        {
                            ServiceName = values[0],
                            Descriptionn = values[1],
                            Price = decimal.Parse(values[2])
                        });
                        break;

                    case 9: // Счета
                        if (values.Length != 6)
                        {
                            MessageBox.Show("Неверное количество данных. Требуется 6 значений.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        context.Invoices.Add(new Invoices
                        {
                            PatientID = int.Parse(values[0]),
                            ServiceID = int.Parse(values[1]),
                            InvoiceDate = DateTime.Parse(values[2]),
                            Amount = decimal.Parse(values[3]),
                            Statuss = values[4],
                            Descriptionn = values[5]
                        });
                        break;
                }

                context.SaveChanges();
                MessageBox.Show("Запись успешно добавлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                // Обновляем данные в текущей открытой странице Program.xaml
                if (MainProgramWindow != null)
                {
                    MainProgramWindow.RefreshCurrentPage();
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении записи: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Medical_ClinicEntities.ResetContext();
            }
        }
    }
}