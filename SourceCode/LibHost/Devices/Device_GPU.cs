using System;


namespace LibHost.Devices
{
    [Serializable] //отмечаем что класс будет сериализовываться
    public class Device_GPU : Device //наследуем от класса device
    {
        
        public string name;
        public uint memory_size;



        public Device_GPU(string name, uint memory_size) //конструктор принимающий информацию об устройстве
        {
            this.device_type = "GPU";

            this.name = name ?? "-1";
            this.memory_size = memory_size;

            string temp = (this.name + this.memory_size.ToString()); //конкатенируем информацию во временную строку 
            this.hash = temp.GetHashCode(); //получаем хеш
        }

        public override string ToString() //переопределенный метод возвращающий строку, нужен был только для отладки, не используется сейчас
        {
            string result = string.Empty;
            result += "\n-----------------------GPU------------------------";
            result += "\nhash = " + this.hash;
            result += "\nname = " + this.name;
            result += "\nmemory_size = " + this.memory_size;
            result += "\ndevice_type = " + this.device_type;

            result += "\n--------------------------------------------------\n";


            return result;
        }
    }
}
