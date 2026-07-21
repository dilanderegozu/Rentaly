using Microsoft.EntityFrameworkCore;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Concrete;
using Rentaly.DataAccessLayer.RepositoryDesignPattern;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.DataAccessLayer.EntityFramework
{
    public class EfBranchDal : GenericRepository<Branch>, IBranchDal
    {
        public EfBranchDal(RentalyContext context) : base(context)
        {
        }

        public async Task<List<Branch>> TGetListAsync()
        {
            return await _context.Branches
                .Include(b => b.Cars)
                .ToListAsync();
        }
    }
}