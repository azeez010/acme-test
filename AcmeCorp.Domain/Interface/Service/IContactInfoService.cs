using AcmeCorp.Domain.DTO;
using AcmeCorp.Domain.Model;
using AcmeCorp.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcmeCorp.Domain.Interface.Service
{
    public interface IContactInfoService 
    {
        Task<BaseResponse> GetContact(int Id);
        Task<BaseResponse> AddContactInfo(AddContactInfoDto contact);
        Task<BaseResponse> UpdateConactInfo(int contactInfo, UpdateContactInforDto contact);
        Task<BaseResponse> DeleteContactInfo(int contactInfo);
    }
}
