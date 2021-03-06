﻿using System;

namespace LibHost.Devices
{
    [Serializable] //отмечаем что класс будет сериализовываться
    public class Device_RAM : Device //наследуем от класса device
    {
        
        public string manufacturer;
        public int clock_speed;
        public int memory_type;
        public int form_factor;
        public ulong size;
        public string DeviceLocator;



        public Device_RAM(string manufacturer, int clock_speed,  int memory_type, int form_factor, ulong size, string DeviceLocator) //конструктор принимающий информацию об устройстве
        {
            this.device_type = "RAM";

            this.manufacturer = manufacturer ?? "-1";
            this.clock_speed = clock_speed;            
            this.memory_type = memory_type;
            this.form_factor = form_factor;
            this.size = size;
            this.DeviceLocator = DeviceLocator;

            string temp = (this.manufacturer + this.clock_speed.ToString() + this.memory_type.ToString() + this.form_factor.ToString() + this.size.ToString() + this.DeviceLocator.ToString());//конкатенируем информацию во временную строку
            this.hash = temp.GetHashCode();//получаем хеш
        }

        public override string ToString()  //переопределенный метод возвращающий строку, нужен был только для отладки, не используется сейчас
        {
            string result = string.Empty;
            result += "\n-----------------------RAM------------------------";
            result += "\nhash = " + this.hash;
            result += "\nmanufacturer = " + this.manufacturer;
            result += "\nclock_speed = " + this.clock_speed;
            result += "\nmemory_type = " + this.memory_type;
            result += "\nform_factor = " + this.form_factor;
            result += "\nsize = " + this.size;
            result += "\nDeviceLocator = " + this.DeviceLocator;
            result += "\ndevice_type = " + this.device_type;

            result += "\n--------------------------------------------------\n";


            return result;
        }


    }
}
