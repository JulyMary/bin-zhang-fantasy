namespace Fantasy.Adaption
{

	using System;

	/// <summary> * An adapter manager maintains a registry of adapter factories.
	/// Clients directly invoke methods on an adapter manager to register and unregister adapters.
	/// All adaptable objects (that is, objects that implement the <see cref="IAdaptable"/> interface) 
	/// funnel <see cref="IAdaptable.GetAdapter"/> invocations to their adapter manager's 
	/// <see cref="IAdapterManager.GetAdapter"/> method. 
	/// The adapter manager then forwards this request unmodified to 
	/// the <code>IAdapterFactory.getAdapter</code> method on one of the registered adapter factories.</summary>
	/// <example>The following code snippet shows how one might register
	/// an adapter of type <code>com.example.acme.Sticky</code> on resources in the workspace.
	/// <code>
	/// IAdapterFactory pr = new IAdapterFactory() {
	///		public Type[] GetAdapterList() {
	///			return new Type[] { com.example.acme.Sticky.class };
	///		}
	///		public Object GetAdapter(Object adaptableObject, adapterType) {
	///			IResource res = (IResource) adaptableObject;
	///			QualifiedName key = new QualifiedName("com.example.acme", "sticky-note");
	///			try {
	///				com.example.acme.Sticky v = (com.example.acme.Sticky) res.getSessionProperty(key);
	///				if (v == null) {
	///					v = new com.example.acme.Sticky();
	///					res.setSessionProperty(key, v);
	///				}
	///			} catch (CoreException e) {
	///			// unable to access session property - ignore
	///			}
	///			return v;
	///		}
	///	}
	///	</code></example>
	public interface IAdapterManager {

		/// <summary>Returns an object which is an instance of the given class associated with the given object. 
		/// Returns <code>null</code> if no such object can be found.</summary>
		/// <param name="adaptee">the adaptable object being queried (usually an instance of IAdaptable).</param>
		/// <param name="targetType">the type of adapter to look up </param>
		/// <returns>a object castable to the given adapter type, 
		/// or null if the given adaptable object does not have an adapter of the given type.</returns>
		/// <remarks>Do not call the adaptee's GetAdapter it the adaptee implements IAdaptable to avoid recursive calls.</remarks>
		Object GetAdapter(Object adaptee, Type targetType);

		/// <summary>
		/// Returns an object which is an instance of the given type associate with the given object.
		/// Returns <code>null</code> if no such object can be found.
		/// </summary>
		/// <typeparam name="T">the type of adapter to look up.</typeparam>
		/// <param name="adaptee">the adaptable object being queried (usually an instance of IAdaptable).</param>
		/// <returns>a object castable to the given adapter type, 
		/// or null if the given adaptable object does not have an adapter of the given type.</returns>
		/// <remarks>Do not call the adaptee's GetAdapter it the adaptee implements IAdaptable to avoid recursive calls.</remarks>
		T GetAdapter<T>(Object adaptee);
		
		/// <summary>Registers the given adapter factory as extending objects of the given type.</summary>
		/// <remarks>If the type being extended is a class, the given factory's adapters are 
		/// available on instances of that class and any of its subclasses. 
		/// If it is an interface, the adapters are 
		/// available to all classes that directly or indirectly implement that interface.</remarks>
		/// <param name="factory">the adapter factory</param>
		/// <param name="adapteeType">the type being extended</param>
		void RegisterFactory(IAdapterFactory factory, Type adapteeType);

		/// <summary>Removes the given adapter factory from the list of factories 
		/// registered as extending the given class. Does nothing if 
		/// the given factory and type combination is not registered.</summary>
		/// <param name="factory">the adapter factory to remove</param>
		/// <param name="adapteeType">one of the types against which the given factory is registered</param>
		void UnregisterFactory(IAdapterFactory factory, Type adapteeType);

		/// <summary>Removes the given adapter factory completely from the list of registered factories. 
		/// Equivalent to calling unregisterAdapters(IAdapterFactory,Class) 
		/// on all classes against which it had been explicitly registered. 
		/// Does nothing if the given factory is not currently registered.</summary>
		/// <param name="factory">the adapter factory to remove.</param>
		void UnregisterFactory(IAdapterFactory factory);

	}
	

}
