using Microsoft.Azure.WebJobs.Host;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TwitterBot.Framework.Contracts.Data;
using TwitterBot.Framework.Types;

namespace TwitterBot.Framework.Exceptions
{
    public class TwitterBotBusinessExceptionFilter : IFunctionExceptionFilter
    {
        private IDocumentDbRepository<Hashtag> _hashTagRepository;

        public TwitterBotBusinessExceptionFilter(IDocumentDbRepository<Hashtag> hashTagRepository)
        {
            _hashTagRepository = hashTagRepository;
        }

        public async Task OnExceptionAsync(FunctionExceptionContext exceptionContext, CancellationToken cancellationToken)
        {
            if (exceptionContext.Exception.InnerException is TwitterBotBusinessException)
            {
                var erroredHashtags = (exceptionContext.Exception.InnerException as TwitterBotBusinessException).Hashtags;
                await ProcessErroredHashtags(erroredHashtags);
            }
        }

        private async Task ProcessErroredHashtags(List<Hashtag> hashtags)
        {
            if (!hashtags.Any())
            {
                return;
            }

            foreach (var hashtag in hashtags)
            {
                hashtag.IsCurrentlyInQueue = false;
                await _hashTagRepository.AddOrUpdateAsync(hashtag);
            }
        }
    }
}
