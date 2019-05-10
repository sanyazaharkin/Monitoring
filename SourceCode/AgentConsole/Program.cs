using System;
using System.Configuration;

namespace AgentConsole
{
    public class Program
    {
        public static void Main(string[] args) //точка входа, метод запускаемый по умолчанию
        {
            LibAgent.Work.DebugInfoSend += ShowMessage; // добавляем обработчик события             
            LibAgent.Work.Main(ConfigurationManager.AppSettings); // и запускаем главный метод агента, в параметры передаем настройки (коллекция Имя-Значение) которые прочитаны из XML файлика рядом с программой       
        }


        public static void ShowMessage(string str) //метод для вывода информации в консоль
        {
            Console.WriteLine(str);
        }
        
    }
}
