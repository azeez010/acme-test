using AcmeCorp.Core.Utilities;
using AcmeCorp.Domain.DTO;
using AcmeCorp.Domain.Interface.Repository;
using AcmeCorp.Domain.Interface.Service;
using AcmeCorp.Domain.Model;
using AcmeCorp.Domain.Response;
using AcmeCorp.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AcmeCorp.Core.Service
{

    public class ContactInfoService : IContactInfoService
    {
        private readonly IContactInfoRepository _contactRepository;
        private readonly ICustomerRepository _customerRepository;


        public ContactInfoService(IContactInfoRepository contactRepository, ICustomerRepository customerRepository)
        {
            _contactRepository = contactRepository;
            _customerRepository = customerRepository;
        }

        public async Task<BaseResponse> AddContactInfo(AddContactInfoDto contact)
        {
            //get customer
            var data = await _customerRepository.GetByData(x => x.CustomerId == contact.CustomerId);
            var customerData = data.FirstOrDefault();
            if (customerData is null)
            {
                //Customer does not exist
                return BaseResponse.Error(message: "Customer Does not exist");
            }

            ContactInfo newContact = new ContactInfo()
            {
                Address = contact.Address,
                Phone = contact.Phone,
                CustomerId = contact.CustomerId,
            };
            //add new customer  to database
            await _contactRepository.Add(newContact);

            customerData.ContactInfo.Add(newContact);
            await _customerRepository.Update(customerData);
            return BaseResponse.Success(data: newContact);
        }

        public async Task<BaseResponse> DeleteContactInfo(int contactInfo)
        {
            var data = await _contactRepository.GetByData(x => x.ContactInfoId == contactInfo);
            var contact = data.FirstOrDefault();
            if (contact is null)
            {
                //A user with that Email or UserName Exist
                return BaseResponse.Error(message: "ContactInfo does not exist");
            }
            var custData = await _customerRepository.GetByData(x => x.CustomerId == contact.CustomerId);
            var customer = custData.FirstOrDefault();
            if (customer is null)
            {
                //A user with that Email or UserName Exist
                return BaseResponse.Error(message: "No Customer Data is Attached to this ContactInfo ");
            }
            customer.ContactInfo.Remove(contact);
            await _customerRepository.Update(customer);
            _contactRepository.Delete(contact);

            return BaseResponse.Error(data: contact, message: "Contact Info Successfully Removed");
        }

        public async Task<BaseResponse> GetContact(int Id)
        {
            var data = await _contactRepository.GetByData(x => x.ContactInfoId == Id);
            var contact = data.FirstOrDefault();
            if (contact is null)
            {
                //A user with that Email or UserName Exist
                return BaseResponse.Error(message: "ContactInfo does not exist");
            }
            return BaseResponse.Success(data: contact);

        }

        public async Task<BaseResponse> UpdateConactInfo(int contactInfo, UpdateContactInforDto contact)
        {
            var data = await _contactRepository.GetByData(x => x.ContactInfoId == contactInfo);
            var contacts = data.FirstOrDefault();
            if (contacts is null)
            {
                //A user with that Email or UserName Exist
                return BaseResponse.Error(message: "ContactInfo does not exist");
            }
            contacts.Address = contact.Address;
            contacts.Phone = contact.Phone;

            await _contactRepository.Update(contacts);
            return BaseResponse.Success(data: contact);
        }
    }

}
