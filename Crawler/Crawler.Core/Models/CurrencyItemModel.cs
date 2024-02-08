using System.Xml.Serialization;

namespace Crawler.Core.Models;

public class CurrencyItemModel
{
    public CurrencyItemModel() { }

    public string Name { get; set; }
    public string EngName { get; set; }
    public int Nominal { get; set; }
    public string ParentCode { get; set; }
    [XmlElement("ISO_Num_Code")]
    public string IsoNumCode { get; set; }
    [XmlElement("ISO_Char_Code")]
    public string IsoCharCode { get; set; }
}