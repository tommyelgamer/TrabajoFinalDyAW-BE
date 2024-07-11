using AutoMapper;
using TrabajoFinalDyAW.DTOs;

namespace TrabajoFinalDyAW.MapperProfiles
{
    public class MerchandiseProfile: Profile
    {
        public MerchandiseProfile()
        {
            CreateMap<Entities.Merchandise, Models.Merchandise>()
                .ForMember(
                    dest => dest.MerchandiseId,
                    opt => opt.MapFrom(m => m.Id)
                )
                .ReverseMap();
            CreateMap<Entities.Merchandise, Presenters.MerchandisePresenter>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(m => m.Id)
                )
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(m => m.MerchandiseName)
                )
                .ForMember(
                    dest => dest.Description,
                    opt => opt.MapFrom(m => m.MerchandiseDescription)
                )
                .ForMember(
                    dest => dest.Stock,
                    opt => opt.MapFrom(m => m.MerchandiseStock)
                )
                .ForMember(
                    dest => dest.Barcode,
                    opt => opt.MapFrom(m => m.MerchandiseBarcode)
                );
            CreateMap<CreateMerchandiseDto, Entities.Merchandise>()
                .ForMember(
                    dest => dest.MerchandiseName,
                    opt => opt.MapFrom(m => m.Name)
                )
                .ForMember(
                    dest => dest.MerchandiseDescription,
                    opt => opt.MapFrom(m => m.Description)
                )
                .ForMember(
                    dest => dest.MerchandiseStock,
                    opt => opt.MapFrom(m => m.Stock)
                )
                .ForMember(
                    dest => dest.MerchandiseBarcode,
                    opt => opt.MapFrom(m => m.Barcode)
                ); ;
            CreateMap<UpdateMerchandiseDto, Entities.Merchandise>()
                .ForMember(
                    dest => dest.MerchandiseName,
                    opt => opt.MapFrom(m => m.Name)
                )
                .ForMember(
                    dest => dest.MerchandiseDescription,
                    opt => opt.MapFrom(m => m.Description)
                )
                .ForMember(
                    dest => dest.MerchandiseStock,
                    opt => opt.MapFrom(m => m.Stock)
                )
                .ForMember(
                    dest => dest.MerchandiseBarcode,
                    opt => opt.MapFrom(m => m.Barcode)
                ); ;
        }
    }
}
