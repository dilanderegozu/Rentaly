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
    public class EfCarModelDal : GenericRepository<CarModel>, ICarModel
    {
        public EfCarModelDal(RentalyContext context) : base(context)
        {
        }
        public async Task<List<CarModel>> TGetListAsync()
        {
            return await _context.CarModels
                .Include(m => m.Brand)
                .Include(m => m.Cars)
                    .ThenInclude(c => c.Category)
                .ToListAsync();
        }
    }
}
