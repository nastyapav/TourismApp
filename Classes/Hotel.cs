using System;
public class Hotel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Stars { get; set; }
    public decimal PriceForNight { get; set; }
    public HotelAddress AddressInfo { get; set; }
    public HotelContacts ContactInfo { get; set; }
}