namespace MyProject.Shared.Models
{
    public record PagingDto<T>(int PageSize, int PageIndex, int Records, int Pages, IEnumerable<T> Items);

}
