using System;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;
using Fantasy.ServiceModel;
namespace Fantasy.Adaption
{



	/// <summary>See <see cref="IAdapterManager"/></summary>
	public class DefaultAdapterManager: ObjectWithSite, IAdapterManager 
	{

		///<summary>Key is an adaptee type, value is a list of factories for the adaptee type.</summary>
		private Dictionary<Type, List<IAdapterFactory>> _AdapteeFactories = new Dictionary<Type, List<IAdapterFactory>>();

		/// <summary>Caches the look up result.
		/// Key is an adaptee type, value is a list of factories for TypeFactoryMap 
		/// that contains factory for each ancestor class for adaptee.</summary>
		private Dictionary<Type, Dictionary<Type, IAdapterFactory>> _AdapterLookup = new Dictionary<Type, Dictionary<Type, IAdapterFactory>>();

		/// <summary>Creates an instance.</summary>
		public DefaultAdapterManager() 
		{
 
		}

		/// <summary>See <see cref="IAdapterManager.GetAdapter"/></summary>
		public object GetAdapter(object adaptee, Type targetType) 
		{
			if (targetType == null)
			{
				throw new ArgumentNullException("targetType");
			}

			object rs = null;

			Type adapteeType = null != adaptee ? adaptee.GetType() : typeof(void); 

			//See if there's any factory can adapt this factory.
			IAdapterFactory factory = GetFactory(adapteeType, targetType);
			if (null != factory) 
			{
				rs = factory.GetAdapter(adaptee, targetType);
				if (rs != null) 
					return rs;
			}

			//check if the adaptee inherits or implements targetType at last.
			if (targetType.IsInstanceOfType(adaptee))
				return adaptee;

			return null;
		}

		public T GetAdapter<T>(object adaptee)
		{
			return (T)GetAdapter(adaptee, typeof(T));
		}

		/// <summary>
		/// Search <see cref="IAdapterFactory"/> by given adaptee type and target type. Returns <code>null</code> if no such factory can be found.
		/// </summary>
		/// <param name="adapteeType">addptee type</param>
		/// <param name="targetType">target type</param>
		/// <returns>factory was matched the given type or <code>null</code>.</returns>
		private IAdapterFactory GetFactory(Type adapteeType, Type targetType) 
		{
			IAdapterFactory rs = null;
			foreach (Type t in GetHierarchyTypes(adapteeType))
			{
				rs = InternalGetFactory(t, targetType);
				if (null != rs)
				{
					return rs;
				}
			}
			return rs;
		}

		private IAdapterFactory InternalGetFactory(Type adapteeType,Type targetType)
		{
			IAdapterFactory rs;
			Dictionary<Type, IAdapterFactory> targetFactories;

			if (!_AdapterLookup.TryGetValue(adapteeType, out targetFactories))
			{
				targetFactories = new Dictionary<Type, IAdapterFactory>();
				_AdapterLookup.Add(adapteeType, targetFactories);
			}


			if (!targetFactories.TryGetValue(targetType, out rs))
			{
				List<IAdapterFactory> factories;

				if (_AdapteeFactories.TryGetValue(adapteeType, out factories))
				{
					foreach (IAdapterFactory factory in factories)
					{
						foreach (Type t in factory.GetTargetTypes())
						{
							if (targetType == t)
							{
								rs = factory;
								targetFactories.Add(targetType, rs);
								return rs;
							}
						}
					}
				}
			}

			return rs;
		}


		private ArrayList GetHierarchyTypes(Type t)
		{
			ArrayList rs = new ArrayList(10);
			if (typeof(void) != t)
			{
				Type temp = t;
				do
				{
					rs.Add(temp);
					rs.AddRange(GetInterfaces(temp));
					temp = temp.BaseType;
				} while (null != temp);

			}
			else
			{
				rs.Add(typeof(void));
			}

			return rs;
		}


		private Type[] GetInterfaces(Type t)
		{
			ArrayList rs = new ArrayList();
			Type[] curIntfs = t.GetInterfaces();

			Type[] parIntfs = t.BaseType != null ? t.BaseType.GetInterfaces() : new Type[0];

			foreach (Type intf in curIntfs)
			{
				if (Array.IndexOf(parIntfs, intf) < 0)
				{
					rs.Add(intf);
				}
			}

			rs.Sort(InterfaceHierarchyComparer.Default);

			return (Type[])rs.ToArray(typeof(Type));
		}



		//private 

		/// <summary>See <see cref="IAdapterManager.RegisterFactory"/></summary>
		public void RegisterFactory(IAdapterFactory factory, Type adapteeType) 
		{
            if (factory is IObjectWithSite)
            {
                ((IObjectWithSite)factory).Site = this.Site;
            }


			if (null == adapteeType)
			{
				adapteeType = typeof(void);
			}
			List<IAdapterFactory> list;
			if ( ! this._AdapteeFactories.TryGetValue(adapteeType, out list))
			{
				list = new List<IAdapterFactory>();
				this._AdapteeFactories.Add(adapteeType, list);
			}




			list.Add(factory);
		}

		/// <summary>See <see cref="IAdapterManager.UnregisterFactory(IAdapterFactory, Type)"/></summary>
		public void UnregisterFactory(IAdapterFactory factory, Type adapteeType) 
		{
			if (null == adapteeType)
			{
				adapteeType = typeof(void);
			}

			List<IAdapterFactory> list;
			if (this._AdapteeFactories.TryGetValue(adapteeType, out list))
			{
				list.Remove(factory);
			}
		}

		/// <summary>See <see cref="IAdapterManager.UnregisterFactory(IAdapterFactory)"/></summary>
		public void UnregisterFactory(IAdapterFactory factory) 
		{
			foreach (List<IAdapterFactory> list in this._AdapteeFactories.Values)
			{
				list.Remove(factory);
			}
		}


		/// <summary>
		/// Provide a method to compare two interfaces by these hierarchy. 
		/// </summary>
		private class InterfaceHierarchyComparer : IComparer
		{
			public int Compare(object x, object y)
			{
				Type intf1 = (Type)x;
				Type intf2 = (Type)y;

				if (intf1.IsSubclassOf(intf2))
				{
					return -1;
				}
				else if (intf2.IsSubclassOf(intf1))
				{
					return 1;
				}
				else
				{
					return 0;
				}
			}

			public static readonly InterfaceHierarchyComparer Default = new InterfaceHierarchyComparer();
		
		}
	}
}
