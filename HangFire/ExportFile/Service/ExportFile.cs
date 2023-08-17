using Aspose.Cells;
using ClosedXML.Excel;
using Hangfire;
using HangFire.Dtos;
using HangFire.Dtos.Mails;
using HangFire.ExportFile.Interface;
using HangFire.Mail.Interfaces;
using HangFire.Mail.Services;
using HangFire.Models;
using HangFire.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HangFire.ExportFile.Service
{
    public class ExportFile : IExportFile
    {
        public IActionResult ExportReportMovie(DataTable dataTable)
        {


            using (var workbook = new XLWorkbook())
            {
                var ws = workbook.Worksheets.Add("Latest Movies");
                ws.Range("D2:I2").Merge();
                ws.Cell(1, 6).Value = "MOVIES REPORT";
                ws.Cell(1, 6).Style.Font.Bold = true;
                ws.Cell(1, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(1, 6).Style.Font.FontSize = 30;

                //Header
                ws.Cell(4, 4).Value = "Title";
                ws.Cell(4, 5).Value = "Year";
                ws.Cell(4, 6).Value = "Rate";
                ws.Cell(4, 7).Value = "StoreLine";
                ws.Cell(4, 8).Value = "Poster";
                ws.Cell(4, 9).Value = "Category";
                ws.Range("D4:I4").Style.Fill.BackgroundColor = XLColor.Alizarin;




                int i = 5;
                foreach (DataRow item in dataTable.Rows)
                {
                    ws.Cell(i, 4).Value = item[0].ToString();
                    ws.Cell(i, 5).Value = item[1].ToString();
                    ws.Cell(i, 6).Value = item[2].ToString();
                    ws.Cell(i, 7).Value = item[3].ToString();
                    ws.Cell(i, 8).Value = item[4].ToString();
                    ws.Cell(i, 9).Value = item[5].ToString();
                    i = i + 1;
                }
                i = i - 1;
                ws.Columns("D:I").Width = 20;
                ws.Cells("D4:I" + i).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ws.Cells("D4:I" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Cells("D4:I" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Cells("D4:I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                     FileContentResult r = new FileContentResult(fileContents: content, contentType: "application/vnd.openxmlformats-officedocument-spreadsheetml.sheet") { FileDownloadName = "Movies.xlsx" };
                    //return File(content, "application/vnd.openxmlformats-officedocument-spreadsheetml.sheet", "Movies.xlsx");
                    return r;
                }
            }


        }


    }
}
