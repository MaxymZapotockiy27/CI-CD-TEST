using OfficeOpenXml;
using System;
using System.IO;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        // Встановлюємо контекст ліцензії для некомерційного використання
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        bool continueProcessing = true;

        while (continueProcessing)
        {
            // Запитуємо шлях до файлу Excel
            Console.WriteLine("Введіть повний шлях до Excel файлу:");
            string excelFilePath = Console.ReadLine();

            // Перевіряємо, чи існує файл
            if (!File.Exists(excelFilePath))
            {
                Console.WriteLine("Файл не знайдено. Перевірте шлях і спробуйте ще раз.");
                continue;
            }

            // Отримуємо директорію та ім'я файлу без розширення
            string directoryPath = Path.GetDirectoryName(excelFilePath);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(excelFilePath);

            // Формуємо шлях для збереження CSV
            string csvFilePath = Path.Combine(directoryPath, fileNameWithoutExtension + ".csv");

            // Читаємо Excel файл і конвертуємо його в CSV
            using (var package = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                var worksheet = package.Workbook.Worksheets[0]; // Вибираємо перший лист Excel
                var csvContent = new StringBuilder();

                // Проходимо по всіх рядках та стовпцях
                for (int row = 1; row <= worksheet.Dimension.Rows; row++)
                {
                    var rowValues = new List<string>();

                    for (int col = 1; col <= worksheet.Dimension.Columns; col++)
                    {
                        var cellValue = worksheet.Cells[row, col].Text;

                        // Якщо в комірці є кома або число 34, додаємо лапки
                        if (cellValue.Contains(",") || cellValue.StartsWith("34"))
                        {
                            cellValue = "\"" + cellValue + "\"";
                        }

                        rowValues.Add(cellValue);
                    }

                    csvContent.AppendLine(string.Join(",", rowValues));
                }

                // Зберігаємо дані у CSV файл
                File.WriteAllText(csvFilePath, csvContent.ToString(), Encoding.UTF8);
            }

            Console.WriteLine($"Конвертація завершена! CSV файл збережено за адресою: {csvFilePath}");

            // Питаємо користувача, чи хоче він обробити ще один файл
            Console.WriteLine("Бажаєте обробити ще один файл? (так/ні):");
            string response = Console.ReadLine().Trim().ToLower();

            if (response != "так")
            {
                continueProcessing = false;
            }
        }

        Console.WriteLine("Програма завершена.");
    }
}
