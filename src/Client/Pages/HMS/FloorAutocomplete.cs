using FSH.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using FSH.BlazorWebAssembly.Client.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace FSH.BlazorWebAssembly.Client.Pages.HMS;

public class FloorAutocomplete : MudAutocomplete<Guid>
{
    [Inject]
    private IStringLocalizer<FloorAutocomplete> L { get; set; } = default!;
    [Inject]
    private IFloorsClient FloorsClient { get; set; } = default!;
    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<FloorDto> _floors = new();

    // supply default parameters, but leave the possibility to override them
    public override Task SetParametersAsync(ParameterView parameters)
    {
        Label = "Floor";
        Variant = Variant.Filled;
        Dense = true;
        Margin = Margin.Dense;
        ResetValueOnEmptyText = true;
        SearchFunc = SearchFloors;
        ToStringFunc = GetFloorName;
        Clearable = true;
        return base.SetParametersAsync(parameters);
    }

    // when the value parameter is set, we have to load that one floor to be able to show the name
    // we can't do that in OnInitialized because of a strange bug (https://github.com/MudBlazor/MudBlazor/issues/3818)
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender &&
            _value != default &&
            await ApiHelper.ExecuteCallGuardedAsync(
                () => FloorsClient.GetAsync(_value), Snackbar) is { } floor)
        {
            _floors.Add(floor);
            ForceRender(true);
        }
    }

    private async Task<IEnumerable<Guid>> SearchFloors(string value)
    {
        var filter = new SearchFloorsRequest
        {
            PageSize = 10,
            AdvancedSearch = new() { Fields = new[] { "name" }, Keyword = value }
        };

        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => FloorsClient.SearchAsync(filter), Snackbar)
            is PaginationResponseOfFloorDto response)
        {
            _floors = response.Data.ToList();
        }

        return _floors.Select(x => x.Id);
    }

    private string GetFloorName(Guid id) =>
        _floors.Find(b => b.Id == id)?.Name ?? string.Empty;
}