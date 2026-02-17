using Inventario.API.Application.DTOs.Dashboard;
using Inventario.API.Domain.Entities;

namespace Inventario.API.Application.Services.Dashboard;
public interface IDashboardService
{
    Task<DashboardDto> Productos();
}
