using Microsoft.Extensions.DependencyInjection;
using Pos.Application.Interfaces.Services;
using Pos.Application.Services.Auth;
using Pos.Application.Services.CashFlows;
using Pos.Application.Services.Categories;
using Pos.Application.Services.Products;
using Pos.Application.Services.SaleItems;
using Pos.Application.Services.Sales;
using Pos.Application.Services.Users;
using Pos.Application.UseCases.CashFlows;
using Pos.Application.UseCases.Categories;
using Pos.Application.UseCases.Products;
using Pos.Application.UseCases.SaleItems;
using Pos.Application.UseCases.Sales;
using Pos.Application.UseCases.Users;

namespace Pos.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationDependency(this IServiceCollection services)
    {
        services.AddScoped<CreateProductUseCase>();
        services.AddScoped<UpdateProductUseCase>();
        services.AddScoped<GetProductByIdUseCase>();
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
        services.AddScoped<GetAllSalesUseCase>();
        services.AddScoped<DeleteSaleUseCase>();
        services.AddScoped<ISaleService, SaleService>();
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
        services.AddScoped<GetAllCashFlowsUseCase>();
        services.AddScoped<DeleteCashFlowUseCase>();
        services.AddScoped<ICashFlowService, CashFlowService>();

        return services;
    }
}
