using ClosedXML;
using HangFire.Dtos.Mails;
using HangFire.ExportFile.Interface;
using HangFire.Mail.Interfaces;
using HangFire.Mail.Services;
using HangFire.Models;
using HangFire.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syncfusion.XlsIO;
using System;
using System.Data;
using System.Net.Mail;
using System.Reflection;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace HangFire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailsController : ControllerBase
    {
        private readonly ISendMails _sendmail;
        private readonly ApplicationDbContext _context; 
        private readonly IExportFile _exportFile;

        public MailsController(ISendMails sendmail,ApplicationDbContext context,IExportFile exportFile)
        {
            _sendmail = sendmail;
            _context = context; 
            _exportFile = exportFile;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMail([FromForm]MailRequestDto maildto)
        {
            List<Movie> result = _context.Movies.Where(x=>x.Year==2023).Include(x => x.Category).ToList();
            if (result.Count() > 0)
            {

                DataTable dt = ToDataTable(result);
                maildto.Attachments= _exportFile.ExportReportMovie(dt);
               

            }

            await _sendmail.SendEmailWithAttachment(maildto.ToEmail,maildto.Subject,maildto.Body,maildto.Attachments,maildto.attachmentFileName);
            return Ok("Email sent successfully.");
        }

        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }



    }
}
