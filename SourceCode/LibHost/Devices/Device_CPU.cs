using System;


namespace LibHost.Devices
{
    [Serializable] //отмечаем что класс будет сериализовываться
    public class Device_CPU : Device //наследуем от класса device
    {
        
        public string manufacturer; //хранение информации о производителе
        public string name; //хранение имени процессора
        public int cores; //количество ядер (физических)
        public int clock_speed; //частота процессора в МГц


        public Device_CPU(string manufacturer, string name,int cores, int clock_speed) //конструктор принимающий информацию о процессоре
        {
            this.device_type = "CPU";

            this.manufacturer = manufacturer ?? "-1"; //присваиваем переменным полученные значения с проверкой на null
            this.name = name ?? "-1";
            this.cores = cores;
            this.clock_speed = clock_speed;

            string temp = (this.manufacturer + this.name + this.cores.ToString() + this.clock_speed.ToString()); //конкатенируем информацию во временную строку        
            this.hash = temp.GetHashCode();   //и из временной строки получаем хеш, уникальный для этого устройства         
        }


        public override string ToString() //переопределенный метод возвращающий строку, нужен был только для отладки, не используется сейчас
        {
            string result = string.Empty;
            result += "\n-----------------------CPU------------------------";
            result += "\nhash = " + this.hash;
            result += "\nmanufacturer = " + this.manufacturer;
            result += "\nname = " + this.name;
            result += "\ncores = " + this.cores;
            result += "\nclock_speed = " + this.clock_speed;
            result += "\ndevice_type = " + this.device_type;
            result += "\n--------------------------------------------------\n";


            return result;
        }
    }
}
