using FSH.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using FSH.BlazorWebAssembly.Client.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace FSH.BlazorWebAssembly.Client.Pages.HMS;

public class RoomtypeAutocomplete : MudAutocomplete<Guid>
{
    [Inject]
    private IStringLocalizer<RoomtypeAutocomplete> L { get; set; } = default!;
    [Inject]
    private IRoomtypesClient RoomtypesClient { get; set; } = default!;
    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<RoomtypeDto> _roomtypes = new();

    // supply default parameters, but leave the possibility to override them
    public override Task SetParametersAsync(ParameterView parameters)
    {
        Label = "Roomtype";
        Variant = Variant.Filled;
        Dense = true;
        Margin = Margin.Dense;
        ResetValueOnEmptyText = true;
        SearchFunc = SearchRoomtypes;
        ToStringFunc = GetRoomtypeName;
        Clearable = true;
        return base.SetParametersAsync(parameters);
    }

    // when the value parameter is set, we have to load that one roomtype to be able to show the name
    // we can't do that in OnInitialized because of a strange bug (https://github.com/MudBlazor/MudBlazor/issues/3818)
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender &&
            _value != default &&
            await ApiHelper.ExecuteCallGuardedAsync(
                () => RoomtypesClient.GetAsync(_value), Snackbar) is { } roomtype)
        {
            _roomtypes.Add(roomtype);
            ForceRender(true);
        }
    }

    private async Task<IEnumerable<Guid>> SearchRoomtypes(string value)
    {
        var filter = new SearchRoomtypesRequest
        {
            PageSize = 10,
            AdvancedSearch = new() { Fields = new[] { "name" }, Keyword = value }
        };

        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => RoomtypesClient.SearchAsync(filter), Snackbar)
            is PaginationResponseOfRoomtypeDto response)
        {
            _roomtypes = response.Data.ToList();
        }

        return _roomtypes.Select(x => x.Id);
    }

    private string GetRoomtypeName(Guid id) =>
        _roomtypes.Find(b => b.Id == id)?.Name ?? string.Empty;
}