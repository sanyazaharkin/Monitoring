using System;
using System.Collections.Generic;



namespace LibHost.Devices
{
    [Serializable]//отмечаем что класс будет сериализовываться
    public class Device_NET : Device //наследуем от класса device
    {
        
        public string mac;
        public string description;
        public List<System.Net.IPAddress> Gateway;
        public List<System.Net.IPAddress> iPAddresses;



        public Device_NET( string mac, string description, List<System.Net.IPAddress> gateway, List<System.Net.IPAddress> iPAddresses) //конструктор принимающий информацию об устройстве
        {
            this.device_type = "NET";

            this.mac = mac ?? "-1";
            this.description = description ?? "-1";
            Gateway = gateway ?? new List<System.Net.IPAddress>();
            this.iPAddresses = iPAddresses ?? new List<System.Net.IPAddress>();

            string temp = (this.mac + this.description);//конкатенируем информацию во временную строку
            this.hash = temp.GetHashCode(); //получаем хеш
        }

        public override string ToString() //переопределенный метод возвращающий строку, нужен был только для отладки, не используется сейчас
        {
            string result = string.Empty;
            result += "\n-----------------------NET------------------------";
            result += "\nhash = " + this.hash;
            result += "\nmac = " + this.mac;
            result += "\ndescription = " + this.description;
            result += "\ngateways = ";
            foreach (var item in Gateway)
            {
                result += item.ToString() + " ";
            }
            result += "\nIPAddresses = " + this.iPAddresses;
            foreach (var item in iPAddresses)
            {
                result += item.ToString() + " ";
            }


            result += "\ndevice_type = " + this.device_type;

            result += "\n--------------------------------------------------\n";


            return result;
        }

    }
}
