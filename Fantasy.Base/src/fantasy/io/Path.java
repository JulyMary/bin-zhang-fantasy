package fantasy.io;

import java.io.File;
import java.nio.file.*;

public final class Path
{
	
	private static  char AltDirectorySeparatorChar = '/';
	public static  char directorySeparatorChar;
	private static final char[] InvalidFileNameChars = new char[] { '"', '<', '>', '|', '\0', '\u0001', '\u0002', '\u0003', '\u0004', '\u0005', '\u0006', '\b', '\t', '\n',  '\f', '\r', '\u000e', '\u000f', '\u0010', '\u0011', '\u0012', '\u0013', '\u0014', '\u0015', '\u0016', '\u0017', '\u0018', '\u0019', '\u001a', '\u001b', '\u001c', '\u001d', '\u001e', '\u001f', ':', '*', '?', '\\', '/' };

	private static final char[] RealInvalidPathChars  = new char[] { '"', '<', '>', '|', '\0', '\u0001', '\u0002', '\u0003', '\u0004', '\u0005', '\u0006',  '\b', '\t', '\n',  '\f', '\r', '\u000e', '\u000f', '\u0010', '\u0011', '\u0012', '\u0013', '\u0014', '\u0015', '\u0016', '\u0017', '\u0018', '\u0019', '\u001a', '\u001b', '\u001c', '\u001d', '\u001e', '\u001f' };
	
	public static final char volumeSeparatorChar = ':';

	
	static
	{
		directorySeparatorChar = File.separatorChar;
	}

	private Path()
	{
		
	}

	public static String changeExtension(String path, String extension)
	{
		if (path == null)
		{
			return null;
		}
		checkInvalidPathChars(path);
		String str = path;
		int length = path.length();
		while (--length >= 0)
		{
			char ch = path.charAt(length);
			if (ch == '.')
			{
				str = path.substring(0, length);
				break;
			}
			if (((ch == directorySeparatorChar) || (ch == AltDirectorySeparatorChar)) || (ch == volumeSeparatorChar))
			{
				break;
			}
		}
		if ((extension == null) || (path.length() == 0))
		{
			return str;
		}
		if ((extension.length() == 0) || (extension.charAt(0) != '.'))
		{
			str = str + ".";
		}
		return (str + extension);
	}

	public static void checkInvalidPathChars(String path)
	{
		for (int i = 0; i < path.length(); i++)
		{
			int num2 = path.charAt(i);
			if (((num2 == 0x22) || (num2 == 60)) || (((num2 == 0x3e) || (num2 == 0x7c)) || (num2 < 0x20)))
			{
				throw new IllegalArgumentException("path");
			}
		}
	}

	public static void checkSearchPattern(String searchPattern)
	{
		int num;
		while ((num = searchPattern.indexOf("..")) != -1)
		{
			if ((num + 2) == searchPattern.length())
			{
				throw new IllegalArgumentException("searchPattern");
			}
			if ((searchPattern.charAt(num + 2) == directorySeparatorChar) || (searchPattern.charAt(num + 2) == AltDirectorySeparatorChar))
			{
				throw new IllegalArgumentException("searchPattern");
			}
			searchPattern = searchPattern.substring(num + 2);
		}
	}

	public static String combine(String[] paths) throws Exception
	{
		if (paths == null || paths.length < 2)
		{
			throw new IllegalArgumentException("paths");
		}
		java.nio.file.Path rs = null;
		for(String seg : paths)
		{
			if(rs == null)
			{
				rs = Paths.get(seg);
			}
			else
			{
				rs = Paths.get(rs.toString(), seg);
			}
		}
		
		
		return rs.normalize().toString();
		
	
		
	}
	
	

	public static String combine(String path1, String path2) throws Exception
	{
		if ((path1 == null) || (path2 == null))
		{
			throw new IllegalArgumentException((path1 == null) ? "path1" : "path2");
		}
		return combine(new String[]{path1, path2});
	}

	public static String combine(String path1, String path2, String path3) throws Exception
	{
		if (((path1 == null) || (path2 == null)) || (path3 == null))
		{
			throw new IllegalArgumentException((path1 == null) ? "path1" : ((path2 == null) ? "path2" : "path3"));
		}
		
		return combine(new String[]{path1, path2, path3});
	}

	public static String combine(String path1, String path2, String path3, String path4) throws Exception
	{
		if (((path1 == null) || (path2 == null)) || ((path3 == null) || (path4 == null)))
		{
			throw new IllegalArgumentException((path1 == null) ? "path1" : ((path2 == null) ? "path2" : ((path3 == null) ? "path3" : "path4")));
		}
		
		return combine(new String[]{path1, path2, path3, path4});
	}

	
	public static String getDirectoryName(String path)
	{
		if (path != null)
		{
			
			return Paths.get(path).getParent().toString();
			
			
		}
		return null;
	}

