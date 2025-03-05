using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using MyShop.Application.Abstractions;
using MyShop.Core.Models.Orders;

namespace MyShop.Infrastructure.DocumentsGenerator;
internal sealed class InvoicePdfGenerator : IInvoiceGenerator, IDisposable
{
    private readonly MemoryStream _memoryStream = new();
    private readonly Document _document = new();

    public MemoryStream Generate(Invoice invoice)
    {
        FillInfo(invoice);

        DefineStyles();
        CreatePage(invoice);

        RenderDocument();

        return _memoryStream;
    }

    private void FillInfo(Invoice invoice)
    {
        _document.Info.Title = $"INV/{invoice.CreatedAt.Month}/{invoice.CreatedAt.Year}-{invoice.InvoiceNumber} - myShop";
        _document.Info.Subject = $"INV/{invoice.CreatedAt.Month}/{invoice.CreatedAt.Year}-{invoice.InvoiceNumber} - myShop";
        _document.Info.Author = "myShop";
    }

    private void DefineStyles()
    {
        var style = _document.Styles["Normal"]!;

        style.Font.Name = "Arial";

        style = _document.Styles[StyleNames.Header]!;
        style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);

        style = _document.Styles[StyleNames.Footer]!;
        style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

        style = _document.Styles.AddStyle("Table", "Normal");
        style.Font.Name = "Arial";
        style.Font.Size = 9;

