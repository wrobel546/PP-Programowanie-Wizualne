using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using NowakowskaWrobel.Smartphones.BLC;
using NowakowskaWrobel.Smartphones.CORE;
using NowakowskaWrobel.Smartphones.INTERFACES;

namespace NowakowskaWrobel.Smartphones.UI.Desktop
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly CatalogService _catalogService;
        private readonly EntityFactory _entityFactory;

        public ObservableCollection<ISmartphone> Smartphones { get; } = new ObservableCollection<ISmartphone>();
        public ObservableCollection<IProducer> Producers { get; } = new ObservableCollection<IProducer>();

        private ISmartphone? _selectedSmartphone;
        public ISmartphone? SelectedSmartphone
        {
            get => _selectedSmartphone;
            set
            {
                if (!Equals(_selectedSmartphone, value))
                {
                    _selectedSmartphone = value;
                    OnPropertyChanged();
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        private IProducer? _selectedProducer;
        public IProducer? SelectedProducer
        {
            get => _selectedProducer;
            set
            {
                if (!Equals(_selectedProducer, value))
                {
                    _selectedProducer = value;
                    OnPropertyChanged();
                }
            }
        }

        // ====== FILTRY ======
        private string? _filterText;
        public string? FilterText
        {
            get => _filterText;
            set
            {
                if (_filterText != value)
                {
                    _filterText = value;
                    OnPropertyChanged();
                    RefreshSmartphones();
                }
            }
        }

        private SmartphoneOs? _filterOperatingSystem;
        public SmartphoneOs? FilterOperatingSystem
        {
            get => _filterOperatingSystem;
            set
            {
                if (_filterOperatingSystem != value)
                {
                    _filterOperatingSystem = value;
                    OnPropertyChanged();
                    RefreshSmartphones();
                }
            }
        }

        private IProducer? _filterProducer;
        public IProducer? FilterProducer
        {
            get => _filterProducer;
            set
            {
                if (_filterProducer != value)
                {
                    _filterProducer = value;
                    OnPropertyChanged();
                    RefreshSmartphones();
                }
            }
        }

        private string? _minPriceText;
        public string? MinPriceText
        {
            get => _minPriceText;
            set
            {
                if (_minPriceText != value)
                {
                    _minPriceText = value;
                    OnPropertyChanged();
                    RefreshSmartphones();
                }
            }
        }

        private string? _maxPriceText;
        public string? MaxPriceText
        {
            get => _maxPriceText;
            set
            {
                if (_maxPriceText != value)
                {
                    _maxPriceText = value;
                    OnPropertyChanged();
                    RefreshSmartphones();
                }
            }
        }

        // COMMANDS
        public ICommand AddSmartphoneCommand { get; }
        public ICommand DeleteSmartphoneCommand { get; }
        public ICommand SaveSmartphoneCommand { get; }
        public ICommand AddProducerCommand { get; }
        public ICommand SaveProducerCommand { get; }
        public ICommand DeleteProducerCommand { get; }
        public ICommand ClearFiltersCommand { get; }

        public MainViewModel(CatalogService catalogService, EntityFactory entityFactory)
        {
            _catalogService = catalogService;
            _entityFactory = entityFactory;

            AddSmartphoneCommand = new RelayCommand(_ => AddSmartphone());
            DeleteSmartphoneCommand = new RelayCommand(_ => DeleteSmartphone(), _ => SelectedSmartphone != null);
            SaveSmartphoneCommand = new RelayCommand(_ => SaveSmartphone(), _ => SelectedSmartphone != null);

            AddProducerCommand = new RelayCommand(_ => AddProducer());
            SaveProducerCommand = new RelayCommand(_ => SaveProducer(), _ => SelectedProducer != null);
            DeleteProducerCommand = new RelayCommand(_ => DeleteProducer(), _ => SelectedProducer != null);

            ClearFiltersCommand = new RelayCommand(_ => ClearFilters());

            RefreshProducers();
            RefreshSmartphones();
        }

        // PRODUCENCI

        private void RefreshProducers()
        {
            Producers.Clear();
            foreach (var p in _catalogService.GetProducers())
            {
                Producers.Add(p);
            }

            if (!Producers.Contains(SelectedProducer))
            {
                SelectedProducer = Producers.FirstOrDefault();
            }
        }

        private void AddProducer()
        {
            var newProducer = _entityFactory.CreateProducer();
            newProducer.Name = "Nowy producent";
            newProducer.Country = "Kraj";

            _catalogService.AddProducer(newProducer);
            RefreshProducers();
            RefreshSmartphones();
            SelectedProducer = newProducer;
        }

        private void SaveProducer()
        {
            if (SelectedProducer == null)
            {
                return;
            }

            if (!ValidateProducer(SelectedProducer, out var error))
            {
                MessageBox.Show(error, "Błędy producenta", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _catalogService.UpdateProducer(SelectedProducer);
            RefreshProducers();
        }

        private void DeleteProducer()
        {
            if (SelectedProducer == null)
            {
                return;
            }

            _catalogService.DeleteProducer(SelectedProducer.Id);
            RefreshProducers();
            RefreshSmartphones();
        }

        // SMARTFONY

        private void RefreshSmartphones()
        {
            Smartphones.Clear();

            var minPrice = ParsePrice(MinPriceText);
            var maxPrice = ParsePrice(MaxPriceText);

            var items = _catalogService.FilterSmartphones(
                _filterText,
                _filterOperatingSystem,
                _filterProducer?.Id,
                minPrice,
                maxPrice);

            var producerById = Producers.ToDictionary(p => p.Id);
            foreach (var s in items)
            {
                if (s.Producer != null && producerById.TryGetValue(s.Producer.Id, out var existing))
                {
                    s.Producer = existing;
                }
                Smartphones.Add(s);
            }

            if (!Smartphones.Contains(SelectedSmartphone))
            {
                SelectedSmartphone = Smartphones.FirstOrDefault();
            }
        }

        private void AddSmartphone()
        {
            var producer = SelectedProducer ?? Producers.FirstOrDefault();

            var newSmartphone = _entityFactory.CreateSmartphone();
            newSmartphone.ModelName = "Nowy model";
            newSmartphone.OperatingSystem = SmartphoneOs.Android;
            newSmartphone.Price = 0;
            newSmartphone.ScreenSize = 6.0;
            newSmartphone.RamGb = 4;
            newSmartphone.StorageGb = 64;
            newSmartphone.Producer = producer;

            _catalogService.AddSmartphone(newSmartphone);
            RefreshSmartphones();
            SelectedSmartphone = newSmartphone;
        }

        private void DeleteSmartphone()
        {
            if (SelectedSmartphone == null)
            {
                return;
            }

            _catalogService.DeleteSmartphone(SelectedSmartphone.Id);
            RefreshSmartphones();
        }

        private void SaveSmartphone()
        {
            if (SelectedSmartphone == null)
            {
                return;
            }

            if (!ValidateSmartphone(SelectedSmartphone, out var error))
            {
                MessageBox.Show(error, "Błędy danych", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _catalogService.UpdateSmartphone(SelectedSmartphone);
            RefreshSmartphones();
        }

        private static decimal? ParsePrice(string? text)
        {
            if (decimal.TryParse(text, out var value))
            {
                return value;
            }
            return null;
        }

        private static bool ValidateSmartphone(ISmartphone smartphone, out string error)
        {
            var messages = new System.Collections.Generic.List<string>();

            if (string.IsNullOrWhiteSpace(smartphone.ModelName))
            {
                messages.Add("Model nie może być pusty.");
            }

            if (smartphone.Price < 0)
            {
                messages.Add("Cena nie może być ujemna.");
            }

            if (smartphone.ScreenSize <= 0)
            {
                messages.Add("Ekran musi mieć dodatnią przekątną.");
            }

            if (smartphone.RamGb <= 0)
            {
                messages.Add("RAM musi być większy od zera.");
            }

            if (smartphone.StorageGb <= 0)
            {
                messages.Add("Pamięć musi być większa od zera.");
            }

            if (smartphone.Producer == null)
            {
                messages.Add("Wybierz producenta.");
            }

            error = string.Join("\n", messages);
            return !messages.Any();
        }

        private static bool ValidateProducer(IProducer producer, out string error)
        {
            var messages = new System.Collections.Generic.List<string>();

            if (string.IsNullOrWhiteSpace(producer.Name) || producer.Name.Length < 2)
            {
                messages.Add("Nazwa producenta musi mieć co najmniej 2 znaki.");
            }

            if (string.IsNullOrWhiteSpace(producer.Country))
            {
                messages.Add("Podaj kraj pochodzenia.");
            }

            error = string.Join("\n", messages);
            return !messages.Any();
        }

        private void ClearFilters()
        {
            FilterText = null;
            FilterOperatingSystem = null;
            FilterProducer = null;
            MinPriceText = null;
            MaxPriceText = null;
            RefreshSmartphones();
        }

        public Array OperatingSystems { get; } = Enum.GetValues(typeof(SmartphoneOs));

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
