package fantasy.xserialization;

import java.lang.reflect.Field;

public interface IFieldFilter {
    boolean filter(Field field);
}
