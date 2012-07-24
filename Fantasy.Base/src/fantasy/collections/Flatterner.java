package fantasy.collections;

import java.util.*;


public class Flatterner {



	private Flatterner()
	{

	}
	
	
	public static <T> Enumerable<T> flatternAncestors(T child, Selector<T, T> selector)
	{
		return new Enumerable<T>(new AncestorIterable<T>(child, selector ));
	}
	
	
	private static class AncestorIterable<T> implements Iterable<T>
	{
		public AncestorIterable(T child, Selector<T, T> selector)
		{
			this._child = child;
			this._selector = selector;
		}

		@Override
		public Iterator<T> iterator() {
			return new AncestorIterator<T>(this._child, this._selector);
		}
		
		private T _child;
		private Selector<T, T> _selector;
		
	}
	
	
	private static class AncestorIterator<T> implements Iterator<T>
	{
		public AncestorIterator(T child, Selector<T, T> selector)
		{
			this._next = child;
			this._selector = selector;
		}

		private T _next;
	
		private Selector<T, T> _selector;
		@Override
		public boolean hasNext() {
			return _next != null;
		}
		@Override
		public T next() {
			T rs = _next;
			_next = _selector.select(_next);
			return rs;
		}
		@Override
		public void remove() {
			throw new UnsupportedOperationException();
			
		}
		
	}

	public static <T>  Enumerable<T> flatternChildren(T ancestor, Selector<T, Iterable<T>> selector)
	{
		Iterable<T> rs = new FlatternerIterable<T>(ancestor, selector);

		return new Enumerable<T>(rs);
	}


	private static class FlatternerIterable<T> implements Iterable<T>
	{
		public FlatternerIterable(final T ancestor, final Selector<T, Iterable<T>> selector)
		{
			this._ancestor = ancestor;
			this._selector = selector;
		}

		private T _ancestor;
		private Selector<T, Iterable<T>> _selector;

		@Override
		public Iterator<T> iterator() {
			return new FlatternerIterator<T>(_ancestor, _selector);
		}
	}
	
	private static class FlatternerIterator<T> implements Iterator<T>
	{



		public FlatternerIterator(final T ancestor, final Selector<T, Iterable<T>> selector)
		{
			this._ancestor = ancestor;
			this._selector = selector;
		}

		private T _ancestor;
		private Selector<T, Iterable<T>> _selector;

		private Iterator<T> _children = null;;

		private FlatternerIterator<T> _childIterator = null;

		//0: init; //1: visit children 2://end;
		private int _state = 0;

		@Override
		public boolean hasNext() {
			return _state < 2;
		}


		private void loadChidren()
		{
			Iterable<T> iterable = _selector.select(_ancestor);
			if(iterable != null)
			{
				_children = iterable.iterator();

				if(_children.hasNext())
				{
					_childIterator = new FlatternerIterator<T>(_children.next(), this._selector);
					_state = 1;
				}
				else
				{
					_state = 2;
				}

			}
			else
			{
				_state = 2;
			}
		}

		@Override
		public T next() {
			if(_state == 0)
			{
				loadChidren();
				return this._ancestor;
			}
			else if(_state == 1)
			{
				T rs = _childIterator.next();

				if(! _childIterator.hasNext())
				{
					if(_children.hasNext())
					{
						_childIterator = new FlatternerIterator<T>(_children.next(), this._selector);
					}
					else
					{
						_state = 2;
					}
				}
				return rs;
			}
			else	
			{
				throw new NoSuchElementException();
			}

		}

		@Override
		public void remove() {
			throw new UnsupportedOperationException();

		}

	}

}

 

