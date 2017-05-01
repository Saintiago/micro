using System.Collections.Generic;
using System.Web.Http;
using System;
using System.Net;
using System.Linq;
using PoemUtils;

namespace Circuit
{
    public class ValuesController : ApiController
    {
        private Publisher _transport;
        private CharCounter _vowelsCounter;

        public ValuesController()
        {
            _transport = new Publisher();
            _vowelsCounter = new CharCounter(Config.VOWELS);
        }

        // POST api/values 
        public string Post([FromBody]string poem)
        {
            poem = WebUtility.UrlDecode(poem);
            string corrId = Guid.NewGuid().ToString();
            
            SendPoemFilteringStartedMessage(corrId, poem);

            List<string> lines = Utils.GetLinesList(poem);
            int lineIndex = 1;
            foreach (string line in lines)
            {
                ConsonantsMessage msg = new ConsonantsMessage();
                msg.corrId = corrId;
                msg.Line = lineIndex + Config.LINE_INDEX_DELIMITER + line;
                msg.VowelsCount = _vowelsCounter.Count(line);
                msg.linesCount = lines.Count();

                Console.WriteLine(msg.corrId);
                Console.WriteLine(msg.Line);
                Console.WriteLine(msg.VowelsCount);
                Console.WriteLine(msg.ConsonantsCount);
                Console.WriteLine(msg.linesCount);

                _transport.GetBus().Publish<ConsonantsMessage>(msg);
                ++lineIndex;
            }

            return corrId;
        }

        private void SendPoemFilteringStartedMessage(string corrId, string poem)
        {
            PoemFilteringStarted msg = new PoemFilteringStarted();
            msg.corrId = corrId;
            msg.poem = poem;
            _transport.GetBus().Publish<PoemFilteringStarted>(msg);
        }
    }
}