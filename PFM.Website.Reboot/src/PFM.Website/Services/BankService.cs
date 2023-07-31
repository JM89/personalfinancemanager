using AutoMapper;
using Microsoft.AspNetCore.Components.Forms;
using PFM.Bank.Api.Contracts.Bank;
using PFM.Website.Configurations;
using PFM.Website.ExternalServices;
using PFM.Website.Models;
using PFM.Website.Persistence;
using PFM.Website.Persistence.Models;

namespace PFM.Website.Services
{
    public class BankService: CoreService
    {
        private readonly IBankApi _api;
        private readonly IObjectStorageService _objectStorageService;
        
        public BankService(Serilog.ILogger logger, IMapper mapper, ApplicationSettings settings,
            IBankApi api, IObjectStorageService objectStorageService)
            : base(logger, mapper, settings)
        {
            _api = api;
            _objectStorageService = objectStorageService;
        }

        public async Task<List<BankListModel>> GetAll()
        {
            var apiResponse = await _api.Get();
            var response = ReadApiResponse<List<BankList>>(apiResponse) ?? new List<BankList>();
            return response.Select(_mapper.Map<BankListModel>).ToList();
        }

        public async Task<BankEditModel> GetById(int id)
        {
            var apiResponse = await _api.Get(id);
            var item = ReadApiResponse<BankDetails>(apiResponse);
            return _mapper.Map<BankEditModel>(item);
        }

        public async Task<bool> Create(BankEditModel model, IBrowserFile bankIconFileInfo)
        {
            var iconPath = await UploadBankIcon(bankIconFileInfo);
            if (string.IsNullOrEmpty(iconPath))
            {
                _logger.Error("Could not upload image");
                return false;
            }

            var request = _mapper.Map<BankDetails>(model);
            request.IconPath = iconPath;

            var apiResponse = await _api.Create(request);
            var result = ReadApiResponse<bool>(apiResponse);
            return result;
        }

        public async Task<bool> Edit(int id, BankEditModel model, IBrowserFile bankIconFileInfo)
        {
            var request = _mapper.Map<BankDetails>(model);

            if (bankIconFileInfo != null)
            {
                var iconPath = await UploadBankIcon(bankIconFileInfo);
                if (string.IsNullOrEmpty(iconPath))
                {
                    _logger.Error("Could not upload image");
                    return false;
                }
                request.IconPath = iconPath;
            }

            var apiResponse = await _api.Edit(id, request);
            var result = ReadApiResponse<bool>(apiResponse);
            return result;
        }

        private async Task<string> UploadBankIcon(IBrowserFile bankIconFileInfo)
        {
            var bankIconParams = new ObjectStorageParams(bankIconFileInfo.ContentType, _settings.BankIconLocation, string.Format("{0}.png", Guid.NewGuid()));
            using Stream imageStream = bankIconFileInfo.OpenReadStream(1024 * 1024 * 10);
            var bankIconPath = await _objectStorageService.UploadFileAsync(bankIconParams, imageStream);
            return bankIconPath;
        }

        public async Task<bool> Delete(int id)
        {
            var apiResponse = await _api.Delete(id);
            var result = ReadApiResponse<bool>(apiResponse);
            return result;
        }
    }
}

