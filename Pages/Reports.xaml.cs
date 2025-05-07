using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.DataVisualization.Charting;
using Word = Microsoft.Office.Interop.Word;
using Информационная_система_медицинской_клиники;

namespace Информационная_система_медицинской_клиники.Pages
{
    public partial class Reports : Page
    {
        private Medical_ClinicEntities _context;

        public Reports()
        {
            InitializeComponent();
            _context = new Medical_ClinicEntities();
        }

        private void UpdateChart(object sender, SelectionChangedEventArgs e)
        {
            if (CmbTable.SelectedItem is ComboBoxItem selectedItem)
            {
                string tableName = selectedItem.Content.ToString();
                switch (tableName)
                {
                    case "Пациенты": ShowPatientsChart(); break;
                    case "Врачи": ShowDoctorsChart(); break;
                    case "Записи на прием": ShowAppointmentsChart(); break;
                    case "Медицинские карты": ShowMedicalRecordsChart(); break;
                    case "Услуги": ShowServicesChart(); break;
                    case "Расписание врачей": ShowSchedulesChart(); break;
                    case "Кабинеты": ShowRoomsChart(); break;
                    case "Выставленные счета": ShowInvoicesChart(); break;
                    case "Лекарства": ShowMedicationsChart(); break;
                    case "Назначения лекарств": ShowPrescriptionsChart(); break;
                    default:
                        Chart.Series.Clear();
                        Chart.Titles.Clear();
                        break;
                }
            }
        }

        private void SetupChart()
        {
            Chart.Series.Clear();
            Chart.Titles.Clear();
            Chart.ChartAreas.Clear();

            var chartArea = new ChartArea("MainArea");
            chartArea.AxisX.LabelStyle.Angle = -45;
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.IsLabelAutoFit = true;
            Chart.ChartAreas.Add(chartArea);
        }

        private void ShowPatientsChart()
        {
            SetupChart();

            var genderData = _context.Patients
                .GroupBy(p => p.Gender)
                .Select(g => new { Пол = g.Key, Количество = g.Count() })
                .ToList();

            Series series = new Series("Пациенты по полу") { ChartType = SeriesChartType.Column };
            foreach (var item in genderData)
                series.Points.AddXY(item.Пол, item.Количество);

            Chart.Series.Add(series);
            Chart.Titles.Add("Пациенты по полу");
        }

        private void ShowDoctorsChart()
        {
            SetupChart();

            var specialtyData = _context.Doctors
                .GroupBy(d => d.Specialization)
                .Select(g => new { Специальность = g.Key, Количество = g.Count() })
                .ToList();

            Series series = new Series("Врачи по специальности") { ChartType = SeriesChartType.Column };
            foreach (var item in specialtyData)
            {
                string label = string.IsNullOrEmpty(item.Специальность) ? "Не указано" : item.Специальность;
                series.Points.AddXY(label, item.Количество);
            }

            Chart.Series.Add(series);
            Chart.Titles.Add("Врачи по специальности");
        }

        private void ShowAppointmentsChart()
        {
            SetupChart();

            var appointmentData = _context.Appointments
                .GroupBy(a => a.Doctors.FullName)
                .Select(g => new { Врач = g.Key, Количество = g.Count() })
                .OrderByDescending(x => x.Количество)
                .Take(10)
                .ToList();

            Series series = new Series("Записи по врачам") { ChartType = SeriesChartType.Column };
            foreach (var item in appointmentData)
            {
                string label = string.IsNullOrEmpty(item.Врач) ? "Не указано" : item.Врач;
                series.Points.AddXY(label, item.Количество);
            }

            Chart.Series.Add(series);
            Chart.Titles.Add("Количество записей на приём по врачам");
        }

        private void ShowMedicalRecordsChart()
        {
            SetupChart();

            var recordsByYear = _context.MedicalRecords
                .GroupBy(r => r.VisitDate.Year)
                .Select(g => new { Год = g.Key, Количество = g.Count() })
                .OrderBy(x => x.Год)
                .ToList();

            Series series = new Series("Медкарты по годам") { ChartType = SeriesChartType.Column };
            foreach (var item in recordsByYear)
                series.Points.AddXY(item.Год.ToString(), item.Количество);

            Chart.Series.Add(series);
            Chart.Titles.Add("Медицинские карты по годам");
        }

        private void ShowServicesChart()
        {
            SetupChart();

            var services = _context.Servicess
                .Select(s => new { Название = s.ServiceName, Цена = s.Price })
                .ToList();

            Series series = new Series("Стоимость услуг") { ChartType = SeriesChartType.Column };
            foreach (var service in services)
            {
                string name = string.IsNullOrEmpty(service.Название) ? "Не указано" : service.Название;
                series.Points.AddXY(name, Convert.ToDouble(service.Цена));
            }

            Chart.Series.Add(series);
            Chart.Titles.Add("Стоимость услуг");
        }

        private void ShowSchedulesChart()
        {
            SetupChart();

            var scheduleData = _context.Schedules
                .GroupBy(s => s.DoctorName)
                .Select(g => new { Врач = g.Key, КоличествоСмен = g.Count() })
                .ToList();

            Series series = new Series("Смены по врачам") { ChartType = SeriesChartType.Column };
            foreach (var item in scheduleData)
            {
                string label = string.IsNullOrEmpty(item.Врач) ? "Не указано" : item.Врач;
                series.Points.AddXY(label, item.КоличествоСмен);
            }

            Chart.Series.Add(series);
            Chart.Titles.Add("Количество смен по врачам");
        }

