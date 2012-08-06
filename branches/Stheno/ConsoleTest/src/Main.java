import java.lang.reflect.Array;

/*
 * Copyright (c) 2006-2008 Hyperic, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */





/**
 * Display cpu information for each cpu found on the system.
 */
public class Main{

   
    

    @SuppressWarnings({ "unused", "rawtypes" })
	public static void main(String[] args) throws Exception {
       String[] array = new String[10];
       String s = array.getClass().getName();
       
       Class cls = Class.forName(s);
       
       Object array2 = Array.newInstance(cls.getComponentType(), 10);
       
    }
}
