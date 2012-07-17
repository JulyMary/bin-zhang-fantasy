package fantasy.xserialization;

import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;

@Target(ElementType.FIELD)
@Retention(RetentionPolicy.RUNTIME)
public @interface XArray
{
	
	@SuppressWarnings("rawtypes")
	public Class serializer();
	
    public int order() default Integer.MAX_VALUE;
	
	public String name();
	
	public String namespaceUri();
	
	@SuppressWarnings("rawtypes")
	public Class converter();
	
	public boolean clear() default true;
	
	public XArrayItem[] items();
	
}