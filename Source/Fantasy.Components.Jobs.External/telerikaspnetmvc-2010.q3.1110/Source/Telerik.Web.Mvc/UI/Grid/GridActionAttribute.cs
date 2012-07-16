// (c) Copyright 2002-2010 Telerik 
// This source is subject to the GNU General Public License, version 2
// See http://www.gnu.org/licenses/gpl-2.0.html. 
// All other rights reserved.

namespace Telerik.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Telerik.Web.Mvc.Extensions;
    using Telerik.Web.Mvc.Infrastructure;
    using Telerik.Web.Mvc.Resources;
    using Telerik.Web.Mvc.UI;

    /// <summary>
    /// Used for action methods when using Ajax or Custom binding
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class GridActionAttribute : FilterAttribute, IActionFilter
    {
        private readonly IGridActionResultAdapterFactory adapterFactory;
        /// <summary>
        /// Initializes a new instance of the <see cref="GridActionAttribute"/> class.
        /// </summary>
        public GridActionAttribute()
        {
            ActionParameterName = "command";
            adapterFactory = DI.Current.Resolve<IGridActionResultAdapterFactory>();
        }

        /// <summary>
        /// Gets or sets the name of the action parameter. The default value is "command".
        /// </summary>
        /// <value>The name of the action parameter.</value>
        /// <example>
        /// <code lang="CS">
        /// [GridAction(ActionParameterName="param")]
        /// public ActionResult Index(GridCommand param)
        /// {
        /// }
        /// </code>
        /// </example>
        public string ActionParameterName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the Grid that is populated by the associated action method. Required
        /// when custom server binding is enabled and the grid query string parameters are prefixed.
        /// </summary>
        /// <example>
        /// <code lang="CS">
        /// [GridAction(EnableCustomBinding=true, GridName="Employees")]
        /// public ActionResult Index(GridCommand param)
        /// {
        /// }
        /// </code>
        /// </example>
        public string GridName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether custom binding is enabled. Used when implementing custom ajax binding.
        /// </summary>
        /// <value><c>true</c> if custom binding is enabled; otherwise, <c>false</c>. The default value is <c>false</c>.</value>
        /// <example>
        /// <code lang="CS">
        /// [GridAction(EnableCustomBinding=true)]
        /// public ActionResult Index(GridCommand param)
        /// {
        /// }
        /// </code>
        /// </example>
        public bool EnableCustomBinding
        {
            get;
            set;
        }

        private string Prefix(string key)
        {
            if (GridName.HasValue())
            {
                return GridName + "-" + key;
            }

            return key;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionParameters.ContainsKey(ActionParameterName))
            {
                GridCommand command = new GridCommand
                {
                    Page = filterContext.Controller.ValueOf<int>(Prefix(GridUrlParameters.CurrentPage)),
                    PageSize = filterContext.Controller.ValueOf<int>(Prefix(GridUrlParameters.PageSize))
                };

                string orderBy = filterContext.Controller.ValueOf<string>(Prefix(GridUrlParameters.OrderBy));

                command.SortDescriptors.AddRange(GridDescriptorSerializer.Deserialize<SortDescriptor>(orderBy));

                string filter = filterContext.Controller.ValueOf<string>(Prefix(GridUrlParameters.Filter));

                command.FilterDescriptors.AddRange(FilterDescriptorFactory.Create(filter));

                string groupBy = filterContext.Controller.ValueOf<string>(Prefix(GridUrlParameters.GroupBy));

                command.GroupDescriptors.AddRange(GridDescriptorSerializer.Deserialize<GroupDescriptor>(groupBy));

                filterContext.ActionParameters[ActionParameterName] = command;
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                return;
            }

            var actionResultAdapter = adapterFactory.Create(filterContext.Result);

            if (actionResultAdapter == null)
            {
                return;
            }

            var dataSource = actionResultAdapter.GetDataSource();

            if (dataSource == null)
            {
                return;
            }

            var total = actionResultAdapter.GetTotal();

            var dataProcessor = new GridDataProcessor(new GridActionBindingContext(EnableCustomBinding, filterContext.Controller, dataSource, total));

            var result = new Dictionary<string, object>();
            var dataTableEnumerable = dataSource as GridDataTableWrapper;
            if (dataTableEnumerable != null && dataTableEnumerable.Table != null)
            {
                result["data"] = dataProcessor.ProcessedDataSource.SerializeToDictionary(dataTableEnumerable.Table);
            }
            else
            {
                result["data"] = dataProcessor.ProcessedDataSource;
            }
                
            result["total"] = dataProcessor.Total;

            var modelState = actionResultAdapter.GetModelState();

            if (modelState != null && !modelState.IsValid)
            {
                result["modelState"] = SerializeErrors(modelState);
            }

            filterContext.Result = new JsonResult
            {
                Data = result
            };
        }       
        
        private object SerializeErrors(ModelStateDictionary modelState)
        {
            return modelState.Where(entry => entry.Value.Errors.Any())
                             .ToDictionary(entry => entry.Key, entry => SerializeModelState(entry.Value));
        }

        private static Dictionary<string, object> SerializeModelState(ModelState modelState)
        {
            var result = new Dictionary<string, object>();
            result["errors"] = modelState.Errors
                                         .Select(error => GetErrorMessage(error, modelState))
                                         .ToArray();
            return result;
        }
        
        private static string GetErrorMessage(ModelError error, ModelState modelState)
        {
            if (!error.ErrorMessage.HasValue())
            {
                if (modelState.Value == null)
                {
                    return error.ErrorMessage;
                }

                return TextResource.ValueNotValidForProperty.FormatWith(modelState.Value.AttemptedValue);
            }

            return error.ErrorMessage;
        }
    }
}