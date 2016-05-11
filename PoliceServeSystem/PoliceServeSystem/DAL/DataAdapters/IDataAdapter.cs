using System.Data.SqlClient;

namespace PoliceServeSystem.DAL.DataAdapters
{
    public interface IDataAdapter<in TDataObject>
    {
        void Materialize(TDataObject tDataOject, SqlDataReader sqlDataReader);
    }
}
