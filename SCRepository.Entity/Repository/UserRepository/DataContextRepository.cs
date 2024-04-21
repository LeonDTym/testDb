using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using SCRepository.Entity.Models.KeyUserModel;
using SCRepository.Entity.Models.UserModels;
using System.Diagnostics;
using System.Linq;

namespace SCRepository.Entity.Repository.UserRepository
{
    public class DataContextRepository : IDataContextRepository
    {
        private UserDBContext _userDBContext;
        public DataContextRepository()
        {
            _userDBContext = new UserDBContext();
        }
        public IQueryable<T> GetAll<T>() where T : class
        {
            return _userDBContext.Set<T>();
        }


        public KeyUser LoginUserSign (KeyModel key)
        {
            KeyUser result = new KeyUser();
            var keyLogin = new SqlParameter
            {
                ParameterName = "@KeySN",
                SqlDbType = System.Data.SqlDbType.VarChar,
                Size = 30,
                Direction = System.Data.ParameterDirection.Input,
                Value = key.KeySN
            };
            var keyPass = new SqlParameter
            {
                ParameterName = "@IdKey",
                SqlDbType = System.Data.SqlDbType.VarChar,
                Size = 100,
                Direction = System.Data.ParameterDirection.Input,
                Value = key.IdKey
            };
            var test = _userDBContext.Database.ExecuteSqlCommand($"call Client.LoginUserSign(@IdKey, @KeySN)", keyPass,keyLogin);
            return result;
        }




        [System.Obsolete]
        public UserInfoModel GetUserInfoModel(LoginModel model)
        {
            UserInfoModel result = new UserInfoModel();
            var outParams = new MySqlParameter[] {
                        new MySqlParameter() {
                            ParameterName = "@inSystem",
                            MySqlDbType =  MySqlDbType.VarChar,
                            Size = 8,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = "ANIS"
                        },
                        new MySqlParameter() {
                            ParameterName = "@inUserName",
                            MySqlDbType =  MySqlDbType.VarChar,
                            Size = 32,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = model.UserName
                        },
                        new MySqlParameter() {
                            ParameterName = "@inPassword",
                            MySqlDbType =  MySqlDbType.VarChar,
                            Size = 255,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = model.Password
                        },
                        new MySqlParameter() {
                            ParameterName = "?outLoginCode",
                            MySqlDbType =  MySqlDbType.Int32,
                            Direction = System.Data.ParameterDirection.Output
                        },
                        new MySqlParameter() {
                            ParameterName = "?outLoginDesc",
                            MySqlDbType =  MySqlDbType.VarChar,
                            Size = 255,
                            Direction = System.Data.ParameterDirection.Output,
                        },
                        new MySqlParameter() {
                            ParameterName = "?outUserRights",
                            MySqlDbType =  MySqlDbType.Int32,
                            Direction = System.Data.ParameterDirection.Output,
                        },
                        new MySqlParameter() {
                            ParameterName = "?outName",
                            MySqlDbType =  MySqlDbType.VarChar,
                            Size = 128,
                            Direction = System.Data.ParameterDirection.Output,
                        },
                        new MySqlParameter() {
                            ParameterName = "?outBranch",
                            MySqlDbType =  MySqlDbType.Int32,
                            Direction = System.Data.ParameterDirection.Output,
                        },
                        new MySqlParameter() {
                            ParameterName = "?outDescription",
                            MySqlDbType =  MySqlDbType.VarChar,
                            Size = 45,
                            Direction = System.Data.ParameterDirection.Output,
                        },
                        new MySqlParameter() {
                            ParameterName = "?outPhone",
                            MySqlDbType =  MySqlDbType.VarChar,
                            Size = 16,
                            Direction = System.Data.ParameterDirection.Output,
                        },
                        new MySqlParameter() {
                            ParameterName = "?outEmail",
                            MySqlDbType =  MySqlDbType.VarChar,
                            Size = 45,
                            Direction = System.Data.ParameterDirection.Output,
                        },
                        new MySqlParameter() {
                            ParameterName = "?outSubSystem",
                            MySqlDbType =  MySqlDbType.VarChar,
                            Size = 1,
                            Direction = System.Data.ParameterDirection.Output,
                        }};

            try
            {
                //result = _userDBContext.UserInfoModel.FromSql($"call users.LoginUser(", outParams + ")").FirstOrDefault();
                var test = _userDBContext.Database.ExecuteSqlCommand($"call users.LoginUser(@inSystem, @inUserName, @inPassword, ?outLoginCode, ?outLoginDesc, ?outUserRights, ?outName, ?outBranch, ?outDescription, ?outPhone, ?outEmail, ?outSubSystem)", outParams);
            }
            catch (SqlException e)
            {
                Debug.WriteLine(e.Message);
            }
            return result;
        }


        public UserEtcData GetEtcData(string model)
        {
            UserEtcData result = new UserEtcData();
            var outParams = new MySqlParameter
            {
                ParameterName = "@uPersNumber",
                MySqlDbType = MySqlDbType.VarChar,
                Size = 10,
                Direction = System.Data.ParameterDirection.Input,
                Value = model
            };
            var test = _userDBContext.Database.ExecuteSqlCommand($"call users.GetUserInfo_BE(@uPersNumber)", outParams);
            return result;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _userDBContext.Dispose();
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~DataContextRepository() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
