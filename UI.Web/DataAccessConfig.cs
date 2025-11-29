namespace NowakowskaWrobel.Smartphones.UI.Web
{
    public class DataAccessConfig
    {
        public string AssemblyName { get; set; } = "DAO.SQL";
        public string ProducerRepositoryType { get; set; } = "NowakowskaWrobel.Smartphones.DAO.SQL.SqlProducerRepository";
        public string SmartphoneRepositoryType { get; set; } = "NowakowskaWrobel.Smartphones.DAO.SQL.SqlSmartphoneRepository";
        public string ProducerType { get; set; } = "NowakowskaWrobel.Smartphones.DAO.SQL.ProducerSqlDO";
        public string SmartphoneType { get; set; } = "NowakowskaWrobel.Smartphones.DAO.SQL.SmartphoneSqlDO";
    }
}
