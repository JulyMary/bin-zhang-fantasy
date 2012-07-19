package fantasy.xserialization;

import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;

@Target(ElementType.TYPE)
@Retention(RetentionPolicy.RUNTIME)
public @interface XSerializable
{

	public String name();
	
	public String namespaceUri();

	@SuppressWarnings("rawtypes")
	public Class converter() default Dummy.class;

}