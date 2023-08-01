using System.Collections.Concurrent;
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
        private readonly ParallelOptions _defaultParallelOptions;

        public BankService(Serilog.ILogger logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, ApplicationSettings settings,
            IBankApi api, IObjectStorageService objectStorageService)
            : base(logger, mapper, httpContextAccessor, settings)
        {
            _api = api;
            _objectStorageService = objectStorageService;
            _defaultParallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = 4 };
        }

        public async Task<List<BankListModel>> GetAll()
        {
            var apiResponse = await _api.Get();
            var response = ReadApiResponse<List<BankList>>(apiResponse) ?? new List<BankList>();
            var models = response.Select(_mapper.Map<BankListModel>).ToList();
            var dl = await DownloadBankIcons(models.Select(x => x.IconPath).ToArray());
            foreach (var model in models)
            {
                model.RenderedIcon = dl.ContainsKey(model.IconPath) ? dl[model.IconPath] : null;
            }
            return models;
        }

        public async Task<BankEditModel> GetById(int id)
        {
            var apiResponse = await _api.Get(id);
            var item = ReadApiResponse<BankDetails>(apiResponse) ?? new BankDetails();
            var model = _mapper.Map<BankEditModel>(item);

            var dl = await DownloadBankIcons(new string[] { model.IconPath });
            model.RenderedIcon = dl.ContainsKey(model.IconPath) ? dl[model.IconPath] : null;

            return model;
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
            var bankIconParams = new ObjectStorageParams(_settings.BankIconLocation, string.Format("{0}.png", Guid.NewGuid()));
            using Stream imageStream = bankIconFileInfo.OpenReadStream(1024 * 1024 * 10);
            var bankIconPath = await _objectStorageService.UploadFileAsync(bankIconParams, imageStream);
            return bankIconPath;
        }

        private async Task<IDictionary<string,string?>> DownloadBankIcons(string[] iconPaths)
        {
            var dict = new ConcurrentDictionary<string, string?>();

            await Parallel.ForEachAsync(iconPaths, _defaultParallelOptions, async (iconPath, ct) =>
            {
                var p = new ObjectStorageParams(_settings.BankIconLocation, iconPath);
                try
                {
                    var dl = await _objectStorageService.DownloadFileAsync(p);
                    if (dl != null)
                    {
                        dict.GetOrAdd(iconPath, $"data:image/png;base64,{Convert.ToBase64String(dl.Stream.ToArray())}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Image {IconPath} could not be downloaded", iconPath);
                }
            });
            return dict;
        }

        public async Task<bool> Delete(int id)
        {
            var existing = await GetById(id);
            if (existing == null)
                return false;

            var apiResponse = await _api.Delete(id);
            var result = ReadApiResponse<bool>(apiResponse);

            if (result)
            {
                try
                {
                    var p = new ObjectStorageParams(_settings.BankIconLocation, existing.IconPath);
                    await _objectStorageService.DeleteFileAsync(p);
                }
                catch(Exception ex)
                {
                    _logger.Error(ex, "Image {IconPath} could not be deleted", existing.IconPath);
                }
            }

            return result;
        }
    }
}

