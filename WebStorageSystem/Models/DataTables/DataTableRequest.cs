namespace WebStorageSystem.Models.DataTables
{
    public class DataTableRequest
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public DataTableRequestColumns[] Columns { get; set; }
        public DataTableRequestOrder[] Order { get; set; }
        public DataTableRequestSearch Search { get; set; }
    }

    public class DataTableRequestColumns
    {
        public string Data { get; set; }
        public string Name { get; set; }
        public bool Searchable { get; set; }
        public bool Orderable { get; set; }
        public DataTableRequestSearch Search { get; set; }
    }

    public class DataTableRequestSearch
    {
        public string Value { get; set; }
        public bool Regex { get; set; }
    }

    public class DataTableRequestOrder
    {
        public int Column { get; set; }
        public DataTableRequestOrderDirection Dir { get; set; }
    }

    public enum DataTableRequestOrderDirection
    {
        asc,
        desc
    }
}