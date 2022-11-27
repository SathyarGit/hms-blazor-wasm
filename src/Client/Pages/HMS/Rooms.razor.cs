using FSH.BlazorWebAssembly.Client.Components.EntityTable;
using FSH.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using FSH.BlazorWebAssembly.Client.Infrastructure.Common;
using FSH.WebApi.Shared.Authorization;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace FSH.BlazorWebAssembly.Client.Pages.HMS;

public partial class Rooms
{
    [Inject]
    protected IRoomsClient RoomsClient { get; set; } = default!;
    [Inject]
    protected IFloorsClient FloorsClient { get; set; } = default!;
    protected IRoomtypesClient RoomtypesClient { get; set; } = default!;

    protected EntityServerTableContext<RoomDto, Guid, RoomViewModel> Context { get; set; } = default!;

    private EntityTable<RoomDto, Guid, RoomViewModel>? _table = default!;

    protected override void OnInitialized() =>
        Context = new(
            entityName: "Room",
            entityNamePlural: "Rooms",
            entityResource: FSHResource.Rooms,
            fields: new()
            {
                new(room => room.Id, L["Id"], "Id"),
                new(room => room.RoomNumber, "RoomNumber", "RoomNumber"),
                new(room => room.NumberOfBeds, "NumberOfBeds", "NumberOfBeds"),
                new(room => room.Notes, "Notes", "Notes"),
                new(room => room.MaintenanceNotes, "MaintenanceNotes", "MaintenanceNotes"),
                new(room => room.FloorName, "Floor", "Floor.Name"),
                new(room => room.RoomtypeName, "Roomtype", "Roomtype.Name")
            },
            enableAdvancedSearch: true,
            idFunc: room => room.Id,
            searchFunc: async filter =>
            {
                var roomFilter = filter.Adapt<SearchRoomsRequest>();

                roomFilter.FloorId = SearchFloorId == default ? null : SearchFloorId;
                roomFilter.RoomtypeId = SearchRoomtypeId == default ? null : SearchRoomtypeId;
                var result = await RoomsClient.SearchAsync(roomFilter);
                return result.Adapt<PaginationResponse<RoomDto>>();
            },
            createFunc: async room =>
            {
                if (!string.IsNullOrEmpty(room.ImageInBytes))
                {
                    room.Image = new FileUploadRequest() { Data = room.ImageInBytes, Extension = room.ImageExtension ?? string.Empty, Name = $"{room.RoomNumber}_{Guid.NewGuid():N}" };
                }

                await RoomsClient.CreateAsync(room.Adapt<CreateRoomRequest>());
                room.ImageInBytes = string.Empty;
            },
            updateFunc: async (id, room) =>
            {
                if (!string.IsNullOrEmpty(room.ImageInBytes))
                {
                    room.DeleteCurrentImage = true;
                    room.Image = new FileUploadRequest() { Data = room.ImageInBytes, Extension = room.ImageExtension ?? string.Empty, Name = $"{room.RoomNumber}_{Guid.NewGuid():N}" };
                }

                await RoomsClient.UpdateAsync(id, room.Adapt<UpdateRoomRequest>());
                room.ImageInBytes = string.Empty;
            },
            exportFunc: async filter =>
            {
                var exportFilter = filter.Adapt<ExportRoomsRequest>();

                exportFilter.FloorId = SearchFloorId == default ? null : SearchFloorId;

                return await RoomsClient.ExportAsync(exportFilter);
            },
            deleteFunc: async id => await RoomsClient.DeleteAsync(id));

    // Advanced Search

    private Guid _searchFloorId;
    private Guid SearchFloorId
    {
        get => _searchFloorId;
        set
        {
            _searchFloorId = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private Guid _searchRoomtypeId;
    private Guid SearchRoomtypeId
    {
        get => _searchRoomtypeId;
        set
        {
            _searchRoomtypeId = value;
            _ = _table.ReloadDataAsync();
        }
    }

    // TODO : Make this as a shared service or something? Since it's used by Profile Component also for now, and literally any other component that will have image upload.
    // The new service should ideally return $"data:{ApplicationConstants.StandardImageFormat};base64,{Convert.ToBase64String(buffer)}"
    private async Task UploadFiles(InputFileChangeEventArgs e)
    {
        if (e.File != null)
        {
            string? extension = Path.GetExtension(e.File.Name);
            if (!ApplicationConstants.SupportedImageFormats.Contains(extension.ToLower()))
            {
                Snackbar.Add("Image Format Not Supported.", Severity.Error);
                return;
            }

            Context.AddEditModal.RequestModel.ImageExtension = extension;
            var imageFile = await e.File.RequestImageFileAsync(ApplicationConstants.StandardImageFormat, ApplicationConstants.MaxImageWidth, ApplicationConstants.MaxImageHeight);
            byte[]? buffer = new byte[imageFile.Size];
            await imageFile.OpenReadStream(ApplicationConstants.MaxAllowedSize).ReadAsync(buffer);
            Context.AddEditModal.RequestModel.ImageInBytes = $"data:{ApplicationConstants.StandardImageFormat};base64,{Convert.ToBase64String(buffer)}";
            Context.AddEditModal.ForceRender();
        }
    }

    public void ClearImageInBytes()
    {
        Context.AddEditModal.RequestModel.ImageInBytes = string.Empty;
        Context.AddEditModal.ForceRender();
    }

    public void SetDeleteCurrentImageFlag()
    {
        Context.AddEditModal.RequestModel.ImageInBytes = string.Empty;
        Context.AddEditModal.RequestModel.ImagePath = string.Empty;
        Context.AddEditModal.RequestModel.DeleteCurrentImage = true;
        Context.AddEditModal.ForceRender();
    }
}

public class RoomViewModel : UpdateRoomRequest
{
    public string? ImagePath { get; set; }
    public string? ImageInBytes { get; set; }
    public string? ImageExtension { get; set; }
}