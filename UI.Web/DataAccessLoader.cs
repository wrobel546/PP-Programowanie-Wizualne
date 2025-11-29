using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using NowakowskaWrobel.Smartphones.INTERFACES;

namespace NowakowskaWrobel.Smartphones.UI.Web
{
    public static class DataAccessLoader
    {
        private const string ConfigFileName = "appsettings.json";

        public static DataAccessContext Load()
        {
            var config = LoadConfig();

            // prosta wersja: ładujemy dll z folderu aplikacji i tworzymy instancje po nazwie typu
            var dllPath = Path.Combine(AppContext.BaseDirectory, $"{config.AssemblyName}.dll");
            var assembly = Assembly.LoadFrom(dllPath);

            var producerRepository = (IProducerRepository)Activator.CreateInstance(
                assembly.GetType(config.ProducerRepositoryType, throwOnError: true)!);
            var smartphoneRepository = (ISmartphoneRepository)Activator.CreateInstance(
                assembly.GetType(config.SmartphoneRepositoryType, throwOnError: true)!,
                producerRepository)!;

            var producerType = assembly.GetType(config.ProducerType, throwOnError: true)!;
            var smartphoneType = assembly.GetType(config.SmartphoneType, throwOnError: true)!;

            var factory = new EntityFactory(producerType, smartphoneType);

            return new DataAccessContext(config, producerRepository, smartphoneRepository, factory);
        }

        private static DataAccessConfig LoadConfig()
        {
            var fallback = new DataAccessConfig();
            var configPath = Path.Combine(AppContext.BaseDirectory, ConfigFileName);
            if (!File.Exists(configPath))
            {
                return fallback;
            }

            try
            {
                var json = File.ReadAllText(configPath);
                var root = JsonSerializer.Deserialize<ConfigRoot>(json);
                return root?.DataAccess ?? fallback;
            }
            catch
            {
                return fallback;
            }
        }

        private class ConfigRoot
        {
            public DataAccessConfig? DataAccess { get; set; }
        }
    }
}
