using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Juice.Core.Domain;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using Xunit;

namespace Juice.DataAccess.Tests
{
    public class GenerateSchema
    {
        [Fact]
        void Can_generate_schema()
        {
            var cfg = new Configuration();
            cfg.Configure();
            cfg.AddAssembly(typeof(User).Assembly);

            new SchemaExport(cfg).Execute(false, true, false, false);
        }
    }
}
