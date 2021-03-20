using System;
using System.Collections.Generic;
using System.Linq;
using CurrencyRateApp.Data;

namespace CurrencyRateApp.Models
{
    /// <summary>
    /// Модель для представления динамики курса валюты.
    /// </summary>
    public class DynamicViewModel
    {
        /// <summary>
        /// Перечень валют для выбора.
        /// </summary>
        public IEnumerable<Currency> Currencies => RatesService.DailyRatedCurrencies;

        /// <summary>
        /// Выбранная валюта.
        /// </summary>
        public Currency Currency { get; private set; }

        /// <summary>
        /// Начало периода.
        /// </summary>
        public DateTime StartDate { get; private set; }

        /// <summary>
        /// Конец периода.
        /// </summary>
        public DateTime EndDate { get; private set; }

        /// <summary>
        /// Перечень ежедневных курсов валюты за период.
        /// </summary>
        public IEnumerable<DateRate> RatesDynamic =>
            RatesService.GetRatesForCurrency(Currency, StartDate, EndDate);

        /// <summary>
        /// Конструктор для первой загрузки страницы, без указания параметров.
        /// </summary>
        public DynamicViewModel()
        {
            Currency = null;
            StartDate = DateTime.Today;
            EndDate = DateTime.Today;
        }

        /// <summary>
        /// Конструктор для повторной загрузки, с указанием параметров.
        /// <param name="code">Код валюты (Vcode).</param>
        /// <param name="startDate">Дата начала периода.</param>
        /// <param name="endDate">Дата конца периода.</param>
        /// </summary>
        public DynamicViewModel(string code, DateTime startDate, DateTime endDate)
        {
            Currency = Currencies.FirstOrDefault(c => c.Code == code);
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
