package Fantasy.IO;

import Fantasy.IO.Interop.*;

public final class LongPath
{
	public static String GetRelativePath(String pathFrom, String pathTo)
	{
		Uri uri = new Uri(pathFrom);
		String str = Uri.UnescapeDataString(uri.MakeRelativeUri(new Uri(pathTo)).toString()).Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
		if (!str.contains(Path.DirectorySeparatorChar.toString()))
		{
			str = "." + Path.DirectorySeparatorChar + str;
		}

		if (str.startsWith("file:\\\\"))
		{
			str = str.substring((new String("file:")).length());
		}

		if (str.startsWith("\\\\\\"))
		{
			str = str.substring((new String("\\\\\\")).length());
		}

		return str;
	}

	public static String GetDirectoryName(String path)
	{

		Uri uri = new Uri(path);

		String rs = uri.LocalPath;


		if (rs.endsWith(Path.DirectorySeparatorChar.toString()))
		{
			rs.substring(0, rs.length() - 1);
		}

		int index = rs.lastIndexOf(Path.DirectorySeparatorChar);
		if (index > 0)
		{
			rs = rs.substring(0, index);
		}

		if (rs.startsWith("file:\\\\"))
		{
			rs = rs.substring((new String("file:")).length());
		}

		return rs;
	}

	public static String Combine(String path1, String path2)
	{
		if (path1 == null)
		{
			throw new ArgumentNullException("root");
		}
		if (path2 == null)
		{
			throw new ArgumentNullException("relative");
		}
		if (!path1.endsWith("\\") && !path1.endsWith("//"))
		{
			path1 += "\\";
		}
		Uri r;
		Uri rs;
		try
		{
		   r = new Uri(path1);
		}
		catch(UriFormatException error)
		{
			throw new UriFormatException(error.getMessage() + " Uri:" + path1);
		}

		try
		{
			rs = new Uri(r, path2);
		}
		catch (UriFormatException error)
		{
			throw new UriFormatException(error.getMessage() + " Uri:" + path2);
		}

		return rs.LocalPath;
		//return Path.Combine(path1, path2);
	}

	public static String Combine(String[] paths)
	{
		if (paths == null)
		{
			throw new ArgumentNullException("paths");
		}
		if (paths.length == 0)
		{
			throw new IllegalArgumentException("path");
		}

		String rs = paths[0];
		for (int i = 1; i < paths.length; i++)
		{
			rs = Combine(rs, paths[i]);
		}
		return rs;
	}

	public static String Combine(String path1, String path2, String path3)
	{
		return Combine(Combine(path1, path2), path3);
	}

	public static String Combine(String path1, String path2, String path3, String path4)
	{
		return Combine(Combine(Combine(path1, path2), path3), path4);
	}


	public static String GetShortPathName(String path)
	{
		StringBuilder rs = new StringBuilder(255);
		NativeMethods.GetShortPathName(path, rs, 255);

		return rs.toString();
	}

	public static String GetLongPathName(String path)
	{
	   StringBuilder rs = new StringBuilder(1024);
		int len = NativeMethods.GetLongPathName(path, rs, 1024);
		if(len > 1024)
		{
			rs = new StringBuilder(len);
			len = NativeMethods.GetLongPathName(path, rs, len);
		}

		return rs.ToString(0, len);
	}



	public static String GetExtension(String path)
	{
		String file = GetFileName(path);
		int pos = file.lastIndexOf('.');
		if (pos == -1)
		{
			return "";
		}
		else
		{
			return file.substring(pos);
		}
	}

	public static String GetFileName(String path)
	{
		return Path.GetFileName(path);
	}

	public static String GetFileNameWithoutExtension(String path)
	{
		path = GetFileName(path);
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

	public static String GetFullPath(String path)
	{
		Uri uri = new Uri(path);
		if (uri.IsFile)
		{
			return LongPathCommon.NormalizeLongPath(path);
		}
		else
		{
			return path;
		}
	}

	public static String ChangeExtension(String path, String extension)
	{
		return Path.ChangeExtension(path, extension);
	}


	public static char[] GetInvalidFileNameChars()
	{
		return Path.GetInvalidFileNameChars();
	}

	public static char[] GetInvalidPathChars()
	{
		return Path.GetInvalidPathChars();
	}

//        private static Regex _pathRootRegex = new Regex(@"^\\\\\?\\(?<letter>[a-z])\:", RegexOptions.IgnoreCase); 
//
//        public static string GetPathRoot(string path)
//        {
//            if (path != null)
//            {
//                path = LongPathCommon.NormalizeLongPath(path);
//                Match m = _pathRootRegex.Match(path);
//                if (m.Success)
//                {
//                    return m.Groups["letter"].Value;
//                }
//                else
//                {
//                    return string.Empty;
//                }
//            }
//            else
//            {
//                return null;
//            }
//
//        }


	public static boolean IsPathRooted(String path)
	{
		return Path.IsPathRooted(path);
	}

	public static boolean HasExtension(String path)
	{
		return Path.HasExtension(path);
	}

	private static Regex escapeRegex = new Regex("[\\\\\\/\\:\\*\\?\\\"\\<\\>\\|]");
//C# TO JAVA CONVERTER TODO TASK: C# optional parameters are not converted to Java:
//ORIGINAL LINE: public static string EscapeFileName(string fileName, string replacement = "")
	public static String EscapeFileName(String fileName, String replacement)
	{
		return escapeRegex.Replace(fileName, replacement).trim();
	}
}