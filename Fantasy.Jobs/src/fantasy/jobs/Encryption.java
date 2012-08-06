package fantasy.jobs;

@SuppressWarnings("unused")
public final class Encryption
{


	private static String _passPhrase = "F^nt^3y"; // can be any string
	private static String _saltValue = "JobS1v1c3"; // can be any string
	private static String _hashAlgorithm = "SHA1"; // can be "MD5"
	
	private static int _passwordIterations = 2; // can be any number
	private static String _initVector = "@1B2c3D4e5F6g7H8"; // must be 16 bytes
	private static int _keySize = 256; // can be 192 or 128

	/** 
	 Encrypts specified plaintext using Rijndael symmetric key algorithm
	 and returns a base64-encoded result.
	 
	 @param plainText
	 Plaintext value to be encrypted.
	 
	 @param passPhrase
	 Passphrase from which a pseudo-random password will be derived. The
	 derived password will be used to generate the encryption key.
	 Passphrase can be any string. In this example we assume that this
	 passphrase is an ASCII string.
	 
	 @param saltValue
	 Salt value used along with passphrase to generate password. Salt can
	 be any string. In this example we assume that salt is an ASCII string.
	 
	 @param hashAlgorithm
	 Hash algorithm used to generate password. Allowed values are: "MD5" and
	 "SHA1". SHA1 hashes are a bit slower, but more secure than MD5 hashes.
	 
	 @param passwordIterations
	 Number of iterations used to generate password. One or two iterations
	 should be enough.
	 
	 @param initVector
	 Initialization vector (or IV). This value is required to encrypt the
	 first block of plaintext data. For RijndaelManaged class IV must be 
	 exactly 16 ASCII characters long.
	 
	 @param keySize
	 Size of encryption key in bits. Allowed values are: 128, 192, and 256. 
	 Longer keys are more secure than shorter keys.
	 
	 @return 
	 Encrypted value formatted as a base64-encoded string.
	 
	*/
	public static String Encrypt(String plainText)
	{
		return plainText;
/*		if (StringUtils2.isNullOrEmpty(plainText))
		{
			return plainText;
		}




		// Convert strings into byte arrays.
		// Let us assume that strings only contain ASCII codes.
		// If strings include Unicode characters, use Unicode, UTF7, or UTF8 
		// encoding.
		byte[] initVectorBytes = Encoding.ASCII.GetBytes(_initVector);
		byte[] saltValueBytes = Encoding.ASCII.GetBytes(_saltValue);

		// Convert our plaintext into a byte array.
		// Let us assume that plaintext contains UTF8-encoded characters.
		byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

		// First, we must create a password, from which the key will be derived.
		// This password will be generated from the specified passphrase and 
		// salt value. The password will be created using the specified hash 
		// algorithm. Password creation can be done in several iterations.
		PasswordDeriveBytes password = new PasswordDeriveBytes(_passPhrase, saltValueBytes, _hashAlgorithm, _passwordIterations);

		// Use the password to generate pseudo-random bytes for the encryption
		// key. Specify the size of the key in bytes (instead of bits).
		byte[] keyBytes = password.GetBytes(_keySize / 8);

		// Create uninitialized Rijndael encryption object.
		RijndaelManaged symmetricKey = new RijndaelManaged();

		// It is reasonable to set encryption mode to Cipher Block Chaining
		// (CBC). Use default options for other symmetric key parameters.
		symmetricKey.Mode = CipherMode.CBC;

		// Generate encryptor from the existing key bytes and initialization 
		// vector. Key size will be defined based on the number of the key 
		// bytes.
		ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);

		// Define memory stream which will be used to hold encrypted data.
		MemoryStream memoryStream = new MemoryStream();

		// Define cryptographic stream (always use Write mode for encryption).
		CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
		// Start encrypting.
		cryptoStream.Write(plainTextBytes, 0, plainTextBytes.length);

		// Finish encrypting.
		cryptoStream.FlushFinalBlock();

		// Convert our encrypted data from a memory stream into a byte array.
		byte[] cipherTextBytes = memoryStream.toArray();

		// Close both streams.
		memoryStream.Close();
		cryptoStream.Close();

		// Convert encrypted data into a base64-encoded string.
		String cipherText = Convert.ToBase64String(cipherTextBytes);

		// Return encrypted string.
		return cipherText;*/
	}

