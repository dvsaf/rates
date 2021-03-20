using System;

namespace CurrencyRateApp.Data
{
    /// <summary>
    /// Курс указанной валюты.
    /// </summary>
    public class CurrencyRate
    {
        public readonly Currency Currency;
        public readonly decimal Rate;

        public CurrencyRate(Currency currency, decimal rate)
        {
            Currency = currency;
            Rate = rate;
        }
    }

    /// <summary>
    /// Курс некоторой валюты на указанную дату.
    /// </summary>
    public class DateRate
    {
        public readonly DateTime Date;
        public readonly decimal Rate;

        public DateRate(DateTime date, decimal rate)
        {
            Date = date;
            Rate = rate;
        }
    }
}
