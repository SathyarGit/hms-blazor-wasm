using FSH.BlazorWebAssembly.Client.Components.EntityTable;
using FSH.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using FSH.BlazorWebAssembly.Client.Infrastructure.Common;
using FSH.WebApi.Shared.Authorization;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace FSH.BlazorWebAssembly.Client.Pages.HMS;

public partial class Customers
{
    [Inject]
    protected ICustomersClient CustomersClient { get; set; } = default!;
    [Inject]
    protected ICustomerclassificationsClient CustomerclassificationsClient { get; set; } = default!;

    protected EntityServerTableContext<CustomerDto, Guid, CustomerViewModel> Context { get; set; } = default!;

    private EntityTable<CustomerDto, Guid, CustomerViewModel>? _table = default!;

    protected override void OnInitialized() =>
        Context = new(
            entityName: "Customer",
            entityNamePlural: "Customers",
            entityResource: FSHResource.Customers,
            fields: new()
            {
                new(customer => customer.Id, L["Id"], "Id"),
                new(customer => customer.Name, "Name", "Name"),
                new(customer => customer.AddressLine1, "AddressLine1", "AddressLine1"),
                new(customer => customer.AddressLine2, "AddressLine2", "AddressLine2"),
                new(customer => customer.City, "City", "City"),
                new(customer => customer.Country, "Country", "Country"),
                new(customer => customer.Pincode, "Pincode", "Pincode"),
                new(customer => customer.PhoneNumber, "PhoneNumber", "PhoneNumber"),
                new(customer => customer.Email, "Email", "Email"),
                new(customer => customer.Notes, "Notes", "Notes"),
                new(customer => customer.CustomerclassificationName, "Customerclassification", "Customerclassification")
            },
            enableAdvancedSearch: true,
            idFunc: customer => customer.Id,
            searchFunc: async filter =>
            {
                var customerFilter = filter.Adapt<SearchCustomersRequest>();

                customerFilter.CustomerclassificationId = SearchCustclassificationId == default ? null : SearchCustclassificationId;
                var result = await CustomersClient.SearchAsync(customerFilter);
                return result.Adapt<PaginationResponse<CustomerDto>>();
            },
            createFunc: async customer =>
            {
                if (!string.IsNullOrEmpty(customer.ImageInBytes))
                {
                    customer.Image = new FileUploadRequest() { Data = customer.ImageInBytes, Extension = customer.ImageExtension ?? string.Empty, Name = $"{customer.Name}_{Guid.NewGuid():N}" };
                }

                await CustomersClient.CreateAsync(customer.Adapt<CreateCustomerRequest>());
                customer.ImageInBytes = string.Empty;
            },
            updateFunc: async (id, customer) =>
            {
                if (!string.IsNullOrEmpty(customer.ImageInBytes))
                {
                    customer.DeleteCurrentImage = true;
                    customer.Image = new FileUploadRequest() { Data = customer.ImageInBytes, Extension = customer.ImageExtension ?? string.Empty, Name = $"{customer.Name}_{Guid.NewGuid():N}" };
                }

                await CustomersClient.UpdateAsync(id, customer.Adapt<UpdateCustomerRequest>());
                customer.ImageInBytes = string.Empty;
            },
            exportFunc: async filter =>
            {
                var exportFilter = filter.Adapt<ExportCustomersRequest>();

                exportFilter.CustomerclassificationId = SearchCustclassificationId == default ? null : SearchCustclassificationId;

                return await CustomersClient.ExportAsync(exportFilter);
            },
            deleteFunc: async id => await CustomersClient.DeleteAsync(id));

    // Advanced Search

    private Guid _searchCustclassificationId;
    private Guid SearchCustclassificationId
    {
        get => _searchCustclassificationId;
        set
        {
            _searchCustclassificationId = value;
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

public class CustomerViewModel : UpdateCustomerRequest
{
    public string? ImagePath { get; set; }
    public string? ImageInBytes { get; set; }
    public string? ImageExtension { get; set; }
}