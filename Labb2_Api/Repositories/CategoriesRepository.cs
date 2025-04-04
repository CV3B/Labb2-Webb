using Labb2_Api.Data;
using Labb2_Shared.Model;

namespace Labb2_Api.Repositories;

public class CategoriesRepository(AppDbContext context) : Repository<Category>(context), ICategoriesRepository
{
    private readonly AppDbContext context = context;
}