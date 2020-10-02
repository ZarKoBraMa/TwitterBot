using AutoMapper;

namespace TwitterBot.Framework.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Tweetinvi.Models.ITweet, Types.Tweet>()
                .ForMember(dest => dest.TweetUrl, opts => opts.MapFrom(src => src.Url))
                .ForMember(dest => dest.TweetCreatedBy, opts => opts.MapFrom(src => src.CreatedBy.Name))
                .ForMember(dest => dest.TweetCreatedByUrl, opts => opts.MapFrom(src => src.CreatedBy.Url))
                .ForMember(dest => dest.TweetCreatedOn, opts => opts.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.IdStr))
                .ForMember(dest => dest.Hashtags, opts => opts.Ignore());
        }
    }
}
