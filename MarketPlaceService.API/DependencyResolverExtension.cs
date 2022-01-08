using System;
using MarketPlaceService.BLL;
using MarketPlaceService.BLL.Contracts;
using MarketPlaceService.BLL.Contracts.UtilityServiceContracts;
using MarketPlaceService.BLL.UtilityService;
using MarketPlaceService.DAL;
using MarketPlaceService.DAL.Contract;
using MarketPlaceService.DAL.Utilities;
using Microsoft.Extensions.DependencyInjection;
using MarketPlaceService.Utilities;
using CommonUtilities;

namespace MarketPlaceService.API
{
    public static class DependencyResolverExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IProfileService, ProfileService>()
            .AddScoped<ISiteService, SiteService>()
            .AddScoped<ISubscriberService, SubscriberService>()
            .AddScoped(typeof(ITransactionLoggerService<>), typeof(TransactionLoggerService<>))
            .AddScoped<ISupplierStatusesService, SupplierStatusesService>()
            .AddScoped<IAgentsService, AgentsService>()
            .AddScoped<IServiceStatusesService, ServiceStatusesService>()
            .AddScoped<ITravelStudioService, TravelStudioService>()
            .AddScoped<IChangeHistoryService, ChangeHistoryService>()
            .AddScoped<IMasterDataConfigService, MasterDataConfigService>()
            .AddScoped<IMasterDataService, MasterDataService>()
            .AddScoped<IMasterDataRatingsService, MasterDataRatingsService>()
            .AddScoped<IMasterDataRegionService, MasterDataRegionService>()
            .AddScoped<IMasterDataServiceTypeService, MasterDataServiceTypeService>()
            .AddScoped<IAPIManagerHelperService, APIManagerHelperService>()
            .AddScoped<IAPIManagerService, APIManagerService>()
            .AddScoped<IMappingDataConfigService, MappingDataConfigService>()
            .AddScoped<IMappingDataService, MappingDataService>()
            .AddScoped<IMappingDataRatingService, MappingDataRatingService>()
            .AddScoped<ISubscriptionProductsService, SubscriptionProductsService>()
            .AddScoped<IMappingJsonUtilityService, MappingJsonUtilityService>()
            .AddScoped<ICommonService, CommonService>()
            .AddScoped<IQueuedUpdateService, QueuedUpdateService>()
            .AddScoped<IBookingService, BookingService>()
            .AddScoped<IPricingService, PricingService>()
            .AddScoped<IChangesDetectedService, ChangesDetectedService>()
            .AddScoped<IJobService, JobService>()
            .AddScoped<IPackageStatuses, PackageStatuses>()
            .AddScoped<IMasterDataPackageService, MasterDataPackageService>();
            
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection repositories)
        {
            repositories.AddScoped<IPublisherRepository, PublisherRepository>()
            .AddScoped<ISiteRepository, SiteRepository>()
            .AddScoped<ISubscriberRepository, SubscriberRepository>()
            .AddScoped(typeof(ITransactionLoggerRepository<>),typeof(TransactionLoggerRepository<>))
            .AddScoped<ICommonRepository, CommonRepository>()
            .AddScoped<ISupplierStatusesRepository, SupplierStatusesRepository>()
            .AddScoped<IChangeHistoryRepository, ChangeHistoryRepository>()
            .AddScoped<IMasterDataConfigRepository, MasterDataConfigRepository>()
            .AddScoped<IMasterDataRepository, MasterDataRepository>()
            .AddScoped<IMasterDataRatingsRepository, MasterDataRatingsRepository>()
            .AddScoped<IMasterDataRegionRepository, MasterDataRegionRepository>()
            .AddScoped<IMasterDataServiceTypeRepository, MasterDataServiceTypeRepository>()
            .AddScoped<IMappingDataConfigRepository, MappingDataConfigRepository>()
            .AddScoped<IMappingDataRepository, MappingDataRepository>()
            .AddScoped<IMappingDataRatingRepository, MappingDataRatingRepository>()
            .AddScoped<ISubscriptionProductsRepository, SubscriptionProductRepository>()
            .AddScoped<IMappingJsonUtility, MappingJsonUtility>()
            .AddScoped<IMarketPlaceProductRepository, MarketPlaceProductRepository>()
            .AddScoped<IQueuedUpdatesRepository,QueuedUpdateRepository>()
            .AddScoped<IBookingRepository,BookingRepository>()
            .AddScoped<IPricingRepository,PricingRepository>()
            .AddScoped<IJobRepository, JobRepository>()
            .AddScoped<IMasterDataPackageRepository, MasterDataPackageRepository>();
            return repositories;
        }

        public static IServiceCollection AddUtilities(this IServiceCollection utilities)
        {
            utilities.AddScoped<IChangeHistoryHelper, MasterDataChangeHistoryHelper>()
            .AddScoped<IChangeHistoryHelper, MasterDataRatingsChangeHistoryhelper>()
            .AddScoped<IChangeHistoryHelper, MasterDataRegionChangeHistoryHelper>()
            .AddScoped<IChangeHistoryHelper, MasterDataServiceTypeChangeHistoryHelper>()
            .AddScoped<IChangeHistoryHelper, MappingDataChangeHistoryHelper>()
            .AddScoped<IChangeHistoryHelper, MappingDataRatingsChangeHistoryHelper>()
            .AddScoped<IRequestResponseLoggingHelper, RequestResponseLoggingHelper>();
            return utilities;
        }
    }
}
