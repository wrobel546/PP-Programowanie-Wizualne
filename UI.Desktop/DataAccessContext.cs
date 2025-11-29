using NowakowskaWrobel.Smartphones.INTERFACES;

namespace NowakowskaWrobel.Smartphones.UI.Desktop
{
    public class DataAccessContext
    {
        public DataAccessContext(
            DataAccessConfig config,
            IProducerRepository producerRepository,
            ISmartphoneRepository smartphoneRepository,
            EntityFactory entityFactory)
        {
            Config = config;
            ProducerRepository = producerRepository;
            SmartphoneRepository = smartphoneRepository;
            EntityFactory = entityFactory;
        }

        public DataAccessConfig Config { get; }
        public IProducerRepository ProducerRepository { get; }
        public ISmartphoneRepository SmartphoneRepository { get; }
        public EntityFactory EntityFactory { get; }
    }
}
