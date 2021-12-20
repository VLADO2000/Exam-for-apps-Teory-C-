using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoting
{
    class RIN
    {
        int min = 0;    // нижняя граница
        int max = 100;  // верхняя граница
        public string info = "число"; // имя параметра
        public bool n_bool; // успешность преобразования

        // Ограниченное число: поле n и свойство N
        int n;
        public int N    // свойство
        {
            get { return n; }
            set
            {
                if (value < min)
                {
                    n = min;
                    n_bool = false;
                }
                else if (value > max)
                {
                    n = max;
                    n_bool = false;
                }
                else
                {
                    n = value;
                    n_bool = true;
                }
            }
        }

        // конструктор
        public RIN(string n_st, int n_min, int n_max, int n_def, string n_info)
        {
            min = n_min;
            max = n_max;
            info = n_info;
            n_bool = true;
            try
            {
                N = Convert.ToInt32(n_st);
                if (!n_bool)
                {
                    info = "Ошибка ввода параметра << " + info + " >>. Число вне диапазона. Автоматически присваивается нижняя/верхняя граница. Для изменения введите целое число от " + min.ToString() + " до " + max.ToString();
                }
            }
            catch
            {
                info = "Ошибка ввода параметра << " + info + " >>. Введите целое число от " + min.ToString() + " до " + max.ToString() + ". По умолчанию параметр =  " + n_def.ToString(); ;
                n = n_def;
                n_bool = false;
            }
        }
    }   // end class RIN
}
