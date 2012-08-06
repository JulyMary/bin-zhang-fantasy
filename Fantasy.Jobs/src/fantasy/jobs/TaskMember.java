package fantasy.jobs;

import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;



@Target(ElementType.FIELD)
@Retention(RetentionPolicy.RUNTIME)
public @interface TaskMember
{
	String name();
	TaskMemberFlags[] flags() default TaskMemberFlags.Input;
	boolean parseInline() default true;
	String description() default "";
}