        private void ShowRoomsChart()
        {
            SetupChart();

            var roomUsage = _context.Rooms
                .GroupBy(s => s.RoomNumber)
                .Select(g => new { Кабинет = g.Key, Использований = g.Count() })
                .ToList();

            Series series = new Series("Загруженность кабинетов") { ChartType = SeriesChartType.Column };
            foreach (var item in roomUsage)
            {
                string label = string.IsNullOrEmpty(item.Кабинет) ? "Не указано" : item.Кабинет;
                series.Points.AddXY(label, item.Использований);
            }

            Chart.Series.Add(series);
            Chart.Titles.Add("Загруженность кабинетов");
        }

        private void ShowInvoicesChart()
        {
            SetupChart();

            var rawInvoiceData = _context.Invoices
                .GroupBy(i => new { i.InvoiceDate.Year, i.InvoiceDate.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Сумма = g.Sum(i => i.Amount)
                })
                .ToList();

            var invoiceData = rawInvoiceData
                .Select(x => new
                {
                    Период = $"{x.Month:D2}.{x.Year}",
                    x.Сумма
                })
                .OrderBy(x => x.Период)
                .ToList();

            Series series = new Series("Сумма счетов по месяцам") { ChartType = SeriesChartType.Column };
            foreach (var item in invoiceData)
                series.Points.AddXY(item.Период, Convert.ToDouble(item.Сумма));

            Chart.Series.Add(series);
            Chart.Titles.Add("Сумма счетов по месяцам");
        }

        private void ShowMedicationsChart()
        {
            SetupChart();

            var medicationData = _context.Medications
                .Select(m => new { Название = m.MedicationName, Цена = m.Price })
                .ToList();

            Series series = new Series("Цена лекарств") { ChartType = SeriesChartType.Column };
            foreach (var item in medicationData)
            {
                string label = string.IsNullOrEmpty(item.Название) ? "Не указано" : item.Название;
                series.Points.AddXY(label, Convert.ToDouble(item.Цена));
            }

            Chart.Series.Add(series);
            Chart.Titles.Add("Цена лекарств");
        }

        private void ShowPrescriptionsChart()
        {
            SetupChart();

            var prescriptionData = _context.Prescriptions
                .GroupBy(p => p.MedicationName)
                .Select(g => new { Лекарство = g.Key, Назначений = g.Count() })
                .OrderByDescending(x => x.Назначений)
                .ToList();

            Series series = new Series("Назначения лекарств") { ChartType = SeriesChartType.Column };
            foreach (var item in prescriptionData)
            {
                string label = string.IsNullOrEmpty(item.Лекарство) ? "Не указано" : item.Лекарство;
                series.Points.AddXY(label, item.Назначений);
            }

            Chart.Series.Add(series);
            Chart.Titles.Add("Количество назначений по лекарствам");
        }

        private void buttonWord_Click(object sender, RoutedEventArgs e)
        {
            if (CmbTable.SelectedItem is ComboBoxItem selectedItem)
            {
                string tableName = selectedItem.Content.ToString();
                switch (tableName)
                {
                    case "Пациенты":
                        ExportPatientsToWord();
                        break;
                    case "Врачи":
                        ExportDoctorsToWord();
                        break;
                    case "Записи на прием":
                        ExportAppointmentsToWord();
                        break;
                    case "Медицинские карты":
                        ExportMedicalRecordsToWord();
                        break;
                    case "Услуги":
                        ExportServicesToWord();
                        break;
                    case "Расписание врачей":
                        ExportSchedulesToWord();
                        break;
                    case "Кабинеты":
                        ExportRoomsToWord();
                        break;
                    case "Выставленные счета":
                        ExportInvoicesToWord();
                        break;
                    case "Лекарства":
                        ExportMedicationsToWord();
                        break;
                    case "Назначения лекарств":
                        ExportPrescriptionsToWord();
                        break;
                }
            }
        }

        private void buttonExcel_Click(object sender, RoutedEventArgs e)
        {
            if (CmbTable.SelectedItem is ComboBoxItem selectedItem)
            {
                string tableName = selectedItem.Content.ToString();
                switch (tableName)
                {
                    case "Пациенты":
                        ExportPatientsToExcel();
                        break;
                    case "Врачи":
                        ExportDoctorsToExcel();
                        break;
                    case "Записи на прием":
                        ExportAppointmentsToExcel();
                        break;
                    case "Медицинские карты":
                        ExportMedicalRecordsToExcel();
                        break;
                    case "Услуги":
                        ExportServicesToExcel();
                        break;
                    case "Расписание врачей":
                        ExportSchedulesToExcel();
                        break;
                    case "Кабинеты":
                        ExportRoomsToExcel();
                        break;
                    case "Выставленные счета":
                        ExportInvoicesToExcel();
                        break;
                    case "Лекарства":
                        ExportMedicationsToExcel();
                        break;
                    case "Назначения лекарств":
                        ExportPrescriptionsToExcel();
                        break;
                }
            }
        }


