using ASP_.NET_InvoiceManagementAuth.DTOs;
using ASP_.NET_InvoiceManagementAuth.Models;
using AutoMapper;

namespace ASP_.NET_InvoiceManagementAuth.Mapping;

/// <summary>
/// Mapping profile for AutoMapper to define how domain models 
/// are transformed into DTOs and vice versa.
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// Mapping configuration for Invoice and Customer entities to their respective DTOs.
    /// </summary>
    public MappingProfile()
    {
        CreateMap<Invoice, InvoiceResponseDTO>()
           .ForMember(
               dest => dest.Status,
               opt => opt.MapFrom(src => src.Status.ToString())
           )
           .ForMember(
               dest => dest.InvoiceRowsCount,
               opt => opt.MapFrom(src => src.Rows.Count())
           )
           .ForMember(
                dest => dest.CustomerName,
                opt => opt.MapFrom(src => src.Customer.Name)
            );


        CreateMap<InvoiceResponseDTO, Invoice>()
            .ForMember(
                dest => dest.Status,
                opt => opt.MapFrom(src =>
                    Enum.Parse<InvoiceStatus>(src.Status, true)
                )
            )
            .ForMember(dest => dest.Rows, opt => opt.Ignore())
            .ForMember(dest => dest.Customer, opt => opt.Ignore())
            .ForMember(dest => dest.CustomerId, opt => opt.Ignore())
            .ForMember(dest => dest.TotalSum, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<UpdateInvoiceRequest, Invoice>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Rows, opt => opt.Ignore())
                .ForMember(dest => dest.TotalSum, opt => opt.Ignore())
                .ForMember(dest => dest.Customer, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CustomerId, opt => opt.Ignore())
                .ForMember(
                    dest => dest.UpdatedAt,
                    opt => opt.MapFrom(_ => DateTimeOffset.UtcNow)
                );

        CreateMap<Invoice, UpdateInvoiceRequest>();


        CreateMap<CreateInvoiceRequest, Invoice>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Rows, opt => opt.Ignore())
            .ForMember(dest => dest.TotalSum, opt => opt.Ignore())
            .ForMember(dest => dest.Customer, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTimeOffset.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore());

        CreateMap<Invoice, CreateInvoiceRequest>();

        CreateMap<Customer, CustomerResponseDTO>()
            .ForMember(dest => dest.InvoiceCount,
                       opt => opt.MapFrom(src => src.Invoices.Count))
            .ForMember(dest => dest.HasInvoices,
                       opt => opt.MapFrom(src => src.Invoices.Any()));

        CreateMap<CustomerResponseDTO, Customer>()
            .ForMember(dest => dest.Invoices, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore());

        CreateMap<Customer, CreateCustomerRequest>();

        CreateMap<CreateCustomerRequest, Customer>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Invoices, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTimeOffset.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore());


        CreateMap<Customer, UpdateCustomerRequest>();

        CreateMap<UpdateCustomerRequest, Customer>()
           .ForMember(dest => dest.Invoices, opt => opt.Ignore())
           .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
           .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
           .ForMember(
                dest => dest.UpdatedAt,
                opt => opt.MapFrom(_ => DateTimeOffset.UtcNow)
              );

        CreateMap<ProfileEditRequest, AppUser>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTimeOffset.UtcNow))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore())
            .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));

        CreateMap<ProfileEditRequest, AuthResponseDTO>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));

        CreateMap<AppUser, AuthResponseDTO>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email ?? string.Empty))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName ?? string.Empty))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName ?? string.Empty))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address ?? string.Empty))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber ?? string.Empty))
            .ForMember(dest => dest.AccessToken, opt => opt.Ignore())
            .ForMember(dest => dest.RefreshToken, opt => opt.Ignore())
            .ForMember(dest => dest.ExpiresAt, opt => opt.Ignore())
            .ForMember(dest => dest.RefreshTokenExpiresAt, opt => opt.Ignore())
            .ForMember(dest => dest.Roles, opt => opt.Ignore());
    }







}
