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
            var ext = fileName.Substring(fileName.LastIndexOf('.')).ToLower();
            var name = fileName.Substring(0, fileName.LastIndexOf('.'));
            fileName = name + "_" + Guid.NewGuid().ToString() + ext;
            return fileName.Replace("\"", string.Empty);
        }
    }
}