using System.Collections.Generic;
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
            throw new System.NotImplementedException();
        }

        public Task<OperationResult> DeleteNace(long Id)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<NaceViewModel>> GetAllNaces()
        {
            throw new System.NotImplementedException();
        }

        public Task<NaceViewModel> GetSingleNaces(long Id)
        {
            throw new System.NotImplementedException();
        }
    }
}