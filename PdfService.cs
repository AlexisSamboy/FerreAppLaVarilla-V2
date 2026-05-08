using FerreAppLaVarilla.UI.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace FerreAppLaVarilla.UI.Services
{
    public class PdfService
    {
        public byte[] GenerarFactura(Factura factura)
        {
            using var stream = new MemoryStream();

            PdfWriter writer = new PdfWriter(stream);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            // TITULO
            Paragraph titulo = new Paragraph("FACTURA");
            titulo.SetFontSize(24);

            document.Add(titulo);

            // DATOS
            document.Add(new Paragraph("Factura: " + factura.NumeroFactura));
            document.Add(new Paragraph("Cliente: " + factura.Cliente));
            document.Add(new Paragraph("Cédula: " + factura.CedulaRnc));
            document.Add(new Paragraph("Fecha: " + factura.Fecha.ToString("dd/MM/yyyy")));

            document.Add(new Paragraph(" "));

            // TABLA
            Table table = new Table(4);

            table.AddHeaderCell("Producto");
            table.AddHeaderCell("Cantidad");
            table.AddHeaderCell("Precio");
            table.AddHeaderCell("Subtotal");

            foreach (var item in factura.Detalles)
            {
                table.AddCell(item.Producto);
                table.AddCell(item.Cantidad.ToString());
                table.AddCell("RD$ " + item.Precio.ToString("N2"));
                table.AddCell("RD$ " + item.Subtotal.ToString("N2"));
            }

            document.Add(table);

            document.Add(new Paragraph(" "));

            Paragraph total = new Paragraph("TOTAL: RD$ " + factura.Total.ToString("N2"));
            total.SetFontSize(18);

            document.Add(total);

            document.Close();

            return stream.ToArray();
        }
    }
}