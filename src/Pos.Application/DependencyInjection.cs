using Microsoft.Extensions.DependencyInjection;
using Pos.Application.Interfaces.Services;
using Pos.Application.Services.Auth;
using Pos.Application.Services.CashBoxes;
using Pos.Application.Services.CashFlows;
using Pos.Application.Services.Categories;
using Pos.Application.Services.Products;
using Pos.Application.Services.SaleItems;
using Pos.Application.Services.Sales;
using Pos.Application.Services.Users;
using Pos.Application.Services.Returns;
using Pos.Application.UseCases.CashBoxes;
using Pos.Application.UseCases.CashFlows;
using Pos.Application.UseCases.Categories;
using Pos.Application.UseCases.Products;
using Pos.Application.UseCases.Reports;
using Pos.Application.UseCases.SaleItems;
using Pos.Application.UseCases.Sales;
using Pos.Application.UseCases.Users;
using Pos.Application.UseCases.Returns;

namespace Pos.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationDependency(this IServiceCollection services)
    {
        services.AddScoped<CreateProductUseCase>();
        services.AddScoped<UpdateProductUseCase>();
        services.AddScoped<GetProductByIdUseCase>();
        services.AddScoped<GetProductBySkuUseCase>();
        services.AddScoped<SearchProductsUseCase>();
        services.AddScoped<GetProductsByCategoryUseCase>();
        services.AddScoped<GetAllProductsUseCase>();
        services.AddScoped<DeleteProductUseCase>();

        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<CreateUserUseCase>();
        services.AddScoped<UpdateUserUseCase>();
        services.AddScoped<GetUserByIdUseCase>();
        services.AddScoped<GetUserByEmailUseCase>();
        services.AddScoped<GetUserByNormaliceNameUseCase>();
        services.AddScoped<GetAllUsersUseCase>();
        services.AddScoped<DeleteUserUseCase>();
        services.AddScoped<GetUserPermissionsUseCase>();
        services.AddScoped<UpdateUserPermissionsUseCase>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<CreateCategoryUseCase>();
        services.AddScoped<UpdateCategoryUseCase>();
        services.AddScoped<GetCategoryByIdUseCase>();
        services.AddScoped<GetCategoryByNameUseCase>();
        services.AddScoped<GetAllCategoriesUseCase>();
        services.AddScoped<DeleteCategoryUseCase>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<CreateSaleUseCase>();
        services.AddScoped<UpdateSaleUseCase>();
        services.AddScoped<GetSaleByIdUseCase>();
        services.AddScoped<GetSalesByUserIdUseCase>();
        services.AddScoped<GetSalesByDateRangeUseCase>();
        services.AddScoped<GetSalesByCashBoxIdUseCase>();
        services.AddScoped<GetAllSalesUseCase>();
        services.AddScoped<DeleteSaleUseCase>();
        services.AddScoped<GetSalesByDepartmentByCashBoxIdUseCase>();
        services.AddScoped<ISaleService, SaleService>();
        services.AddScoped<CreateReturnUseCase>();
        services.AddScoped<GetReturnsBySaleIdUseCase>();
        services.AddScoped<GetReturnsByCashBoxIdUseCase>();
        services.AddScoped<GetReturnSummaryByCashBoxIdUseCase>();
        services.AddScoped<GetReturnByIdUseCase>();
        services.AddScoped<IReturnService, ReturnService>();
        services.AddScoped<CreateSaleItemUseCase>();
        services.AddScoped<UpdateSaleItemUseCase>();
        services.AddScoped<GetSaleItemByIdUseCase>();
        services.AddScoped<GetSaleItemsBySaleIdUseCase>();
        services.AddScoped<GetSaleItemsByProductIdUseCase>();
        services.AddScoped<GetAllSaleItemsUseCase>();
        services.AddScoped<DeleteSaleItemUseCase>();
        services.AddScoped<ISaleItemService, SaleItemService>();
        services.AddScoped<CreateCashFlowUseCase>();
        services.AddScoped<UpdateCashFlowUseCase>();
        services.AddScoped<GetCashFlowByIdUseCase>();
        services.AddScoped<GetCashFlowsByUserIdUseCase>();
        services.AddScoped<GetCashFlowsByDateRangeUseCase>();
        services.AddScoped<GetCashFlowsByCashBoxIdUseCase>();
        services.AddScoped<GetCashFlowSummaryByCashBoxIdUseCase>();
        services.AddScoped<GetAllCashFlowsUseCase>();
        services.AddScoped<DeleteCashFlowUseCase>();
        services.AddScoped<ICashFlowService, CashFlowService>();
        services.AddScoped<CreateCashBoxUseCase>();
        services.AddScoped<UpdateCashBoxUseCase>();
        services.AddScoped<GetCashBoxByIdUseCase>();
        services.AddScoped<GetCashBoxByDateUseCase>();
        services.AddScoped<GetLatestCashBoxByUserIdUseCase>();
        services.AddScoped<GetCashBoxesByUserIdUseCase>();
        services.AddScoped<GetAllCashBoxesUseCase>();
        services.AddScoped<GetEmployeeShiftSummaryUseCase>();
        services.AddScoped<GetShiftCutSummaryUseCase>();
        services.AddScoped<DeleteCashBoxUseCase>();
        services.AddScoped<ICashBoxService, CashBoxService>();

        return services;
    }
}
