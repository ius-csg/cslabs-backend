using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace CSLabs.Api.ResponseModels
{
    public class UploadProgress
    {
        public double Progress { get; }
        [JsonConverter(typeof(StringEnumConverter))]
        public EUploadStatus Status { get; }

        public UploadProgress(EUploadStatus status)
        {
            Status = status;
        }
        
        public UploadProgress(EUploadStatus status, double progress)
        {
            Status = status;
            Progress = progress;
        }
    }
}