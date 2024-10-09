using EstrellaAccesoriosWpf.Models;
using Microsoft.Office.Interop.Word;
using QRCoder;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Windows;
using Word = Microsoft.Office.Interop.Word;

namespace EstrellaAccesoriosWpf.Services;

public class WordService(string qrFolderPath, string wordFolderPath)
{
    public void GenerateQrOnlyTickets(List<TicketItem> ticketItems)
    {

        string filePath = Path.Combine(wordFolderPath,$"Etiquetas_Solo_Qr {DateTime.Now:dd-MM-yyyy HH-mm}.docx");

        if (IsFileOpen(filePath))
        {
            MessageBox.Show("No se puede modificar el archivo porque ya esta abierto en otra aplicación.");
            return;
        }

        int rowCount = 0;
        foreach (TicketItem ticketItem in ticketItems)
        {
            rowCount += ticketItem.Quantity;
        }
        rowCount = rowCount / 2 + rowCount % 2;

        if (rowCount == 0)
        {
            MessageBox.Show("Al menos debe elegir un producto.");
            return;
        }

        Word.Application wordApp = new()
        {
            Visible = false
        };

        // Create a new document
        Word.Document doc = wordApp.Documents.Add();

        doc.PageSetup.TextColumns.SetCount(5);

        doc.PageSetup.PaperSize = Word.WdPaperSize.wdPaperA4; // Set paper size to A4
        doc.PageSetup.TopMargin = wordApp.CentimetersToPoints(1.27f);
        doc.PageSetup.BottomMargin = wordApp.CentimetersToPoints(1.27f);
        doc.PageSetup.LeftMargin = wordApp.CentimetersToPoints(1.27f);
        doc.PageSetup.RightMargin = wordApp.CentimetersToPoints(1.27f);
        // Add a table with one column and numRows rows
        Word.Table table = doc.Tables.Add(doc.Range(0, 0), rowCount, 1);
        int i = 0;
        Word.Cell cell = table.Cell(1, 1);
        Word.Table innerTable = cell.Range.Tables.Add(cell.Range, 1, 2);
        foreach (TicketItem item in ticketItems)
        {
            string qrCodePath = GenerateQrCodeImage(item.Barcode, 1f);

            int j = 0;
            while (j < item.Quantity)
            {
                if(i % 2 == 0)
                {
                    cell = table.Cell(i/2+1, 1); // Access cell in row i, column 1
                    cell.Width = wordApp.CentimetersToPoints(3.54f); // Set cell width
                    cell.Height = wordApp.CentimetersToPoints(1.69f); // Set cell height
                    cell.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                    cell.HeightRule = Word.WdRowHeightRule.wdRowHeightExactly;

                    innerTable = cell.Range.Tables.Add(cell.Range, 1, 2);
                    innerTable.Borders.Enable = 0;
                }
                InsertQr(innerTable.Cell(1, i%2+1), qrCodePath, item.Barcode);
                j++;
                i++;
            }
        }

        // Save and close the document
        doc.SaveAs2(filePath);
        doc.Close();
        wordApp.Quit();

        Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
    }

    

