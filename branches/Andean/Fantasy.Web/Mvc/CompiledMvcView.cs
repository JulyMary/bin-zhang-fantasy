using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.WebPages;
using System.IO;

namespace Fantasy.Web.Mvc
{
    public class CompiledMvcView : IView, IDisposable
    {
        private readonly Type _type;
        private readonly string _virtualPath;

        public CompiledMvcView(Type type, bool runViewStartPages, IEnumerable<string> fileExtension)
        {
            _type = type;
            _virtualPath = type.GetCustomAttributes(typeof(PageVirtualPathAttribute), true).Cast<PageVirtualPathAttribute>().Single().VirtualPath;
            RunViewStartPages = runViewStartPages;
            ViewStartFileExtensions = fileExtension;
        }


        private WebViewPage _page;

        public bool RunViewStartPages
        {
            get;
            private set;
        }

        public IEnumerable<string> ViewStartFileExtensions
        {
            get;
            private set;
        }

        public void Render(ViewContext viewContext, TextWriter writer)
        {
            object instance = Activator.CreateInstance(_type);

            this._page = instance as WebViewPage;

            if (_page == null)
            {
                throw new InvalidOperationException("Invalid view type");
            }

            _page.VirtualPath = _virtualPath;
            _page.ViewContext = viewContext;
            _page.ViewData = viewContext.ViewData;
            _page.InitHelpers();

            WebPageRenderingBase startPage = null;
            if (this.RunViewStartPages)
            {
                startPage = StartPage.GetStartPage(_page, "_ViewStart", ViewStartFileExtensions);
            }

            var pageContext = new WebPageContext(viewContext.HttpContext, _page, null);
            _page.ExecutePageHierarchy(pageContext, writer, startPage);
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (this._page is IDisposable)
            {
                ((IDisposable)this._page).Dispose();
            }
        }

        #endregion
    }
}
