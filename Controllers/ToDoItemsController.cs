using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bif4DotNetDemo
{
    [ApiController]
    [Route("api/todos")]
    public class ToDoItemsController : ControllerBase
    {
        private readonly ToDoDbContext dbContext;

        public ToDoItemsController(ToDoDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreateToDoItem([FromBody] ToDoItem toDoItem)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            toDoItem.Id = 0;
            dbContext.ToDoItems.Add(toDoItem);
            await dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        public async Task<List<ToDoItem>> ListToDoItems()
        {
            var items = await dbContext.ToDoItems.ToListAsync();
            return items;
        }

        [HttpGet("{id}")]
        public async Task<ToDoItem> GetToDoItem(int id)
        {
            return await dbContext.ToDoItems.FindAsync(id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await dbContext.ToDoItems.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            dbContext.ToDoItems.Remove(item);
            await dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}