using FSH.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using FSH.BlazorWebAssembly.Client.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace FSH.BlazorWebAssembly.Client.Pages.HMS;

public class VendorAutocomplete : MudAutocomplete<Guid>
{
    [Inject]
    private IStringLocalizer<VendorAutocomplete> L { get; set; } = default!;
    [Inject]
    private IVendorsClient VendorsClient { get; set; } = default!;
    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<VendorDto> _vendors = new();

    // supply default parameters, but leave the possibility to override them
    public override Task SetParametersAsync(ParameterView parameters)
    {
        Label = "Vendor";
        Variant = Variant.Filled;
        Dense = true;
        Margin = Margin.Dense;
        ResetValueOnEmptyText = true;
        SearchFunc = SearchVendors;
        ToStringFunc = GetVendorName;
        Clearable = true;
        return base.SetParametersAsync(parameters);
    }

    // when the value parameter is set, we have to load that one vendor to be able to show the name
    // we can't do that in OnInitialized because of a strange bug (https://github.com/MudBlazor/MudBlazor/issues/3818)
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender &&
            _value != default &&
            await ApiHelper.ExecuteCallGuardedAsync(
                () => VendorsClient.GetAsync(_value), Snackbar) is { } vendor)
        {
            _vendors.Add(vendor);
            ForceRender(true);
        }
    }

    private async Task<IEnumerable<Guid>> SearchVendors(string value)
    {
        var filter = new SearchVendorsRequest
        {
            PageSize = 10,
            AdvancedSearch = new() { Fields = new[] { "name" }, Keyword = value }
        };

        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => VendorsClient.SearchAsync(filter), Snackbar)
            is PaginationResponseOfVendorDto response)
        {
            _vendors = response.Data.ToList();
        }

        return _vendors.Select(x => x.Id);
    }

    private string GetVendorName(Guid id) =>
        _vendors.Find(b => b.Id == id)?.Name ?? string.Empty;
}