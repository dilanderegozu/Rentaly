using Microsoft.EntityFrameworkCore;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Concrete;
using Rentaly.DataAccessLayer.RepositoryDesignPattern;
using Rentaly.EntityLayer.Entities;

public class EfCarDal : GenericRepository<Car>, ICarDal
{
    public EfCarDal(RentalyContext context) : base(context)
    {
    }

    public List<Car> GetCarsByCategoryId(int id)
    {
        return _context.Cars
            .Where(x => x.CategoryId == id)
            .Include(x => x.Category)
            .ToList();
    }

    public async Task<List<Car>> GetAllCarWithCategoryAsync()
    {
        return await _context.Cars
            .Include(x => x.Category)
            .ToListAsync();
    }

    public async Task<List<Car>> TGetListAsync()
    {
        return await _context.Cars
            .Include(c => c.Brand)
            .Include(c => c.CarModel)
            .Include(c => c.Category)
            .Include(c => c.VehicleType)
            .Include(c=>c.Branch)
            .ToListAsync();
    }

    public async Task<Car> GetCarWithDetailsByIdAsync(int id)
    {
        return await _context.Cars
            .Include(c => c.Brand)
            .Include(c => c.CarModel)
            .Include(c => c.Category)
            .Include(c => c.VehicleType)
             .Include(c => c.Branch)
              .Include(c => c.Images)
            .FirstOrDefaultAsync(c => c.CarId == id);
    }
}