using AutoMapper;

namespace TwitterBot.Framework.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Tweetinvi.Models.ITweet, Types.Tweet>();
        }
    }
}
