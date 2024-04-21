using MySql.Data.MySqlClient;
using SCRepository.Entity.Models.UserModels;
using System;
using System.Data;
using System.Diagnostics;

namespace StudentCardsAdmin.Services
{
    public class AutorizationService
    {
        private readonly string connectionString;

        public AutorizationService(string connectionString)
        {

            this.connectionString = connectionString;
        }
        public UserInfoModel CheckUser(string username, string password)
        {
            UserInfoModel userInfo = new UserInfoModel();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand("LoginUser;", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                try { 
                conn.Open();
                command.Parameters.AddWithValue("inSystem", "SC");
                command.Parameters.AddWithValue("inUserName", username);
                command.Parameters.AddWithValue("inPassword", password);
                command.Parameters.Add(new MySqlParameter("outLoginCode", MySqlDbType.Int32));
                command.Parameters["outLoginCode"].Direction = ParameterDirection.Output;
                command.Parameters.Add(new MySqlParameter("outLoginDesc", MySqlDbType.VarChar));
                command.Parameters["outLoginDesc"].Direction = ParameterDirection.Output;
                command.Parameters.Add(new MySqlParameter("outUserRights", MySqlDbType.Int32));
                command.Parameters["outUserRights"].Direction = ParameterDirection.Output;
                command.Parameters.Add(new MySqlParameter("outName", MySqlDbType.VarChar));
                command.Parameters["outName"].Direction = ParameterDirection.Output;
                command.Parameters.Add(new MySqlParameter("outBranch", MySqlDbType.Int32));
                command.Parameters["outBranch"].Direction = ParameterDirection.Output;
                command.Parameters.Add(new MySqlParameter("outDescription", MySqlDbType.VarChar));
                command.Parameters["outDescription"].Direction = ParameterDirection.Output;
                command.Parameters.Add(new MySqlParameter("outPhone", MySqlDbType.VarChar));
                command.Parameters["outPhone"].Direction = ParameterDirection.Output;
                command.Parameters.Add(new MySqlParameter("outEmail", MySqlDbType.VarChar));
                command.Parameters["outEmail"].Direction = ParameterDirection.Output;
                command.Parameters.Add(new MySqlParameter("outSubSystem", MySqlDbType.VarChar));
                command.Parameters["outSubSystem"].Direction = ParameterDirection.Output;
                command.ExecuteNonQuery();

                userInfo.System = command.Parameters["inSystem"].Value.ToString();
                userInfo.UserName = command.Parameters["inUserName"].Value.ToString();
                userInfo.Password = command.Parameters["inPassword"].Value.ToString();
                userInfo.LoginCode = command.Parameters["outLoginCode"].Value.ToString();
                userInfo.LoginDesc = command.Parameters["outLoginDesc"].Value.ToString();
                userInfo.UserRights = command.Parameters["outUserRights"].Value.ToString();
                userInfo.Name = command.Parameters["outName"].Value.ToString();
                userInfo.Branch = command.Parameters["outBranch"].Value.ToString();
                userInfo.Description = command.Parameters["outDescription"].Value.ToString();
                userInfo.Phone = command.Parameters["outPhone"].Value.ToString();
                userInfo.Email = command.Parameters["outEmail"].Value.ToString();
                userInfo.SubSystem = command.Parameters["outSubSystem"].Value.ToString();
                }catch(Exception e)
                {
                }
            }
            return userInfo;
        }


        public UserEtcData CheckUserEtc(string username)
        {
            UserEtcData userEtc = new UserEtcData();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand("GetUserInfo_BE;", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                conn.Open();
                //command.Parameters.AddWithValue("inSystem", "ANIS");
                command.Parameters.AddWithValue("uPersNumber", username);
                //command.Parameters.AddWithValue("inPassword", password);
                var reader = command.ExecuteReader();
                reader.Read();
                //var test = conn.Database.ExecuteSqlCommand($"call users.GetUserInfo_BE(@uPersNumber)", outParams);

                userEtc.jobPost = reader.GetString(0);
                userEtc.jobName = reader.GetString(1);
                try {
                    userEtc.objName = reader.GetString(2);
                } catch { userEtc.objName = "ЦБУ: " + reader.GetString(5); }
                
                //userEtc.objID = reader.GetString(3);
                userEtc.fil = reader.GetString(4);
                userEtc.cbu = reader.GetString(5);
                userEtc.otd = reader.GetString(6);
            }
            return userEtc;
        }


        public ChangeInfoModel ChangePassword(string username, string password)
        {
            ChangeInfoModel changeInfo = new ChangeInfoModel();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand("ChangePassword_New;", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                conn.Open();
                command.Parameters.AddWithValue("inSystem", "SC");
                command.Parameters.AddWithValue("inUserName", username);
                command.Parameters.AddWithValue("inPassword", password);
                command.Parameters.AddWithValue("inMustChange", 0);
                command.Parameters.Add(new MySqlParameter("outResultCode", MySqlDbType.Int32));
                command.Parameters["outResultCode"].Direction = ParameterDirection.Output;
                command.Parameters.Add(new MySqlParameter("outResultDesc", MySqlDbType.VarChar));
                command.Parameters["outResultDesc"].Direction = ParameterDirection.Output;
                command.ExecuteNonQuery();

                changeInfo.System = command.Parameters["inSystem"].Value.ToString();
                changeInfo.UserName = command.Parameters["inUserName"].Value.ToString();
                changeInfo.Password = command.Parameters["inPassword"].Value.ToString();
                changeInfo.ResultCode = command.Parameters["outResultCode"].Value.ToString();
                changeInfo.ResultDesc = command.Parameters["outResultDesc"].Value.ToString();
            }
            return changeInfo;
        }
    }
}
