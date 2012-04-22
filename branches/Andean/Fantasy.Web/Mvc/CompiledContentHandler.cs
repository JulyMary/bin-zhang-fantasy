using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Fantasy.Web.Mvc
{
    class CompiledContentHandler : IHttpHandler
    {
        private string _extension;
        private Stream _stream;
        public CompiledContentHandler(string extension, Stream stream)
        {
            this._extension = extension;
            this._stream = stream;
        }


       
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = MIMETypes.GetMIMETypeForExtension(this._extension);

            byte[] buffer = new byte[this._stream.Length];
            this._stream.Read(buffer, 0, (int)this._stream.Length);

            context.Response.BinaryWrite(buffer);
            context.Response.Flush();
            this._stream.Close();

        }

       
       
    }
}