using AutoMapper;
using Ditransa.Application.DTOs;
using Ditransa.Domain.Entities;

namespace Ditransa.Infrastructure.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Zone, ZoneDto>().ReverseMap();
            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<Partner, PartnerDto>().ReverseMap();
            CreateMap<CustomerContact, CustomerContactDto>().ReverseMap();
            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Option, OptionDto>().ReverseMap();
            CreateMap<CustomerDocument, CustomerDocumentDto>().ReverseMap();
            CreateMap<PolicyType, PolicyTypeDto>().ReverseMap();
            CreateMap<UpdateCustomerDto, CustomerDto>().ReverseMap();
            CreateMap<UpdateCustomerDto, Customer>().ReverseMap();
            CreateMap<Pep, PepDto>().ReverseMap();
            CreateMap<BusinessLine, BusinessLineDto>().ReverseMap();
            CreateMap<ParameterType, ParameterTypeDto>().ReverseMap();
            CreateMap<Parameter, ParameterDto>().ReverseMap();
            CreateMap<City, CityDto>().ReverseMap();
            CreateMap<Department, DepartmentDto>().ReverseMap();
            CreateMap<Parameter, CreateParameterDto>().ReverseMap();
            CreateMap<CustomerHistory, CustomerHistoryDto>().ReverseMap();
            CreateMap<SurveyGroup, SurveyGroupDto>().ReverseMap();

            CreateMap<CustomerHistory, CustomerHistoryDto>().ReverseMap();
        }
    }
}