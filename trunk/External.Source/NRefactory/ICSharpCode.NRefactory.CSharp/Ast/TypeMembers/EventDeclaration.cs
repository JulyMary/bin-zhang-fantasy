﻿// 
// EventDeclaration.cs
//
// Author:
//       Mike Krüger <mkrueger@novell.com>
// 
// Copyright (c) 2009 Novell, Inc (http://www.novell.com)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System.Collections.Generic;

namespace ICSharpCode.NRefactory.CSharp
{
	public class EventDeclaration : AttributedNode
	{
		public override NodeType NodeType {
			get { return NodeType.Member; }
		}
		
		public AstType ReturnType {
			get { return GetChildByRole (Roles.Type); }
			set { SetChildByRole(Roles.Type, value); }
		}
		
		public AstNodeCollection<VariableInitializer> Variables {
			get { return GetChildrenByRole (Roles.Variable); }
		}
		
		public override S AcceptVisitor<T, S> (IAstVisitor<T, S> visitor, T data = default(T))
		{
			return visitor.VisitEventDeclaration (this, data);
		}
		
		protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
		{
			EventDeclaration o = other as EventDeclaration;
			return o != null && this.MatchAttributesAndModifiers(o, match)
				&& this.ReturnType.DoMatch(o.ReturnType, match) && this.Variables.DoMatch(o.Variables, match);
		}
	}
	
	public class CustomEventDeclaration : MemberDeclaration
	{
		public static readonly Role<Accessor> AddAccessorRole = new Role<Accessor>("AddAccessor", Accessor.Null);
		public static readonly Role<Accessor> RemoveAccessorRole = new Role<Accessor>("RemoveAccessor", Accessor.Null);
		
		public CSharpTokenNode LBraceToken {
			get { return GetChildByRole (Roles.LBrace); }
		}
		
		public Accessor AddAccessor {
			get { return GetChildByRole (AddAccessorRole); }
			set { SetChildByRole (AddAccessorRole, value); }
		}
		
		public Accessor RemoveAccessor {
			get { return GetChildByRole (RemoveAccessorRole); }
			set { SetChildByRole (RemoveAccessorRole, value); }
		}
		
		public CSharpTokenNode RBraceToken {
			get { return GetChildByRole (Roles.RBrace); }
		}
		
		public override S AcceptVisitor<T, S> (IAstVisitor<T, S> visitor, T data = default(T))
		{
			return visitor.VisitCustomEventDeclaration (this, data);
		}
		
		protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
		{
			CustomEventDeclaration o = other as CustomEventDeclaration;
			return o != null && this.MatchMember(o, match)
				&& this.AddAccessor.DoMatch(o.AddAccessor, match) && this.RemoveAccessor.DoMatch(o.RemoveAccessor, match);
		}
	}
}
