package Fantasy.Collections;

//C# TO JAVA CONVERTER WARNING: Java does not allow user-defined value types. The behavior of this class will differ from the original:
//ORIGINAL LINE: public struct Key<T1, T2, T3>
public final class Key<T1, T2, T3>
{
	public Key(T1 v1, T2 v2, T3 v3)
	{
		_v1 = v1;
		_v2 = v2;
		_v3 = v3;

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

	private T3 _v3;

	public T3 getV3()
	{
		return _v3;
	}



	@Override
	public boolean equals(Object obj)
	{
		if (obj instanceof Key<T1, T2, T3>)
		{
			Key<T1, T2, T3> o = (Key<T1, T2, T3>)obj;
			return KeyHelper.equals(new Object[] { this.getV1(), this.getV2(), this.getV3() }, new Object[] { o.getV1(), o.getV2(), o.getV3() });
		}
		else
		{
			return false;
		}
	}

	@Override
	public int hashCode()
	{
		return KeyHelper.hashCode(new Object[] { this.getV1(), this.getV2(), this.getV3() });
	}

	public static boolean OpEquality(Key<T1, T2, T3> x, Key<T1, T2, T3> y)
	{
		return x.equals(y);
	}

	public static boolean OpInequality(Key<T1, T2, T3> x, Key<T1, T2, T3> y)
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