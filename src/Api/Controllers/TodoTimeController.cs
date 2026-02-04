using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace todoApi.Controllers;

public class TodoTimeController : ControllerBase
{
    private readonly DbContext _dbContext;
    
    public TodoTimeController(DbContext dbContext) => _dbContext = dbContext;
    
    
}