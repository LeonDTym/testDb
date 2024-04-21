using SCRepository.Entity.Models;
using SCRepository.Entity.Models.KeyUserModel;
using System;
using System.Collections.Generic;

namespace SCRepository.Entity.Repository.RepositoryData
{
    public interface ICryptographyVerificationRepository
    {
        string MessageError { get; }
        ResultDataVerificationOnServer ResultDataVerificationOnServer { get; }
        bool Verification(byte[] dataVerification);
    }
}