        private void ExportPatientsToWord()
        {
            var patients = _context.Patients.ToList();

            string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            string fileName = $"Записи на прием_{DateTime.Now:dd.MM.yyyy}.docx";
            string fullPath = System.IO.Path.Combine(downloadsPath, fileName);

            Word.Application wordApp = new Word.Application();
            Word.Document doc = wordApp.Documents.Add();
            wordApp.Visible = false;

            Word.Paragraph title = doc.Paragraphs.Add();
            title.Range.Text = "Список пациентов";
            title.Range.Font.Size = 16;
            title.Range.Font.Bold = 1;
            title.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            title.Range.InsertParagraphAfter();

            Word.Table table = doc.Tables.Add(title.Range, patients.Count + 1, 7);
            table.Borders.Enable = 1;

            string[] headers = { "ФИО", "Дата рождения", "Пол", "Адрес", "Телефон", "Email", "Полис" };
            for (int i = 0; i < headers.Length; i++)
                table.Cell(1, i + 1).Range.Text = headers[i];

            for (int i = 0; i < patients.Count; i++)
            {
                var p = patients[i];
                table.Cell(i + 2, 1).Range.Text = p.FullName;
                table.Cell(i + 2, 2).Range.Text = p.DateOfBirth?.ToString("dd.MM.yyyy");
                table.Cell(i + 2, 3).Range.Text = p.Gender;
                table.Cell(i + 2, 4).Range.Text = p.Addresss;
                table.Cell(i + 2, 5).Range.Text = p.Phone;
                table.Cell(i + 2, 6).Range.Text = p.Email;
                table.Cell(i + 2, 7).Range.Text = p.InsuranceNumber;
            }

            doc.SaveAs2(fullPath);
            doc.Close(); wordApp.Quit();
            MessageBox.Show($"Файл сохранён в: {fullPath}", "Экспорт завершён", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExportDoctorsToWord()
        {
            var doctors = _context.Doctors.ToList();

            string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            string fileName = $"Врачи_{DateTime.Now:dd.MM.yyyy}.docx";
            string fullPath = System.IO.Path.Combine(downloadsPath, fileName);

            Word.Application wordApp = new Word.Application();
            Word.Document doc = wordApp.Documents.Add();
            wordApp.Visible = false;

            Word.Paragraph title = doc.Paragraphs.Add();
            title.Range.Text = "Список врачей";
            title.Range.Font.Size = 16;
            title.Range.Font.Bold = 1;
            title.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            title.Range.InsertParagraphAfter();

            Word.Table table = doc.Tables.Add(title.Range, doctors.Count + 1, 5);
            table.Borders.Enable = 1;

            string[] headers = { "ФИО", "Специальность", "Телефон", "Email", "Дата приёма" };
            for (int i = 0; i < headers.Length; i++)
                table.Cell(1, i + 1).Range.Text = headers[i];

            for (int i = 0; i < doctors.Count; i++)
            {
                var d = doctors[i];
                table.Cell(i + 2, 1).Range.Text = d.FullName;
                table.Cell(i + 2, 2).Range.Text = d.Specialization;
                table.Cell(i + 2, 3).Range.Text = d.Phone;
                table.Cell(i + 2, 4).Range.Text = d.Email;
                table.Cell(i + 2, 5).Range.Text = d.HireDate?.ToString("dd.MM.yyyy");
            }

            doc.SaveAs2(fullPath);
            doc.Close(); wordApp.Quit();
            MessageBox.Show($"Файл сохранён в: {fullPath}", "Экспорт завершён", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExportAppointmentsToWord()
        {
            var appointments = _context.Appointments.ToList();

            string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            string fileName = $"Записи на прием_{DateTime.Now:dd.MM.yyyy}.docx";
            string fullPath = System.IO.Path.Combine(downloadsPath, fileName);

            Word.Application wordApp = new Word.Application();
            wordApp.Visible = false;

            Word.Document doc = wordApp.Documents.Add();

            Word.Paragraph title = doc.Paragraphs.Add();
            title.Range.Text = "Список записей на приём";
            title.Range.Font.Size = 16;
            title.Range.Font.Bold = 1;
            title.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            title.Range.InsertParagraphAfter();

            int rows = appointments.Count + 1;
            int cols = 4;

            Word.Table table = doc.Tables.Add(title.Range, rows, cols);
            table.Borders.Enable = 1;
            table.Range.Font.Size = 11;

            table.Cell(1, 1).Range.Text = "Пациент";
            table.Cell(1, 2).Range.Text = "Врач";
            table.Cell(1, 3).Range.Text = "Дата приёма";
            table.Cell(1, 4).Range.Text = "Статус";

            for (int i = 0; i < appointments.Count; i++)
            {
                var a = appointments[i];
                int row = i + 2;

                table.Cell(row, 1).Range.Text = a.PatientName;
                table.Cell(row, 2).Range.Text = a.DoctorName;
                table.Cell(row, 3).Range.Text = a.AppointmentDate.ToString("dd.MM.yyyy HH:mm");
                table.Cell(row, 4).Range.Text = a.Statuss;
            }

            doc.SaveAs2(fullPath);
            doc.Close();
            wordApp.Quit();

            MessageBox.Show($"Файл сохранён в: {fullPath}", "Экспорт завершён", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void ExportMedicalRecordsToWord()
        {
            var records = _context.MedicalRecords.ToList();

            string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            string fileName = $"Медицинские карты_{DateTime.Now:dd.MM.yyyy}.docx";
            string fullPath = System.IO.Path.Combine(downloadsPath, fileName);

            Word.Application wordApp = new Word.Application();
            wordApp.Visible = false;

            Word.Document doc = wordApp.Documents.Add();

            Word.Paragraph title = doc.Paragraphs.Add();
            title.Range.Text = "Медицинские карты";
            title.Range.Font.Size = 16;
            title.Range.Font.Bold = 1;
            title.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            title.Range.InsertParagraphAfter();

            int rows = records.Count + 1;
            int cols = 6;

            Word.Table table = doc.Tables.Add(title.Range, rows, cols);
            table.Borders.Enable = 1;
            table.Range.Font.Size = 11;

            table.Cell(1, 1).Range.Text = "Пациент";
            table.Cell(1, 2).Range.Text = "Врач";
            table.Cell(1, 3).Range.Text = "Дата визита";
            table.Cell(1, 4).Range.Text = "Диагноз";
            table.Cell(1, 5).Range.Text = "Назначения";
            table.Cell(1, 6).Range.Text = "Комментарии";

            for (int i = 0; i < records.Count; i++)
            {
                var r = records[i];
                int row = i + 2;

                table.Cell(row, 1).Range.Text = r.PatientName;
                table.Cell(row, 2).Range.Text = r.DoctorName;
                table.Cell(row, 3).Range.Text = r.VisitDate.ToString("dd.MM.yyyy");
                table.Cell(row, 4).Range.Text = r.Diagnosis;
                table.Cell(row, 5).Range.Text = r.Prescriptions;
                table.Cell(row, 6).Range.Text = r.Comments;
            }

            doc.SaveAs2(fullPath);
            doc.Close();
            wordApp.Quit();

            MessageBox.Show($"Файл сохранён в: {fullPath}", "Экспорт завершён", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExportServicesToWord()
        {
            var services = _context.Servicess.ToList();

            string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            string fileName = $"Услуги_{DateTime.Now:dd.MM.yyyy}.docx";
            string fullPath = System.IO.Path.Combine(downloadsPath, fileName);

            Word.Application wordApp = new Word.Application();
            wordApp.Visible = false;

            Word.Document doc = wordApp.Documents.Add();

            Word.Paragraph title = doc.Paragraphs.Add();
            title.Range.Text = "Список услуг";
            title.Range.Font.Size = 16;
            title.Range.Font.Bold = 1;
            title.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            title.Range.InsertParagraphAfter();

            int rows = services.Count + 1;
            int cols = 3;

            Word.Table table = doc.Tables.Add(title.Range, rows, cols);
            table.Borders.Enable = 1;
            table.Range.Font.Size = 11;

            table.Cell(1, 1).Range.Text = "Название";
            table.Cell(1, 2).Range.Text = "Описание";
            table.Cell(1, 3).Range.Text = "Цена (₽)";

            for (int i = 0; i < services.Count; i++)
            {
                var s = services[i];
                int row = i + 2;

                table.Cell(row, 1).Range.Text = s.ServiceName;
                table.Cell(row, 2).Range.Text = s.Descriptionn;
                table.Cell(row, 3).Range.Text = (s.Price ?? 0).ToString("0.00");

            }

            doc.SaveAs2(fullPath);
            doc.Close();
            wordApp.Quit();

            MessageBox.Show($"Файл сохранён в: {fullPath}", "Экспорт завершён", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExportSchedulesToWord()
        {
            var schedules = _context.Schedules.ToList();

            string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            string fileName = $"Расписание врачей_{DateTime.Now:dd.MM.yyyy}.docx";
            string fullPath = System.IO.Path.Combine(downloadsPath, fileName);

            Word.Application wordApp = new Word.Application();
            wordApp.Visible = false;

            Word.Document doc = wordApp.Documents.Add();

            Word.Paragraph title = doc.Paragraphs.Add();
            title.Range.Text = "Расписание врачей";
            title.Range.Font.Size = 16;
            title.Range.Font.Bold = 1;
            title.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            title.Range.InsertParagraphAfter();

            int rows = schedules.Count + 1;
            int cols = 4;

            Word.Table table = doc.Tables.Add(title.Range, rows, cols);
            table.Borders.Enable = 1;
            table.Range.Font.Size = 11;

            table.Cell(1, 1).Range.Text = "Врач";
            table.Cell(1, 2).Range.Text = "День недели";
            table.Cell(1, 3).Range.Text = "Начало смены";
            table.Cell(1, 4).Range.Text = "Конец смены";

            for (int i = 0; i < schedules.Count; i++)
            {
                var s = schedules[i];
                int row = i + 2;

                table.Cell(row, 1).Range.Text = s.DoctorName;
                table.Cell(row, 2).Range.Text = s.Day_of_week;
                table.Cell(row, 3).Range.Text = s.StartTime.HasValue ? $"{s.StartTime.Value.Hours:D2}:{s.StartTime.Value.Minutes:D2}" : "";
                table.Cell(row, 4).Range.Text = s.EndTime.HasValue ? $"{s.EndTime.Value.Hours:D2}:{s.EndTime.Value.Minutes:D2}" : "";
            }

            doc.SaveAs2(fullPath);
            doc.Close();
            wordApp.Quit();

            MessageBox.Show($"Файл сохранён в: {fullPath}", "Экспорт завершён", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExportRoomsToWord()
        {
            var rooms = _context.Rooms.ToList();

            string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            string fileName = $"Кабинеты_{DateTime.Now:dd.MM.yyyy}.docx";
            string fullPath = System.IO.Path.Combine(downloadsPath, fileName);

            Word.Application wordApp = new Word.Application();
            wordApp.Visible = false;

            Word.Document doc = wordApp.Documents.Add();

            Word.Paragraph title = doc.Paragraphs.Add();
            title.Range.Text = "Список кабинетов";
            title.Range.Font.Size = 16;
            title.Range.Font.Bold = 1;
            title.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            title.Range.InsertParagraphAfter();

            int rows = rooms.Count + 1;
            int cols = 2;

            Word.Table table = doc.Tables.Add(title.Range, rows, cols);
            table.Borders.Enable = 1;
            table.Range.Font.Size = 11;

            table.Cell(1, 1).Range.Text = "Номер кабинета";
            table.Cell(1, 2).Range.Text = "Описание";

            for (int i = 0; i < rooms.Count; i++)
            {
                var r = rooms[i];
                int row = i + 2;

                table.Cell(row, 1).Range.Text = r.RoomNumber;
                table.Cell(row, 2).Range.Text = r.Descriptionn;
            }

            doc.SaveAs2(fullPath);
            doc.Close();
            wordApp.Quit();

            MessageBox.Show($"Файл сохранён в: {fullPath}", "Экспорт завершён", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExportInvoicesToWord()
        {
            var invoices = _context.Invoices.ToList();

            string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            string fileName = $"Выставленные счета_{DateTime.Now:dd.MM.yyyy}.docx";
            string fullPath = System.IO.Path.Combine(downloadsPath, fileName);

            Word.Application wordApp = new Word.Application();
            wordApp.Visible = false;

            Word.Document doc = wordApp.Documents.Add();

            Word.Paragraph title = doc.Paragraphs.Add();
            title.Range.Text = "Список выставленных счетов";
            title.Range.Font.Size = 16;
            title.Range.Font.Bold = 1;
            title.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            title.Range.InsertParagraphAfter();

            int rows = invoices.Count + 1;
            int cols = 5;

            Word.Table table = doc.Tables.Add(title.Range, rows, cols);
            table.Borders.Enable = 1;
            table.Range.Font.Size = 11;

            table.Cell(1, 1).Range.Text = "Пациент";
            table.Cell(1, 2).Range.Text = "Услуга";
            table.Cell(1, 3).Range.Text = "Дата счёта";
            table.Cell(1, 4).Range.Text = "Сумма";
            table.Cell(1, 5).Range.Text = "Статус";

            for (int i = 0; i < invoices.Count; i++)
            {
                var inv = invoices[i];
                int row = i + 2;

                table.Cell(row, 1).Range.Text = inv.PatientName ?? "Не указано";
                table.Cell(row, 2).Range.Text = inv.ServiceName ?? "Не указано";
                table.Cell(row, 3).Range.Text = inv.InvoiceDate.ToString("dd.MM.yyyy");
                table.Cell(row, 4).Range.Text = $"{inv.Amount:0.00}";
                table.Cell(row, 5).Range.Text = inv.Statuss;
            }

            doc.SaveAs2(fullPath);
            doc.Close();
            wordApp.Quit();

            MessageBox.Show($"Файл сохранён в: {fullPath}", "Экспорт завершён", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExportMedicationsToWord()
        {
            var meds = _context.Medications.ToList();

            string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            string fileName = $"Лекарства_{DateTime.Now:dd.MM.yyyy}.docx";
            string fullPath = System.IO.Path.Combine(downloadsPath, fileName);

            Word.Application wordApp = new Word.Application();
            wordApp.Visible = false;

            Word.Document doc = wordApp.Documents.Add();

            Word.Paragraph title = doc.Paragraphs.Add();
            title.Range.Text = "Список лекарств";
            title.Range.Font.Size = 16;
            title.Range.Font.Bold = 1;
            title.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            title.Range.InsertParagraphAfter();

            int rows = meds.Count + 1;
            int cols = 4;

            Word.Table table = doc.Tables.Add(title.Range, rows, cols);
            table.Borders.Enable = 1;
            table.Range.Font.Size = 11;

            table.Cell(1, 1).Range.Text = "Название";
            table.Cell(1, 2).Range.Text = "Описание";
            table.Cell(1, 3).Range.Text = "Дозировка";
            table.Cell(1, 4).Range.Text = "Цена";

            for (int i = 0; i < meds.Count; i++)
            {
                var m = meds[i];
                int row = i + 2;

                table.Cell(row, 1).Range.Text = m.MedicationName;
                table.Cell(row, 2).Range.Text = m.Descriptionn;
                table.Cell(row, 3).Range.Text = m.Dosage;
                table.Cell(row, 4).Range.Text = $"{m.Price:0.00}";
            }

            doc.SaveAs2(fullPath);
            doc.Close();
            wordApp.Quit();

            MessageBox.Show($"Файл сохранён в: {fullPath}", "Экспорт завершён", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExportPrescriptionsToWord()
        {
            var prescriptions = _context.Prescriptions.ToList();

            string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            string fileName = $"Назначения лекарств_{DateTime.Now:dd.MM.yyyy}.docx";
            string fullPath = System.IO.Path.Combine(downloadsPath, fileName);

            Word.Application wordApp = new Word.Application();
            wordApp.Visible = false;

            Word.Document doc = wordApp.Documents.Add();

            Word.Paragraph title = doc.Paragraphs.Add();
            title.Range.Text = "Список назначений лекарств";
            title.Range.Font.Size = 16;
            title.Range.Font.Bold = 1;
            title.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            title.Range.InsertParagraphAfter();

            int rows = prescriptions.Count + 1;
            int cols = 3;

            Word.Table table = doc.Tables.Add(title.Range, rows, cols);
            table.Borders.Enable = 1;
            table.Range.Font.Size = 11;

            table.Cell(1, 1).Range.Text = "ID Мед. карты";
            table.Cell(1, 2).Range.Text = "Лекарство";
            table.Cell(1, 3).Range.Text = "Инструкция по дозировке";

            for (int i = 0; i < prescriptions.Count; i++)
            {
                var p = prescriptions[i];
                int row = i + 2;

                table.Cell(row, 1).Range.Text = p.RecordID.ToString();
                table.Cell(row, 2).Range.Text = p.MedicationName ?? "—";
                table.Cell(row, 3).Range.Text = p.DosageInstructions ?? "—";
            }

            doc.SaveAs2(fullPath);
            doc.Close();
            wordApp.Quit();

            MessageBox.Show($"Файл сохранён в: {fullPath}", "Экспорт завершён", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExportPatientsToExcel()
        {
            var patients = _context.Patients.ToList();

            string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            string fileName = $"Пациенты_{DateTime.Now:dd.MM.yyyy}.xlsx";
            string fullPath = System.IO.Path.Combine(downloadsPath, fileName);

            var excelApp = new Microsoft.Office.Interop.Excel.Application();
            var workbook = excelApp.Workbooks.Add();
            var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.ActiveSheet;
            worksheet.Name = "Пациенты";

            worksheet.Cells[1, 1] = "ФИО";
            worksheet.Cells[1, 2] = "Дата рождения";
            worksheet.Cells[1, 3] = "Пол";
            worksheet.Cells[1, 4] = "Адрес";
            worksheet.Cells[1, 5] = "Телефон";
            worksheet.Cells[1, 6] = "Email";
            worksheet.Cells[1, 7] = "Полис";

            for (int i = 0; i < patients.Count; i++)
            {
                var p = patients[i];
                worksheet.Cells[i + 2, 1] = p.FullName;
                worksheet.Cells[i + 2, 2] = p.DateOfBirth?.ToString("dd.MM.yyyy");
                worksheet.Cells[i + 2, 3] = p.Gender;
                worksheet.Cells[i + 2, 4] = p.Addresss;
                worksheet.Cells[i + 2, 5] = p.Phone;
                worksheet.Cells[i + 2, 6] = p.Email;
                worksheet.Cells[i + 2, 7] = p.InsuranceNumber;
            }

            workbook.SaveAs(fullPath);
            workbook.Close();
            excelApp.Quit();

            MessageBox.Show($"Файл сохранён в: {fullPath}", "Экспорт завершён", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExportDoctorsToExcel()
        {
            var doctors = _context.Doctors.ToList();

            string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            string fileName = $"Врачи_{DateTime.Now:dd.MM.yyyy}.xlsx";
            string fullPath = System.IO.Path.Combine(downloadsPath, fileName);

            var excelApp = new Microsoft.Office.Interop.Excel.Application();
            var workbook = excelApp.Workbooks.Add();
            var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.ActiveSheet;
            worksheet.Name = "Врачи";

            worksheet.Cells[1, 1] = "ФИО";
            worksheet.Cells[1, 2] = "Специализация";
            worksheet.Cells[1, 3] = "Телефон";
            worksheet.Cells[1, 4] = "Email";

            for (int i = 0; i < doctors.Count; i++)
            {
                var d = doctors[i];
                worksheet.Cells[i + 2, 1] = d.FullName;
                worksheet.Cells[i + 2, 2] = d.Specialization;
                worksheet.Cells[i + 2, 3] = d.Phone;
                worksheet.Cells[i + 2, 4] = d.Email;
            }

            workbook.SaveAs(fullPath);
            workbook.Close();
            excelApp.Quit();

            MessageBox.Show($"Файл сохранён в: {fullPath}", "Экспорт завершён", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExportAppointmentsToExcel()
        {
            var appointments = _context.Appointments.ToList();

            string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            string fileName = $"Записи на прием_{DateTime.Now:dd.MM.yyyy}.xlsx";
            string fullPath = System.IO.Path.Combine(downloadsPath, fileName);

            var excelApp = new Microsoft.Office.Interop.Excel.Application();
            var workbook = excelApp.Workbooks.Add();
            var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.ActiveSheet;
            worksheet.Name = "Записи";

            worksheet.Cells[1, 1] = "ID";
            worksheet.Cells[1, 2] = "Пациент";
            worksheet.Cells[1, 3] = "Врач";
            worksheet.Cells[1, 4] = "Дата и время";
            worksheet.Cells[1, 5] = "Статус";

            for (int i = 0; i < appointments.Count; i++)
            {
                var a = appointments[i];
                worksheet.Cells[i + 2, 1] = a.AppointmentID;
                worksheet.Cells[i + 2, 2] = a.PatientName;
                worksheet.Cells[i + 2, 3] = a.DoctorName;
                worksheet.Cells[i + 2, 4] = a.AppointmentDate.ToString("dd.MM.yyyy HH:mm");
                worksheet.Cells[i + 2, 5] = a.Statuss;
            }

            workbook.SaveAs(fullPath);
            workbook.Close();
            excelApp.Quit();

            MessageBox.Show($"Файл сохранён в: {fullPath}", "Экспорт завершён", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExportMedicalRecordsToExcel()
        {
            var records = _context.MedicalRecords.ToList();

            string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            string fileName = $"Медицинские карты_{DateTime.Now:dd.MM.yyyy}.xlsx";
            string fullPath = System.IO.Path.Combine(downloadsPath, fileName);

            var excelApp = new Microsoft.Office.Interop.Excel.Application();
            var workbook = excelApp.Workbooks.Add();
            var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.ActiveSheet;
            worksheet.Name = "Медкарты";

            worksheet.Cells[1, 1] = "ID";
            worksheet.Cells[1, 2] = "Пациент";
            worksheet.Cells[1, 3] = "Врач";
            worksheet.Cells[1, 4] = "Дата посещения";
            worksheet.Cells[1, 5] = "Диагноз";
            worksheet.Cells[1, 6] = "Назначения";
            worksheet.Cells[1, 7] = "Комментарии";

            for (int i = 0; i < records.Count; i++)
            {
                var r = records[i];
                worksheet.Cells[i + 2, 1] = r.RecordID;
                worksheet.Cells[i + 2, 2] = r.PatientName;
                worksheet.Cells[i + 2, 3] = r.DoctorName;
                worksheet.Cells[i + 2, 4] = r.VisitDate.ToString("dd.MM.yyyy");
                worksheet.Cells[i + 2, 5] = r.Diagnosis;
                worksheet.Cells[i + 2, 6] = r.Prescriptions;
                worksheet.Cells[i + 2, 7] = r.Comments;
            }

            workbook.SaveAs(fullPath);
            workbook.Close();
            excelApp.Quit();

            MessageBox.Show($"Файл сохранён в: {fullPath}", "Экспорт завершён", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExportServicesToExcel()
        {
            var services = _context.Servicess.ToList();

            string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            string fileName = $"Услуги_{DateTime.Now:dd.MM.yyyy}.xlsx";
            string fullPath = System.IO.Path.Combine(downloadsPath, fileName);

            var excelApp = new Microsoft.Office.Interop.Excel.Application();
            var workbook = excelApp.Workbooks.Add();
            var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.ActiveSheet;
            worksheet.Name = "Услуги";

            worksheet.Cells[1, 1] = "Название";
            worksheet.Cells[1, 2] = "Описание";
            worksheet.Cells[1, 3] = "Цена";

            for (int i = 0; i < services.Count; i++)
            {
                var s = services[i];
                worksheet.Cells[i + 2, 1] = s.ServiceName;
                worksheet.Cells[i + 2, 2] = s.Descriptionn;
                worksheet.Cells[i + 2, 3] = (s.Price ?? 0).ToString("0.00");
            }

            workbook.SaveAs(fullPath);
            workbook.Close();
            excelApp.Quit();

            MessageBox.Show($"Файл сохранён в: {fullPath}", "Экспорт завершён", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExportSchedulesToExcel()
        {
            var schedules = _context.Schedules.ToList();

            string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            string fileName = $"Расписание врачей_{DateTime.Now:dd.MM.yyyy}.xlsx";
            string fullPath = System.IO.Path.Combine(downloadsPath, fileName);

            var excelApp = new Microsoft.Office.Interop.Excel.Application();
            var workbook = excelApp.Workbooks.Add();
            var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.ActiveSheet;
            worksheet.Name = "Расписание";

            worksheet.Cells[1, 1] = "ID";
            worksheet.Cells[1, 2] = "Врач";
            worksheet.Cells[1, 3] = "День недели";
            worksheet.Cells[1, 4] = "Начало";
            worksheet.Cells[1, 5] = "Окончание";

            for (int i = 0; i < schedules.Count; i++)
            {
                var s = schedules[i];
                worksheet.Cells[i + 2, 1] = s.ScheduleID;
                worksheet.Cells[i + 2, 2] = s.DoctorName;
                worksheet.Cells[i + 2, 3] = s.Day_of_week;
                worksheet.Cells[i + 2, 4] = s.StartTime?.ToString(@"hh\:mm");
                worksheet.Cells[i + 2, 5] = s.EndTime?.ToString(@"hh\:mm");
            }

            workbook.SaveAs(fullPath);
            workbook.Close();
            excelApp.Quit();

            MessageBox.Show($"Файл сохранён в: {fullPath}", "Экспорт завершён", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExportRoomsToExcel()
        {
            var rooms = _context.Rooms.ToList();

            string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            string fileName = $"Кабинеты_{DateTime.Now:dd.MM.yyyy}.xlsx";
            string fullPath = System.IO.Path.Combine(downloadsPath, fileName);

            var excelApp = new Microsoft.Office.Interop.Excel.Application();
            var workbook = excelApp.Workbooks.Add();
            var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.ActiveSheet;
            worksheet.Name = "Кабинеты";

            worksheet.Cells[1, 1] = "Номер кабинета";
            worksheet.Cells[1, 2] = "Описание";

            for (int i = 0; i < rooms.Count; i++)
            {
                var r = rooms[i];
                worksheet.Cells[i + 2, 1] = r.RoomNumber;
                worksheet.Cells[i + 2, 2] = r.Descriptionn;
            }

            workbook.SaveAs(fullPath);
            workbook.Close();
            excelApp.Quit();

            MessageBox.Show($"Файл сохранён в: {fullPath}", "Экспорт завершён", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExportInvoicesToExcel()
        {
            var invoices = _context.Invoices.ToList();

            string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            string fileName = $"Выставленные счета_{DateTime.Now:dd.MM.yyyy}.xlsx";
            string fullPath = System.IO.Path.Combine(downloadsPath, fileName);

            var excelApp = new Microsoft.Office.Interop.Excel.Application();
            var workbook = excelApp.Workbooks.Add();
            var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.ActiveSheet;
            worksheet.Name = "Счета";

            worksheet.Cells[1, 1] = "ID счета";
            worksheet.Cells[1, 2] = "Пациент";
            worksheet.Cells[1, 3] = "Услуга";
            worksheet.Cells[1, 4] = "Дата";
            worksheet.Cells[1, 5] = "Сумма";
            worksheet.Cells[1, 6] = "Статус";

            for (int i = 0; i < invoices.Count; i++)
            {
                var inv = invoices[i];
                worksheet.Cells[i + 2, 1] = inv.InvoiceID;
                worksheet.Cells[i + 2, 2] = inv.PatientName;
                worksheet.Cells[i + 2, 3] = inv.ServiceName;
                worksheet.Cells[i + 2, 4] = inv.InvoiceDate.ToShortDateString();
                worksheet.Cells[i + 2, 5] = (inv.Amount ?? 0).ToString("0.00");
                worksheet.Cells[i + 2, 6] = inv.Statuss;
            }

            workbook.SaveAs(fullPath);
            workbook.Close();
            excelApp.Quit();

            MessageBox.Show($"Файл сохранён в: {fullPath}", "Экспорт завершён", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExportMedicationsToExcel()
        {
            var meds = _context.Medications.ToList();

            string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            string fileName = $"Лекарства_{DateTime.Now:dd.MM.yyyy}.xlsx";
            string fullPath = System.IO.Path.Combine(downloadsPath, fileName);

            var excelApp = new Microsoft.Office.Interop.Excel.Application();
            var workbook = excelApp.Workbooks.Add();
            var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.ActiveSheet;
            worksheet.Name = "Лекарства";

            worksheet.Cells[1, 1] = "Название";
            worksheet.Cells[1, 2] = "Описание";
            worksheet.Cells[1, 3] = "Дозировка";
            worksheet.Cells[1, 4] = "Цена";

            for (int i = 0; i < meds.Count; i++)
            {
                var m = meds[i];
                worksheet.Cells[i + 2, 1] = m.MedicationName;
                worksheet.Cells[i + 2, 2] = m.Descriptionn;
                worksheet.Cells[i + 2, 3] = m.Dosage;
                worksheet.Cells[i + 2, 4] = $"{m.Price:0.00}";
            }

            workbook.SaveAs(fullPath);
            workbook.Close();
            excelApp.Quit();

            MessageBox.Show($"Файл сохранён в: {fullPath}", "Экспорт завершён", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExportPrescriptionsToExcel()
        {
            var prescs = _context.Prescriptions.ToList();

            string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            string fileName = $"Назначения лекарств_{DateTime.Now:dd.MM.yyyy}.xlsx";
            string fullPath = System.IO.Path.Combine(downloadsPath, fileName);

            var excelApp = new Microsoft.Office.Interop.Excel.Application();
            var workbook = excelApp.Workbooks.Add();
            var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.ActiveSheet;
            worksheet.Name = "Назначения";

            worksheet.Cells[1, 1] = "ID назначения";
            worksheet.Cells[1, 2] = "ID медкарты";
            worksheet.Cells[1, 3] = "Лекарство";
            worksheet.Cells[1, 4] = "Инструкция";

            for (int i = 0; i < prescs.Count; i++)
            {
                var p = prescs[i];
                worksheet.Cells[i + 2, 1] = p.PrescriptionID;
                worksheet.Cells[i + 2, 2] = p.RecordID;
                worksheet.Cells[i + 2, 3] = p.MedicationName;
                worksheet.Cells[i + 2, 4] = p.DosageInstructions;
            }

            workbook.SaveAs(fullPath);
            workbook.Close();
            excelApp.Quit();

            MessageBox.Show($"Файл сохранён в: {fullPath}", "Экспорт завершён", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}
