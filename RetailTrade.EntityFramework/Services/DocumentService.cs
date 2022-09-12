using Microsoft.EntityFrameworkCore;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTrade.Domain.Views;
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
        public event Action<EnterDocumentView> OnEnterCreated;
        public event Action<EnterDocumentView> OnEnterUpdated;
        public event Action<LossDocumentView> OnLossCreated;
        public event Action<LossDocumentView> OnLossUpdated;
        public event Action<MoveDocumentView> OnMoveCreated;
        public event Action<MoveDocumentView> OnMoveUpdated;

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
                        OnLossCreated?.Invoke(await GetLossDocumentView(entity.Id));
                        break;
                    case DocumentTypeEnum.Enter:
                        OnEnterCreated?.Invoke(await GetEnterDocumentView(entity.Id));
                        break;
                    case DocumentTypeEnum.Move:
                        OnMoveCreated?.Invoke(await GetMoveDocumentView(entity.Id));
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

        public async Task<EnterDocumentView> GetEnterDocumentView(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.EnterDocumentViews
                    .FirstOrDefaultAsync(e => e.Id == id);
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<LossDocumentView> GetLossDocumentView(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.LossDocumentViews
                    .FirstOrDefaultAsync(e => e.Id == id);
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<MoveDocumentView> GetMoveDocumentView(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.MoveDocumentViews
                    .FirstOrDefaultAsync(e => e.Id == id);
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<EnterDocumentView>> GetEnterDocumentViews()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.EnterDocumentViews
                    .ToListAsync();
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<LossDocumentView>> GetLossDocumentViews()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.LossDocumentViews
                    .ToListAsync();
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<MoveDocumentView>> GetMoveDocumentViews()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.MoveDocumentViews
                    .ToListAsync();
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<Document> GetIncludeEnterProduct(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Documents
                    .Include(d => d.EnterProducts)
                    .FirstOrDefaultAsync((e) => e.Id == id);
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<Document> GetIncludeLossProduct(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Documents
                    .Include(d => d.LossProducts)
                    .FirstOrDefaultAsync((e) => e.Id == id);
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<Document> GetIncludeMoveProduct(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Documents
                    .Include(d => d.MoveProducts)
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
                switch ((DocumentTypeEnum)result.DocumentTypeId)
                {
                    case DocumentTypeEnum.Supply:
                        break;
                    case DocumentTypeEnum.PurchaseReturn:
                        break;
                    case DocumentTypeEnum.Loss:
                        OnLossUpdated?.Invoke(await GetLossDocumentView(result.Id));
                        break;
                    case DocumentTypeEnum.Enter:
                        OnEnterUpdated?.Invoke(await GetEnterDocumentView(result.Id));
                        break;
                    case DocumentTypeEnum.Move:
                        OnMoveUpdated?.Invoke(await GetMoveDocumentView(result.Id));
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
    }
}
