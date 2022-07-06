﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _0_Framework;
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
            if (Command.NaceId != 0)
            {

                if (Command.SelectItemDetails != null && Command.ItemdetailIndex != null && Command.ItemdetailValues != null)
                {
                    for (var counter = 0;
                        counter < (Command.ItemdetailIndex.Count + Command.SelectItemDetails.Count);
                        counter++)
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
                }

                if (Command.SelectItemDetails != null && Command.ItemdetailIndex == null && Command.ItemdetailValues == null)
                {
                    for (var counter = 0;
                        counter < (Command.SelectItemDetails.Count);
                        counter++)
                    {
                        naceDataList.Add(
                                new NaceDetailData(Command
                                    .SelectItemDetails[counter], ""));
                    }
                }

                if (Command.SelectItemDetails == null && Command.ItemdetailIndex != null && Command.ItemdetailValues != null)
                {
                    for (var counter = 0;
                        counter < (Command.ItemdetailIndex.Count);
                        counter++)
                    {
                        naceDataList.Add(
                               new NaceDetailData(Command.
                                   ItemdetailIndex[counter], Command.ItemdetailValues[counter]));
                    }
                }


                var naceData = new NaceData(naceDataList, Command.ListingId, Command.NaceId);
                _naceDataRepository.Create(naceData);
                _naceDataRepository.SaveChanges();
                return Task.FromResult(result.Succeeded());
            }

            return Task.FromResult(result.Failed(ApplicationMessage.RecordNotFound));
        }

        public Task<OperationResult> EditNaceData(NaceDataDTO Command)
        {
            var result = new OperationResult();
            if (_naceDataRepository.Exist(x => x.Id == Command.Id))
            {
                var naceData = _naceDataRepository.Get(Command.Id).Result;
                if (Command.ItemdetailIndex != null && Command.ItemdetailValues != null && Command.SelectItemDetails != null)
                {
                    var selectListitem = naceData.NaceDetailDatas
                        .Where(x => x.NaceData == "").ToList();
                    for (var counter = 0; counter < Command.SelectItemDetails.Count; counter++)
                    {
                        selectListitem[counter].Edit(Command.SelectItemDetails[counter], "");
                    }

                    foreach (var stringValue in Command
                        .ItemdetailIndex.Select((value, index) => new { value, index }))
                    {
                        foreach (var item in naceData.NaceDetailDatas)
                        {
                            if (item.ItemId == stringValue.value)
                                item.Edit(item.ItemId, Command.ItemdetailValues[stringValue.index]);
                        }
                    }
                }


                if (Command.SelectItemDetails != null && Command.ItemdetailIndex == null && Command.ItemdetailValues == null)
                {
                    var item = naceData.NaceDetailDatas
                        .Where(x => x.NaceData == "").ToList();
                    for (var counter = 0; counter < Command.SelectItemDetails.Count; counter++)
                    {
                        item[counter].Edit(Command.SelectItemDetails[counter], "");
                    }
                }

                if (Command.SelectItemDetails == null && Command.ItemdetailIndex != null && Command.ItemdetailValues != null)
                {
                    foreach (var stringValue in Command
                        .ItemdetailIndex.Select((value, index) => new { value, index }))
                    {
                        foreach (var item in naceData.NaceDetailDatas)
                        {
                            if (item.ItemId == stringValue.value)
                                item.Edit(item.ItemId, Command.ItemdetailValues[stringValue.index]);
                        }
                    }
                }

                _naceDataRepository.SaveChanges();

                return Task.FromResult(result.Succeeded());
            }

            return Task.FromResult(result.Failed(ApplicationMessage.RecordNotFound));
        }

        public NaceDataViewModel GetNaceData(long ListingId)
        {
            if (_naceDataRepository.Exist(x => x.ListingId == ListingId && !x.IsDeleted))
            {
                return _naceDataRepository.GetNaceData(ListingId);
            }

            return new NaceDataViewModel();
        }

        public Task<OperationResult> DeleteNaceData(long Id)
        {
            var result = new OperationResult();
            if (_naceDataRepository.Exist(x => x.Id == Id))
            {
                _naceDataRepository.DeleteNaceData(Id);
                _naceDataRepository.SaveChanges();
                return Task.FromResult(result.Succeeded());
            }
            return Task.FromResult(result.Failed(ApplicationMessage.RecordNotFound));
        }
    }
}