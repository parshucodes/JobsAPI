using AutoMapper;
using JobsAPI.Models;

namespace JobsAPI.WebModels.LocationWebModel
{
    public class AutoMapperLocationProfile : Profile
    {
        public AutoMapperLocationProfile()
        {
            CreateMap<Location, GetLocationResponseWebModel>();
            CreateMap<CreateLocationRequestWebModel, Location>();
        }
    }
}
