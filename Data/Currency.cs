using System;
using System.Xml.Linq;

namespace CurrencyRateApp.Data
{
    /// <summary>
    /// Валюта.
    /// </summary>
    public class Currency
    {
        /*
            С сервера приходит набор элементов такой структуры:
            <EnumValutes diffgr:id="EnumValutes1" msdata:rowOrder="0">
                <Vcode>R01010 </Vcode>
                <Vname>Австралийский доллар </Vname>
                <VEngname>Australian Dollar </VEngname>
                <Vnom>1</Vnom>
                <VcommonCode>R01010 </VcommonCode>
                <VnumCode>36</VnumCode>
                <VcharCode>AUD</VcharCode>
            </EnumValutes>
        */

        public readonly string Code; // Vcode
        public readonly string Name; // Vname
        public readonly string EnglishName; // VEngname
        public readonly decimal Nominal; // Vnom
        public readonly int NumericCode; // VnumCode
        public readonly string CharCode; // VcharCode

        /// <summary>
        /// Конструктор на основе XML-элемента из набора, загруженного с сервера ЦБ.
        /// </summary>
        public Currency(XElement responseRecord)
        {
            Code = responseRecord.Element("Vcode")?.Value?.Trim();
            Int32.TryParse(responseRecord.Element("VnumCode")?.Value, out NumericCode);
            Name = responseRecord.Element("Vname")?.Value?.Trim();
            EnglishName = responseRecord.Element("VEngname")?.Value?.Trim();
            Nominal = Decimal.Parse(responseRecord.Element("Vnom")?.Value);
            CharCode = responseRecord.Element("VcharCode")?.Value?.Trim();
        }
    }
}
