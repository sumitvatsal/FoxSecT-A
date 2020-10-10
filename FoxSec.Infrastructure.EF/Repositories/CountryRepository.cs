using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects ;
using System.Linq;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Database;

namespace FoxSec.Infrastructure.EF.Repositories
{
    internal class CountryRepository : LookupTitleRepositoryBase<Country>, ICountryRepository
    {
        public CountryRepository(IDatabaseFactory factory) : base(factory) {}

        protected override IQueryable<Country> All()
        {
            return (base.All() as ObjectSet<Country>).Where(x => !x.IsDeleted);
        }
    }
}