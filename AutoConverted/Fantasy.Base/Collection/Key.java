package Fantasy.Collections;

//C# TO JAVA CONVERTER WARNING: Java does not allow user-defined value types. The behavior of this class will differ from the original:
//ORIGINAL LINE: public struct Key<T1, T2>
public final class Key<T1, T2>
{
	public Key(T1 v1, T2 v2)
	{
		_v1 = v1;
		_v2 = v2;
	}

	private T1 _v1;

	public T1 getV1()
	{
		return _v1;
	}


	private T2 _v2;

	public T2 getV2()
	{
		return _v2;
	}


	@Override
	public boolean equals(Object obj)
	{
		if (obj instanceof Key<T1, T2>)
		{
			Key<T1, T2> o = (Key<T1, T2>)obj;
			return KeyHelper.equals(new Object[] { this.getV1(), this.getV2() }, new Object[] { o.getV1(), o.getV2() });
		}
		else
		{
			return false;
		}
	}

	@Override
	public int hashCode()
	{
		return KeyHelper.hashCode(new Object[] { this.getV1(), this.getV2() });
	}

	public static boolean OpEquality(Key<T1, T2> x, Key<T1, T2> y)
	{
		return x.equals(y);
	}

	public static boolean OpInequality(Key<T1, T2> x, Key<T1, T2> y)
	{
		return !x.equals(y);
	}

	public Key clone()
	{
		Key varCopy = new Key();

		varCopy._v1 = this._v1;
		varCopy._v2 = this._v2;
		varCopy._v1 = this._v1;
		varCopy._v2 = this._v2;
		varCopy._v3 = this._v3;
		varCopy._v1 = this._v1;
		varCopy._v2 = this._v2;
		varCopy._v3 = this._v3;
		varCopy._v4 = this._v4;
		varCopy._v1 = this._v1;
		varCopy._v2 = this._v2;
		varCopy._v3 = this._v3;
		varCopy._v4 = this._v4;
		varCopy._v5 = this._v5;

		return varCopy;
	}
}