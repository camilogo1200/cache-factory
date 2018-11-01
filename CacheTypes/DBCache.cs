using System;
using System.Data;
using System.Data.SqlClient;
using Cache.Factory.Interfaces;
using Microsoft.Practices.EnterpriseLibrary.Caching;

namespace Cache.Factory.CacheType
{
    public class DBCache : ICacheBehavior
    {
        private SqlDataAdapter sqlDAdapter;
        private String _ConexionString;

        public DBCache()
        {
            _ConexionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringCache"].ToString();
            sqlDAdapter = new SqlDataAdapter();
        }

        /// <summary>
        /// Add Item to CacheDB
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Boolean AddItem(String key, Object value)
        {
            try
            {
                Microsoft.Practices.EnterpriseLibrary.Caching.CacheItem newItem = new Microsoft.Practices.EnterpriseLibrary.Caching.CacheItem(key, value, CacheItemPriority.Normal, null);
                byte[] valueBytes = SerializationUtility.ToBytes(newItem.Value);
                byte[] expirationBytes = SerializationUtility.ToBytes(newItem.GetExpirations());
                byte[] refreshActionBytes = SerializationUtility.ToBytes(newItem.RefreshAction);
                CacheItemPriority scavengingPriority = newItem.ScavengingPriority;
                DateTime lastAccessedTime = newItem.LastAccessedTime;

                return addDBItemvalue(key, valueBytes, expirationBytes, lastAccessedTime) > 0;
            }
            catch
            {
                return false;
            }
        }

        public Object GetItem(String key)
        {
            return GetDBItem(key);
        }

        public Boolean DeleteItem(String cacheKey)
        {
            if (deleteDBItem(cacheKey) > 0)
                return true;
            else
                return false;
        }

        public Boolean ExistItem(String cacheKey)
        {
            return existDBItem(cacheKey);
        }

        #region Private Functions

        private int addDBItemvalue(String key, byte[] valueBytes, byte[] expirationBytes, DateTime LastAccessedTime)
        {
            using (SqlConnection _sqlConnection = new SqlConnection(_ConexionString))
            {
                _sqlConnection.Open();

                sqlDAdapter.SelectCommand = new SqlCommand("Cache.AddItem", _sqlConnection);
                sqlDAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                sqlDAdapter.SelectCommand.Parameters.Add(new SqlParameter("@storagekey", key));
                sqlDAdapter.SelectCommand.Parameters.Add(new SqlParameter("@value", valueBytes));
                sqlDAdapter.SelectCommand.Parameters.Add(new SqlParameter("@expirations", expirationBytes));
                sqlDAdapter.SelectCommand.Parameters.Add(new SqlParameter("@lastAccessedTime", LastAccessedTime));

                return sqlDAdapter.SelectCommand.ExecuteNonQuery();
            }
        }

        private Object GetDBItem(String key)
        {
            using (SqlConnection _sqlConnection = new SqlConnection(_ConexionString))
            {
                _sqlConnection.Open();
                sqlDAdapter.SelectCommand = new SqlCommand("Cache.GetItem", _sqlConnection);
                sqlDAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDAdapter.SelectCommand.Parameters.Add(new SqlParameter("@storagekey", key));

                return Deserializevalue(sqlDAdapter.SelectCommand.ExecuteScalar());
            }
        }

        private Object Deserializevalue(Object value)
        {
            if (value == DBNull.Value)
            {
                return null;
            }
            else
            {
                byte[] valueBytes = (byte[])value;
                return SerializationUtility.ToObject(valueBytes);
            }
        }

        private int deleteDBItem(String key)
        {
            using (SqlConnection _sqlConnection = new SqlConnection(_ConexionString))
            {
                _sqlConnection.Open();

                sqlDAdapter.SelectCommand = new SqlCommand("Cache.DeleteItem", _sqlConnection);
                sqlDAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                sqlDAdapter.SelectCommand.Parameters.Add(new SqlParameter("@storagekey", key));

                return sqlDAdapter.SelectCommand.ExecuteNonQuery();
            }
        }

        private bool existDBItem(String key)
        {
            using (SqlConnection _sqlConnection = new SqlConnection(_ConexionString))
            {
                _sqlConnection.Open();

                sqlDAdapter.SelectCommand = new SqlCommand("Cache.ExistsItem", _sqlConnection);
                sqlDAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                sqlDAdapter.SelectCommand.Parameters.Add(new SqlParameter("@storagekey", key));

                return Convert.ToBoolean(sqlDAdapter.SelectCommand.ExecuteScalar());
            }
        }

        public bool AddItem(string key, object value, int expireMinutes)
        {
            throw new NotImplementedException();
        }

        public bool CleanCache()
        {
            throw new NotImplementedException();
        }

        #endregion Private Functions
    }
}