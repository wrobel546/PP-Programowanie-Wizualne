using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NowakowskaWrobel.Smartphones.INTERFACES;


namespace NowakowskaWrobel.Smartphones.DAO.SQL
{
    public class ProducerSqlDO : IProducer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public DateTime? Founded { get; set; }

        public ICollection<SmartphoneSqlDO> Smartphones { get; set; } = new List<SmartphoneSqlDO>();
    }
}
