using FSH.BlazorWebAssembly.Client.Infrastructure.Auth;
using FSH.BlazorWebAssembly.Client.Infrastructure.Common;
using FSH.WebApi.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace FSH.BlazorWebAssembly.Client.Shared;

public partial class NavMenu
{
    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = default!;
    [Inject]
    protected IAuthorizationService AuthService { get; set; } = default!;

    private string? _hangfireUrl;
    private bool _canViewHangfire;
    private bool _canViewDashboard;
    private bool _canViewRoles;
    private bool _canViewUsers;
    private bool _canViewProducts;
    private bool _canViewBrands;
    private bool _canViewTenants;
    private bool _canViewRooms;
    private bool _canViewRoomtypes;
    private bool _canViewRoomstatuses;
    private bool _canViewDepartments;
    private bool _canViewAccountentries;
    private bool _canViewBookings;
    private bool _canViewBookingstatuses;
    private bool _canViewCharges;
    private bool _canViewCustomers;
    private bool _canViewCustomerclassifications;
    private bool _canViewEmployees;
    private bool _canViewExpensecategories;
    private bool _canViewFloors;
    private bool _canViewFolios;
    private bool _canViewFoliotypes;
    private bool _canViewPaymentmodes;
    private bool _canViewPurchases;
    private bool _canViewRoomsbookeds;
    private bool _canViewTransactiontypes;
    private bool _canViewTransactionstatuses;
    private bool _canViewTravelagents;
    private bool _canViewVendors;

    private bool CanViewAdministrationGroup => _canViewUsers || _canViewRoles || _canViewTenants;

    protected override async Task OnParametersSetAsync()
    {
        _hangfireUrl = Config[ConfigNames.ApiBaseUrl] + "jobs";
        var user = (await AuthState).User;
        _canViewHangfire = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Hangfire);
        _canViewDashboard = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Dashboard);
        _canViewRoles = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Roles);
        _canViewUsers = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Users);
        _canViewProducts = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Products);
        _canViewBrands = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Brands);
        _canViewTenants = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Tenants);
        _canViewRooms = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Rooms);
        _canViewRoomtypes = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Roomtypes);
        _canViewRoomstatuses = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Roomstatuses);
        _canViewDepartments = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Departments);
        _canViewAccountentries = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Accountentries);
        _canViewBookings = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Bookings);
        _canViewBookingstatuses = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Bookingstatuses);
        _canViewCharges = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Charges);
        _canViewCustomers = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Customers);
        _canViewCustomerclassifications = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Customerclassifications);
        _canViewEmployees = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Employees);
        _canViewExpensecategories = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Expensecategories);
        _canViewFloors = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Floors);
        _canViewFolios = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Folios);
        _canViewFoliotypes = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Foliotypes);
        _canViewPaymentmodes = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Paymentmodes);
        _canViewPurchases = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Purchases);
        _canViewRoomsbookeds = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Roomsbookeds);
        _canViewTransactiontypes = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Transactiontypes);
        _canViewTransactionstatuses = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Transactionstatuses);
        _canViewTravelagents = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Travelagents);
        _canViewVendors = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Vendors);

    }
}