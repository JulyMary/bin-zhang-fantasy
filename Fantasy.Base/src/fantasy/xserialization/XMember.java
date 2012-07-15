package fantasy.xserialization;
import java.lang.annotation.*;

@Target(ElementType.FIELD)
@Retention(RetentionPolicy.RUNTIME)
public @interface XMember
{
	
	public int order() default Integer.MAX_VALUE;
	
	public String name();
	
	public String namespaceUri();
	
	@SuppressWarnings("rawtypes")
	public Class converter();
	

}