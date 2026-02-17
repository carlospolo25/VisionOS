using Inventario.API.Application.DTOs.Reportes;
using Inventario.API.Application.Services.Reportes;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

public class PdfReporteService : IPdfReporteService
{
    public byte[] GenerarPdf(ReporteInventarioDto reporte)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);

                page.Header()
                    .Text("Reporte General de Inventario")
                    .FontSize(20)
                    .Bold();

                page.Content().Column(col =>
                {
                    col.Spacing(10);

                    col.Item().Text($"Total Productos: {reporte.TotalProductos}");
                    col.Item().Text($"Activos: {reporte.Activos}");
                    col.Item().Text($"Inactivos: {reporte.Inactivos}");
                    col.Item().Text($"Stock Crítico: {reporte.StockCritico}");
                    col.Item().Text($"Sin Stock: {reporte.SinStock}");

                    col.Item().Text($"Valor Inventario: ${reporte.ValorInventario:N2}");

                    col.Item().Text(" ");

                    col.Item().Text("Productos en Riesgo").Bold();

                    foreach (var p in reporte.ProductosEnRiesgo)
                    {
                        col.Item().Text($"{p.Nombre} - Stock: {p.Stock}");
                    }
                });

                page.Footer()
                    .AlignCenter()
                    .Text($"Generado: {DateTime.Now}");
            });
        })
        .GeneratePdf();
    }
}
