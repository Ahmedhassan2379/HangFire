﻿using Aspose.Cells;
using ClosedXML.Excel;
using Hangfire;
using HangFire.Dtos;
using HangFire.Dtos.Mails;
using HangFire.ExportFile.Interface;
using HangFire.ListToDataTable;
using HangFire.Mail.Interfaces;
using HangFire.Mail.Services;
using HangFire.Models;
using HangFire.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection;

namespace HangFire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ISendMails _setting;
        private readonly IExportFile _exportFile;
        private readonly IListToDataTable _dataTable;
        private List<string> allowedExtention = new List<string>() { ".png" };
        private long allowedsize = 1048576;
        public MoviesController(ApplicationDbContext context,ISendMails setting,IExportFile exportFile,IListToDataTable dataTable)
        {
            _context = context;
            _setting = setting;
            _exportFile = exportFile;   
            _dataTable = dataTable;
        }

        [HttpGet]
        public async Task<IActionResult> GetMovie()
        {
            List<Movie> movies = await _context.Movies.Include(x => x.Category).ToListAsync();
            return Ok(movies);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateMovie([FromForm]MovieModelDto movieDto)
        {
            //if (allowedExtention.Contains(Path.GetExtension(movieDto.Poster.FileName).ToLower()))
            //{
            //    return BadRequest("the image must be (jpg or png)");
            //}
            //if(movieDto.Poster.Length > allowedsize)
            //{
            //    return BadRequest("the size of image must be less than 1m");
            //}
            var dataStream = new MemoryStream();
            await movieDto.Poster.CopyToAsync(dataStream);
            Movie movie = new Movie()
            {
                CategoryId = movieDto.CategoryId,
                Tiltle=movieDto.Tiltle,
                Poster=dataStream.ToArray(),
                Rate = movieDto.Rate,
                Year = movieDto.Year,
                StoreLine=movieDto.StoreLine
            };
            await _context.AddAsync(movie);
            _context.SaveChanges();

            BackgroundJob.Enqueue(() => _setting.SendMailAsync("mhelal909@gmail.com" ,"Ahmed Hassan", "ahmed", null ));
            return Ok(movie);
        }


        [HttpGet("Export")]
        public IActionResult ExportMovie()
        {
            List<Movie> result = _context.Movies.Where(x => x.Year == 2023).Include(x => x.Category).ToList();
            DataTable dt = _dataTable.ToDataTable(result);
            return _exportFile.ExportReportMovie(dt);
        }
    }
}

