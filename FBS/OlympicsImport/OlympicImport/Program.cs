using System;
using System.Diagnostics;
using System.IO;
using OlympicImport.Services;

namespace OlympicImport
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                FileInfo fileName = new FileInfo(args[0]);
                if (!fileName.Exists)
                {
                    Console.WriteLine("Файл не найден: {0} ({1})", fileName.Name, fileName.FullName);
                }

                Console.WriteLine("Импорт файла: {0} ({1})", fileName.Name, fileName.FullName);
                Console.WriteLine("Год олимпиады: {0}", ImportSettings.OlympiadYear);
                Console.WriteLine("Удалить олимпиады: {0}", ImportSettings.RemoveOlympics ? "да" : "нет");
                Console.WriteLine("Удалить дипломантов: {0}", ImportSettings.RemoveDiplomants ? "да" : "нет");
                Console.WriteLine("Удалить предметы и их привязки: {0}", ImportSettings.RemoveSubjects ? "да" : "нет");
                Console.Write("Продолжить? (y/n):");

                var confirm = Console.ReadKey();

                if (confirm.KeyChar == 'y' || confirm.KeyChar == 'Y')
                {
                    Console.WriteLine();
                    var ctl = new OlympicImportController(GetConnectionStringProvider(), fileName);
                    Stopwatch timer = new Stopwatch();
                    timer.Start();
                    ctl.CleanTables(ImportSettings.RemoveSubjects, ImportSettings.RemoveDiplomants, ImportSettings.RemoveOlympics);
                    timer.Stop();
                    Console.WriteLine("Очистка таблиц за {0}", timer.Elapsed);
                    timer.Reset();
                    timer.Start();
                    ctl.Run();
                    timer.Stop();
                    Console.WriteLine("Импорт завершен за {0}", timer.Elapsed);
                }
            }
            else
            {
                PrintUsage();
            }
        }

        private static IConnectionStringProvider GetConnectionStringProvider()
        {
            return new FbsConfigConnectionStringProvider();
        }

        static void PrintUsage()
        {
            Console.WriteLine("Использование: OlympicImport <имя_файла>");
        }
    }
}
