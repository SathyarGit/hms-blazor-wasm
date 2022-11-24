using FSH.BlazorWebAssembly.Client.Components.EntityTable;
using FSH.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using FSH.BlazorWebAssembly.Client.Infrastructure.Common;
using FSH.WebApi.Shared.Authorization;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace FSH.BlazorWebAssembly.Client.Pages.HMS;

public partial class Travelagents
{
    [Inject]
    protected ITravelagentsClient TravelagentsClient { get; set; } = default!;

    protected EntityServerTableContext<TravelagentDto, Guid, TravelagentViewModel> Context { get; set; } = default!;

    private EntityTable<TravelagentDto, Guid, TravelagentViewModel> _table = default!;

    protected override void OnInitialized() =>
        Context = new(
            entityName: "Travelagent",
            entityNamePlural: "Travelagents",
            entityResource: FSHResource.Travelagents,
            fields: new()
            {
                new(travelagent => travelagent.Id, L["Id"], "Id"),
                new(travelagent => travelagent.Name, "Name", "Name"),
                new(travelagent => travelagent.AddressLine1, "AddressLine1", "Address Line1"),
                new(travelagent => travelagent.AddressLine2, "AddressLine2", "Address Line2"),
                new(travelagent => travelagent.City, "City", "City"),
                new(travelagent => travelagent.Country, "Country", "Country"),
                new(travelagent => travelagent.Pincode, "Pincode", "Pincode"),
                new(travelagent => travelagent.PhoneNumber, "PhoneNumber", "Phone Number"),
                new(travelagent => travelagent.Email, "Email", "Email"),
                new(travelagent => travelagent.Notes, "Notes", "Notes"),
            },
            enableAdvancedSearch: false,
            idFunc: travelagent => travelagent.Id,
            searchFunc: async filter => (await TravelagentsClient
                .SearchAsync(filter.Adapt<SearchTravelagentsRequest>()))
                .Adapt<PaginationResponse<TravelagentDto>>(),
            createFunc: async travelagent => await TravelagentsClient.CreateAsync(travelagent.Adapt<CreateTravelagentRequest>()),
            updateFunc: async (id, travelagent) => await TravelagentsClient.UpdateAsync(id, travelagent.Adapt<UpdateTravelagentRequest>()),
            exportFunc: async filter =>
            {
                var exportFilter = filter.Adapt<ExportTravelagentsRequest>();
                return await TravelagentsClient.ExportAsync(exportFilter);
            },
            deleteFunc: async id => await TravelagentsClient.DeleteAsync(id));
}

public class TravelagentViewModel : UpdateTravelagentRequest
{
}