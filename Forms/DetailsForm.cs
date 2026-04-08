using System;
using System.Windows.Forms;
namespace TourismApp
{
    public partial class DetailsForm : Form
    {
        public DetailsForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Метод для загрузки объекта Tour
        /// </summary>
        public void ShowTourDetails(Tour tour)
        {
            if (tour == null) return;
            this.Text = $"Тур: {tour.Title}";
            var details = $"━━━━━━━━━━━━━━━━━━━━\n";
            details += $"📍 Название: {tour.Title}\n";
            details += $"💰 Цена: {tour.Price:N0} {tour.CountryInfo.Valuta}\n";
            details += $"📅 Длительность: {tour.DurationDays} дней\n";
            details += $"🌍 Страна: {tour.CountryInfo.Name}\n";
            details += $"💱 Валюта: {tour.CountryInfo.Valuta}\n";
            details += $"━━━━━━━━━━━━━━━━━━━━\n";
            details += $"👨‍ Гид: {tour.GuideInfo.FullName}\n";
            details += $"⭐ Рейтинг: {tour.GuideInfo.Rating}\n";
            txtDetails.Text = details;
        }
        /// <summary>
        /// Метод для загрузки объекта Tourist
        /// </summary>
        public void ShowTouristDetails(Tourist tourist)
        {
            if (tourist == null) return;
            this.Text = $"Турист: {tourist.FullName}";
            var details = $"━━━━━━━━━━━━━━━━━━━━\n";
            details += $"👤 ФИО: {tourist.FullName}\n";
            details += $"🎂 Возраст: {tourist.Age} лет\n";
            details += $"📄 Паспорт: {tourist.PassportData.Series}-{tourist.PassportData.Number}\n";
            details += $"━━━━━━━━━━━━━━━━━━━━\n";
            details += $"🍽️ Питание: {GetFoodTypeName(tourist.TourPreferences.Food)}\n";
            details += $"🚗 Транспорт: {GetTransportTypeName(tourist.TourPreferences.Transport)}\n";
            txtDetails.Text = details;
        }
        /// <summary>
        /// Метод для загрузки объекта Hotel
        /// </summary>
        public void ShowHotelDetails(Hotel hotel)
        {
            if (hotel == null) return;
            this.Text = $"Отель: {hotel.Name}";
            var details = $"━━━━━━━━━━━━━━━━━━━━\n";
            details += $"🏨 Название: {hotel.Name}\n";
            details += $"⭐ Звёзды: {GetStars(hotel.Stars)}\n";
            details += $"💰 Цена за ночь: {hotel.PriceForNight:N0}\n";
            details += $"📍 Город: {hotel.AddressInfo.City}\n";
            details += $"🏠 Улица: {hotel.AddressInfo.Street}\n";
            details += $"━━━━━━━━━━━━━━━━━━━━\n";
            details += $"📧 Email: {hotel.ContactInfo.Email}\n";
            details += $"📞 Телефон: {hotel.ContactInfo.Phone}\n";
            txtDetails.Text = details;
        }
        /// <summary>
        /// Кнопка "Закрыть" — полностью закрывает форму деталей
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// вспомогательные методы
        private string GetFoodTypeName(FoodType food)
        {
            return food.ToString();
        }
        private string GetTransportTypeName(TransportType transport)
        {
            return transport.ToString();
        }
        private string GetStars(int count)
        {
            return new string('⭐', count);
        }
    }
}