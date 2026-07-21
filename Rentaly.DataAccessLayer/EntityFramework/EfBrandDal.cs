using Microsoft.EntityFrameworkCore;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Concrete;
using Rentaly.DataAccessLayer.RepositoryDesignPattern;
using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.DataAccessLayer.EntityFramework
{
    public class EfBrandDal : GenericRepository<Brand>, IBrandDal
    {
        public EfBrandDal(RentalyContext context) : base(context)
        {

        }
  
        public async Task<List<Brand>> TGetListAsync()
        {
            return await _context.Brands
                .Include(b => b.Cars)
                .ToListAsync();
        }
    }
}