        style = _document.Styles.AddStyle("Reference", "Normal");
        style.ParagraphFormat.SpaceBefore = "5mm";
        style.ParagraphFormat.SpaceAfter = "5mm";
        style.ParagraphFormat.TabStops.AddTabStop("16cm", TabAlignment.Right);
    }

    private void CreatePage(Invoice invoice)
    {
        var section = _document.AddSection();

        var paragraph = section.Headers.Primary.AddParagraph();
        paragraph.AddText($"Country, {invoice.CreatedAt:d}");
        paragraph.AddTab();
        paragraph.AddText($"INV/{invoice.CreatedAt.Month}/{invoice.CreatedAt.Year}-{invoice.InvoiceNumber}");


        paragraph = section.Headers.Primary.AddParagraph();
        paragraph.Format.SpaceBefore = "1cm";
        paragraph.AddText($"INVOICE");
        paragraph.Format.Alignment = ParagraphAlignment.Left;
        paragraph.Format.Font.Size = 24;
        paragraph.Format.Font.Bold = true;
        paragraph.Format.SpaceAfter = "1cm";

        var billToFrame = section.AddTextFrame();
        billToFrame.Height = "3.0cm";
        billToFrame.Width = "7.0cm";
        billToFrame.Left = ShapePosition.Left;
        billToFrame.RelativeHorizontal = RelativeHorizontal.Margin;
        billToFrame.Top = "5.0cm";
        billToFrame.RelativeVertical = RelativeVertical.Page;

        paragraph = billToFrame.AddParagraph();
        paragraph.Format.Font.Size = 10;
        paragraph.Format.Font.Bold = true;
        paragraph.AddText("From:");
        paragraph.Format.SpaceAfter = 2;
        paragraph = billToFrame.AddParagraph();
        paragraph.Format.Font.Size = 9;
        paragraph.Format.Font.Bold = false;
        paragraph.AddText("myShop");
        paragraph.Format.SpaceAfter = 0;
        paragraph = billToFrame.AddParagraph();
        paragraph.Format.Font.Size = 9;
        paragraph.AddText("Street 11/1");
        paragraph.Format.SpaceAfter = 0;
        paragraph = billToFrame.AddParagraph();
        paragraph.Format.Font.Size = 9;
        paragraph.AddText("11-111 City");
        paragraph.Format.SpaceAfter = 0;
        paragraph = billToFrame.AddParagraph();
        paragraph.Format.Font.Size = 9;
        paragraph.AddText("Country");
        paragraph.Format.SpaceAfter = 0;

        var toFrame = section.AddTextFrame();
        toFrame.Height = "3.0cm";
        toFrame.Width = "7.0cm";
        toFrame.Left = ShapePosition.Right;
        toFrame.RelativeHorizontal = RelativeHorizontal.Margin;
        toFrame.Top = "5.0cm";
        toFrame.RelativeVertical = RelativeVertical.Page;

        paragraph = toFrame.AddParagraph();
        paragraph.Format.Font.Size = 10;
        paragraph.Format.Font.Bold = true;
        paragraph.AddText("To:");
        paragraph.Format.SpaceAfter = 2;
        paragraph = toFrame.AddParagraph();
        paragraph.Format.Font.Size = 9;
        paragraph.Format.Font.Bold = false;
        paragraph.AddText($"{invoice.Order.FirstName} {invoice.Order.LastName}");
        paragraph.Format.SpaceAfter = 0;
        paragraph = toFrame.AddParagraph();
        paragraph.Format.Font.Size = 9;
        paragraph.AddText($"{invoice.Order.StreetName} {invoice.Order.BuildingNumber}{invoice.Order.ApartmentNumber switch
        {
            null => "",
            _ => $"/{invoice.Order.ApartmentNumber}"
        }}");
        paragraph.Format.SpaceAfter = 0;
        paragraph = toFrame.AddParagraph();
        paragraph.Format.Font.Size = 9;
        paragraph.AddText($"{invoice.Order.ZipCode} {invoice.Order.City}");
        paragraph.Format.SpaceAfter = 0;
        paragraph = toFrame.AddParagraph();
        paragraph.Format.Font.Size = 9;
        paragraph.AddText(invoice.Order.Country);

        paragraph = section.AddParagraph();
        paragraph.Format.Font.Size = 9;
        paragraph.AddFormattedText("Payment Method: ", TextFormat.Bold);
        paragraph.AddText(invoice.Order.PaymentMethod);
        paragraph.Format.SpaceBefore = "5cm";
        paragraph.Format.SpaceAfter = 20;

        var table = section.AddTable();
        table.Style = "Table";
        table.Borders.Width = 0;


        var column = table.AddColumn("1cm");
        column.Format.Alignment = ParagraphAlignment.Center;

        column = table.AddColumn("6cm");
        column.Format.Alignment = ParagraphAlignment.Left;

        column = table.AddColumn("3cm");
        column.Format.Alignment = ParagraphAlignment.Center;

        column = table.AddColumn("3cm");
        column.Format.Alignment = ParagraphAlignment.Center;

        column = table.AddColumn("3cm");
        column.Format.Alignment = ParagraphAlignment.Center;


        var row = table.AddRow();
        row.VerticalAlignment = VerticalAlignment.Center;

        string[] tableHeadersDescription = ["No.", "Item", "Quantity", "Unit Price", "Amount"];

        for (int i = 0; i < tableHeadersDescription.Length; i++)
        {
            row.Cells[i].AddParagraph(tableHeadersDescription[i]);
            row.Cells[i].Format.Font.Bold = true;                     
            row.Cells[i].Row!.TopPadding = 5;
            row.Cells[i].Row!.BottomPadding = 5;
            row.Cells[i].Borders.Bottom.Width = 1;
        }

        decimal amount;
        decimal totalPrice = 0;

        foreach(var item in invoice.Order.OrderProducts)
        {
            row = table.AddRow();
            row.VerticalAlignment = VerticalAlignment.Center;

            amount = item.Price * item.Quantity;
            totalPrice += amount;

            for (int i = 0; i < tableHeadersDescription.Length; i++)
            {
                row.Cells[i].AddParagraph(i switch
                {
                    0 => (table.Rows.Count - 1).ToString(),
                    1 => item.ProductVariant.GetProductVariantFullName(),
                    2 => item.Quantity.ToString(),
                    3 => item.Price.Value.ToString("c"),
                    4 => amount.ToString("c"),
                    _ => throw new NotImplementedException()
                });
               
                row.Cells[i].Row!.TopPadding = 5;
                row.Cells[i].Row!.BottomPadding = 5;
            }
        }

        row = table.AddRow();
        row.VerticalAlignment = VerticalAlignment.Center;

        for (int i = 0; i < tableHeadersDescription.Length; i++)
        {
            row.Cells[i].AddParagraph(i switch
            {
                { } when i == tableHeadersDescription.Length - 2 => "Total",
                { } when i == tableHeadersDescription.Length - 1 => totalPrice.ToString("c"),
                _ => ""
            });

            row.Cells[i].Format.Font.Bold = true;
            row.Cells[i].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[i].Borders.Top.Width = 1;
            row.Cells[i].Row!.TopPadding = 7;
            row.Cells[i].Row!.BottomPadding = 7;
        }           

        paragraph = section.Footers.Primary.AddParagraph();
        paragraph.AddText("myShop E-Commerce");
        paragraph.Format.Font.Size = 7;
        paragraph.Format.Font.Bold = true;
        paragraph.Format.Alignment = ParagraphAlignment.Center;

    }

    private void RenderDocument()
    {
        PdfDocumentRenderer renderer = new()
        {
            Document = _document
        };

        renderer.RenderDocument();
        renderer.PdfDocument.Save(_memoryStream);
        _memoryStream.Position = 0;
    }

    public void Dispose()
    {
        _memoryStream.Close();
    }
}
