using System.Xml.Serialization;

namespace Crawler.Core.Models;

public class DailyCurrencyItemModel
{
    [XmlElement("Name")]
    public string Name { get; set; }

    [XmlElement("NumCode")]
    public string NumCodeString 
    { 
        get => NumCode == null ? "" : NumCode.ToString();
        set { if (!value.Equals("")) NumCode = int.Parse(value); }
    }

    [XmlIgnore]
    public int? NumCode { get; set; }

    [XmlElement("CharCode")]
    public string CharCode { get; set; }

    [XmlElement("Nominal")]
    public int Nominal { get; set; }

    [XmlElement("Value")]
    public string ValueString
    {
        get => Value == null ? "" : Value.ToString();
        set
        {
            if (!value.Equals("")) Value = double.Parse(value);
        }
    }

    [XmlIgnore]
    public double Value { get; set; }
}