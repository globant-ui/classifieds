using System;
using System.Net.Http;
using System.Net.Http.Headers;


namespace Classifieds.ListingsAPI.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public class StreamProvider : MultipartFormDataStreamProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uploadPath"></param>
        public StreamProvider(string uploadPath)
            : base(uploadPath)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            string fileName = headers.ContentDisposition.FileName;
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = Guid.NewGuid().ToString() + ".data";
            }
            return fileName.Replace("\"", string.Empty);
        }
    }
}