﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web.Razor.Generator;

namespace RazorGenerator.Core
{
    [Export("MvcHelper", typeof(IRazorCodeTransformer))]
    public class MvcHelperTransformer : AggregateCodeTransformer
    {
        private const string WriteToMethodName = "WebViewPage.WriteTo";
        private const string WriteLiteralToMethodName = "WebViewPage.WriteLiteralTo";
        private readonly IRazorCodeTransformer[] _transformers = new IRazorCodeTransformer[] {
            new SetImports(MvcViewTransformer.MvcNamespaces, replaceExisting: false),
            new AddGeneratedClassAttribute(),
            new DirectivesBasedTransformers(),
            new MakeTypeStatic(),
            new MakeTypeHelper(),
            new RemoveLineHiddenPragmas(),
            new MvcWebConfigTransformer(),
        };

        protected override IEnumerable<IRazorCodeTransformer> CodeTransformers
        {
            get { return _transformers; }
        }

        public override void Initialize(RazorHost razorHost, IDictionary<string, string> directives)
        {
            base.Initialize(razorHost, directives);
            razorHost.DefaultBaseClass = String.Empty;

            razorHost.GeneratedClassContext = new GeneratedClassContext(
                    executeMethodName: GeneratedClassContext.DefaultExecuteMethodName,
                    writeMethodName: GeneratedClassContext.DefaultWriteMethodName,
                    writeLiteralMethodName: GeneratedClassContext.DefaultWriteLiteralMethodName,
                    writeToMethodName: WriteToMethodName,
                    writeLiteralToMethodName: WriteLiteralToMethodName,
                    templateTypeName: typeof(System.Web.WebPages.HelperResult).FullName,
                    defineSectionMethodName: "DefineSection"
            );
        }

        public override void ProcessGeneratedCode(CodeCompileUnit codeCompileUnit,
                                                      CodeNamespace generatedNamespace,
                                                      CodeTypeDeclaration generatedClass,
                                                      CodeMemberMethod executeMethod)
        {

            // Run the base processing
            base.ProcessGeneratedCode(codeCompileUnit, generatedNamespace, generatedClass, executeMethod);

            // Remove the constructor 
            generatedClass.Members.Remove(generatedClass.Members.OfType<CodeConstructor>().SingleOrDefault());
        }
    }
}
