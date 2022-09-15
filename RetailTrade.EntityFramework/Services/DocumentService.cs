using Microsoft.EntityFrameworkCore;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTrade.EntityFramework.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetailTrade.EntityFramework.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly RetailTradeDbContextFactory _contextFactory;
        private readonly NonQueryDataService<Document> _nonQueryDataService;

        public DocumentService(RetailTradeDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<Document>(_contextFactory);
        }

        public event Action<Document> OnEdited;
        public event Action<Document> OnCreated;

        public async Task<Document> CreateAsync(Document entity)
        {
            var result = await _nonQueryDataService.Create(entity);
            if (result != null)
            {
                OnCreated?.Invoke(result);
                return result;
            }
            return null;
        }

        public async Task<Document> CreateAsync(Document entity, DocumentTypeEnum documentType)
        {
            entity.Number = await GetNewNumber(documentType);
            entity.DocumentTypeId = (int)documentType;
            var result = await _nonQueryDataService.Create(entity);
            if (result != null)
            {
                switch (documentType)
                {
                    case DocumentTypeEnum.Supply:
                        break;
                    case DocumentTypeEnum.PurchaseReturn:
                        break;
                    case DocumentTypeEnum.Loss:
                        //OnLossCreated?.Invoke(await GetLossDocumentView(entity.Id));
                        break;
                    case DocumentTypeEnum.Enter:
                        //OnEnterCreated?.Invoke(await GetEnterDocumentView(entity.Id));
                        break;
                    case DocumentTypeEnum.Move:
                        //OnMoveCreated?.Invoke(await GetMoveDocumentView(entity.Id));
                        break;
                    case DocumentTypeEnum.Inventory:
                        break;
                    case DocumentTypeEnum.CashIn:
                        break;
                    case DocumentTypeEnum.CashOut:
                        break;
                    case DocumentTypeEnum.CashboxAdjustment:
                        break;
                    case DocumentTypeEnum.AccountAdjustment:
                        break;
                    case DocumentTypeEnum.RetailDemand:
                        break;
                    case DocumentTypeEnum.RetailSalesReturn:
                        break;
                    case DocumentTypeEnum.RetailShift:
                        break;
                    case DocumentTypeEnum.RetailDrawerCashIn:
                        break;
                    case DocumentTypeEnum.RetailDrawerCashOut:
                        break;
                }
                return result;
            }
            return null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await _nonQueryDataService.Delete(id);
            }
            catch (Exception)
            {
                //ignore
            }
            return false;
        }

        public IEnumerable<Document> GetAll()
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return context.Documents
                    .ToList();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<Document>> GetAllAsync()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Documents
                    .ToListAsync();
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<Document> GetAsync(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Documents
                    .FirstOrDefaultAsync((e) => e.Id == id);
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<Document> UpdateAsync(int id, Document entity)
        {
            try
            {
                var result = await _nonQueryDataService.Update(id, entity);
                return result;
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<string> GetNewNumber(DocumentTypeEnum documentType)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                int count = await context.Documents
                    .CountAsync(d => d.DocumentTypeId == (int)documentType) + 1;
                return string.Format("{0:d5}", count);
            }
            catch (Exception)
            {
                //ignore
            }
            return string.Empty;
        }

        public async Task<bool> CheckNumber(string number, DocumentTypeEnum documentType)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return context.Documents.FirstOrDefaultAsync(d => d.DocumentTypeId == (int)documentType && d.Number == number) != null;
            }
            catch (Exception)
            {
                //ignore
            }
            return false;
        }

        public async Task<Document> GetDocumentByIncludeAsync(int id, DocumentTypeEnum documentType)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Documents
                    .Include(d => d.DocumentProducts)
                    .FirstOrDefaultAsync(d => d.DocumentTypeId == (int)documentType);
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }
    }
}