    public void GenerateTicketsWithDescription(List<TicketItem> ticketItems)
    {
        string filePath = Path.Combine(wordFolderPath, $"Etiquetas_Con_Descripcion {DateTime.Now:dd-MM-yyyy HH-mm}.docx");

        if (IsFileOpen(filePath))
        {
            MessageBox.Show("No se puede modificar el archivo porque ya esta abierto en otra aplicación.");
            return;
        }

        int rowCount = 0;
        foreach (TicketItem ticketItem in ticketItems)
        {
            rowCount += ticketItem.Quantity;
        }

        if (rowCount == 0)
        {
            MessageBox.Show("Al menos debe elegir un producto.");
            return;
        }

        Word.Application wordApp = new()
        {
            Visible = false
        };

        // Create a new document
        Word.Document doc = wordApp.Documents.Add();

        doc.PageSetup.TextColumns.SetCount(5);

        doc.PageSetup.PaperSize = Word.WdPaperSize.wdPaperA4; // Set paper size to A4
        doc.PageSetup.TopMargin = wordApp.CentimetersToPoints(1.27f);
        doc.PageSetup.BottomMargin = wordApp.CentimetersToPoints(1.27f);
        doc.PageSetup.LeftMargin = wordApp.CentimetersToPoints(1.27f);
        doc.PageSetup.RightMargin = wordApp.CentimetersToPoints(1.27f);
        // Add a table with one column and numRows rows
        Word.Table table = doc.Tables.Add(doc.Range(0, 0), rowCount, 1);
        int i = 0;
        Word.Cell cell = table.Cell(1, 1);
        Word.Table innerTable = cell.Range.Tables.Add(cell.Range, 1, 2);
        foreach (TicketItem item in ticketItems)
        {
            string qrCodePath = GenerateQrCodeImage(item.Barcode, 1f);

            int j = 0;
            while (j < item.Quantity)
            {
                cell = table.Cell(i + 1, 1); // Access cell in row i, column 1
                cell.Width = wordApp.CentimetersToPoints(3.54f); // Set cell width
                cell.Height = wordApp.CentimetersToPoints(1.69f); // Set cell height
                cell.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                cell.HeightRule = Word.WdRowHeightRule.wdRowHeightExactly;

                innerTable = cell.Range.Tables.Add(cell.Range, 1, 2);
                innerTable.Borders.Enable = 0;

                Word.Cell leftCell = innerTable.Cell(1, 1);
                leftCell.Width = wordApp.CentimetersToPoints(1.44f);

                Word.Cell rightCell = innerTable.Cell(1, 2);
                rightCell.Width = wordApp.CentimetersToPoints(2.10f);

                InsertQr(leftCell, qrCodePath, item.Barcode);
                InsertDescription(rightCell, item.Description);
                j++;
                i++;
            }
        }

        // Save and close the document
        doc.SaveAs2(filePath);
        doc.Close();
        wordApp.Quit();

        Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
    }

    public void GenerateTicketsInCardFormat(List<TicketItem> ticketItems)
    {
        string filePath = Path.Combine(wordFolderPath, $"Etiquetas_Con_Formato_Tarjeta {DateTime.Now:dd-MM-yyyy HH-mm}.docx");

        if (IsFileOpen(filePath))
        {
            MessageBox.Show("No se puede modificar el archivo porque ya esta abierto en otra aplicación.");
            return;
        }

        int rowCount = 0;
        foreach (TicketItem ticketItem in ticketItems)
        {
            rowCount += ticketItem.Quantity;
        }

        if (rowCount == 0)
        {
            MessageBox.Show("Al menos debe elegir un producto.");
            return;
        }

        Word.Application wordApp = new()
        {
            Visible = false
        };

        // Create a new document
        Word.Document doc = wordApp.Documents.Add();

        doc.PageSetup.TextColumns.SetCount(3);

        doc.PageSetup.PaperSize = Word.WdPaperSize.wdPaperA4; // Set paper size to A4
        doc.PageSetup.TopMargin = wordApp.CentimetersToPoints(1.27f);
        doc.PageSetup.BottomMargin = wordApp.CentimetersToPoints(1.27f);
        doc.PageSetup.LeftMargin = wordApp.CentimetersToPoints(1.27f);
        doc.PageSetup.RightMargin = wordApp.CentimetersToPoints(1.27f);
        // Add a table with one column and numRows rows
        Word.Table table = doc.Tables.Add(doc.Range(0, 0), rowCount, 1);
        int i = 0;
        Word.Cell cell = table.Cell(1, 1);
        Word.Table innerTable = cell.Range.Tables.Add(cell.Range, 1, 2);
        foreach (TicketItem item in ticketItems)
        {
            string qrCodePath = GenerateQrCodeImage(item.Barcode, 1.67f, true);

            int j = 0;
            while (j < item.Quantity)
            {
                cell = table.Cell(i + 1, 1); // Access cell in row i, column 1
                cell.Width = wordApp.CentimetersToPoints(5.9f); // Set cell width
                cell.Height = wordApp.CentimetersToPoints(2.81f); // Set cell height
                cell.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                cell.HeightRule = Word.WdRowHeightRule.wdRowHeightExactly;

                innerTable = cell.Range.Tables.Add(cell.Range, 1, 2);
                innerTable.Borders.Enable = 0;

                Word.Cell leftCell = innerTable.Cell(1, 1);
                leftCell.Width = wordApp.CentimetersToPoints(2.4f);

                Word.Cell rightCell = innerTable.Cell(1, 2);
                rightCell.Width = wordApp.CentimetersToPoints(3.5f);

                InsertQr(leftCell, qrCodePath, item.Barcode, true);
                InsertDescription(rightCell, item.Description, true);
                j++;
                i++;
            }
        }

        // Save and close the document
        doc.SaveAs2(filePath);
        doc.Close();
        wordApp.Quit();

        Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
    }

