using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HangFire.ExportFile.Interface
{
    public interface IExportFile
    {
        byte[] ExportReportMovie(DataTable dataTable);
    }
}