	/** 
	 Decrypts specified ciphertext using Rijndael symmetric key algorithm.
	 
	 @param cipherText
	 Base64-formatted ciphertext value.
	 
	 @param passPhrase
	 Passphrase from which a pseudo-random password will be derived. The
	 derived password will be used to generate the encryption key.
	 Passphrase can be any string. In this example we assume that this
	 passphrase is an ASCII string.
	 
	 @param saltValue
	 Salt value used along with passphrase to generate password. Salt can
	 be any string. In this example we assume that salt is an ASCII string.
	 
	 @param hashAlgorithm
	 Hash algorithm used to generate password. Allowed values are: "MD5" and
	 "SHA1". SHA1 hashes are a bit slower, but more secure than MD5 hashes.
	 
	 @param passwordIterations
	 Number of iterations used to generate password. One or two iterations
	 should be enough.
	 
	 @param initVector
	 Initialization vector (or IV). This value is required to encrypt the
	 first block of plaintext data. For RijndaelManaged class IV must be
	 exactly 16 ASCII characters long.
	 
	 @param keySize
	 Size of encryption key in bits. Allowed values are: 128, 192, and 256.
	 Longer keys are more secure than shorter keys.
	 
	 @return 
	 Decrypted string value.
	 
	 
	 Most of the logic in this function is similar to the Encrypt
	 logic. In order for decryption to work, all parameters of this function
	 - except cipherText value - must match the corresponding parameters of
	 the Encrypt function which was called to generate the
	 ciphertext.
	 
	*/
	public static String Decrypt(String cipherText)
	{
		return cipherText;

		/*if (StringUtils2.isNullOrEmpty(cipherText))
		{
			return cipherText;
		}

		// Convert strings defining encryption key characteristics into byte
		// arrays. Let us assume that strings only contain ASCII codes.
		// If strings include Unicode characters, use Unicode, UTF7, or UTF8
		// encoding.
		byte[] initVectorBytes = Encoding.ASCII.GetBytes(_initVector);
		byte[] saltValueBytes = Encoding.ASCII.GetBytes(_saltValue);

		// Convert our ciphertext into a byte array.
		byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

		// First, we must create a password, from which the key will be 
		// derived. This password will be generated from the specified 
		// passphrase and salt value. The password will be created using
		// the specified hash algorithm. Password creation can be done in
		// several iterations.
		PasswordDeriveBytes password = new PasswordDeriveBytes(_passPhrase, saltValueBytes, _hashAlgorithm, _passwordIterations);

		// Use the password to generate pseudo-random bytes for the encryption
		// key. Specify the size of the key in bytes (instead of bits).
		byte[] keyBytes = password.GetBytes(_keySize / 8);

		// Create uninitialized Rijndael encryption object.
		RijndaelManaged symmetricKey = new RijndaelManaged();

		// It is reasonable to set encryption mode to Cipher Block Chaining
		// (CBC). Use default options for other symmetric key parameters.
		symmetricKey.Mode = CipherMode.CBC;

		// Generate decryptor from the existing key bytes and initialization 
		// vector. Key size will be defined based on the number of the key 
		// bytes.
		ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);

		// Define memory stream which will be used to hold encrypted data.
		MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

		// Define cryptographic stream (always use Read mode for encryption).
		CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

		// Since at this point we don't know what the size of decrypted data
		// will be, allocate the buffer long enough to hold ciphertext;
		// plaintext is never longer than ciphertext.
		byte[] plainTextBytes = new byte[cipherTextBytes.length];

		// Start decrypting.
		int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.length);

		// Close both streams.
		memoryStream.Close();
		cryptoStream.Close();

		// Convert decrypted data into a string. 
		// Let us assume that the original plaintext string was UTF8-encoded.
		String plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);

		// Return decrypted string.   
		return plainText;*/
	}

	public static String Decrypt(String cipherText, boolean throwException)
	{
		String rs = null;
		try
		{
			rs = Decrypt(cipherText);
		}
		catch (java.lang.Exception e)
		{
			if (throwException)
			{
				throw e;
			}
		}
		return rs;
	}
}