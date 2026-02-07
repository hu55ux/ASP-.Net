using ASP_.NET_InvoiceManagment.DTOs.CustomerDTOs;
using ASP_.NET_InvoiceManagment.DTOs.InvoiceDTOs;
using ASP_.NET_InvoiceManagment.Models;
using AutoMapper;

namespace ASP_.NET_InvoiceManagment.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Invoice, InvoiceResponseDTO>()
           .ForMember(
               dest => dest.Status,
               opt => opt.MapFrom(src => src.Status.ToString())
           )
           .ForMember(
               dest => dest.InvoiceRowsCount,
               opt => opt.MapFrom(src => src.Rows.Count)
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
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore());

        CreateMap<UpdateInvoiceRequest, Invoice>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Rows, opt => opt.Ignore())
                .ForMember(dest => dest.TotalSum, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.Customer, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
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


    }







}
