using Aspose.Cells;
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
using System;
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
        public dynamic GetMovie()
        {
           var movies =  _context.Movies.Include(x => x.Category).Select(m=>new MovieModelDto
            {
                CategoryId = m.CategoryId,
                CategoryName=m.Category.Name,
               Id = m.Id,
               Poster=m.Poster,
               Rate=m.Rate,
               StoreLine=m.StoreLine,
               Tiltle=m.Tiltle,
               Year = m.Year
            }).ToList();
            return movies.ToArray();
        }

        [HttpGet("movieByCategory")]
        public dynamic getMoviesByCategory(int categoryId)
        {
            var movies = _context.Movies.Include(c=>c.Category).Where(x => x.CategoryId == categoryId).ToList();
            return movies;
        }
        [HttpPost]
        public async Task<IActionResult> CreateMovie([FromForm]MovieModelDto movieDto)
        {
          
            //var dataStream = new MemoryStream();
            //await movieDto.Poster.CopyToAsync();
            Movie movie = new Movie()
            {
                CategoryId = movieDto.CategoryId,
                Tiltle=movieDto.Tiltle,
                Poster=movieDto.Poster,
                Rate = movieDto.Rate,
                Year = movieDto.Year,
                StoreLine=movieDto.StoreLine
            };
            await _context.AddAsync(movie);
            _context.SaveChanges();
            MailRequestDto dto = new MailRequestDto()
            {
                Body="ssss",
                Subject="bbbb",
                ToEmail="ahmedhassan2379@gmail.com"
            };
            //BackgroundJob.Enqueue(() => _setting.SendMailAsync(dto));
            return Ok(movie);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, [FromBody] MovieModelDto movieDto)
        {
            var movie = await _context.Movies.SingleOrDefaultAsync(x => x.Id == id);
            if (movie == null)
            {
                Console.WriteLine("This Movie Not Found");
            }
        
            movie.Tiltle = movieDto.Tiltle;
            movie.Poster = movieDto.Poster;
            movie.Rate = movieDto.Rate;
            movie.Year = movieDto.Year;
            movie.StoreLine = movieDto.StoreLine;
            movie.CategoryId = movieDto.CategoryId;
            
                
            _context.SaveChanges();
            return Ok(movie);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.SingleOrDefaultAsync(x => x.Id == id);
            if (movie == null)
            {
                Console.WriteLine("This Movie Not Found");
            }

            _context.Movies.Remove(movie);
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("Export")]
        public FileContentResult ExportMovie()
        {
           var result = _context.Movies.Include(x => x.Category).Select(m=>new MovieModelDto {
                CategoryId = m.CategoryId,
                CategoryName = m.Category.Name,
                Id = m.Id,
                Poster = m.Poster,
                Rate = m.Rate,
                StoreLine = m.StoreLine,
                Tiltle = m.Tiltle,
                Year = m.Year
            }).Where(x => x.Year == 2023).ToList();
            DataTable dt = _dataTable.ToDataTable(result);
           var File = _exportFile.ExportReportMovie(dt);
            FileContentResult r = new FileContentResult(fileContents: File, contentType: "application/vnd.openxmlformats-officedocument-spreadsheetml.sheet") { FileDownloadName = "Movies.xlsx" };

            return r;
        }
    }
}

