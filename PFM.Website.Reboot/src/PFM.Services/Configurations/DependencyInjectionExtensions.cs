using System.Text.Json;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PFM.Services.ExternalServices;
using PFM.Services.ExternalServices.InMemoryStorage;
using PFM.Services.Mappers;
using PFM.Services.Persistence;
using PFM.Website.Configurations;
using Refit;

namespace PFM.Services.Configurations;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddExternalServices(this IServiceCollection services, ExternalServiceSettings settings, bool isDev)
    {
        services.AddSingleton(settings);
        services.AddHttpContextAccessor();
        services.AddObjectStorage(settings);
        services.AddTransient<AuthHeaderHandler>();

        services.AddPfmApi(settings.PfmApiOptions, isDev);
        
        return services
            .AddSingleton<ExpenseTypeService>()
            .AddSingleton<BankService>()
            .AddSingleton<CountryService>()
            .AddSingleton<BankAccountService>()
            .AddSingleton<CurrencyService>()
            .AddSingleton<IncomeService>()
            .AddSingleton<SavingService>()
            .AddSingleton<AtmWithdrawService>()
            .AddSingleton<ExpenseService>()
            .AddSingleton<PaymentMethodService>()
            .AddSingleton<MovementSummaryService>()
            .AddSingleton<BudgetPlanService>();
    }
    
    private static IServiceCollection AddPfmApi(this IServiceCollection services, PfmApiOptions options, bool isDevelopmentEnvironment)
        {
            if (!options.Enabled)
            {
                services
                    .AddSingleton<IExpenseTypeApi, ExpenseTypeInMemory>()
                    .AddSingleton<IBankApi, BankInMemory>()
                    .AddSingleton<ICountryApi, CountryInMemory>()
                    .AddSingleton<IBankAccountApi, BankAccountInMemory>()
                    .AddSingleton<ICurrencyApi, CurrencyInMemory>()
                    .AddSingleton<IIncomeApi, IncomeInMemory>()
                    .AddSingleton<ISavingApi, SavingInMemory>()
                    .AddSingleton<IAtmWithdrawApi, AtmWithdrawInMemory>()
                    .AddSingleton<IExpenseApi, ExpenseInMemory>()
                    .AddSingleton<IPaymentMethodApi, PaymentMethodInMemory>()
                    .AddSingleton<IMovementSummaryApi, MovementSummaryInMemory>()
                    .AddSingleton<IBudgetPlanApi, BudgetPlanInMemory>();
                return services;
            }

            var refitSettings = new RefitSettings()
            {
                ContentSerializer = new SystemTextJsonContentSerializer(
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        Converters =
                        {
                            new ObjectToInferredTypesConverter(),
                        }
                    }
                ),
                ExceptionFactory = httpResponse => Task.FromResult<Exception?>(null)
            };

            var httpClientHandler = !isDevelopmentEnvironment ? new HttpClientHandler() : new HttpClientHandler { ServerCertificateCustomValidationCallback = (message, cert, chain, sslErrors) => true };

            var apiConfigs = options.EndpointUrl;
            services
                .AddPfmApiClient<IExpenseTypeApi>(apiConfigs, refitSettings, httpClientHandler)
                .AddPfmApiClient<ICountryApi>(apiConfigs, refitSettings, httpClientHandler)
                .AddPfmApiClient<IBankApi>(apiConfigs, refitSettings, httpClientHandler)
                .AddPfmApiClient<ICurrencyApi>(apiConfigs, refitSettings, httpClientHandler)
                .AddPfmApiClient<IBankAccountApi>(apiConfigs, refitSettings, httpClientHandler)
                .AddPfmApiClient<IIncomeApi>(apiConfigs, refitSettings, httpClientHandler)
                .AddPfmApiClient<ISavingApi>(apiConfigs, refitSettings, httpClientHandler)
                .AddPfmApiClient<IAtmWithdrawApi>(apiConfigs, refitSettings, httpClientHandler)
                .AddPfmApiClient<IExpenseApi>(apiConfigs, refitSettings, httpClientHandler)
                .AddPfmApiClient<IPaymentMethodApi>(apiConfigs, refitSettings, httpClientHandler)
                .AddPfmApiClient<IMovementSummaryApi>(apiConfigs, refitSettings, httpClientHandler)
                .AddPfmApiClient<IBudgetPlanApi>(apiConfigs, refitSettings, httpClientHandler);

            return services;
        }

        private static IServiceCollection AddPfmApiClient<T>(this IServiceCollection services, string baseUrl, RefitSettings refitSettings, HttpClientHandler httpClientHandler)
            where T : class
        {
            services
               .AddRefitClient<T>(refitSettings)
               .ConfigureHttpClient(c => c.BaseAddress = new Uri(baseUrl))
               .ConfigurePrimaryHttpMessageHandler(() => httpClientHandler)
               .AddHttpMessageHandler<AuthHeaderHandler>();

            return services;
        }

        public static IServiceCollection AddObjectMapper(this IServiceCollection services)
        {
            var configuration = new MapperConfiguration(cfg => {
                cfg.AddProfile<ModelToResponseProfile>();
                cfg.AddProfile<RequestToModelProfile>();
            });

            services.AddSingleton(configuration);
            services.AddSingleton<IMapper>(configuration.CreateMapper());

            return services;
        }
}