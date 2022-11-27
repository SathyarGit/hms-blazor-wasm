using FSH.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using FSH.BlazorWebAssembly.Client.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace FSH.BlazorWebAssembly.Client.Pages.HMS;

public class TransactionstatusAutocomplete : MudAutocomplete<Guid>
{
    [Inject]
    private IStringLocalizer<TransactionstatusAutocomplete> L { get; set; } = default!;
    [Inject]
    private ITransactionstatusesClient TransactionstatusesClient { get; set; } = default!;
    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<TransactionstatusDto> _transactionstatuses = new();

    // supply default parameters, but leave the possibility to override them
    public override Task SetParametersAsync(ParameterView parameters)
    {
        Label = "Transactionstatus";
        Variant = Variant.Filled;
        Dense = true;
        Margin = Margin.Dense;
        ResetValueOnEmptyText = true;
        SearchFunc = SearchTransactionstatuses;
        ToStringFunc = GetTransactionstatusName;
        Clearable = true;
        return base.SetParametersAsync(parameters);
    }

    // when the value parameter is set, we have to load that one transactionstatus to be able to show the name
    // we can't do that in OnInitialized because of a strange bug (https://github.com/MudBlazor/MudBlazor/issues/3818)
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender &&
            _value != default &&
            await ApiHelper.ExecuteCallGuardedAsync(
                () => TransactionstatusesClient.GetAsync(_value), Snackbar) is { } transactionstatus)
        {
            _transactionstatuses.Add(transactionstatus);
            ForceRender(true);
        }
    }

    private async Task<IEnumerable<Guid>> SearchTransactionstatuses(string value)
    {
        var filter = new SearchTransactionstatusesRequest
        {
            PageSize = 10,
            AdvancedSearch = new() { Fields = new[] { "name" }, Keyword = value }
        };

        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => TransactionstatusesClient.SearchAsync(filter), Snackbar)
            is PaginationResponseOfTransactionstatusDto response)
        {
            _transactionstatuses = response.Data.ToList();
        }

        return _transactionstatuses.Select(x => x.Id);
    }

    private string GetTransactionstatusName(Guid id) =>
        _transactionstatuses.Find(b => b.Id == id)?.Name ?? string.Empty;
}