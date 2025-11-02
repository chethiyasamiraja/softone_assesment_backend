using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Web;
using System.Web.Http;

namespace ReportingApi
{
    public class FileResult : IHttpActionResult
    {
        private readonly Stream _fileStream;
        private readonly string _contentType;
        private readonly string _fileName;

        public FileResult(Stream fileStream, string contentType, string fileName)
        {
            _fileStream = fileStream;
            _contentType = contentType;
            _fileName = fileName;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage()
            {
                Content = new StreamContent(_fileStream)
                {
                    Headers = { ContentType = new MediaTypeHeaderValue(_contentType) }
                }
            };

            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = _fileName
            };

            return Task.FromResult(response);
        }
    }
}