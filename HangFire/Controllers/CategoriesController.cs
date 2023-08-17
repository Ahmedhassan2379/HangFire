using HangFire.Dtos;
using HangFire.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace HangFire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> getCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return Ok(categories);
        }

        [HttpPost(Name = "AddCategory")]
        public async Task<IActionResult> AddCategory(CategoryModelDtos categoryDto)
        {
            Category category = new Category()
            {
                Name = categoryDto.Name,
            };
            await _context.Categories.AddAsync(category);
            _context.SaveChanges();
            return Ok(category);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id , [FromBody]CategoryModelDtos categoryDto)
        {
            var category = await _context.Categories.SingleOrDefaultAsync(x => x.Id == id);
            if(category == null)
            {
                Console.WriteLine("This Category Not Found");
            }
            category.Name = categoryDto.Name;
            _context.SaveChanges();
                return Ok(category);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.SingleOrDefaultAsync(x => x.Id == id);
            if (category == null)
            {
                Console.WriteLine("This Category Not Found");
            }

            _context.Categories.Remove(category);
            _context.SaveChanges();
            return Ok();
        }

    }
}
