using FSH.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using FSH.BlazorWebAssembly.Client.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace FSH.BlazorWebAssembly.Client.Pages.HMS;

public class DepartmentAutocomplete : MudAutocomplete<Guid>
{
    [Inject]
    private IStringLocalizer<DepartmentAutocomplete> L { get; set; } = default!;
    [Inject]
    private IDepartmentsClient DepartmentsClient { get; set; } = default!;
    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<DepartmentDto> _departments = new();

    // supply default parameters, but leave the possibility to override them
    public override Task SetParametersAsync(ParameterView parameters)
    {
        Label = "Department";
        Variant = Variant.Filled;
        Dense = true;
        Margin = Margin.Dense;
        ResetValueOnEmptyText = true;
        SearchFunc = SearchDepartments;
        ToStringFunc = GetDepartmentName;
        Clearable = true;
        return base.SetParametersAsync(parameters);
    }

    // when the value parameter is set, we have to load that one department to be able to show the name
    // we can't do that in OnInitialized because of a strange bug (https://github.com/MudBlazor/MudBlazor/issues/3818)
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender &&
            _value != default &&
            await ApiHelper.ExecuteCallGuardedAsync(
                () => DepartmentsClient.GetAsync(_value), Snackbar) is { } department)
        {
            _departments.Add(department);
            ForceRender(true);
        }
    }

    private async Task<IEnumerable<Guid>> SearchDepartments(string value)
    {
        var filter = new SearchDepartmentsRequest
        {
            PageSize = 10,
            AdvancedSearch = new() { Fields = new[] { "name" }, Keyword = value }
        };

        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => DepartmentsClient.SearchAsync(filter), Snackbar)
            is PaginationResponseOfDepartmentDto response)
        {
            _departments = response.Data.ToList();
        }

        return _departments.Select(x => x.Id);
    }

    private string GetDepartmentName(Guid id) =>
        _departments.Find(b => b.Id == id)?.Name ?? string.Empty;
}