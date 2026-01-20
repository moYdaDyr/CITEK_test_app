using CITEK_test_app.Models;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Pdfa;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CITEK_test_app
{
    internal static class ReportCreator
    {

        static void CreateReport(ObservableDictionary<int, AddressObjectTable> addressObjectTables, string header)
        {
            try
            {
                Logger.UpdateLog("Создание отчёта...");

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "(*.pdf)|*.pdf";

                sfd.DefaultDirectory = Configuration.defaultReportPath;

                sfd.FileName = header.Replace(' ','_');

                if (sfd.ShowDialog() == false)
                {
                    Logger.UpdateLog("Создание отчёта отменено");
                    return;
                }

                string file = sfd.FileName;

                Logger.UpdateLog("Место расположения файла отчёта выбрано");

                using (var writer = new PdfWriter(file))
                {
                    using (var pdfDocument = new PdfDocument(writer))
                    {
                        PageSize pageSize = PageSize.A4;

                        using (var report = new Document(pdfDocument, pageSize))
                        {
                            PdfFont _baseFont = PdfFontFactory.CreateFont(Configuration.fontPath, "cp1251", PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED, true);

                            Paragraph head = new Paragraph(header);
                            head.SetFont(_baseFont);
                            head.SetFontSize(20);
                            head.SimulateBold();
                            head.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);
                            head.SetMarginTop(0);
                            head.SetMarginBottom(12);
                            report.Add(head);

                            LineSeparator ls = new LineSeparator(new SolidLine());
                            report.Add(ls);

                            foreach (var addressObjectCategory in addressObjectTables)
                            {
                                if (addressObjectCategory.Value.AddressObjects.Count == 0) continue;

                                Paragraph categoryHeader = new Paragraph(addressObjectCategory.Value.CategoryName);
                                categoryHeader.SetFont(_baseFont);
                                categoryHeader.SetFontSize(16);
                                categoryHeader.SimulateBold();
                                categoryHeader.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);
                                categoryHeader.SetMarginTop(0);
                                categoryHeader.SetMarginBottom(12);
                                report.Add(categoryHeader);

                                Table table = new Table(2, false);

                                Cell typeHeaderCell = new Cell(1, 1);
                                typeHeaderCell.SetFont(_baseFont);
                                typeHeaderCell.SimulateBold();
                                typeHeaderCell.SetFontSize(11);
                                typeHeaderCell.Add(new Paragraph("Тип объекта"));
                                typeHeaderCell.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);
                                typeHeaderCell.SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE);
                                table.AddCell(typeHeaderCell);

                                Cell nameHeaderCell = new Cell(1, 1);
                                nameHeaderCell.SetFont(_baseFont);
                                nameHeaderCell.SimulateBold();
                                nameHeaderCell.SetFontSize(11);
                                nameHeaderCell.Add(new Paragraph("Наименование"));
                                nameHeaderCell.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);
                                nameHeaderCell.SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE);
                                table.AddCell(nameHeaderCell);

                                foreach (var row in addressObjectCategory.Value.AddressObjects)
                                {
                                    Cell typeCell = new Cell(1, 1);
                                    typeCell.SetFont(_baseFont);
                                    typeCell.SetFontSize(11);
                                    typeCell.Add(new Paragraph(row.Type));
                                    typeCell.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);
                                    typeCell.SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE);
                                    table.AddCell(typeCell);

                                    Cell nameCell = new Cell(1, 1);
                                    nameCell.SetFont(_baseFont);
                                    nameCell.SetFontSize(11);
                                    nameCell.Add(new Paragraph(row.Name));
                                    nameCell.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);
                                    nameCell.SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE);
                                    table.AddCell(nameCell);
                                }
                                report.Add(table);
                            } //  foreach (var addressObjectCategory...
                        } // using (var report...
                    } // using (var pdfDocument...
                } // using (var writer...

                Logger.UpdateLog("Отчёт по объектам создан");
                MessageBox.Show("Отчёт сохранён успешно", "Сохранение отчёта", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                Logger.UpdateLog($"Ошибка при сохранении отчёта. подробности ошибки: {ex.Message}");
                MessageBox.Show($"Ошибка при сохранении отчёта. подробности ошибки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static async void CreateReportAsync(ObservableDictionary<int, AddressObjectTable> addressObjectTables, string date)
        {
            await Task.Run(() => CreateReport(addressObjectTables, date));
        }
    }
}
