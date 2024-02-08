using System.Xml.Serialization;

namespace Crawler.Core.Models;

[XmlRoot("ValCurs")]
public class DailyCurrencyListModel
{
    public DailyCurrencyListModel() { }

    [XmlIgnore]
    public DateTime Date { get; set; }

    [XmlAttribute("Date")]
    public string CurrentDate
    {
        get => Date == null ? "" : Date.ToString();
        set
        {
            if (!value.Equals("")) Date = DateTime.Parse(value);
        }
    }

    [XmlElement("Valute")]
    public DailyCurrencyItemModel[] Items { get; set; }
}