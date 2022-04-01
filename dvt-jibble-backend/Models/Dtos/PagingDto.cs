namespace dvt_jibble_backend.Models.Dtos
{
    public class PagingDto
    {
        public int CurrentPage { get; set; }
        public int ItemPerPage { get; set; }

        public int Start
        {
            get { return (CurrentPage - 1) * ItemPerPage; }
        }
    }
}
