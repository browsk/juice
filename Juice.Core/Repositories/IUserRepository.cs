using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Juice.Core.Domain;

namespace Juice.DataAccess.Domain
{
    public interface IUserRepository
    {
        void Add(User user);
        void Update(User user);
        void Remove(User user);
        User GetById(int id);
    }
}
