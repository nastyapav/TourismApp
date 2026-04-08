using System;
public class Tour
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public int DurationDays { get; set; }
    public Country CountryInfo { get; set; }
    public Guide GuideInfo { get; set; }
}