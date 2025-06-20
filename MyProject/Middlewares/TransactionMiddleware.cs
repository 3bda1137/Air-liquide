using System.Data;
using DotNetCore.CAP;
using MyProject.Infrastructures.DbContexts;

namespace MyProject.Middlewares
{
    public class TransactionMiddleware(ApplicationDbContext dbContext, ICapPublisher capPublisher)
      : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted, capPublisher);

            try
            {
                await next(context);
                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