    public bool IsFileOpen(string filePath)
    {
        if (!File.Exists(filePath)) return false;
        try
        {
            // Try to open the file in read/write mode
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                return false; // File is not open by another process
            }
        }
        catch (IOException)
        {
            return true; // File is open by another process
        }
    }

    public string GenerateQrCodeImage(string qrContent, float size, bool isCardFormat = false)
    {
        // Define the path where you will save the QR code images
        string fileName = isCardFormat ? $"{qrContent}_card.bmp" : $"{qrContent}.bmp";
        string qrCodePath = Path.Combine(qrFolderPath, fileName);

        if (File.Exists(qrCodePath))
        {
            return qrCodePath;
        }

        // Define QR code generation parameters
        QRCodeGenerator qrGenerator = new();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrContent, QRCodeGenerator.ECCLevel.Q);
        QRCode qrCode = new(qrCodeData);

        //Convert the size from cm to pixels(96 DPI)
        float dpi = 1920f; // Standard screen DPI
        float pixelsPerCm = dpi / 2.54f; // 1 inch = 2.54 cm
        int qrCodeSizeInPixels = (int)(size * pixelsPerCm); // Calculate the size in pixels

        // Generate the QR code image without a white border (set 'quietZoneModules' to 0)
        Bitmap qrCodeImage = qrCode.GetGraphic(80, Color.Black, Color.White, false); // Default size, adjust multiplier for better clarity
        Bitmap resizedQrCode = new(qrCodeImage, new System.Drawing.Size(qrCodeSizeInPixels, qrCodeSizeInPixels)); // Resize to desired size
        resizedQrCode.SetResolution(dpi, dpi);
        // Save the QR code image to the specified path
        resizedQrCode.Save(qrCodePath, ImageFormat.Bmp);

        // Return the file path of the generated QR code image
        return qrCodePath;
    }

    private static void InsertQr(Cell qrCell, string qrCodePath, string barcode, bool isCardFormat = false)
    {
        if (File.Exists(qrCodePath))
        {
            qrCell.Range.InlineShapes.AddPicture(qrCodePath, false, true); // Add QR code image to cell
        }
        qrCell.Range.InsertAfter(barcode);

        // Format the human-readable text (font size 4, Arial or Times New Roman)
        qrCell.Range.Font.Size = isCardFormat? 7 : 4;
        qrCell.Range.Font.Name = "Times New Roman";
    }

    private static void InsertDescription(Cell cell, string description, bool isCardFormat = false)
    {
        cell.Range.Text = description;
        cell.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
        cell.Range.Font.Size = isCardFormat ? 9 : 5;
        cell.Range.Font.Name = "Times New Roman";
    }
}
