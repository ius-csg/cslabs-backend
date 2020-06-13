using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using CSLabs.Api.Models;
using CSLabs.Api.Models.UserModels;
using CSLabs.Api.RequestModels;
using CSLabs.Api.ResponseModels;
using CSLabs.Api.Util;
using Microsoft.Extensions.DependencyInjection;

namespace CSLabs.Api.Services
{
    public class UrlBasedUploadManager
    {
        private Dictionary<string, UploadProgress> UploadProgresses { get; } = new Dictionary<string, UploadProgress>();
        private IServiceProvider _serviceProvider;

        public UrlBasedUploadManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public UploadProgress GetProgress(string requestId)
        {
            lock(this)
            {
                if(UploadProgresses.ContainsKey(requestId))
                {
                    var progress = UploadProgresses[requestId];
                    if (progress.Status == EUploadStatus.Complete || progress.Status == EUploadStatus.Error)
                    {
                        UploadProgresses.Remove(requestId);
                    }

                    return progress;
                }
                return new UploadProgress(EUploadStatus.NotFound);
            }
        }

        private HttpWebResponse DownloadFile(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.MaximumResponseHeadersLength = 100;
            return (HttpWebResponse)request.GetResponse();
        }

        public void QueueUpload(FromUrlRequest request, string requestId, User user)
        {
            ThreadPool.QueueUserWorkItem(async _ =>
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetService<DefaultContext>();
                var service = scope.ServiceProvider.GetService<ProxmoxVmTemplateService>();
                try
                {
                    using var response = DownloadFile(ShareLinkConverter.ConvertUrl(request.Url));
                    long contentLength = long.Parse(response.Headers["Content-Length"]);
                    using var stream = response.GetResponseStream();
                    await service.UploadTemplate(context, request.Name, user, stream, contentLength, progress => SetProgress(requestId, progress));
                    SetComplete(requestId);
                }
                catch (Exception e)
                {
                    SetError(requestId);
                    Console.Error.WriteLine(e);
                }
            });
        }
        
        public void SetProgress(string requestId, UploadProgress progress)
        {
            lock (this)
            {
                UploadProgresses[requestId] = progress;
            }
        }
        
        public void SetProgress(string requestId, double progress)
        {
            SetProgress(requestId, new UploadProgress(EUploadStatus.Downloading, progress));
        }
        
        public void SetError(string requestId)
        {
           
            SetProgress(requestId, new UploadProgress(EUploadStatus.Error));
        }
        
        public void SetComplete(string requestId)
        {
           
            SetProgress(requestId, new UploadProgress(EUploadStatus.Complete));
        }
    }
}