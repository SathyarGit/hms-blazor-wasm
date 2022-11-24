using System.Collections.ObjectModel;

namespace FSH.WebApi.Shared.Authorization;

public static class FSHAction
{
    public const string View = nameof(View);
    public const string Search = nameof(Search);
    public const string Create = nameof(Create);
    public const string Update = nameof(Update);
    public const string Delete = nameof(Delete);
    public const string Export = nameof(Export);
    public const string Generate = nameof(Generate);
    public const string Clean = nameof(Clean);
    public const string UpgradeSubscription = nameof(UpgradeSubscription);
}

public static class FSHResource
{
    public const string Tenants = nameof(Tenants);
    public const string Dashboard = nameof(Dashboard);
    public const string Hangfire = nameof(Hangfire);
    public const string Users = nameof(Users);
    public const string UserRoles = nameof(UserRoles);
    public const string Roles = nameof(Roles);
    public const string RoleClaims = nameof(RoleClaims);
    public const string Products = nameof(Products);
    public const string Brands = nameof(Brands);
    public const string Floors = nameof(Floors);
    public const string Departments = nameof(Departments);
    public const string Rooms = nameof(Rooms);
    public const string Employees = nameof(Employees);
    public const string Vendors = nameof(Vendors);
    public const string Travelagents = nameof(Travelagents);
    public const string Transactiontypes = nameof(Transactiontypes);
    public const string Transactionstatuses = nameof(Transactionstatuses);
    public const string Roomtypes = nameof(Roomtypes);
    public const string Roomstatuses = nameof(Roomstatuses);
    public const string Roomsbookeds = nameof(Roomsbookeds);
    public const string Purchases = nameof(Purchases);
    public const string Paymentmodes = nameof(Paymentmodes);
    public const string Foliotypes = nameof(Foliotypes);
    public const string Folios = nameof(Folios);
    public const string Expensecategories = nameof(Expensecategories);
    public const string Customers = nameof(Customers);
    public const string Customerclassifications = nameof(Customerclassifications);
    public const string Charges = nameof(Charges);
    public const string Bookings = nameof(Bookings);
    public const string Bookingstatuses = nameof(Bookingstatuses);
    public const string Accountentries = nameof(Accountentries);
}

