using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using _0_Framework;
using _0_Framework.Application;
using AM.Application.Contracts.Nace;
using AM.Domain.NaceAggregate;

namespace AM.Application
{
    public class NaceApplication : INaceApplication
    {
        private readonly INaceRepository _naceRepository;

        public NaceApplication(INaceRepository naceRepository)
        {
            _naceRepository = naceRepository;
        }

        public Task<OperationResult> CreateNace(CreateNace Command)
        {
            var result = new OperationResult();
            if (_naceRepository.Exist(x => x.Title == Command.Title))
                return Task.FromResult(result.Failed(ApplicationMessage.RecordExists));

            var counter = 0;
            var createNace = new Nace(Command.Title, new List<Detail>());
            foreach (var item in Command.Items)
            {
                createNace.Items.Add(new Detail(item.DetailBody, new List<ListItems>()));
                foreach (var listItem in item.ItemDetailList)
                {
                    createNace.Items[counter].ListItems.Add(new ListItems(listItem));
                }

                counter++;
            }
            _naceRepository.Create(createNace);
            _naceRepository.SaveChanges();
            return Task.FromResult(result.Succeeded());
        }

        public Task<OperationResult> EditNace(EditNace Command)
        {
            var result = new OperationResult();

            if (_naceRepository.Exist(x => x.Title == Command.Title & x.Id != Command.NaceId))
                return Task.FromResult(result.Failed(ApplicationMessage.RecordExists));
            var targetNace = _naceRepository.Get(Command.NaceId).Result;
            var newIndices = new List<int>();
            // In the first step we handle delete and modification then we start to add the new fields

            Command.EditIndices.ForEach(item =>
            {
                if (item.IndexId > 0)
                {
                    targetNace.Items
                        .First(x => x.NaceDetailId == item.IndexId)
                        .Edit(item.DetailBody);

                    if (item.IsDeleted)
                    {
                        targetNace.Items!
                            .First(x => x.NaceDetailId == item.IndexId).DeleteNaceDetail();
                    }

                    item.ItemDetailList.ForEach(itemDetail =>
                    {
                        if (itemDetail.RefId != 0)
                        {
                            if (itemDetail.IndexDetailId != 0)
                            {
                                _naceRepository
                                    .EditIndexDetail(Command.NaceId, item.IndexId,
                                        itemDetail.IndexDetailId, itemDetail.DetailString);
                                if (itemDetail.IsDeleted)
                                    _naceRepository
                                        .DeleteIndexDetail(Command.NaceId, item.IndexId, itemDetail.IndexDetailId);
                            }
                        }
                        if (itemDetail.IndexDetailId == 0)
                        {
                            _naceRepository
                                .AddIndexDetail(Command.NaceId, item.IndexId,
                                    itemDetail.DetailString);
                            _naceRepository.SaveChanges();
                        }
                    });
                }
            });

            targetNace.Edit(Command.Title);
            _naceRepository.SaveChanges();

            // Then we start off by adding indices and then index details
            Command.EditIndices.ForEach(item =>
            {
                if (item.IndexId < 0)
                {
                    _naceRepository.AddIndex(Command.NaceId, item.DetailBody);
                    _naceRepository.SaveChanges();
                    var indexId = _naceRepository.LastInputId(Command.NaceId);
                    newIndices.Add(indexId);
                    Command.EditIndices
                        .First(x => x.IndexId == item.IndexId).IndexId = indexId;
                }
            });

            if (newIndices.Count > 0)
                newIndices.ForEach(item =>
                {
                    var editIndexDetailItemsList = Command.EditIndices
                        .First(x => x.IndexId == item).ItemDetailList;
                    if (editIndexDetailItemsList != null)
                        editIndexDetailItemsList.ForEach(itemDetail =>
                            {
                                _naceRepository.AddIndexDetail(Command.NaceId, item, itemDetail.DetailString);
                            });
                });

            _naceRepository.SaveChanges();

            return Task.FromResult(result.Succeeded());
        }

        public Task<OperationResult> DeleteNace(long Id)
        {
            var result = new OperationResult();
            if (_naceRepository.Exist(x => x.Id == Id))
            {
                _naceRepository.Get(Id).Result.DeleteNace();
                _naceRepository.SaveChanges();
                return Task.FromResult(result.Succeeded());
            }
            return Task.FromResult(result.Failed(ApplicationMessage.RecordNotFound));
        }

        public Task<List<NaceViewModel>> GetAllNaces()
        {
            return Task.FromResult(_naceRepository.GetAllNaces());
        }

        public Task<NaceViewModel> GetSingleNace(long Id)
        {
            var result = new OperationResult();
            if (_naceRepository.Exist(x => x.Id == Id))
            {
                return Task.FromResult(_naceRepository.GetSingleNace(Id));
            }
            return Task.FromResult(new NaceViewModel());
        }

        public Task<EditNace> EditSingleNace(long Id)
        {
            var result = new OperationResult();
            if (_naceRepository.Exist(x => x.Id == Id))
            {
                return Task.FromResult(_naceRepository.EditSingleNace(Id));
            }
            return Task.FromResult(new EditNace());
        }

        Task<OperationResult> INaceApplication.UndeleteNace(long Id)
        {
            var result = new OperationResult();
            if (_naceRepository.Exist(x => x.Id == Id))
            {
                _naceRepository.Get(Id).Result.UndeleteNace();
                _naceRepository.SaveChanges();
                return Task.FromResult(result.Succeeded());
            }
            return Task.FromResult(result.Failed(ApplicationMessage.RecordNotFound));
        }
    }
}