	public static String getExtension(String path)
	{
		if (path == null)
		{
			return null;
		}
		checkInvalidPathChars(path);
		int length = path.length();
		int startIndex = length;
		while (--startIndex >= 0)
		{
			char ch = path.charAt(startIndex);
			if (ch == '.')
			{
				if (startIndex != (length - 1))
				{
					return path.substring(startIndex, length);
				}
				return "";
			}
			if (((ch == directorySeparatorChar) || (ch == AltDirectorySeparatorChar)) || (ch == volumeSeparatorChar))
			{
				break;
			}
		}
		return "";
	}

	public static String getFileName(String path)
	{
		if (path != null)
		{
			return Paths.get(path).getFileName().toString();
		}
		return path;
	}

	public static String getFileNameWithoutExtension(String path)
	{
		path = getFileName(path);
		if (path == null)
		{
			return null;
		}
		int length = path.lastIndexOf('.');
		if (length == -1)
		{
			return path;
		}
		return path.substring(0, length);
	}


	public static String getFullPath(String path)
	{
		
		return Paths.get(path).toAbsolutePath().toString();
		
	}



	public static char[] getInvalidFileNameChars()
	{
		return (char[]) InvalidFileNameChars.clone();
	}

	public static char[] getInvalidPathChars()
	{
		return (char[]) RealInvalidPathChars.clone();
	}

	public static String getPathRoot(String path)
	{
		if (path == null)
		{
			return null;
		}
		
		return Paths.get(path).getRoot().toString();
	
	}

	

	public static int getRootLength(String path)
	{
		checkInvalidPathChars(path);
		int num = 0;
		int length = path.length();
		if ((length >= 1) && isDirectorySeparator(path.charAt(0)))
		{
			num = 1;
			if ((length >= 2) && isDirectorySeparator(path.charAt(1)))
			{
				num = 2;
				int num3 = 2;
				while ((num < length) && (((path.charAt(num) != directorySeparatorChar) && (path.charAt(num) != AltDirectorySeparatorChar)) || (--num3 > 0)))
				{
					num++;
				}
			}
			return num;
		}
		if ((length >= 2) && (path.charAt(1) == volumeSeparatorChar))
		{
			num = 2;
			if ((length >= 3) && isDirectorySeparator(path.charAt(2)))
			{
				num++;
			}
		}
		return num;
	}

	public static String getTempFileName() throws Exception
	{
		
		File f = File.createTempFile("fts", null);
		String rs = f.getAbsolutePath();
		return rs;
		
		
		
	}


	public static String getTempPath()
	{
		return System.getProperty("java.io.tmpdir");
	}

	public static boolean hasExtension(String path)
	{
		if (path != null)
		{
			checkInvalidPathChars(path);
			int length = path.length();
			while (--length >= 0)
			{
				char ch = path.charAt(length);
				if (ch == '.')
				{
					return (length != (path.length() - 1));
				}
				if (((ch == directorySeparatorChar) || (ch == AltDirectorySeparatorChar)) || (ch == volumeSeparatorChar))
				{
					break;
				}
			}
		}
		return false;
	}

	
	public static boolean isDirectorySeparator(char c)
	{
		if (c != directorySeparatorChar)
		{
			return (c == AltDirectorySeparatorChar);
		}
		return true;
	}

	public static boolean isPathRooted(String path)
	{
		if (path != null)
		{
			checkInvalidPathChars(path);
			int length = path.length();
			if (((length >= 1) && ((path.charAt(0) == directorySeparatorChar) || (path.charAt(0) == AltDirectorySeparatorChar))) || ((length >= 2) && (path.charAt(1) == volumeSeparatorChar)))
			{
				return true;
			}
		}
		return false;
	}

	public static boolean isRelative(String path)
	{
		return (((((path.length() < 3) || (path.charAt(1) != volumeSeparatorChar)) || (path.charAt(2) != directorySeparatorChar)) || (((path.charAt(0) < 'a') || (path.charAt(0) > 'z')) && ((path.charAt(0) < 'A') || (path.charAt(0) > 'Z')))) && (((path.length() < 2) || (path.charAt(0) != '\\')) || (path.charAt(1) != '\\')));
	}

	public static String getRelativePath(String base, String path) {
		String rs = new File(base).toURI().relativize(new File(path).toURI()).getPath(); 
		return rs;
	}

	
	
}



