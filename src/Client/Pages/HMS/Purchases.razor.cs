using FSH.BlazorWebAssembly.Client.Components.EntityTable;
using FSH.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using FSH.BlazorWebAssembly.Client.Infrastructure.Common;
using FSH.WebApi.Shared.Authorization;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace FSH.BlazorWebAssembly.Client.Pages.HMS;

public partial class Purchases
{
    [Inject]
    protected IPurchasesClient PurchasesClient { get; set; } = default!;
    [Inject]
    protected IDepartmentsClient DepartmentsClient { get; set; } = default!;
    protected IVendorsClient VendorsClient { get; set; } = default!;
    protected ITransactionstatusesClient TransactionstatusesClient { get; set; } = default!;

    protected EntityServerTableContext<PurchaseDto, Guid, PurchaseViewModel> Context { get; set; } = default!;

    private EntityTable<PurchaseDto, Guid, PurchaseViewModel>? _table = default!;

    protected override void OnInitialized() =>
        Context = new(
            entityName: "Purchase",
            entityNamePlural: "Purchases",
            entityResource: FSHResource.Purchases,
            fields: new()
            {
                new(purchase => purchase.Id, L["Id"], "Id"),
                new(purchase => purchase.PurchaseDate, "PurchaseDate", "PurchaseDate"),
                new(purchase => purchase.Amount, "Amount", "Amount"),
                new(purchase => purchase.Notes, "Notes", "Notes"),
                new(purchase => purchase.BillsOrInvoiceNumber, "BillsOrInvoiceNumber", "BillsOrInvoiceNumber"),
                new(purchase => purchase.DepartmentName, "Department", "Department.Name"),
                new(purchase => purchase.TransactionstatusName, "Transactionstatus", "Transactionstatus.Name"),
                new(purchase => purchase.VendorName, "Vendor", "Vendor.Name")
            },
            enableAdvancedSearch: true,
            idFunc: purchase => purchase.Id,
            searchFunc: async filter =>
            {
                var purchaseFilter = filter.Adapt<SearchPurchasesRequest>();

                purchaseFilter.DepartmentId = SearchDepartmentId == default ? null : SearchDepartmentId;
                purchaseFilter.VendorId = SearchVendorId == default ? null : SearchVendorId;
                purchaseFilter.TransactionstatusId = SearchTransactionstatusId == default ? null : SearchTransactionstatusId;
                var result = await PurchasesClient.SearchAsync(purchaseFilter);
                return result.Adapt<PaginationResponse<PurchaseDto>>();
            },
            createFunc: async purchase =>
            {
                if (!string.IsNullOrEmpty(purchase.ImageInBytes))
                {
                    purchase.Image = new FileUploadRequest() { Data = purchase.ImageInBytes, Extension = purchase.ImageExtension ?? string.Empty, Name = $"{purchase.BillsOrInvoiceNumber}_{Guid.NewGuid():N}" };
                }

                await PurchasesClient.CreateAsync(purchase.Adapt<CreatePurchaseRequest>());
                purchase.ImageInBytes = string.Empty;
            },
            updateFunc: async (id, purchase) =>
            {
                if (!string.IsNullOrEmpty(purchase.ImageInBytes))
                {
                    purchase.DeleteCurrentImage = true;
                    purchase.Image = new FileUploadRequest() { Data = purchase.ImageInBytes, Extension = purchase.ImageExtension ?? string.Empty, Name = $"{purchase.BillsOrInvoiceNumber}_{Guid.NewGuid():N}" };
                }

                await PurchasesClient.UpdateAsync(id, purchase.Adapt<UpdatePurchaseRequest>());
                purchase.ImageInBytes = string.Empty;
            },
            exportFunc: async filter =>
            {
                var exportFilter = filter.Adapt<ExportPurchasesRequest>();

                exportFilter.DepartmentId = SearchDepartmentId == default ? null : SearchDepartmentId;

                return await PurchasesClient.ExportAsync(exportFilter);
            },
            deleteFunc: async id => await PurchasesClient.DeleteAsync(id));

    // Advanced Search

    private Guid _searchDepartmentId;
    private Guid SearchDepartmentId
    {
        get => _searchDepartmentId;
        set
        {
            _searchDepartmentId = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private Guid _searchVendorId;
    private Guid SearchVendorId
    {
        get => _searchVendorId;
        set
        {
            _searchVendorId = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private Guid _searchTransactionstatusId;
    private Guid SearchTransactionstatusId
    {
        get => _searchTransactionstatusId;
        set
        {
            _searchTransactionstatusId = value;
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

public class PurchaseViewModel : UpdatePurchaseRequest
{
    public string? ImagePath { get; set; }
    public string? ImageInBytes { get; set; }
    public string? ImageExtension { get; set; }
}