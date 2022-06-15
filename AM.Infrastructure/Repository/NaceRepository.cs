using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Infrastructure;
using AM.Application.Contracts.Nace;
using AM.Domain.NaceAggregate;
using Microsoft.EntityFrameworkCore;

namespace AM.Infrastructure.Repository
{
    public class NaceRepository : RepositoryBase<long, Nace>, INaceRepository
    {
        private readonly AMContext _amContext;
        public NaceRepository(AMContext amContext) : base(amContext)
        {
            _amContext = amContext;
        }

        public List<NaceViewModel> GetAllNaces()
        {
            return
                _amContext.Naces.AsSingleQuery().Select(x => new NaceViewModel
                {
                    Title = x.Title,
                    NaceId = x.Id,
                    IsDeleted = x.IsDeleted,
                    Items = x.Items.Select(y => new DetailViewModel(y.NaceDetailId, y.DetailBody, y.IsDeleted,
                        y.ListItems.Select(z => new ListItemsViewModel(z.ListItemDetailId, z.IsDeleted, z.ListItemDetail)).ToList())).ToList()
                }).ToList();
        }

        public NaceViewModel GetSingleNace(long Id)
        {
            return
                _amContext.Naces.Where(x => x.Id == Id).Select(x => new NaceViewModel
                {
                    Title = x.Title,
                    NaceId = x.Id,
                    IsDeleted = x.IsDeleted,
                    Items = x.Items.Select(y => new DetailViewModel(y.NaceDetailId, y.DetailBody, y.IsDeleted,
                        y.ListItems.Select(z => new ListItemsViewModel(z.ListItemDetailId, z.IsDeleted, z.ListItemDetail)).ToList())).ToList()
                }).First();
        }

        public EditNace EditSingleNace(long Id)
        {
            return
                _amContext.Naces.AsSingleQuery().Where(x => x.Id == Id).Select(x => new EditNace
                {
                    Title = x.Title,
                    NaceId = x.Id,
                    IsDeleted = x.IsDeleted,
                    EditIndices = x.Items.Select(y => new EditIndexDetail
                    {
                        IndexId = y.NaceDetailId,
                        DetailBody = y.DetailBody,
                        IsDeleted = y.IsDeleted,
                        ItemDetailList = y.ListItems.Select(z => new EditIndexDetailItems
                        {
                            IndexDetailId = z.ListItemDetailId,
                            RefId = y.NaceDetailId,
                            IsDeleted = z.IsDeleted,
                            DetailString = z.ListItemDetail
                        }).ToList()
                    }).ToList()
                }).First();
        }
    }
    public class NaceDataRepository : RepositoryBase<long, NaceData>, INaceDataRepository
    {
        private readonly AMContext _amContext;
        public NaceDataRepository(AMContext amContext) : base(amContext)
        {
            amContext = _amContext;
        }
    }
}
