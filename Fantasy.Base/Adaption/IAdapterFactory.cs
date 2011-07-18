namespace Fantasy.Adaption
{

	using System;

	/// <summary> An adapter factory defines behavioral extensions for
	/// one or more classes that implements the <see cref="IAdaptable"/> interface. 
	/// Adapter factories are registered with an <see cref="IAdapterManager"/>.</summary>
	public interface IAdapterFactory {
		
		/// <summary>
		/// Returns an object which is an instance of the given class associated with this object. 
		/// Returns <code>null</code> if no such object can be found.
		/// </summary>
		/// <param name="adaptee">the adaptable object being queried
		///	(usually an instance of <see cref="IAdaptable"/>)</param>
		/// <param name="targetType">the adapter class to look up.</param>
		/// <returns>a object castable to the given class, 
		/// or <code>null</code> if this object does not have an adapter for the given class</returns>
		Object GetAdapter(Object adaptee, Type targetType);

		/**
		 * Returns the collection of adapter types handled by this
		 * factory.
		 * <p>
		 * This method is generally used by an adapter manager
		 * to discover which adapter types are supported, in advance
		 * of dispatching any actual <code>getAdapter</code> requests.
		 * </p>
		 *
		 * @return the collection of target types
		 */
		/// <summary>Returns the collection of adapter types handled by this factory.</summary>
		/// <remarks>This method is generally used by an adapter manager
		/// to discover which adapter types are supported, in advance
		/// of dispatching any actual <see cref="GetAdapter"/> requests.</remarks>
		/// <returns>a collection of target types.</returns>
		Type[] GetTargetTypes();

	}
	
}
