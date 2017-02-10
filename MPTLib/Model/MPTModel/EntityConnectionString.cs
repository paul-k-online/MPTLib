using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;

namespace MPT.Model
{
    public partial class MPTEntities
    {
        private const string DefaultProvider = "System.Data.SqlClient";
        private const string DefaultProviderConnectionString = @"data source=192.168.100.220\wincc;initial catalog=MPT;persist security info=True;user id=RemoteWorkstation;password=RemoteWorkstation123;MultipleActiveResultSets=True;App=EntityFramework&quot;";
        private const string DefaultMetadate = "res://*/Model.MPTModel.csdl|res://*/Model.MPTModel.ssdl|res://*/Model.MPTModel.msl";

        public static EntityConnectionStringBuilder GetBuilder(string providerConnectionString = null, string provider = null)
        {
            var csb = new EntityConnectionStringBuilder();
            csb.Provider = string.IsNullOrWhiteSpace(provider) ? DefaultProvider : provider;
            csb.Metadata = DefaultMetadate;
            csb.ProviderConnectionString = string.IsNullOrWhiteSpace(providerConnectionString)
                ? DefaultProviderConnectionString
                : providerConnectionString;
            return csb;
        }

        public MPTEntities(EntityConnectionStringBuilder connectionString) : base(connectionString.ToString())
        { }
    }
}
