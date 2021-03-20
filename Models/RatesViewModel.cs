using System;
using System.Collections.Generic;
using System.Linq;
using CurrencyRateApp.Data;

namespace CurrencyRateApp.Models
{
    /// <summary>
    /// Модель для представления курсов всех валют на заданную дату.
    /// </summary>
    public class RatesViewModel
    {
        /// <summary>
        /// Перечень валют для выбора.
        /// </summary>
        public IEnumerable<Currency> Currencies => RatesService.DailyRatedCurrencies;

        /// <summary>
        /// Перечень валют, для которых отображается курс.
        /// </summary>
        public IList<string> VisibleCurrencies { get; private set; }

        /// <summary>
        /// Дата, на которую отображается курс.
        /// </summary>
        public DateTime Date { get; private set; }

        /// <summary>
        /// Перечень курсов всех валют на указанную дату.
        /// </summary>
        public IEnumerable<CurrencyRate> Rates => RatesService.GetRatesForDate(Date);

        /// <summary>
        /// Конструктор для первой загрузки страницы, без указания параметров..
        /// </summary>
        public RatesViewModel()
        {
            Date = DateTime.Today;
            VisibleCurrencies = new List<string>();
        }

        /// <summary>
        /// Конструктор для повторной загрузки, с указанием параметров.
        /// <param name="date">Дата, на которую отображается курс.</param>
        /// <param name="visible">Коды валют (Vcode), для которых отображаются курс, разделённые запятыми.</param>
        /// </summary>
        public RatesViewModel(DateTime date, string visible)
        {
            Console.WriteLine("!!!!! " + visible);
            Date = date;
            VisibleCurrencies = visible.Split(',').ToList();
        }
    }
}
