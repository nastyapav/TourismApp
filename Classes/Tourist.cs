using System;
public class Tourist
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public int Age { get; set; }
    public Passport PassportData { get; set; }
    public Preferences TourPreferences { get; set; }
}