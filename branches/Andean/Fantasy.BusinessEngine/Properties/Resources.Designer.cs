﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Fantasy.BusinessEngine.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Fantasy.BusinessEngine.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There are zero or more than two BusinessApplication have code {0}..
        /// </summary>
        internal static string ApplicationByNameExceptionMessage {
            get {
                return ResourceManager.GetString("ApplicationByNameExceptionMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot find a BusinessApplication which type is {0}..
        /// </summary>
        internal static string ApplicationByTypeExceptionMessage {
            get {
                return ResourceManager.GetString("ApplicationByTypeExceptionMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to References.
        /// </summary>
        internal static string AssemblyReferenceGroupName {
            get {
                return ResourceManager.GetString("AssemblyReferenceGroupName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Misc.
        /// </summary>
        internal static string BusinessPropertyDefaultCategory {
            get {
                return ResourceManager.GetString("BusinessPropertyDefaultCategory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There are more than one class view settings defined for class {0} in application {1}..
        /// </summary>
        internal static string DuplicatedClassViewDefined {
            get {
                return ResourceManager.GetString("DuplicatedClassViewDefined", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Entity {0} ({1}) does not exist..
        /// </summary>
        internal static string EntityNotFoundExceptionMessage {
            get {
                return ResourceManager.GetString("EntityNotFoundExceptionMessage", resourceCulture);
            }
        }
        
        internal static System.Drawing.Bitmap FolderTemplate {
            get {
                object obj = ResourceManager.GetObject("FolderTemplate", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Misc.
        /// </summary>
        internal static string MiscCategoryName {
            get {
                return ResourceManager.GetString("MiscCategoryName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There are no property/association named {0} in business class {1}..
        /// </summary>
        internal static string PropertyNotFoundMessage {
            get {
                return ResourceManager.GetString("PropertyNotFoundMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot find property &apos;{0}&apos;.
        /// </summary>
        internal static string SecurityObjectPropertyNotFoundMessage {
            get {
                return ResourceManager.GetString("SecurityObjectPropertyNotFoundMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Templates can be used only with property access..
        /// </summary>
        internal static string TemplateLimitations {
            get {
                return ResourceManager.GetString("TemplateLimitations", resourceCulture);
            }
        }
    }
}
