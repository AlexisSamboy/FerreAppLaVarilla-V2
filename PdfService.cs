using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using iText.Layout.Borders;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using FerreAppLaVarilla.UI.Models;

namespace FerreAppLaVarilla.UI.Services
{
    public class PdfService
    {
        // ==========================================
        // MÉTODO 1: GENERAR FACTURA
        // ==========================================
        public byte[] GenerarFactura(Factura factura)
        {
            using (var stream = new MemoryStream())
            {
                var writer = new PdfWriter(stream);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf, iText.Kernel.Geom.PageSize.A4);
                document.SetMargins(30, 30, 30, 30);

                PdfFont fontNormal = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                PdfFont fontBold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

                DeviceRgb colorAzulOscuro = new DeviceRgb(11, 49, 102);
                DeviceRgb colorAmarillo = new DeviceRgb(255, 184, 0);
                DeviceRgb colorRojo = new DeviceRgb(220, 53, 69);
                DeviceRgb colorGrisBorde = new DeviceRgb(222, 226, 230);
                DeviceRgb colorGrisClaro = new DeviceRgb(248, 249, 250);

                Table headerTable = new Table(UnitValue.CreatePercentArray(new float[] { 50, 50 })).UseAllAvailableWidth();

                Cell leftHeader = new Cell().SetBorder(Border.NO_BORDER);
                Paragraph logo = new Paragraph()
                    .Add(new Text("⚡ FerreApp\n").SetFont(fontBold).SetFontSize(24).SetFontColor(colorAzulOscuro))
                    .Add(new Text("La Varilla").SetFont(fontBold).SetFontSize(16).SetFontColor(colorAmarillo));

                leftHeader.Add(logo);
                leftHeader.Add(new Paragraph("Ferretería La Varilla, SRL")
                    .SetFont(fontBold).SetFontSize(12).SetFontColor(colorAzulOscuro));
                leftHeader.Add(new Paragraph("Venta de materiales de construcción, ferretería,\nherramientas, pinturas y artículos para el hogar.\n\nAv. Duarte #123, La Vega, República Dominicana\n809-555-0000\nventas@ferreapplavarilla.com\nRNC: 1-32-45678-9")
                    .SetFont(fontNormal).SetFontSize(8).SetFontColor(ColorConstants.DARK_GRAY));

                headerTable.AddCell(leftHeader);

                Cell rightHeader = new Cell().SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.RIGHT);
                rightHeader.Add(new Paragraph("FACTURA").SetFont(fontBold).SetFontSize(22).SetFontColor(colorAzulOscuro));
                rightHeader.Add(new Paragraph("COMPROBANTE FISCAL").SetFont(fontBold).SetFontSize(10).SetFontColor(colorAzulOscuro));
                rightHeader.Add(new Paragraph("B01-0000001234").SetFont(fontBold).SetFontSize(14).SetFontColor(colorRojo));

