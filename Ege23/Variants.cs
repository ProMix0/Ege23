using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ege23
{
    /// <summary>
    /// Класс для решения 23 задания ЕГЭ
    /// </summary>
    public class Variants:IDisposable
    {
        private readonly int start;
        private readonly int stop;
        private List<Rule> rules;
        private Dictionary<int, int> table;

        private int result;
        private bool haveResult = false;

        private List<int> include, exclude;

        private Variants(int start, int stop, List<Rule> rules)
        {
            this.start = start;
            this.stop = stop;
            this.rules = rules;

            // Инициализация списков включения и выключения стартовым и конечным значением
            include = new() { start, stop };
            exclude = new();

            // Заполнение таблицы
            table = new();
            for (int i = start; i <= stop; i++)
            {
                table.Add(i, 0);
            }
            table[start] = 1;
        }

        #region extensions

        /// <summary>
        /// Функция, задающая траекторию
        /// </summary>
        /// <param name="include">Число, через которое должна проходить траектория</param>
        /// <returns>Количество вариантов</returns>
        public Variants Include(params int[] include)
        {
            return Include((IEnumerable<int>)include);
        }
        /// <summary>
        /// Функция, задающая траекторию
        /// </summary>
        /// <param name="include">Число, через которое должна проходить траектория</param>
        /// <returns>Количество вариантов</returns>
        public Variants Include(IEnumerable<int> include)
        {
            foreach (var i in include)
                Include(i);

            return this;
        }
        /// <summary>
        /// Функция, задающая траекторию
        /// </summary>
        /// <param name="include">Число, через которое должна проходить траектория</param>
        /// <returns>Количество вариантов</returns>
        public Variants Include(int include)
        {
            if (!table.ContainsKey(include))
                throw new ArgumentOutOfRangeException(nameof(include));

            this.include.Add(include);

            return this;
        }

        /// <summary>
        /// Функция, задающая траекторию
        /// </summary>
        /// <param name="exclude">Число, через которое не должна проходить траектория</param>
        /// <returns>Количество вариантов</returns>
        public Variants Exclude(params int[] exclude)
        {
            return Exclude((IEnumerable<int>)exclude);
        }
        /// <summary>
        /// Функция, задающая траекторию
        /// </summary>
        /// <param name="exclude">Число, через которое не должна проходить траектория</param>
        /// <returns>Количество вариантов</returns>
        public Variants Exclude(IEnumerable<int> exclude)
        {
            foreach (var i in exclude)
                Exclude(i);

            return this;
        }
        /// <summary>
        /// Функция, задающая траекторию
        /// </summary>
        /// <param name="exclude">Число, через которое не должна проходить траектория</param>
        /// <returns>Количество вариантов</returns>
        public Variants Exclude(int exclude)
        {
            if (!table.ContainsKey(exclude))
                throw new ArgumentOutOfRangeException(nameof(exclude));

            this.exclude.Add(exclude);

            return this;
        }

        #endregion


        //public static explicit operator int(VariantsResult param) => param.Result();

        public static implicit operator int(Variants param) => param.Result();

        /// <summary>
        /// Внутренняя функция для вычисления результата
        /// </summary>
        /// <returns>Количество вариантов</returns>
        private int Result()
        {
            if (!haveResult)
            {
                // Подготовка списков
                include.Sort();
                foreach (var i in exclude.Distinct())
                    table.Remove(i);

                // Расчёт по промежуткам
                int? previous = null;
                foreach (var i in include)
                {
                    if (previous != null)
                        SubResult(previous.Value, i);
                    previous = i;
                }

                result = table[stop];
                haveResult = true;
            }

            return result;
        }

        /// <summary>
        /// Функция для вычисления вариантов на определённом промежутке
        /// </summary>
        /// <param name="start">Стартовое значение</param>
        /// <param name="stop">Конечное значение</param>
        private void SubResult(int start, int stop)
        {
            if (start == stop) return;

            for (int i = start; i <= stop; i++)
            {
                if (!table.ContainsKey(i)) continue;
                foreach (var rule in rules)
                {
                    // Добавление вариантов в ячейку, вычисляемую по правилу
                    int number = rule(i);
                    if (table.ContainsKey(number) && (number <= start || number <= stop))
                        table[number] += table[i];
                }
            }
        }

        public override string ToString()
        {
            return Result().ToString();
        }

        /// <summary>
        /// Делегат, представляющий команды исполнителя
        /// </summary>
        /// <param name="input">Входное значение</param>
        /// <returns>Результат функции</returns>
        public delegate int Rule(int input);

        /// <summary>
        /// Функция для ввода данных
        /// </summary>
        /// <param name="start">Начальное число</param>
        /// <param name="stop">Конечное число</param>
        /// <param name="rules">Команды исполнителя</param>
        /// <returns>Количество вариантов</returns>
        public static Variants Count(int start, int stop, List<Rule> rules)
        {
            return new(start, stop, rules);
        }

        public void Dispose()
        {
            table = null;
            rules = null;
            include = null;
            exclude = null;
        }
    }
}
