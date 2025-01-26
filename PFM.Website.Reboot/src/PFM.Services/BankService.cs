using System.Collections.Concurrent;
using AutoMapper;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using PFM.Bank.Api.Contracts.Bank;
using PFM.Models;
using PFM.Services.Configurations;
using PFM.Services.ExternalServices;
using PFM.Services.Persistence;
using PFM.Services.Persistence.Models;

namespace PFM.Services
{
    public class BankService(
        Serilog.ILogger logger,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        IBankApi api,
        ExternalServiceSettings settings,
        IObjectStorageService objectStorageService)
        : CoreService(logger, mapper, httpContextAccessor)
    {
        private readonly ParallelOptions _defaultParallelOptions = new() { MaxDegreeOfParallelism = 4 };

        public async Task<List<BankListModel>> GetAll()
        {
            var apiResponse = await api.Get(GetCurrentUserId());
            var response = ReadApiResponse<List<BankList>>(apiResponse) ?? new List<BankList>();
            var models = response.Select(Mapper.Map<BankListModel>).ToList();
            var dl = await DownloadBankIcons(models.Select(x => x.IconPath).ToArray());
            foreach (var model in models)
            {
                model.RenderedIcon = dl.ContainsKey(model.IconPath) ? dl[model.IconPath] : null;
            }
            return models;
        }

        public async Task<BankEditModel> GetById(int id)
        {
            var apiResponse = await api.Get(id);
            var item = ReadApiResponse<BankDetails>(apiResponse) ?? new BankDetails();
            var model = Mapper.Map<BankEditModel>(item);

            var dl = await DownloadBankIcons(new string[] { model.IconPath });
            model.RenderedIcon = dl.ContainsKey(model.IconPath) ? dl[model.IconPath] : null;

            return model;
        }

        public async Task<bool> Create(BankEditModel model, IBrowserFile bankIconFileInfo)
        {
            var iconPath = await UploadBankIcon(bankIconFileInfo);
            if (string.IsNullOrEmpty(iconPath))
            {
                Logger.Error("Could not upload image");
                return false;
            }

            var request = Mapper.Map<BankDetails>(model);
            request.IconPath = iconPath;

            var apiResponse = await api.Create(GetCurrentUserId(), request);
            var result = ReadApiResponse<bool>(apiResponse);
            return result;
        }

        public async Task<bool> Edit(int id, BankEditModel model, IBrowserFile bankIconFileInfo)
        {
            var request = Mapper.Map<BankDetails>(model);

            if (bankIconFileInfo != null)
            {
                var iconPath = await UploadBankIcon(bankIconFileInfo);
                if (string.IsNullOrEmpty(iconPath))
                {
                    Logger.Error("Could not upload image");
                    return false;
                }
                request.IconPath = iconPath;
            }

            var apiResponse = await api.Edit(id, GetCurrentUserId(), request);
            var result = ReadApiResponse<bool>(apiResponse);
            return result;
        }

        private async Task<string> UploadBankIcon(IBrowserFile bankIconFileInfo)
        {
            var bankIconParams = new ObjectStorageParams(settings.BankIconSettings.Location, string.Format("{0}.png", Guid.NewGuid()));
            using Stream imageStream = bankIconFileInfo.OpenReadStream(1024 * 1024 * 10);
            var bankIconPath = await objectStorageService.UploadFileAsync(bankIconParams, imageStream);
            return bankIconPath;
        }

        private async Task<IDictionary<string,string?>> DownloadBankIcons(string[] iconPaths)
        {
            var dict = new ConcurrentDictionary<string, string?>();

            await Parallel.ForEachAsync(iconPaths, _defaultParallelOptions, async (iconPath, ct) =>
            {
                var p = new ObjectStorageParams(settings.BankIconSettings.Location, iconPath);
                try
                {
                    var dl = await objectStorageService.DownloadFileAsync(p);
                    if (dl is { Stream: not null })
                    {
                        dict.GetOrAdd(iconPath, $"data:image/png;base64,{Convert.ToBase64String(dl.Stream.ToArray())}");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Image {IconPath} could not be downloaded", iconPath);
                }
            });
            return dict;
        }

        public async Task<bool> Delete(int id)
        {
            var existing = await GetById(id);
            if (existing == null)
                return false;

            var apiResponse = await api.Delete(id);
            var result = ReadApiResponse<bool>(apiResponse);

            if (result)
            {
                try
                {
                    var p = new ObjectStorageParams(settings.BankIconSettings.Location, existing.IconPath);
                    await objectStorageService.DeleteFileAsync(p);
                }
                catch(Exception ex)
                {
                    Logger.Error(ex, "Image {IconPath} could not be deleted", existing.IconPath);
                }
            }

            return result;
        }
    }
}

