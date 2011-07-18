namespace Fantasy.Adaption
{

	using System;

	/// <summary>An interface for an adaptable object.</summary>
	/// <remarks>
	/// <para>Adaptable objects can be dynamically extended to provide different interfaces (or "adapters"). 
	/// Adapters are created by adapter factories, which are in turn managed by type by adapter managers.</para>
	/// <para>The key goal of <see cref="IAdaptable"/> is to extends the functionality of an object 
	/// at runtime against compile-time.</para>
	/// </remarks>
	/// <example>
	///      IAdaptable a = [some adaptable];
	///      IFoo x = (IFoo)a.GetAdapter(typeof(IFoo));
	///		 if (x != null)
	///			 [do IFoo things with x]
	/// </example>
	public interface IAdaptable {

		/// <summary>
		/// Returns an object which is an instance of the given class associated with this object. 
		/// Returns <code>null</code> if no such object can be found.
		/// </summary>
		/// <param name="targetType">the adapter class to look up.</param>
		/// <returns>a object castable to the given class, 
		/// or <code>null</code> if this object does not have an adapter for the given class</returns>
		Object GetAdapter(Type targetType);

	}
	
}
