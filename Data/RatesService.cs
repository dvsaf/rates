using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using ServiceReference;

namespace CurrencyRateApp.Data
{
    /// <summary>
    /// Методы взаимодействия с сервером ЦБ.
    /// </summary>
    public static class RatesService
    {
        /// <summary>
        /// Перечень валют, котируемых ежедневно.
        /// </summary>
        public static IEnumerable<Currency> DailyRatedCurrencies
        {
            get
            {
                if (_dailyRatedCurrencies == null)
                    _dailyRatedCurrencies = LoadCurrencies(false);
                return _dailyRatedCurrencies == null ? new List<Currency>() : _dailyRatedCurrencies;
            }
        }

        /// <summary>
        /// Получить курсы ежедневно котируемых валют на заданную дату.
        /// Данные кэшируются.
        /// </summary>
        public static IEnumerable<CurrencyRate> GetRatesForDate(DateTime date)
        {
            IEnumerable<CurrencyRate> result;

            if (_rates.ContainsKey(date))
            {
                result = _rates[date];
            }
            else
            {
                result = LoadRatesForDate(date);
                if (result == null)
                    result = new List<CurrencyRate>();
                else
                    _rates[date] = result;
            }

            return result;
        }

        /// <summary>
        /// Получить курс заданной валюты в заданном диапазоне дат.
        /// </summary>
        public static IEnumerable<DateRate> GetRatesForCurrency(Currency currency, DateTime startDate, DateTime endDate)
        {
            var result = LoadRatesForCurrency(currency, startDate, endDate);
            return result == null ? new List<DateRate>() : result;
        }

        /// <summary>
        /// Загрузить список валют.
        /// <param name="monthlyRated">(в описании службы называется Seld)
        ///     false — перечень ежедневных валют
        ///     true — перечень ежемесячных валют
        /// </param>
        /// </summary>
        private static IEnumerable<Currency> LoadCurrencies(bool monthlyRated)
        {
            try
            {
                var callResult = Service.EnumValutesAsync(monthlyRated).Result;
                var rootElement = callResult.Nodes.Last();
                var dataElement = rootElement.Element("ValuteData");
                var result = dataElement.Elements("EnumValutes").Select(element => new Currency(element)).ToList();
                foreach (var item in result)
                {
                    if (!_currenciesByNumericCode.ContainsKey(item.NumericCode))
                        _currenciesByNumericCode[item.NumericCode] = item;
                }

                return result;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Загрузить курсы всех котируемых ежедневно валют на заданную дату.
        /// </summary>
        private static IEnumerable<CurrencyRate> LoadRatesForDate(DateTime date)
        {
            /*
                С сервера приходит набор таких записей:
                <ValuteCursOnDate diffgr:id="ValuteCursOnDate1" msdata:rowOrder="0" …>
                    <Vname>Австралийский доллар                                                                                                                                                                                                                                          </Vname>
                    <Vnom>1</Vnom>
                    <Vcurs>43.6882</Vcurs>
                    <Vcode>36</Vcode>
                    <VchCode>AUD</VchCode>
                </ValuteCursOnDate>
            */
            try
            {
                var rates = new List<CurrencyRate>();

                if (DailyRatedCurrencies.Any()) // to fill '_currenciesByNumericCode'
                {
                    var result = Service.GetCursOnDateAsync(date).Result;
                    var rootElement = result.Nodes.Last();
                    var dataElement = rootElement.Element("ValuteData");

                    foreach (var record in dataElement.Elements("ValuteCursOnDate"))
                    {
                        var vcode = Int32.Parse(record.Element("Vcode").Value);
                        var vcurs = Decimal.Parse(record.Element("Vcurs").Value.Trim(), CultureInfo.InvariantCulture.NumberFormat);
                        if (_currenciesByNumericCode.ContainsKey(vcode))
                            rates.Add(new CurrencyRate(_currenciesByNumericCode[vcode], vcurs));
                    }
                }

                return rates;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Загрузить курс заданной валюты в заданном диапазоне дат..
        /// </summary>
        private static IEnumerable<DateRate> LoadRatesForCurrency(Currency currency, DateTime startDate, DateTime endDate)
        {
            try
            {
                /*
                    С сервера приходит набор таких записей:
                    <ValuteCursDynamic diffgr:id="ValuteCursDynamic1" msdata:rowOrder="0" …>
                      <CursDate>2019-11-07T00:00:00+03:00</CursDate>
                      <Vcode>R01010</Vcode>
                      <Vnom>1</Vnom>
                      <Vcurs>43.8630</Vcurs>
                    </ValuteCursDynamic>
                */

                var rates = new List<DateRate>();

                var result = Service.GetCursDynamicAsync(startDate, endDate, currency.Code).Result;
                var rootElement = result.Nodes.Last();
                var dataElement = rootElement.Element("ValuteData");

                foreach (var record in dataElement.Elements("ValuteCursDynamic"))
                {
                    var date = DateTime.Parse(record.Element("CursDate").Value);
                    var vcurs = Decimal.Parse(record.Element("Vcurs").Value.Trim(), CultureInfo.InvariantCulture.NumberFormat);
                    rates.Add(new DateRate(date, vcurs));
                }

                return rates;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Объект доступа к сервису ЦБ.
        /// </summary>
        private static DailyInfoSoap Service
        {
            get
            {
                if (_service == null)
                    _service = new DailyInfoSoapClient(DailyInfoSoapClient.EndpointConfiguration.DailyInfoSoap12);

                return _service;
            }
        }

        private static IEnumerable<Currency> _dailyRatedCurrencies;
        private static Dictionary<int, Currency> _currenciesByNumericCode = new Dictionary<int, Currency>();
        private static Dictionary<DateTime, IEnumerable<CurrencyRate>> _rates = new Dictionary<DateTime, IEnumerable<CurrencyRate>>();
        private static DailyInfoSoap _service;
    }
}