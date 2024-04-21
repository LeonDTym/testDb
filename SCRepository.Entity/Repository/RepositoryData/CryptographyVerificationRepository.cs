using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SCRepository.Entity.Models;
using SCRepository.Entity.Models.KeyUserModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace SCRepository.Entity.Repository.RepositoryData
{
    public class CryptographyVerificationRepository : ICryptographyVerificationRepository
    {
        private DataForSendServer _dataForSendServer;
        public ResultDataVerificationOnServer ResultDataVerificationOnServer { get; private set; }
        public string MessageError { get; private set; }

        private readonly VerificationSetting _verificationSetting;

        public CryptographyVerificationRepository(VerificationSetting serviceOptions)
        {
            _verificationSetting = serviceOptions;
        }
        public bool Verification(byte[] dataVerification)
        {
            _dataForSendServer = new DataForSendServer(dataVerification.ToList());
            byte[] reseivedBytes = new byte[dataVerification.Count() * 4];
            byte[] reseived = new byte[dataVerification.Count() * 4];
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            {
                SendTimeout = 60000,
                ReceiveTimeout = 60000
            };
            IPAddress ipAddress = IPAddress.Parse(_verificationSetting.IpCryptography);
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, _verificationSetting.PortCryptography);
            try
            {
                socket.Connect(ipEndPoint);
                if (socket.Connected)
                {
                    socket.Send(_dataForSendServer.SendMessageVerifying());
                    int index = 0;
                    int totalbyte = 1;
                    while (totalbyte != 0)
                    {
                        totalbyte = socket.Receive(reseivedBytes);
                        Array.Copy(reseivedBytes, 0, reseived, index, totalbyte);
                        index += totalbyte;
                    }
                    var dataByteReceive = new DataReceiveServer(reseived);
                    if (dataByteReceive.LengthDate == 0 && dataByteReceive.LengthError > 0)
                    {
                        //if (!string.IsNullOrWhiteSpace(_verificationSetting.FileCryptographyLoggingError))
                        //{
                        //    using (StreamWriter outFile = new StreamWriter(_verificationSetting.FileCryptographyLoggingError, true))
                        //    {
                        //        outFile.WriteLine($"{DateTime.Now}\t>>> SSF_SERVER ({_verificationSetting.IpCryptography}:{_verificationSetting.PortCryptography}) ERROR VERIFICATION - {dataByteReceive.MessageError}");
                        //    }
                        //}
                        MessageError = dataByteReceive.MessageError;
                        return false;
                    }
                    ResultDataVerificationOnServer = dataByteReceive.VerifyReceive();
                }
                socket.Close();
            }
            catch (SocketException ex)
            {
                if (!string.IsNullOrWhiteSpace(_verificationSetting.FileCryptographyLoggingError))
                {
                    using (StreamWriter outFile = new StreamWriter(_verificationSetting.FileCryptographyLoggingError, true))
                    {
                        outFile.WriteLine("{1}\t>>> SocketException - {0}", ex.Message, DateTime.Now);
                    }
                }
                return false;

            }
            catch (Exception ex)
            {
                if (!string.IsNullOrWhiteSpace(_verificationSetting.FileCryptographyLoggingError))
                {
                    using (StreamWriter outFile = new StreamWriter(_verificationSetting.FileCryptographyLoggingError, true))
                    {
                        outFile.WriteLine("{1}\t>>> Exception - {0}", ex.Message, DateTime.Now);
                    }
                }
                return false;
            }
            return true;


        }
    }
}