using System.Collections.Generic;
/// <summary>
/// Вспомогательный класс для десериализации XML
/// </summary>
public class XmlDataRoot
{
    public List<Tour> Tours { get; set; }
    public List<Tourist> Tourists { get; set; }
    public List<Hotel> Hotels { get; set; }
}