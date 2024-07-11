using AutoMapper;

namespace TrabajoFinalDyAW.MapperProfiles
{
    public class MerchandiseProfile: Profile
    {
        public MerchandiseProfile()
        {
            CreateMap<Entities.Merchandise, Models.Merchandise>().ReverseMap();
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
            CreateMap<DTOs.CreateMerchandiseDto, Entities.Merchandise>();
            CreateMap<DTOs.UpdateMerchandiseDto, Entities.Merchandise>();
        }
    }
}
