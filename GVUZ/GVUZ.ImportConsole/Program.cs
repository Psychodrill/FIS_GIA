using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GVUZ.ImportService2016.Core.Main;
using GVUZ.ImportService2016.Core.Main.Log;

namespace GVUZ.ImportConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            SystemInfo si = new SystemInfo();
            ShowVersion();
            Console.WriteLine("Число процессоров: {0} число ядер: {1}", si.Processors, si.Cores);
            //ProcessingManager.Start();
            string input = "";
            int package_id = 25665785;
            Console.WriteLine("Для выхода напишите 'exit' или 'quit'; 'help' - для отображения списка команд");
            bool run = true;
            do
            {
                Console.Write("Введите номер пакета или команду: ");
                input = Console.ReadLine();
                string command = input.ToLower();
                if (command == "exit" || command == "quit")
                {
                    run = false;
                }
                else
                {
                    if (command.StartsWith("help"))
                    {
                        ShowHelp();
                    }
                    else if (command.StartsWith("file"))
                    {
                        string[] c_args = command.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (c_args.Length < 2)
                        {
                            Console.WriteLine("Ошибка! Укажите имя файла для обработки!");
                        }
                        else
                        {
                            string file_name = c_args[1];
                            if (!System.IO.File.Exists(file_name))
                            {
                                Console.WriteLine("Ошибка! Файл '{0}' не найден!", file_name);
                            }
                            else
                            {
                                using (System.IO.StreamReader readFile = new System.IO.StreamReader(file_name, System.Text.Encoding.GetEncoding(1251), false))
                                {
                                    //string[] row;
                                    int position = 1;
                                    string line = "";
                                    while ((line = readFile.ReadLine()) != null)
                                    {
                                        ProcessInput(line, out package_id);
                                        position++;
                                    }
                                    readFile.Close();
                                }
                            }
                        }
                    }
                    else
                    {
                        ProcessInput(input, out package_id);
                        //if (int.TryParse(input, out package_id))
                        //{
                        //    Console.WriteLine("Попытка обработки пакета №{0}....:", package_id);
                        //    GVUZ.ImportService2016.Core.Main.Repositories.ADOPackageRepository.ResetImportPackages(false, false, true, package_id);
                        //    ProcessingManager.StartWinForms(package_id, true);
                        //    Console.WriteLine("Команда выполнена", package_id);
                        //}
                        //else
                        //{
                        //    Console.WriteLine("Некорректно указан номер пакета: '{0}'....:", input);
                        //}
                    }
                }
            } while (run);

            ProcessingManager.Stop();
        }

        static void ShowVersion()
        {
            System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
            var assembly_data = asm.GetName().Version;
            //Console.WriteLine("");
            Console.WriteLine("Загрузка пакетов, версия {0}", assembly_data);
        }

        static void ShowHelp()
        {
            Console.WriteLine("+--------------------------------------------------------------------------------------------+");
            Console.WriteLine("| Для обработки одного пакета просто введите него номер (id)                                 |");
            Console.WriteLine("| Для обработки из файла наберите 'file <file_name>', где <file_name> - полный путь до файла |");
            Console.WriteLine("| Для выхода наберите 'exit' или 'quit'                                                      |");
            Console.WriteLine("| Наберите 'help' для отображения этого сообщения                                            |");
            Console.WriteLine("+--------------------------------------------------------------------------------------------+");
        }

        static void ProcessInput(string input, out int package_id)
        {
            if (int.TryParse(input, out package_id))
            {
                Console.WriteLine("Попытка обработки пакета №{0}....:", package_id);
                GVUZ.ImportService2016.Core.Main.Repositories.ADOPackageRepository.ResetImportPackages(false, false, true, package_id);
                ProcessingManager.StartWinForms(package_id, true);
                Console.WriteLine("Команда выполнена", package_id);
            }
            else
            {
                Console.WriteLine("Некорректно указан номер пакета: '{0}'....:", input);
            }
            return;
        }
    }
}
