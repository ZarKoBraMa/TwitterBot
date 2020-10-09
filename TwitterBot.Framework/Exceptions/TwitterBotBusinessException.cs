using System;
using System.Collections.Generic;
using System.Linq;
using TwitterBot.Framework.Types;

namespace TwitterBot.Framework.Exceptions
{
    public class TwitterBotBusinessException : Exception
    {
        public List<Hashtag> Hashtags { get; set; }

        public TwitterBotBusinessException(List<Hashtag> hashtags)
        {
            Hashtags = hashtags;
        }
        
        public override string Message
        {
            get
            {
                return string.Format("Exception Occurred while processing: {0} ", string.Join(",", Hashtags.Select(p => p.Text)));
            }
        }
    }
}