                Table metaTable = new Table(UnitValue.CreatePercentArray(new float[] { 50, 50 })).UseAllAvailableWidth().SetMarginTop(10);
                void AddMeta(string label, string value)
                {
                    metaTable.AddCell(new Cell().Add(new Paragraph(label).SetFont(fontBold).SetFontSize(8)).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));
                    metaTable.AddCell(new Cell().Add(new Paragraph(value).SetFont(fontNormal).SetFontSize(8)).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.RIGHT));
                }
                AddMeta("Número de Comprobante:", "B01-0000001234");
                AddMeta("Fecha de Emisión:", factura.Fecha.ToString("dd/MM/yyyy"));
                AddMeta("Hora de Emisión:", factura.Fecha.ToString("hh:mm:ss tt"));
                AddMeta("Moneda:", "DOP - Peso Dominicano");
                AddMeta("Condición de Pago:", "Contado");
                AddMeta("Vendedor:", "Caja Principal");
                rightHeader.Add(metaTable);

                headerTable.AddCell(rightHeader);
                document.Add(headerTable);
                document.Add(new Paragraph("\n").SetFontSize(5));

                Table blocksTable = new Table(UnitValue.CreatePercentArray(new float[] { 48, 4, 48 })).UseAllAvailableWidth();

                Table CrearBloque(string titulo, Dictionary<string, string> datos)
                {
                    Table tablaBox = new Table(1).UseAllAvailableWidth();
                    tablaBox.AddCell(new Cell().Add(new Paragraph(titulo).SetFont(fontBold).SetFontSize(9).SetFontColor(ColorConstants.WHITE))
                        .SetBackgroundColor(colorAzulOscuro).SetBorder(new SolidBorder(colorAzulOscuro, 1)).SetPadding(4));

                    Table contentTable = new Table(UnitValue.CreatePercentArray(new float[] { 40, 60 })).UseAllAvailableWidth();
                    foreach (var item in datos)
                    {
                        contentTable.AddCell(new Cell().Add(new Paragraph(item.Key).SetFont(fontBold).SetFontSize(8)).SetBorder(Border.NO_BORDER));
                        contentTable.AddCell(new Cell().Add(new Paragraph(item.Value).SetFont(fontNormal).SetFontSize(8)).SetBorder(Border.NO_BORDER));
                    }
                    tablaBox.AddCell(new Cell().Add(contentTable).SetBorder(new SolidBorder(colorGrisBorde, 1)).SetPadding(6));
                    return tablaBox;
                }

                var datosCliente = new Dictionary<string, string> {
                    { "Nombre/Razón Social:", factura.Cliente },
                    { "RNC/Cédula:", factura.CedulaRnc },
                    { "Dirección:", "Ciudad, Rep. Dom." },
                    { "Teléfono:", "N/A" }
                };
                blocksTable.AddCell(new Cell().Add(CrearBloque("DATOS DEL CLIENTE", datosCliente)).SetBorder(Border.NO_BORDER));
                blocksTable.AddCell(new Cell().SetBorder(Border.NO_BORDER));

                var datosFiscales = new Dictionary<string, string> {
                    { "NCF:", "B010000001234" },
                    { "Válido hasta:", "31/12/2026" },
                    { "Punto de Venta:", "01" },
                    { "Tipo de Impresión:", "Electrónica" }
                };
                blocksTable.AddCell(new Cell().Add(CrearBloque("DATOS FISCALES", datosFiscales)).SetBorder(Border.NO_BORDER));

                document.Add(blocksTable);
                document.Add(new Paragraph("\n").SetFontSize(5));

                Table itemsTable = new Table(UnitValue.CreatePercentArray(new float[] { 5, 15, 30, 8, 8, 12, 10, 12 })).UseAllAvailableWidth();
                itemsTable.SetMarginTop(10);

                string[] headers = { "#", "CÓDIGO", "DESCRIPCIÓN", "CANT.", "UNIDAD", "PRECIO UNIT.", "ITBIS %", "SUBTOTAL" };
                foreach (var h in headers)
                {
                    itemsTable.AddHeaderCell(new Cell().Add(new Paragraph(h).SetFont(fontBold).SetFontSize(8).SetFontColor(ColorConstants.WHITE))
                        .SetBackgroundColor(colorAzulOscuro).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.CENTER).SetPadding(5));
                }

                int index = 1;
                foreach (var item in factura.Detalles)
                {
                    Color bgColor = index % 2 == 0 ? colorGrisClaro : ColorConstants.WHITE;

                    void AddItemCell(string text, TextAlignment align)
                    {
                        itemsTable.AddCell(new Cell().Add(new Paragraph(text).SetFont(fontNormal).SetFontSize(8))
                            .SetBackgroundColor(bgColor).SetBorderBottom(new SolidBorder(colorGrisBorde, 1)).SetBorderTop(Border.NO_BORDER).SetBorderLeft(Border.NO_BORDER).SetBorderRight(Border.NO_BORDER)
                            .SetTextAlignment(align).SetPadding(6));
                    }

                    AddItemCell(index.ToString(), TextAlignment.CENTER);
                    AddItemCell($"PRD-{index:D3}", TextAlignment.CENTER);
                    AddItemCell(item.Producto, TextAlignment.LEFT);
                    AddItemCell(item.Cantidad.ToString(), TextAlignment.CENTER);
                    AddItemCell("UD", TextAlignment.CENTER);
                    AddItemCell($"RD$ {item.Precio:N2}", TextAlignment.RIGHT);
                    AddItemCell("18%", TextAlignment.CENTER);
                    AddItemCell($"RD$ {item.Subtotal:N2}", TextAlignment.RIGHT);
                    index++;
                }
                document.Add(itemsTable);

                Table bottomTable = new Table(UnitValue.CreatePercentArray(new float[] { 55, 45 })).UseAllAvailableWidth();
                bottomTable.SetMarginTop(15);

                Table sonBox = new Table(1).UseAllAvailableWidth();
                sonBox.AddCell(new Cell().Add(new Paragraph("SON:").SetFont(fontBold).SetFontSize(8)).SetBorder(Border.NO_BORDER));
                sonBox.AddCell(new Cell().Add(new Paragraph("VALOR TOTAL EXPRESADO EN PESOS DOMINICANOS.\n(Favor revisar su mercancía antes de salir)").SetFont(fontNormal).SetFontSize(8)).SetBorder(Border.NO_BORDER));
                bottomTable.AddCell(new Cell().Add(sonBox).SetBorder(new SolidBorder(colorGrisBorde, 1)).SetPadding(10));

                Table totalsTable = new Table(UnitValue.CreatePercentArray(new float[] { 50, 50 })).UseAllAvailableWidth();
                void AddTotalRow(string label, string value, bool isBold = false)
                {
                    totalsTable.AddCell(new Cell().Add(new Paragraph(label).SetFont(isBold ? fontBold : fontNormal).SetFontSize(9)).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT).SetPaddingBottom(4));
                    totalsTable.AddCell(new Cell().Add(new Paragraph(value).SetFont(isBold ? fontBold : fontNormal).SetFontSize(9)).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.RIGHT).SetPaddingBottom(4));
                }

                AddTotalRow("Subtotal:", $"RD$ {factura.Subtotal:N2}");
                AddTotalRow("Descuento:", "RD$ 0.00");
                AddTotalRow("ITBIS (18%):", $"RD$ {factura.Itbis:N2}");

                decimal totalPagar = factura.Subtotal + factura.Itbis;
                totalsTable.AddCell(new Cell().Add(new Paragraph("TOTAL A PAGAR:").SetFont(fontBold).SetFontSize(10).SetFontColor(ColorConstants.WHITE)).SetBackgroundColor(colorAzulOscuro).SetBorder(Border.NO_BORDER).SetPadding(6));
                totalsTable.AddCell(new Cell().Add(new Paragraph($"RD$ {totalPagar:N2}").SetFont(fontBold).SetFontSize(10).SetFontColor(ColorConstants.WHITE)).SetBackgroundColor(colorAzulOscuro).SetBorder(Border.NO_BORDER).SetPadding(6).SetTextAlignment(TextAlignment.RIGHT));

                bottomTable.AddCell(new Cell().Add(totalsTable).SetBorder(Border.NO_BORDER).SetPaddingLeft(20));
                document.Add(bottomTable);

                document.Add(new Paragraph("\nSello Digital DGII\n").SetFont(fontBold).SetFontSize(9).SetFontColor(colorAzulOscuro));
                document.Add(new Paragraph("MIIFdzCCBF+gAwIBAgIUMDAwMDAwMDAwMDAwMDAwMDAwDQYJKoZIhvcNAQELBQAwggGEMS\nAwHgYDVQQDDBdVTOURIDAUGQCERVEIFIIKBAQCIOTIBEGIIIENQASEIDIXEJAQBg... (Firma Digital Electrónica Avanzada)")
                    .SetFont(fontNormal).SetFontSize(7).SetFontColor(ColorConstants.DARK_GRAY));

                document.Add(new Paragraph("\n¡Gracias por su compra!").SetFont(fontBold).SetFontSize(12).SetTextAlignment(TextAlignment.CENTER));
                document.Add(new Paragraph("Este documento es una representación impresa de un Comprobante Fiscal Digital.\nVálido para crédito fiscal. Conserve este comprobante.")
                    .SetFont(fontNormal).SetFontSize(8).SetTextAlignment(TextAlignment.CENTER).SetFontColor(ColorConstants.GRAY));

                Table yellowFooter = new Table(1).UseAllAvailableWidth().SetMarginTop(10);
                yellowFooter.AddCell(new Cell().Add(new Paragraph("Síguenos en @ferreapplavarilla | Calidad, confianza y servicio para construir tus proyectos.").SetFont(fontNormal).SetFontSize(8).SetTextAlignment(TextAlignment.CENTER)).SetBackgroundColor(colorAmarillo).SetBorder(Border.NO_BORDER).SetPadding(5));
                document.Add(yellowFooter);

                document.Close();
                return stream.ToArray();
            }
        }

        // ==========================================
        // MÉTODO 2: GENERAR COTIZACIÓN
        // ==========================================
        public byte[] GenerarCotizacionPdf(Factura cotizacion)
        {
            using (var stream = new MemoryStream())
            {
                var writer = new PdfWriter(stream);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf, iText.Kernel.Geom.PageSize.A4);
                document.SetMargins(30, 30, 30, 30);

                // IMPORTANTE: Aquí creamos la fuente cursiva/oblicua para arreglar el error SetItalic
                PdfFont fontNormal = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                PdfFont fontBold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                PdfFont fontItalic = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_OBLIQUE);

                DeviceRgb colorAzulOscuro = new DeviceRgb(11, 49, 102);
                DeviceRgb colorAmarillo = new DeviceRgb(255, 184, 0);
                DeviceRgb colorGrisBorde = new DeviceRgb(222, 226, 230);
                DeviceRgb colorGrisClaro = new DeviceRgb(248, 249, 250);

                Table headerTable = new Table(UnitValue.CreatePercentArray(new float[] { 45, 55 })).UseAllAvailableWidth();

                Cell leftHeader = new Cell().SetBorder(Border.NO_BORDER);
                leftHeader.Add(new Paragraph().Add(new Text("⚡ FerreApp\n").SetFont(fontBold).SetFontSize(24).SetFontColor(colorAzulOscuro))
                                              .Add(new Text("La Varilla").SetFont(fontBold).SetFontSize(16).SetFontColor(colorAmarillo)));
                leftHeader.Add(new Paragraph("Ferretería La Varilla, SRL\n").SetFont(fontBold).SetFontSize(12).SetFontColor(colorAzulOscuro)
                    .Add(new Text("Venta de materiales de construcción, ferretería,\nherramientas, pinturas y artículos para el hogar.\n\nAv. Duarte #123, La Vega, Rep. Dominicana\n809-555-0000\nventas@ferreapplavarilla.com\nRNC: 1-32-45678-9").SetFont(fontNormal).SetFontSize(8).SetFontColor(ColorConstants.DARK_GRAY)));
                headerTable.AddCell(leftHeader);

                Cell rightHeader = new Cell().SetBorder(Border.NO_BORDER);
                rightHeader.Add(new Paragraph("COTIZACIÓN").SetFont(fontBold).SetFontSize(24).SetFontColor(colorAzulOscuro).SetTextAlignment(TextAlignment.RIGHT));

                Table pillTable = new Table(1).SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.RIGHT).SetMarginBottom(10);
                pillTable.AddCell(new Cell().Add(new Paragraph($"No. {cotizacion.NumeroFactura}").SetFont(fontBold).SetFontSize(10).SetFontColor(ColorConstants.WHITE))
                    .SetBackgroundColor(colorAzulOscuro).SetBorder(Border.NO_BORDER).SetPaddingLeft(10).SetPaddingRight(10).SetPaddingTop(4).SetPaddingBottom(4));
                rightHeader.Add(pillTable);

                Table metaTable = new Table(UnitValue.CreatePercentArray(new float[] { 40, 60 })).UseAllAvailableWidth();
                void AddMeta(string label, string value)
                {
                    metaTable.AddCell(new Cell().Add(new Paragraph(label).SetFont(fontBold).SetFontSize(8)).SetBorder(Border.NO_BORDER));
                    metaTable.AddCell(new Cell().Add(new Paragraph(value).SetFont(fontNormal).SetFontSize(8)).SetBorder(Border.NO_BORDER));
                }
                AddMeta("Fecha de emisión:", cotizacion.Fecha.ToString("dd/MM/yyyy"));
                AddMeta("Hora:", cotizacion.Fecha.ToString("hh:mm tt"));
                AddMeta("Asesor de ventas:", "Caja Principal");
                AddMeta("Válida hasta:", cotizacion.Fecha.AddDays(14).ToString("dd/MM/yyyy") + " (14 días)");
                AddMeta("Condición de pago:", "Contado");
                AddMeta("Tiempo de entrega:", "1 a 3 días laborables");
                AddMeta("Moneda:", "Peso Dominicano (DOP)");
                rightHeader.Add(metaTable);

                headerTable.AddCell(rightHeader);
                document.Add(headerTable);
                document.Add(new Paragraph("\n").SetFontSize(5));

                Table blocksTable = new Table(UnitValue.CreatePercentArray(new float[] { 48, 4, 48 })).UseAllAvailableWidth();

                Table CrearBloque(string titulo, Action<Table> llenarContenido)
                {
                    Table box = new Table(1).UseAllAvailableWidth();
                    box.AddCell(new Cell().Add(new Paragraph(titulo).SetFont(fontBold).SetFontSize(9).SetFontColor(ColorConstants.WHITE))
                        .SetBackgroundColor(colorAzulOscuro).SetBorder(new SolidBorder(colorAzulOscuro, 1)).SetPadding(4));

                    Table content = new Table(1).UseAllAvailableWidth();
                    llenarContenido(content);
                    box.AddCell(new Cell().Add(content).SetBorder(new SolidBorder(colorGrisBorde, 1)).SetPadding(6).SetMinHeight(75));
                    return box;
                }

                blocksTable.AddCell(new Cell().Add(CrearBloque("DATOS DEL CLIENTE", (content) => {
                    Table inner = new Table(UnitValue.CreatePercentArray(new float[] { 30, 70 })).UseAllAvailableWidth();
                    void AddInfo(string lbl, string val)
                    {
                        inner.AddCell(new Cell().Add(new Paragraph(lbl).SetFont(fontBold).SetFontSize(8)).SetBorder(Border.NO_BORDER));
                        inner.AddCell(new Cell().Add(new Paragraph(val).SetFont(fontNormal).SetFontSize(8)).SetBorder(Border.NO_BORDER));
                    }
                    AddInfo("Nombre:", cotizacion.Cliente);
                    AddInfo("RNC/Cédula:", cotizacion.CedulaRnc);
                    AddInfo("Dirección:", "Ciudad, Rep. Dom.");
                    AddInfo("Teléfono:", "N/A");
                    content.AddCell(new Cell().Add(inner).SetBorder(Border.NO_BORDER));
                })).SetBorder(Border.NO_BORDER));

                blocksTable.AddCell(new Cell().SetBorder(Border.NO_BORDER));

                blocksTable.AddCell(new Cell().Add(CrearBloque("DETALLES DE LA COTIZACIÓN", (content) => {
                    content.AddCell(new Cell().Add(new Paragraph("Esta cotización contempla los precios y existencias disponibles a la fecha de emisión.\n\nPara confirmar su pedido, por favor comuníquese con su asesor de ventas.")
                        .SetFont(fontNormal).SetFontSize(8).SetFontColor(colorAzulOscuro)).SetBorder(Border.NO_BORDER).SetPaddingTop(10));
                })).SetBorder(Border.NO_BORDER));

                document.Add(blocksTable);
                document.Add(new Paragraph("\n").SetFontSize(5));

                Table itemsTable = new Table(UnitValue.CreatePercentArray(new float[] { 5, 15, 30, 8, 8, 12, 10, 12 })).UseAllAvailableWidth().SetMarginTop(5);
                string[] headers = { "#", "CÓDIGO", "DESCRIPCIÓN", "CANT.", "UNIDAD", "PRECIO UNIT.", "ITBIS", "SUBTOTAL" };

                foreach (var h in headers)
                {
                    itemsTable.AddHeaderCell(new Cell().Add(new Paragraph(h).SetFont(fontBold).SetFontSize(8).SetFontColor(ColorConstants.WHITE))
                        .SetBackgroundColor(colorAzulOscuro).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.CENTER).SetPadding(5));
                }

                int index = 1;
                foreach (var item in cotizacion.Detalles)
                {
                    Color bgColor = index % 2 == 0 ? colorGrisClaro : ColorConstants.WHITE;
                    void AddItemCell(string text, TextAlignment align)
                    {
                        itemsTable.AddCell(new Cell().Add(new Paragraph(text).SetFont(fontNormal).SetFontSize(8))
                            .SetBackgroundColor(bgColor).SetBorderBottom(new SolidBorder(colorGrisBorde, 1)).SetBorderTop(Border.NO_BORDER).SetBorderLeft(Border.NO_BORDER).SetBorderRight(Border.NO_BORDER)
                            .SetTextAlignment(align).SetPadding(6));
                    }
                    AddItemCell(index.ToString(), TextAlignment.CENTER);
                    AddItemCell($"PRD-{index:D3}", TextAlignment.CENTER);
                    AddItemCell(item.Producto, TextAlignment.LEFT);
                    AddItemCell(item.Cantidad.ToString(), TextAlignment.CENTER);
                    AddItemCell("UD", TextAlignment.CENTER);
                    AddItemCell($"RD$ {item.Precio:N2}", TextAlignment.RIGHT);
                    AddItemCell($"RD$ {(item.Subtotal * 0.18m):N2}", TextAlignment.RIGHT);
                    AddItemCell($"RD$ {item.Subtotal:N2}", TextAlignment.RIGHT);
                    index++;
                }
                document.Add(itemsTable);
                document.Add(new Paragraph("\n").SetFontSize(5));

                Table lowerTable = new Table(UnitValue.CreatePercentArray(new float[] { 55, 45 })).UseAllAvailableWidth();

                Table obsTable = new Table(1).UseAllAvailableWidth();
                obsTable.AddCell(new Cell().Add(new Paragraph("Observaciones:").SetFont(fontBold).SetFontSize(8))
                    .Add(new Paragraph("• Precios sujetos a cambios sin previo aviso.\n• Existencias sujetas a disponibilidad.\n\n").SetFont(fontNormal).SetFontSize(8))

                    // AQUI SE USA LA FUENTE CURSIVA QUE CREAMOS ARRIBA
                    .Add(new Paragraph("¡Gracias por preferir FerreApp La Varilla!").SetFont(fontItalic).SetFontSize(9).SetFontColor(colorAzulOscuro))

                    .SetBorder(new SolidBorder(colorGrisBorde, 1)).SetBackgroundColor(colorGrisClaro).SetPadding(10));
                lowerTable.AddCell(new Cell().Add(obsTable).SetBorder(Border.NO_BORDER).SetPaddingRight(10));

                Table totalsTable = new Table(UnitValue.CreatePercentArray(new float[] { 50, 50 })).UseAllAvailableWidth();
                totalsTable.AddCell(new Cell().Add(new Paragraph("Subtotal:").SetFont(fontBold).SetFontSize(9)).SetBorder(Border.NO_BORDER).SetPaddingBottom(5));
                totalsTable.AddCell(new Cell().Add(new Paragraph($"RD$ {cotizacion.Subtotal:N2}").SetFont(fontBold).SetFontSize(9)).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.RIGHT).SetPaddingBottom(5));

                totalsTable.AddCell(new Cell().Add(new Paragraph("ITBIS (18%):").SetFont(fontBold).SetFontSize(9)).SetBorder(Border.NO_BORDER).SetPaddingBottom(10));
                totalsTable.AddCell(new Cell().Add(new Paragraph($"RD$ {cotizacion.Itbis:N2}").SetFont(fontBold).SetFontSize(9)).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.RIGHT).SetPaddingBottom(10));

                decimal total = cotizacion.Subtotal + cotizacion.Itbis;
                totalsTable.AddCell(new Cell().Add(new Paragraph("TOTAL GENERAL:").SetFont(fontBold).SetFontSize(10).SetFontColor(ColorConstants.WHITE)).SetBackgroundColor(colorAzulOscuro).SetBorder(Border.NO_BORDER).SetPadding(6));
                totalsTable.AddCell(new Cell().Add(new Paragraph($"RD$ {total:N2}").SetFont(fontBold).SetFontSize(10).SetFontColor(ColorConstants.WHITE)).SetBackgroundColor(colorAzulOscuro).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.RIGHT).SetPadding(6));

                lowerTable.AddCell(new Cell().Add(totalsTable).SetBorder(new SolidBorder(colorGrisBorde, 1)).SetBackgroundColor(colorGrisClaro).SetPadding(10));
                document.Add(lowerTable);
                document.Add(new Paragraph("\n").SetFontSize(5));

                Table termsTable = new Table(UnitValue.CreatePercentArray(new float[] { 55, 45 })).UseAllAvailableWidth();

                Cell termCell = new Cell().SetBorder(Border.NO_BORDER).SetPaddingRight(10);
                termCell.Add(new Paragraph("TÉRMINOS Y CONDICIONES").SetFont(fontBold).SetFontSize(9).SetFontColor(colorAzulOscuro));
                termCell.Add(new Paragraph("✓ Esta cotización es válida por el período indicado.\n✓ Los productos se reservan por 48 horas luego de emitida.\n✓ El pago debe realizarse antes del despacho o entrega.\n✓ No incluye flete (Consultar condiciones de entrega).").SetFont(fontNormal).SetFontSize(8));
                termsTable.AddCell(termCell);

                Cell signCell = new Cell().SetBorder(Border.NO_BORDER);
                signCell.Add(new Paragraph("ACEPTACIÓN DEL CLIENTE").SetFont(fontBold).SetFontSize(9).SetFontColor(colorAzulOscuro));
                signCell.Add(new Paragraph("Acepto los términos y condiciones de esta cotización.\n\nNombre: __________________________________________\n\nFirma: ________________________ Fecha: ___________").SetFont(fontNormal).SetFontSize(8));
                termsTable.AddCell(signCell);

                document.Add(termsTable);

                document.Add(new Paragraph("\n"));
                Table yellowFooter = new Table(1).UseAllAvailableWidth();
                yellowFooter.AddCell(new Cell().Add(new Paragraph("Calidad, confianza y servicio para construir tus proyectos.").SetFont(fontBold).SetFontSize(8).SetTextAlignment(TextAlignment.CENTER).SetFontColor(ColorConstants.WHITE)).SetBackgroundColor(colorAmarillo).SetBorder(Border.NO_BORDER).SetPadding(4));
                document.Add(yellowFooter);

                document.Close();
                return stream.ToArray();
            }
        }
    }
}