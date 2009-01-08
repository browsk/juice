using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Juice.Core.Domain;
using NHibernate;
using NHibernate.Cfg;

namespace Juice.DataAccess.Repositories
{
    public class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;
        private readonly static object _lock = new object();

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    lock(_lock)
                    {
                        if (_sessionFactory == null)
                        {
                            var configuration = new Configuration();
                            configuration.Configure();
                            configuration.AddAssembly(typeof(User).Assembly);

                            _sessionFactory = configuration.BuildSessionFactory();

                        }
                    }
                }

                return _sessionFactory;
            }
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}
