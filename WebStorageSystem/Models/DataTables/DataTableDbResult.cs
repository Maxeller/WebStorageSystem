namespace WebStorageSystem.Models.DataTables
{
    public class DataTableDbResult<T>
    {
        public T[] Data { get; set; }
        public int RecordsTotal { get; set; }
    }
}
