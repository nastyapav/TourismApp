using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using System.Xml.Serialization;
namespace TourismApp
{
    public partial class MainForm : Form
    {
        // поля для хранения данных всех сущностей
        private List<Tour> _tours;
        private List<Tourist> _tourists;
        private List<Hotel> _hotels;
        public MainForm()
        {
            InitializeComponent();
            this.Text = Resources.AppTitle;
            // автоматически загружаю данные при запуске 
            InitializeTreeView();
            LoadAllFromJson();
            if (treeViewEntities.Nodes.Count > 0)
            {
                treeViewEntities.SelectedNode = treeViewEntities.Nodes[0]; // выбираю "Туры"
                DisplayToursInGrid(); // показываю в таблице
            }
        }
        /// <summary>
        /// Загрузка всех данных из JSON 
        /// </summary>
        private void LoadAllFromJson()
        {
            try
            {
                var jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "data.json");
                if (!File.Exists(jsonPath))
                {
                    MessageBox.Show(Resources.MsgJsonFileNotFound, Resources.MsgError,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var jsonContent = File.ReadAllText(jsonPath);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
                };
                var data = JsonSerializer.Deserialize<JsonDataRoot>(jsonContent, options);
                _tours = data.Tours;
                _tourists = data.Tourists;
                _hotels = data.Hotels;
                ShowLoadSummary();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Resources.MsgLoadError + ex.Message, Resources.MsgError,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Загрузка всех данных из XML 
        /// </summary> 
        private void LoadAllFromXml()
        {
            try
            {
                var xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "data.xml");
                if (!File.Exists(xmlPath))
                {
                    MessageBox.Show(Resources.MsgXmlFileNotFound, Resources.MsgError,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // читаем XML 
                var xmlContent = File.ReadAllText(xmlPath);
                //  сериализатор 
                var serializer = new XmlSerializer(typeof(XmlDataRoot));
                // десериализация XML
                using (var stringReader = new StringReader(xmlContent))
                {
                    var data = (XmlDataRoot)serializer.Deserialize(stringReader);
                    _tours = data.Tours;
                    _tourists = data.Tourists;
                    _hotels = data.Hotels;
                }
                // показывает результат
                ShowLoadSummary();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Resources.MsgLoadError + ex.Message, Resources.MsgError,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Показывает сводку о загруженных данных
        /// </summary>
        private void ShowLoadSummary()
        {
            var message = Resources.MsgSuccess + "!\n\n";
            if (_tours != null)
                message += Resources.MsgToursLoaded + _tours.Count + "\n";
            if (_tourists != null)
                message += Resources.MsgTouristsLoaded + _tourists.Count + "\n";
            if (_hotels != null)
                message += Resources.MsgHotelsLoaded + _hotels.Count + "\n";
            MessageBox.Show(message, Resources.MsgSuccess,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /// <summary>
        /// Показать информацию о всех турах
        /// </summary>
        private void ShowAllTours()
        {
            if (_tours == null || _tours.Count == 0)
            {
                MessageBox.Show(Resources.MsgNoData, Resources.MsgInfo,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var message = Resources.MsgToursLoaded + _tours.Count + "\n\n";
            foreach (var tour in _tours)
            {
                message += $"━━━━━━━━━━━━━━━━━━━━\n";
                message += $"📍 {tour.Title}\n";
                message += $"💰 Цена: {tour.Price:N0} {tour.CountryInfo.Valuta}\n";
                message += $"📅 Дней: {tour.DurationDays}\n";
                message += $"🌍 Страна: {tour.CountryInfo.Name}\n";
                message += $"👨‍🏫 Гид: {tour.GuideInfo.FullName} (рейтинг {tour.GuideInfo.Rating})\n\n";
            }
            MessageBox.Show(message, Resources.BtnShowTours,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /// <summary>
        /// Показать информацию о всех туристах
        /// </summary>
        private void ShowAllTourists()
        {
            if (_tourists == null || _tourists.Count == 0)
            {
                MessageBox.Show(Resources.MsgNoData, Resources.MsgInfo,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var message = Resources.MsgTouristsLoaded + _tourists.Count + "\n\n";
            foreach (var tourist in _tourists)
            {
                message += $"━━━━━━━━━━━━━━━━━━━━\n";
                message += $"👤 {tourist.FullName}\n";
                message += $"🎂 Возраст: {tourist.Age} лет\n";
                message += $"📄 Паспорт: {tourist.PassportData.Series}-{tourist.PassportData.Number}\n";
                message += $"🍽️ Питание: {GetFoodTypeName(tourist.TourPreferences.Food)}\n";
                message += $"🚗 Транспорт: {GetTransportTypeName(tourist.TourPreferences.Transport)}\n\n";
            }
            MessageBox.Show(message, Resources.BtnShowTourists,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /// <summary>
        /// Показать информацию о всех отелях
        /// </summary>
        private void ShowAllHotels()
        {
            if (_hotels == null || _hotels.Count == 0)
            {
                MessageBox.Show(Resources.MsgNoData, Resources.MsgInfo,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var message = Resources.MsgHotelsLoaded + _hotels.Count + "\n\n";
            foreach (var hotel in _hotels)
            {
                message += $"━━━━━━━━━━━━━━━━━━━━\n";
                message += $"🏨 {hotel.Name} {GetStars(hotel.Stars)}\n";
                message += $"💰 Цена за ночь: {hotel.PriceForNight:N0}\n";
                message += $"📍 Адрес: {hotel.AddressInfo.City}, {hotel.AddressInfo.Street}\n";
                message += $"📧 Email: {hotel.ContactInfo.Email}\n";
                message += $"📞 Телефон: {hotel.ContactInfo.Phone}\n\n";
            }
            MessageBox.Show(message, Resources.BtnShowHotels,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /// <summary>
        /// Преобразует enum FoodType в читаемое название
        /// </summary>
        private string GetFoodTypeName(FoodType food)
        {
            //  switch вместо парсинга
            switch (food)
            {
                case FoodType.Standard:
                    return "Обычное";
                case FoodType.Vegetarian:
                    return "Вегетарианское";
                case FoodType.Vegan:
                    return "Веганское";
                case FoodType.GlutenFree:
                    return "Без глютена";
                case FoodType.Halal:
                    return "Халяль";
                case FoodType.Kosher:
                    return "Кошерное";
                default:
                    return food.ToString();
            }
        }
        /// <summary>
        /// Преобразует enum TransportType в читаемое название
        /// </summary>
        private string GetTransportTypeName(TransportType transport)
        {
            switch (transport)
            {
                case TransportType.Bus:
                    return "Автобус";
                case TransportType.Plane:
                    return "Самолёт";
                case TransportType.Train:
                    return "Поезд";
                case TransportType.Car:
                    return "Автомобиль";
                case TransportType.Ship:
                    return "Корабль";
                case TransportType.Helicopter:
                    return "Вертолёт";
                default:
                    return transport.ToString();
            }
        }
        /// <summary>
        /// Возвращает звёзды отеля в виде строки
        /// </summary>
        private string GetStars(int stars)
        {
            return new string('⭐', stars);
        }
        /// <summary>
        /// TryParse для возраста 
        /// </summary>
        private bool TryParseAge(string input, out int age)
        {
            if (int.TryParse(input, out age))
            {
                if (age >= 0 && age <= 120)
                {
                    return true;
                }
            }
            age = 0;
            return false;
        }
        /// <summary>
        /// Инициализация дерева (текст из ресурсов)
        /// </summary>
        private void InitializeTreeView()
        {
            treeViewEntities.Nodes.Clear();
            var nodeTours = new TreeNode(Resources.LabelTours) { Tag = "Tours" };
            var nodeTourists = new TreeNode(Resources.LabelTourists) { Tag = "Tourists" };
            var nodeHotels = new TreeNode(Resources.LabelHotels) { Tag = "Hotels" };
            treeViewEntities.Nodes.Add(nodeTours);
            treeViewEntities.Nodes.Add(nodeTourists);
            treeViewEntities.Nodes.Add(nodeHotels);
            treeViewEntities.ExpandAll();
        }
        /// <summary>
        /// Одна кнопка для загрузки И XML, И JSON
        /// </summary>
        private void btnLoad_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                // ресурсы для фильтра
                openFileDialog.Filter = $"{Resources.BtnLoadJson}|*.json|{Resources.BtnLoadXml}|*.xml|Все файлы|*.*";
                openFileDialog.Title = Resources.MsgSelectFile; 
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var filePath = openFileDialog.FileName;
                    var extension = Path.GetExtension(filePath).ToLower();
                    switch (extension)
                    {
                        case ".json":
                            LoadAllFromJson();
                            break;
                        case ".xml":
                            LoadAllFromXml();
                            break;
                        default:
                            MessageBox.Show(Resources.MsgUnsupportedFormat, Resources.MsgError,
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// Кнопка "Показать" — отображает данные выбранной сущности
        /// </summary>
        private void btnShow_Click(object sender, EventArgs e)
        {
            if (treeViewEntities.SelectedNode == null)
            {
                MessageBox.Show(Resources.MsgChooseEntity, Resources.MsgInfo,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var selectedText = treeViewEntities.SelectedNode.Text;
            switch (selectedText)
            {
                case var tours when tours == Resources.LabelTours:
                    DisplayToursInGrid();
                    break;
                case var tourists when tourists == Resources.LabelTourists:
                    DisplayTouristsInGrid();
                    break;
                case var hotels when hotels == Resources.LabelHotels:
                    DisplayHotelsInGrid();
                    break;
                default:
                    MessageBox.Show(Resources.MsgChooseEntity, Resources.MsgInfo,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }
        }
        /// <summary>
        /// Кнопка "Выход"
        /// </summary>
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        /// <summary>
        /// Отображение туров в DataGridViewData
        /// </summary>
        private void DisplayToursInGrid()
        {
            dataGridViewData.Rows.Clear(); 
            dataGridViewData.Columns.Clear(); 
            if (_tours == null || _tours.Count == 0)
            {
                MessageBox.Show(Resources.MsgNoData, Resources.MsgInfo,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            dataGridViewData.Columns.Add("colTitle", "Название тура");
            dataGridViewData.Columns.Add("colPrice", "Цена");
            dataGridViewData.Columns.Add("colDays", "Дней");
            dataGridViewData.Columns.Add("colCountry", "Страна");
            dataGridViewData.Columns.Add("colGuide", "Гид");
            foreach (var tour in _tours)
            {
                var rowIndex = dataGridViewData.Rows.Add();
                var row = dataGridViewData.Rows[rowIndex];
                row.Cells["colTitle"].Value = tour.Title;
                row.Cells["colPrice"].Value = $"{tour.Price:N0} {tour.CountryInfo.Valuta}";
                row.Cells["colDays"].Value = tour.DurationDays;
                row.Cells["colCountry"].Value = tour.CountryInfo.Name;
                row.Cells["colGuide"].Value = $"{tour.GuideInfo.FullName} ({tour.GuideInfo.Rating})";
                row.Tag = tour;
            }
        }
        /// <summary>
        /// Отображение туристов в DataGridViewData
        /// </summary>
        private void DisplayTouristsInGrid()
        {
            dataGridViewData.Rows.Clear();
            dataGridViewData.Columns.Clear();
            if (_tourists == null || _tourists.Count == 0)
            {
                MessageBox.Show(Resources.MsgNoData, Resources.MsgInfo,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            dataGridViewData.Columns.Add("colName", "ФИО");
            dataGridViewData.Columns.Add("colAge", "Возраст");
            dataGridViewData.Columns.Add("colPassport", "Паспорт");
            dataGridViewData.Columns.Add("colFood", "Питание");
            dataGridViewData.Columns.Add("colTransport", "Транспорт");
            foreach (var tourist in _tourists)
            {
                var rowIndex = dataGridViewData.Rows.Add();
                var row = dataGridViewData.Rows[rowIndex];

                row.Cells["colName"].Value = tourist.FullName;
                row.Cells["colAge"].Value = tourist.Age;
                row.Cells["colPassport"].Value = $"{tourist.PassportData.Series}-{tourist.PassportData.Number}";
                row.Cells["colFood"].Value = GetFoodTypeName(tourist.TourPreferences.Food);
                row.Cells["colTransport"].Value = GetTransportTypeName(tourist.TourPreferences.Transport);
                row.Tag = tourist;
            }
        }
        /// <summary>
        /// Отображение отелей в DataGridViewData
        /// </summary>
        private void DisplayHotelsInGrid()
        {
            dataGridViewData.Rows.Clear();
            dataGridViewData.Columns.Clear();
            if (_hotels == null || _hotels.Count == 0)
            {
                MessageBox.Show(Resources.MsgNoData, Resources.MsgInfo,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            dataGridViewData.Columns.Add("colName", "Название");
            dataGridViewData.Columns.Add("colStars", "Звёзды");
            dataGridViewData.Columns.Add("colPrice", "Цена/ночь");
            dataGridViewData.Columns.Add("colCity", "Город");
            dataGridViewData.Columns.Add("colContact", "Контакты");
            foreach (var hotel in _hotels)
            {
                var rowIndex = dataGridViewData.Rows.Add();
                var row = dataGridViewData.Rows[rowIndex];

                row.Cells["colName"].Value = hotel.Name;
                row.Cells["colStars"].Value = GetStars(hotel.Stars);
                row.Cells["colPrice"].Value = $"{hotel.PriceForNight:N0}";
                row.Cells["colCity"].Value = hotel.AddressInfo.City;
                row.Cells["colContact"].Value = hotel.ContactInfo.Phone;
                row.Tag = hotel;
            }
        }
        /// <summary>
        /// Двойное касание по строке, для открытия второй формы
        /// </summary>
        private void dataGridViewData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // проверяю, что кликнули по строке (не по заголовку)
            if (e.RowIndex < 0) return;
            // получаю объект из Tag строки
            var selectedRow = dataGridViewData.Rows[e.RowIndex];
            var dataObject = selectedRow.Tag;
            // создаю и показываю вторую форму
            var detailsForm = new DetailsForm();
            // определяю тип объекта и вызываю нужный метод
            switch (dataObject)
            {
                case Tour tour:
                    detailsForm.ShowTourDetails(tour);
                    break;
                case Tourist tourist:
                    detailsForm.ShowTouristDetails(tourist);
                    break;
                case Hotel hotel:
                    detailsForm.ShowHotelDetails(hotel);
                    break;
            }
            detailsForm.ShowDialog();
        }
    }
}