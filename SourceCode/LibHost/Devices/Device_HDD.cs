using System;


namespace LibHost.Devices
{
    [Serializable] //отмечаем что класс будет сериализовываться
    public class Device_HDD : Device //наследуем от класса device
    {
       
        public string description;
        public string caption;
        public ulong size;
        public ulong free_space;
        public string file_system;



        public Device_HDD( string description, string caption, ulong size, ulong free_space, string file_system) //конструктор принимающий информацию об устройстве
        {
            this.device_type = "HDD";

            this.description = description ?? "-1";
            this.caption = caption ?? "-1";
            this.size = size;
            this.free_space = free_space;
            this.file_system = file_system ?? "-1";

            string temp = (this.description + this.caption + this.size.ToString() + this.file_system.ToString()); //конкатенируем информацию во временную строку
            this.hash = temp.GetHashCode(); //получаем хеш
        }

        public override string ToString()//переопределенный метод возвращающий строку, нужен был только для отладки, не используется сейчас
        {
            string result = string.Empty;
            result += "\n-----------------------HDD------------------------";
            result += "\nhash = " + this.hash;
            result += "\ndescription = " + this.description;
            result += "\ncaption = " + this.caption;
            result += "\nsize = " + this.size;
            result += "\nfree_space = " + this.free_space;
            result += "\nfile_system = " + this.file_system;
            result += "\ndevice_type = " + this.device_type;

            result += "\n--------------------------------------------------\n";


            return result;
        }
    }
}
