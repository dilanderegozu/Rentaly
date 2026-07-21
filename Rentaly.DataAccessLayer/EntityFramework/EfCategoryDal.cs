using Microsoft.EntityFrameworkCore;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Concrete;
using Rentaly.DataAccessLayer.RepositoryDesignPattern;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.DataAccessLayer.EntityFramework
{
    public class EfCategoryDal : GenericRepository<Category>, ICategoryDal
    {
        public EfCategoryDal(RentalyContext context) : base(context)
        {
        }

        public async Task<List<Category>> TGetListAsync()
        {
            return await _context.Categories
                .Include(c => c.Cars)
                .ToListAsync();
        }
    }
}