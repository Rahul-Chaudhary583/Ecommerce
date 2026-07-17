using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Infrastructure.Settings
{
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string ProductCollectionName { get; set; }
        public string BrandCollectionName { get; set; }
        public string TypeCollectionName { get; set; }
    }
}
