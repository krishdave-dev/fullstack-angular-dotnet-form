namespace Backend.Models.DTOs;
public class PagedResponse<T>
{
    public IEnumerable<T> Data { get; set; }

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public int TotalRecords { get; set; }

    public int TotalPages => (int)Math.Ceiling((double)TotalRecords/PageSize);

    public bool HasPreviouspage => PageNumber > 1;

    public bool HasNextPage => PageNumber < TotalPages;
}
//example response
//  {
//     "data": [
//         {
//             "id": 1,
//             "firstName": "Krish",
//             "lastName": "Dave"
//         }
//     ],
//     "pageNumber": 1,
//     "pageSize": 10,
//     "totalRecords": 105,
//     "totalPages": 11,
//     "hasPreviousPage": false,
//     "hasNextPage": true
// }