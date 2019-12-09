using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BethanysPieShop.Models
{
    public class PieRepository : IPieRepository
    {
        private readonly AppDbContext _appDbContext;

        public PieRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Pie> AllPies => _appDbContext.Pies.Include(p => p.Category);

        public IEnumerable<Pie> PiesOfTheWeek => AllPies.Where(p => p.IsPieOfTheWeek);

        public Pie GetById(int pieId) => AllPies.FirstOrDefault(p => p.PieId == pieId);
    }
}
