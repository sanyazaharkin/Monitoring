using System;

namespace LibHost
{
    [Serializable] //отмечаем что класс будет сериализовываться
    public class Program
    {
        public int program_id;
        public int hash;
        public string name;
        public string version;
        public string vendor;
        

        public Program(string name, string version, string vendor) //конструктор принимающий информацию о программе
        {
            this.program_id = 0;
            this.name = name ?? "-1";
            this.version = version ?? "-1";
            this.vendor = vendor ?? "-1";
            string temp = (this.name + this.version + this.vendor );

            this.hash = temp.GetHashCode();
        }



        public override string ToString() //переопределенный метод возвращающий строку, нужен был только для отладки, не используется сейчас
        {
            return ("hash: " + this.hash + " name: " + name + " version: " + vendor + " vendor: " + version + " \n");
        }
    }
}