public static class FSHPermissions
{
    private static readonly FSHPermission[] _all = new FSHPermission[]
    {
        new("View Dashboard", FSHAction.View, FSHResource.Dashboard),
        new("View Hangfire", FSHAction.View, FSHResource.Hangfire),
        new("View Users", FSHAction.View, FSHResource.Users),
        new("Search Users", FSHAction.Search, FSHResource.Users),
        new("Create Users", FSHAction.Create, FSHResource.Users),
        new("Update Users", FSHAction.Update, FSHResource.Users),
        new("Delete Users", FSHAction.Delete, FSHResource.Users),
        new("Export Users", FSHAction.Export, FSHResource.Users),
        new("View UserRoles", FSHAction.View, FSHResource.UserRoles),
        new("Update UserRoles", FSHAction.Update, FSHResource.UserRoles),
        new("View Roles", FSHAction.View, FSHResource.Roles),
        new("Create Roles", FSHAction.Create, FSHResource.Roles),
        new("Update Roles", FSHAction.Update, FSHResource.Roles),
        new("Delete Roles", FSHAction.Delete, FSHResource.Roles),
        new("View RoleClaims", FSHAction.View, FSHResource.RoleClaims),
        new("Update RoleClaims", FSHAction.Update, FSHResource.RoleClaims),
        new("View Products", FSHAction.View, FSHResource.Products, IsBasic: true),
        new("Search Products", FSHAction.Search, FSHResource.Products, IsBasic: true),
        new("Create Products", FSHAction.Create, FSHResource.Products),
        new("Update Products", FSHAction.Update, FSHResource.Products),
        new("Delete Products", FSHAction.Delete, FSHResource.Products),
        new("Export Products", FSHAction.Export, FSHResource.Products),
        new("View Brands", FSHAction.View, FSHResource.Brands, IsBasic: true),
        new("Search Brands", FSHAction.Search, FSHResource.Brands, IsBasic: true),
        new("Create Brands", FSHAction.Create, FSHResource.Brands),
        new("Update Brands", FSHAction.Update, FSHResource.Brands),
        new("Delete Brands", FSHAction.Delete, FSHResource.Brands),
        new("Generate Brands", FSHAction.Generate, FSHResource.Brands),
        new("Clean Brands", FSHAction.Clean, FSHResource.Brands),
        new("View Tenants", FSHAction.View, FSHResource.Tenants, IsRoot: true),
        new("Create Tenants", FSHAction.Create, FSHResource.Tenants, IsRoot: true),
        new("Update Tenants", FSHAction.Update, FSHResource.Tenants, IsRoot: true),
        new("Upgrade Tenant Subscription", FSHAction.UpgradeSubscription, FSHResource.Tenants, IsRoot: true),

        new("View Departments", FSHAction.View, FSHResource.Departments, IsBasic: true),
        new("Search Departments", FSHAction.Search, FSHResource.Departments, IsBasic: true),
        new("Create Departments", FSHAction.Create, FSHResource.Departments),
        new("Update Departments", FSHAction.Update, FSHResource.Departments),
        new("Delete Departments", FSHAction.Delete, FSHResource.Departments),
        new("Generate Departments", FSHAction.Generate, FSHResource.Departments),
        new("Clean Departments", FSHAction.Clean, FSHResource.Departments),
        new("View Floors", FSHAction.View, FSHResource.Floors, IsBasic: true),
        new("Search Floors", FSHAction.Search, FSHResource.Floors, IsBasic: true),
        new("Create Floors", FSHAction.Create, FSHResource.Floors),
        new("Update Floors", FSHAction.Update, FSHResource.Floors),
        new("Delete Floors", FSHAction.Delete, FSHResource.Floors),
        new("Generate Floors", FSHAction.Generate, FSHResource.Floors),
        new("Clean Floors", FSHAction.Clean, FSHResource.Floors),
        new("View Rooms", FSHAction.View, FSHResource.Rooms, IsBasic: true),
        new("Search Rooms", FSHAction.Search, FSHResource.Rooms, IsBasic: true),
        new("Create Rooms", FSHAction.Create, FSHResource.Rooms),
        new("Update Rooms", FSHAction.Update, FSHResource.Rooms),
        new("Delete Rooms", FSHAction.Delete, FSHResource.Rooms),
        new("Generate Rooms", FSHAction.Generate, FSHResource.Rooms),
        new("Clean Rooms", FSHAction.Clean, FSHResource.Rooms),
        new("Export Rooms", FSHAction.Export, FSHResource.Rooms),
        new("View Employees", FSHAction.View, FSHResource.Employees, IsBasic: true),
        new("Search Employees", FSHAction.Search, FSHResource.Employees, IsBasic: true),
        new("Create Employees", FSHAction.Create, FSHResource.Employees),
        new("Update Employees", FSHAction.Update, FSHResource.Employees),
        new("Delete Employees", FSHAction.Delete, FSHResource.Employees),
        new("Generate Employees", FSHAction.Generate, FSHResource.Employees),
        new("Clean Employees", FSHAction.Clean, FSHResource.Employees),
        new("Export Employees", FSHAction.Export, FSHResource.Employees),
        new("View Vendors", FSHAction.View, FSHResource.Vendors, IsBasic: true),
        new("Search Vendors", FSHAction.Search, FSHResource.Vendors, IsBasic: true),
        new("Create Vendors", FSHAction.Create, FSHResource.Vendors),
        new("Update Vendors", FSHAction.Update, FSHResource.Vendors),
        new("Delete Vendors", FSHAction.Delete, FSHResource.Vendors),
        new("Generate Vendors", FSHAction.Generate, FSHResource.Vendors),
        new("Clean Vendors", FSHAction.Clean, FSHResource.Vendors),
        new("Export Vendors", FSHAction.Export, FSHResource.Vendors),
        new("View Travelagents", FSHAction.View, FSHResource.Travelagents, IsBasic: true),
        new("Search Travelagents", FSHAction.Search, FSHResource.Travelagents, IsBasic: true),
        new("Create Travelagents", FSHAction.Create, FSHResource.Travelagents),
        new("Update Travelagents", FSHAction.Update, FSHResource.Travelagents),
        new("Delete Travelagents", FSHAction.Delete, FSHResource.Travelagents),
        new("Generate Travelagents", FSHAction.Generate, FSHResource.Travelagents),
        new("Clean Travelagents", FSHAction.Clean, FSHResource.Travelagents),
        new("Export Travelagents", FSHAction.Export, FSHResource.Travelagents),
        new("View Transactiontypes", FSHAction.View, FSHResource.Transactiontypes, IsBasic: true),
        new("Search Transactiontypes", FSHAction.Search, FSHResource.Transactiontypes, IsBasic: true),
        new("Create Transactiontypes", FSHAction.Create, FSHResource.Transactiontypes),
        new("Update Transactiontypes", FSHAction.Update, FSHResource.Transactiontypes),
        new("Delete Transactiontypes", FSHAction.Delete, FSHResource.Transactiontypes),
        new("Generate Transactiontypes", FSHAction.Generate, FSHResource.Transactiontypes),
        new("Clean Transactiontypes", FSHAction.Clean, FSHResource.Transactiontypes),
        new("Export Transactiontypes", FSHAction.Export, FSHResource.Transactiontypes),
        new("View Transactionstatuses", FSHAction.View, FSHResource.Transactionstatuses, IsBasic: true),
        new("Search Transactionstatuses", FSHAction.Search, FSHResource.Transactionstatuses, IsBasic: true),
        new("Create Transactionstatuses", FSHAction.Create, FSHResource.Transactionstatuses),
        new("Update Transactionstatuses", FSHAction.Update, FSHResource.Transactionstatuses),
        new("Delete Transactionstatuses", FSHAction.Delete, FSHResource.Transactionstatuses),
        new("Generate Transactionstatuses", FSHAction.Generate, FSHResource.Transactionstatuses),
        new("Clean Transactionstatuses", FSHAction.Clean, FSHResource.Transactionstatuses),
        new("Export Transactionstatuses", FSHAction.Export, FSHResource.Transactionstatuses),
        new("View Roomtypes", FSHAction.View, FSHResource.Roomtypes, IsBasic: true),
        new("Search Roomtypes", FSHAction.Search, FSHResource.Roomtypes, IsBasic: true),
        new("Create Roomtypes", FSHAction.Create, FSHResource.Roomtypes),
        new("Update Roomtypes", FSHAction.Update, FSHResource.Roomtypes),
        new("Delete Roomtypes", FSHAction.Delete, FSHResource.Roomtypes),
        new("Generate Roomtypes", FSHAction.Generate, FSHResource.Roomtypes),
        new("Clean Roomtypes", FSHAction.Clean, FSHResource.Roomtypes),
        new("Export Roomtypes", FSHAction.Export, FSHResource.Roomtypes),
        new("View Roomstatuses", FSHAction.View, FSHResource.Roomstatuses, IsBasic: true),
        new("Search Roomstatuses", FSHAction.Search, FSHResource.Roomstatuses, IsBasic: true),
        new("Create Roomstatuses", FSHAction.Create, FSHResource.Roomstatuses),
        new("Update Roomstatuses", FSHAction.Update, FSHResource.Roomstatuses),
        new("Delete Roomstatuses", FSHAction.Delete, FSHResource.Roomstatuses),
        new("Generate Roomstatuses", FSHAction.Generate, FSHResource.Roomstatuses),
        new("Clean Roomstatuses", FSHAction.Clean, FSHResource.Roomstatuses),
        new("Export Roomstatuses", FSHAction.Export, FSHResource.Roomstatuses),
        new("View Roomsbookeds", FSHAction.View, FSHResource.Roomsbookeds, IsBasic: true),
        new("Search Roomsbookeds", FSHAction.Search, FSHResource.Roomsbookeds, IsBasic: true),
        new("Create Roomsbookeds", FSHAction.Create, FSHResource.Roomsbookeds),
        new("Update Roomsbookeds", FSHAction.Update, FSHResource.Roomsbookeds),
        new("Delete Roomsbookeds", FSHAction.Delete, FSHResource.Roomsbookeds),
        new("Generate Roomsbookeds", FSHAction.Generate, FSHResource.Roomsbookeds),
        new("Clean Roomsbookeds", FSHAction.Clean, FSHResource.Roomsbookeds),
        new("Export Roomsbookeds", FSHAction.Export, FSHResource.Roomsbookeds),
        new("View Purchases", FSHAction.View, FSHResource.Purchases, IsBasic: true),
        new("Search Purchases", FSHAction.Search, FSHResource.Purchases, IsBasic: true),
        new("Create Purchases", FSHAction.Create, FSHResource.Purchases),
        new("Update Purchases", FSHAction.Update, FSHResource.Purchases),
        new("Delete Purchases", FSHAction.Delete, FSHResource.Purchases),
        new("Generate Purchases", FSHAction.Generate, FSHResource.Purchases),
        new("Clean Purchases", FSHAction.Clean, FSHResource.Purchases),
        new("Export Purchases", FSHAction.Export, FSHResource.Purchases),
        new("View Paymentmodes", FSHAction.View, FSHResource.Paymentmodes, IsBasic: true),
        new("Search Paymentmodes", FSHAction.Search, FSHResource.Paymentmodes, IsBasic: true),
        new("Create Paymentmodes", FSHAction.Create, FSHResource.Paymentmodes),
        new("Update Paymentmodes", FSHAction.Update, FSHResource.Paymentmodes),
        new("Delete Paymentmodes", FSHAction.Delete, FSHResource.Paymentmodes),
        new("Generate Paymentmodes", FSHAction.Generate, FSHResource.Paymentmodes),
        new("Clean Paymentmodes", FSHAction.Clean, FSHResource.Paymentmodes),
        new("Export Paymentmodes", FSHAction.Export, FSHResource.Paymentmodes),
        new("View Foliotypes", FSHAction.View, FSHResource.Foliotypes, IsBasic: true),
        new("Search Foliotypes", FSHAction.Search, FSHResource.Foliotypes, IsBasic: true),
        new("Create Foliotypes", FSHAction.Create, FSHResource.Foliotypes),
        new("Update Foliotypes", FSHAction.Update, FSHResource.Foliotypes),
        new("Delete Foliotypes", FSHAction.Delete, FSHResource.Foliotypes),
        new("Generate Foliotypes", FSHAction.Generate, FSHResource.Foliotypes),
        new("Clean Foliotypes", FSHAction.Clean, FSHResource.Foliotypes),
        new("Export Foliotypes", FSHAction.Export, FSHResource.Foliotypes),
        new("View Folios", FSHAction.View, FSHResource.Folios, IsBasic: true),
        new("Search Folios", FSHAction.Search, FSHResource.Folios, IsBasic: true),
        new("Create Folios", FSHAction.Create, FSHResource.Folios),
        new("Update Folios", FSHAction.Update, FSHResource.Folios),
        new("Delete Folios", FSHAction.Delete, FSHResource.Folios),
        new("Generate Folios", FSHAction.Generate, FSHResource.Folios),
        new("Clean Folios", FSHAction.Clean, FSHResource.Folios),
        new("Export Folios", FSHAction.Export, FSHResource.Folios),
        new("View Expensecategories", FSHAction.View, FSHResource.Expensecategories, IsBasic: true),
        new("Search Expensecategories", FSHAction.Search, FSHResource.Expensecategories, IsBasic: true),
        new("Create Expensecategories", FSHAction.Create, FSHResource.Expensecategories),
        new("Update Expensecategories", FSHAction.Update, FSHResource.Expensecategories),
        new("Delete Expensecategories", FSHAction.Delete, FSHResource.Expensecategories),
        new("Generate Expensecategories", FSHAction.Generate, FSHResource.Expensecategories),
        new("Clean Expensecategories", FSHAction.Clean, FSHResource.Expensecategories),
        new("Export Expensecategories", FSHAction.Export, FSHResource.Expensecategories),
        new("View Customerclassifications", FSHAction.View, FSHResource.Customerclassifications, IsBasic: true),
        new("Search Customerclassifications", FSHAction.Search, FSHResource.Customerclassifications, IsBasic: true),
        new("Create Customerclassifications", FSHAction.Create, FSHResource.Customerclassifications),
        new("Update Customerclassifications", FSHAction.Update, FSHResource.Customerclassifications),
        new("Delete Customerclassifications", FSHAction.Delete, FSHResource.Customerclassifications),
        new("Generate Customerclassifications", FSHAction.Generate, FSHResource.Customerclassifications),
        new("Clean Customerclassifications", FSHAction.Clean, FSHResource.Customerclassifications),
        new("Export Customerclassifications", FSHAction.Export, FSHResource.Customerclassifications),
        new("View Customers", FSHAction.View, FSHResource.Customers, IsBasic: true),
        new("Search Customers", FSHAction.Search, FSHResource.Customers, IsBasic: true),
        new("Create Customers", FSHAction.Create, FSHResource.Customers),
        new("Update Customers", FSHAction.Update, FSHResource.Customers),
        new("Delete Customers", FSHAction.Delete, FSHResource.Customers),
        new("Generate Customers", FSHAction.Generate, FSHResource.Customers),
        new("Clean Customers", FSHAction.Clean, FSHResource.Customers),
        new("Export Customers", FSHAction.Export, FSHResource.Customers),
        new("View Charges", FSHAction.View, FSHResource.Charges, IsBasic: true),
        new("Search Charges", FSHAction.Search, FSHResource.Charges, IsBasic: true),
        new("Create Charges", FSHAction.Create, FSHResource.Charges),
        new("Update Charges", FSHAction.Update, FSHResource.Charges),
        new("Delete Charges", FSHAction.Delete, FSHResource.Charges),
        new("Generate Charges", FSHAction.Generate, FSHResource.Charges),
        new("Clean Charges", FSHAction.Clean, FSHResource.Charges),
        new("Export Charges", FSHAction.Export, FSHResource.Charges),
        new("View Bookings", FSHAction.View, FSHResource.Bookings, IsBasic: true),
        new("Search Bookings", FSHAction.Search, FSHResource.Bookings, IsBasic: true),
        new("Create Bookings", FSHAction.Create, FSHResource.Bookings),
        new("Update Bookings", FSHAction.Update, FSHResource.Bookings),
        new("Delete Bookings", FSHAction.Delete, FSHResource.Bookings),
        new("Generate Bookings", FSHAction.Generate, FSHResource.Bookings),
        new("Clean Bookings", FSHAction.Clean, FSHResource.Bookings),
        new("Export Bookings", FSHAction.Export, FSHResource.Bookings),
        new("View Bookingstatuses", FSHAction.View, FSHResource.Bookingstatuses, IsBasic: true),
        new("Search Bookingstatuses", FSHAction.Search, FSHResource.Bookingstatuses, IsBasic: true),
        new("Create Bookingstatuses", FSHAction.Create, FSHResource.Bookingstatuses),
        new("Update Bookingstatuses", FSHAction.Update, FSHResource.Bookingstatuses),
        new("Delete Bookingstatuses", FSHAction.Delete, FSHResource.Bookingstatuses),
        new("Generate Bookingstatuses", FSHAction.Generate, FSHResource.Bookingstatuses),
        new("Clean Bookingstatuses", FSHAction.Clean, FSHResource.Bookingstatuses),
        new("Export Bookingstatuses", FSHAction.Export, FSHResource.Bookingstatuses),
        new("View Accountentries", FSHAction.View, FSHResource.Accountentries, IsBasic: true),
        new("Search Accountentries", FSHAction.Search, FSHResource.Accountentries, IsBasic: true),
        new("Create Accountentries", FSHAction.Create, FSHResource.Accountentries),
        new("Update Accountentries", FSHAction.Update, FSHResource.Accountentries),
        new("Delete Accountentries", FSHAction.Delete, FSHResource.Accountentries),
        new("Generate Accountentries", FSHAction.Generate, FSHResource.Accountentries),
        new("Clean Accountentries", FSHAction.Clean, FSHResource.Accountentries),
        new("Export Accountentries", FSHAction.Export, FSHResource.Accountentries)
    };

    public static IReadOnlyList<FSHPermission> All { get; } = new ReadOnlyCollection<FSHPermission>(_all);
    public static IReadOnlyList<FSHPermission> Root { get; } = new ReadOnlyCollection<FSHPermission>(_all.Where(p => p.IsRoot).ToArray());
    public static IReadOnlyList<FSHPermission> Admin { get; } = new ReadOnlyCollection<FSHPermission>(_all.Where(p => !p.IsRoot).ToArray());
    public static IReadOnlyList<FSHPermission> Basic { get; } = new ReadOnlyCollection<FSHPermission>(_all.Where(p => p.IsBasic).ToArray());
}

public record FSHPermission(string Description, string Action, string Resource, bool IsBasic = false, bool IsRoot = false)
{
    public string Name => NameFor(Action, Resource);
    public static string NameFor(string action, string resource) => $"Permissions.{resource}.{action}";
}
