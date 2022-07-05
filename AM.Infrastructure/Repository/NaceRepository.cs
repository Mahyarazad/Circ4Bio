using System;
using System.Collections.Generic;
using System.Linq;
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
                    CreationTime = x.CreationTime,
                    Title = x.Title,
                    NaceId = x.Id,
                    IsDeleted = x.IsDeleted,
                    Items = x.Items.Select(y => new DetailViewModel(y.NaceDetailId, y.DetailBody, y.IsDeleted,
                            y.ListItems.Select(z =>
                                new ListItemsViewModel(z.ListItemDetailId, z.IsDeleted, z.ListItemDetail)).ToList()))
                        .ToList()
                }).Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.CreationTime)
                .ToList();
        }

        public List<NaceViewModel> GetAllNaceTitles()
        {
            return
                _amContext.Naces.AsSingleQuery().Where(x => !x.IsDeleted)
                    .Select(x => new NaceViewModel
                    {
                        Title = x.Title,
                        NaceId = x.Id,
                        Items = new List<DetailViewModel>()
                    }).ToList();
        }

        public NaceViewModel GetSingleNace(long Id)
        {
            return
                _amContext.Naces.AsSingleQuery().Where(x => x.Id == Id).Select(x => new NaceViewModel
                {
                    Title = x.Title,
                    NaceId = x.Id,
                    IsDeleted = x.IsDeleted,
                    Items = x.Items.Where(x => !x.IsDeleted).Select(y =>
                              new DetailViewModel(y.NaceDetailId, y.DetailBody, y.IsDeleted,
                                  y.ListItems.Where(x => !x.IsDeleted).Select(z =>
                                      new ListItemsViewModel(z.ListItemDetailId, z.IsDeleted, z.ListItemDetail)).ToList()))
                .ToList()
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

        public void DeleteIndexDetail(long naceId, int indexId, int indexDetailId)
        {
            _amContext.Naces.AsSingleQuery()
                .First(x => x.Id == naceId)
                .Items.First(x => x.NaceDetailId == indexId)
                .ListItems.First(x => x.ListItemDetailId == indexDetailId).DeleteNaceListDetail();
        }

        public void EditIndexDetail(long naceId, int indexId, int indexDetailId, string indexDetailString)
        {
            _amContext.Naces.AsSingleQuery()
                .First(x => x.Id == naceId)
                .Items.First(x => x.NaceDetailId == indexId)
                .ListItems.First(x => x.ListItemDetailId == indexDetailId).Edit(indexDetailString);
        }


        public void AddIndexDetail(long naceId, string indexDetailString)
        {
            throw new NotImplementedException();
        }

        void INaceRepository.AddIndexDetail(long naceId, int indexDetailId, string indexDetailString)
        {
            _amContext.Naces.AsSingleQuery()
                .First(x => x.Id == naceId)
                .Items.First(x => x.NaceDetailId == indexDetailId)
                .AddListItem(new ListItems(indexDetailString));
        }

        void INaceRepository.AddIndex(long naceId, string indexString)
        {
            _amContext.Naces.AsSingleQuery()
                .First(x => x.Id == naceId)
                .AddDetail(new Detail(indexString, new List<ListItems>()));
        }

        public int LastInputId(long naceId)
        {
            return _amContext.Naces.AsSingleQuery()
                .First(x => x.Id == naceId).Items.Last().NaceDetailId;
        }
    }
}
