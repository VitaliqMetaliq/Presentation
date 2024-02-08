using System.Xml.Serialization;

namespace Crawler.Core.Models;

[XmlRoot("Valuta")]
public class BaseCurrencyModel
{
    public BaseCurrencyModel() { }

    [XmlElement("Item")]
    public CurrencyItemModel[] Items { get; set; }
}