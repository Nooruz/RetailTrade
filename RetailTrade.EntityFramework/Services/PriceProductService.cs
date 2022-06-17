//using Microsoft.EntityFrameworkCore;
//using RetailTrade.Domain.Models;
//using RetailTrade.Domain.Services;
//using RetailTrade.EntityFramework.Services.Common;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace RetailTrade.EntityFramework.Services
//{
//    public class PriceProductService : IPriceProductService
//    {
//        private readonly RetailTradeDbContextFactory _contextFactory;
//        private readonly NonQueryDataService<PriceProduct> _nonQueryDataService;

//        public PriceProductService(RetailTradeDbContextFactory contextFactory)
//        {
//            _contextFactory = contextFactory;
//            _nonQueryDataService = new NonQueryDataService<PriceProduct>(_contextFactory);
//        }

//        public event Action<PriceProduct> OnCreated;
//        public event Action<PriceProduct> OnEdited;

//        public async Task<PriceProduct> CreateAsync(PriceProduct entity)
//        {
//            await using var context = _contextFactory.CreateDbContext();
//            PriceProduct result = await _nonQueryDataService.Create(entity);
//            if (result != null)
//            {
//                OnCreated?.Invoke(result);
//            }
//            return result;
//        }

//        public async Task<bool> DeleteAsync(int id)
//        {
//            return await _nonQueryDataService.Delete(id);
//        }

//        public IEnumerable<PriceProduct> GetAll()
//        {
//            try
//            {
//                using var context = _contextFactory.CreateDbContext();
//                return context.PriceProducts.ToList();
//            }
//            catch (Exception)
//            {
//                //ignore
//            }
//            return null;
//        }

//        public async Task<IEnumerable<PriceProduct>> GetAllAsync()
//        {
//            try
//            {
//                await using var context = _contextFactory.CreateDbContext();
//                return await context.PriceProducts.ToListAsync();
//            }
//            catch (Exception)
//            {
//                //ignore
//            }
//            return null;
//        }

//        public async Task<PriceProduct> GetAsync(int id)
//        {
//            try
//            {
//                await using var context = _contextFactory.CreateDbContext();
//                return await context.PriceProducts.FirstOrDefaultAsync(a => a.Id == id);
//            }
//            catch (Exception)
//            {
//                //ignore
//            }
//            return null;
//        }

//        public async Task<PriceProduct> UpdateAsync(int id, PriceProduct entity)
//        {
//            PriceProduct result = await _nonQueryDataService.Update(id, entity);
//            if (result != null)
//            {
//                OnEdited?.Invoke(result);
//            }
//            return result;
//        }
//    }
//}
