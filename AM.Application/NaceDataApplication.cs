using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _0_Framework.Application;
using AM.Application.Contracts.Nace;
using AM.Application.Contracts.NaceData;
using AM.Domain.NaceAggregate;

namespace AM.Application
{
    public class NaceDataApplication : INaceDataApplication
    {
        private readonly INaceDataRepository _naceDataRepository;


        public NaceDataApplication(INaceDataRepository naceDataRepository)
        {
            _naceDataRepository = naceDataRepository;
        }

        public Task<OperationResult> CreateNaceData(NaceDataDTO Command)
        {
            var result = new OperationResult();
            var naceDataList = new List<NaceDetailData>();
            for (var counter = 0; counter < Command.ItemdetailIndex.Count + Command.SelectItemDetails.Count; counter++)
            {
                if (counter < Command.ItemdetailIndex.Count)
                    naceDataList.Add(
                        new NaceDetailData(Command.
                            ItemdetailIndex[counter], Command.ItemdetailValues[counter]));
                if (counter >= Command.ItemdetailIndex.Count)
                {
                    naceDataList.Add(
                        new NaceDetailData(Command
                            .SelectItemDetails[counter - Command.ItemdetailIndex.Count], ""));
                }
            }

            var naceData = new NaceData(naceDataList, Command.ListingId, Command.NaceId);
            _naceDataRepository.Create(naceData);
            return Task.FromResult(result.Succeeded());

        }

        public Task<OperationResult> EditNaceData(NaceDataDTO Command)
        {
            var result = new OperationResult();
            var naceDataList = new List<NaceDetailData>();
            for (var counter = 0; counter < Command.ItemdetailIndex.Count + Command.SelectItemDetails.Count; counter++)
            {
                if (counter < Command.ItemdetailIndex.Count)
                    naceDataList.Add(
                        new NaceDetailData(Command.
                            ItemdetailIndex[counter], Command.ItemdetailValues[counter]));
                if (counter >= Command.ItemdetailIndex.Count)
                {
                    naceDataList.Add(
                        new NaceDetailData(Command
                            .SelectItemDetails[counter - Command.ItemdetailIndex.Count], ""));
                }
            }

            var naceData = new NaceData(naceDataList, Command.ListingId, Command.NaceId);
            _naceDataRepository.Create(naceData);
            return Task.FromResult(result.Succeeded());
        }

        public NaceDataViewModel GetNaceData(long ListingId)
        {
            if (_naceDataRepository.Exist(x => x.ListingId == ListingId))
                return _naceDataRepository.GetNaceData(ListingId);
            return new NaceDataViewModel();
        }
    }
}