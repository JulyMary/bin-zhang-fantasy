﻿package fantasy.xserialization;

import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;

@Target(ElementType.FIELD)
@Retention(RetentionPolicy.RUNTIME)
public @interface XValue {

	
	
//	public String name();
//	
//	public String namespaceUri();
	
	@SuppressWarnings("rawtypes")
	public Class converter() default Dummy.class;
	


}