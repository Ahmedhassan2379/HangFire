using System.Data;

namespace HangFire.ListToDataTable
{
    public interface IListToDataTable
    {
        DataTable ToDataTable<T>(List<T> items);
    }
}
