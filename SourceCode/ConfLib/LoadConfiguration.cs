using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace ConfLib
{
    class LoadConfiguration
    {
        public string Path { get; private set; } //Имя файла.

        [DllImport("kernel32")] //Подключаем kernel32.dll и описываем его функцию WritePrivateProfilesString
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32")] //Еще раз подключаем kernel32.dll, а теперь описываем функцию GetPrivateProfileString
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        
        public LoadConfiguration(string IniPath) // С помощью конструктора записываем пусть до файла и его имя.
        {
            Path = IniPath;            
        }

        
        public string ReadINI(string Section, string Key) //Читаем ini-файл и возвращаем значение указного ключа из заданной секции.
        {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section, Key, "", RetVal, 255, Path);
            return RetVal.ToString();
        }
        
        public void Write(string Section, string Key, string Value) //Записываем в ini-файл. Запись происходит в выбранную секцию в выбранный ключ.
        {
            WritePrivateProfileString(Section, Key, Value, Path);
        }

        
        public void DeleteKey(string Key, string Section = null) //Удаляем ключ из выбранной секции.
        {
            Write(Section, Key, null);
        }
        
        public void DeleteSection(string Section = null) //Удаляем выбранную секцию
        {
            Write(Section, null, null);
        }
        
        public bool KeyExists(string Key, string Section = null) //Проверяем, есть ли такой ключ, в этой секции
        {
            return ReadINI(Section, Key).Length > 0;
        }
    }
}
