using System;
using NowakowskaWrobel.Smartphones.INTERFACES;

namespace NowakowskaWrobel.Smartphones.UI.Web
{
    public class EntityFactory
    {
        private readonly Type _producerType;
        private readonly Type _smartphoneType;

        public EntityFactory(Type producerType, Type smartphoneType)
        {
            _producerType = producerType;
            _smartphoneType = smartphoneType;
        }

        public IProducer CreateProducer()
        {
            return (IProducer)Activator.CreateInstance(_producerType)!;
        }

        public ISmartphone CreateSmartphone()
        {
            return (ISmartphone)Activator.CreateInstance(_smartphoneType)!;
        }
    }
}
