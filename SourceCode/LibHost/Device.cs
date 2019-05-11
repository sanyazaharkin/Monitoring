using System;


namespace LibHost
{
    [Serializable] //отмечаем что класс будет сериализовываться
    public class Device // базовый класс для устройств
    {
        public int id; //для хранения ID устройства
        public string device_type; //для хранения типа устройства (здесь скорее нужно было использовать перечисление, но решил использовать строку)
        public int hash; //храними уникальный хэш

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
