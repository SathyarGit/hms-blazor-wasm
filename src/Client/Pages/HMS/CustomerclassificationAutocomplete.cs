using FSH.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using FSH.BlazorWebAssembly.Client.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace FSH.BlazorWebAssembly.Client.Pages.HMS;

public class CustomerclassificationAutocomplete : MudAutocomplete<Guid>
{
    [Inject]
    private IStringLocalizer<CustomerclassificationAutocomplete> L { get; set; } = default!;
    [Inject]
    private ICustomerclassificationsClient CustomerclassificationsClient { get; set; } = default!;
    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<CustomerclassificationDto> _customerclassifications = new();

    // supply default parameters, but leave the possibility to override them
    public override Task SetParametersAsync(ParameterView parameters)
    {
        Label = "Customerclassification";
        Variant = Variant.Filled;
        Dense = true;
        Margin = Margin.Dense;
        ResetValueOnEmptyText = true;
        SearchFunc = SearchCustomerclassifications;
        ToStringFunc = GetCustomerclassificationName;
        Clearable = true;
        return base.SetParametersAsync(parameters);
    }

    // when the value parameter is set, we have to load that one customerclassification to be able to show the name
    // we can't do that in OnInitialized because of a strange bug (https://github.com/MudBlazor/MudBlazor/issues/3818)
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender &&
            _value != default &&
            await ApiHelper.ExecuteCallGuardedAsync(
                () => CustomerclassificationsClient.GetAsync(_value), Snackbar) is { } customerclassification)
        {
            _customerclassifications.Add(customerclassification);
            ForceRender(true);
        }
    }

    private async Task<IEnumerable<Guid>> SearchCustomerclassifications(string value)
    {
        var filter = new SearchCustomerclassificationsRequest
        {
            PageSize = 10,
            AdvancedSearch = new() { Fields = new[] { "name" }, Keyword = value }
        };

        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => CustomerclassificationsClient.SearchAsync(filter), Snackbar)
            is PaginationResponseOfCustomerclassificationDto response)
        {
            _customerclassifications = response.Data.ToList();
        }

        return _customerclassifications.Select(x => x.Id);
    }

    private string GetCustomerclassificationName(Guid id) =>
        _customerclassifications.Find(b => b.Id == id)?.Name ?? string.Empty;
}