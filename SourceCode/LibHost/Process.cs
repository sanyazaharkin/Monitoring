using System;


namespace LibHost
{
    [Serializable] //отмечаем что класс будет сериализовываться
    public class Process 
    {
        public int process_id;
        public string name;

        public Process(string name) //конструктор принимающий информацию о процессе
        {
            this.process_id = 0;
            this.name = name ?? "-1";
        }


        public override string ToString() //переопределенный метод возвращающий строку, нужен был только для отладки, не используется сейчас
        {
            return (name + "\n");
        }
    }
}
