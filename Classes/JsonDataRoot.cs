using System.Collections.Generic;
namespace TourismApp
{
    /// <summary>
    /// Класс-обёртка для десериализации JSON
    /// описывает просто 3 сущности
    /// </summary>
    public class JsonDataRoot
    {
        public List<Tour> Tours { get; set; }
        public List<Tourist> Tourists { get; set; }
        public List<Hotel> Hotels { get; set; }
    }
}