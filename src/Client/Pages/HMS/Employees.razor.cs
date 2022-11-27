using FSH.BlazorWebAssembly.Client.Components.EntityTable;
using FSH.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using FSH.BlazorWebAssembly.Client.Infrastructure.Common;
using FSH.WebApi.Shared.Authorization;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace FSH.BlazorWebAssembly.Client.Pages.HMS;

public partial class Employees
{
    [Inject]
    protected IEmployeesClient EmployeesClient { get; set; } = default!;
    [Inject]
    protected IDepartmentsClient DepartmentsClient { get; set; } = default!;

    protected EntityServerTableContext<EmployeeDto, Guid, EmployeeViewModel> Context { get; set; } = default!;

    private EntityTable<EmployeeDto, Guid, EmployeeViewModel>? _table = default!;

    protected override void OnInitialized() =>
        Context = new(
            entityName: "Employee",
            entityNamePlural: "Employees",
            entityResource: FSHResource.Employees,
            fields: new()
            {
                new(employee => employee.Id, L["Id"], "Id"),
                new(employee => employee.Name, "Name", "Name"),
                new(employee => employee.Address, "Address", "Address"),
                new(employee => employee.Notes, "Notes", "Notes"),
                new(employee => employee.DepartmentName, "Department Name", "Department.Name"),
            },
            enableAdvancedSearch: true,
            idFunc: employee => employee.Id,
            searchFunc: async filter =>
            {
                var employeeFilter = filter.Adapt<SearchEmployeesRequest>();

                employeeFilter.DepartmentId = SearchDepartmentId == default ? null : SearchDepartmentId;
                var result = await EmployeesClient.SearchAsync(employeeFilter);
                return result.Adapt<PaginationResponse<EmployeeDto>>();
            },
            createFunc: async employee =>
            {
                if (!string.IsNullOrEmpty(employee.ImageInBytes))
                {
                    employee.Image = new FileUploadRequest() { Data = employee.ImageInBytes, Extension = employee.ImageExtension ?? string.Empty, Name = $"{employee.Name}_{Guid.NewGuid():N}" };
                }

                await EmployeesClient.CreateAsync(employee.Adapt<CreateEmployeeRequest>());
                employee.ImageInBytes = string.Empty;
            },
            updateFunc: async (id, employee) =>
            {
                if (!string.IsNullOrEmpty(employee.ImageInBytes))
                {
                    employee.DeleteCurrentImage = true;
                    employee.Image = new FileUploadRequest() { Data = employee.ImageInBytes, Extension = employee.ImageExtension ?? string.Empty, Name = $"{employee.Name}_{Guid.NewGuid():N}" };
                }

                await EmployeesClient.UpdateAsync(id, employee.Adapt<UpdateEmployeeRequest>());
                employee.ImageInBytes = string.Empty;
            },
            exportFunc: async filter =>
            {
                var exportFilter = filter.Adapt<ExportEmployeesRequest>();

                exportFilter.DepartmentId = SearchDepartmentId == default ? null : SearchDepartmentId;

                return await EmployeesClient.ExportAsync(exportFilter);
            },
            deleteFunc: async id => await EmployeesClient.DeleteAsync(id));

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

public class EmployeeViewModel : UpdateEmployeeRequest
{
    public string? ImagePath { get; set; }
    public string? ImageInBytes { get; set; }
    public string? ImageExtension { get; set; }
}