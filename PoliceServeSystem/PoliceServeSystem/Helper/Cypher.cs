using System;
//using Credex.ViewModel;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace PoliceServeSystem.Helper
{
	/// <summary>
	/// Cypher class provides strong encryption routines for securing sensitive data across the application
	/// using a common encryption algorithm.
	/// </summary>
	public static class Cypher {

		/// <summary>
		///		Secures a plain text string by encrypting and encoding the string.
		/// </summary>
		/// <param name="S">Plain text string to be secured.</param>
		/// <returns>
		///		Encrypted and base64 encoded representation of the plain text string. Base64 encoding ensures
		///		against data loss that might occur with an encrypted only version on the string that may contain
		///		non-printable binary data, in particular null characters.
		/// </returns>
		/// <remarks>
		///		Use the DecryptString to reverse the results of this method and thus obtain the original
		///		unsecured plain text string.
		///		
		///		Database Storage - use the following guidline for declaring database fields with enough
		///     space to contain the encrypted version of a plain text strength
		///		(i.e. For plain text strings <= # in lenght you should delcare a database field of size #)
		///		<= 15 : 24			   <= 511 : 684
		///		<= 31 : 44			   <= 527 : 704
		///		<= 47 : 64			   <= 543 : 728
		///		<= 63 : 88			   <= 559 : 748
		///		<= 79 : 108			   <= 575 : 768
		///		<= 95 : 128			   <= 591 : 792
		///		<= 111 : 152		   <= 607 : 812
		///		<= 127 : 172		   <= 623 : 832
		///		<= 143 : 192		   <= 639 : 856
		///		<= 159 : 216		   <= 655 : 876
		///		<= 175 : 236		   <= 671 : 896
		///		<= 191 : 256		   <= 687 : 920
		///		<= 207 : 280		   <= 703 : 940
		///		<= 223 : 300		   <= 719 : 960
		///		<= 239 : 320		   <= 735 : 984
		///		<= 255 : 344		   <= 751 : 1004
		///		<= 271 : 364		   <= 767 : 1024
		///		<= 287 : 384		   <= 783 : 1048
		///		<= 303 : 408		   <= 799 : 1068
		///		<= 319 : 428		   <= 815 : 1088
		///		<= 335 : 448		   <= 831 : 1112
		///		<= 351 : 472		   <= 847 : 1132
		///		<= 367 : 492		   <= 863 : 1152
		///		<= 383 : 512		   <= 879 : 1176
		///		<= 399 : 536		   <= 895 : 1196
		///		<= 415 : 556		   <= 911 : 1216
		///		<= 431 : 576		   <= 927 : 1240
		///		<= 447 : 600		   <= 943 : 1260
		///		<= 463 : 620		   <= 959 : 1280
		///		<= 479 : 640		   <= 975 : 1304
		///		<= 495 : 664		   <= 991 : 1324	
		/// </remarks>
		/// <seealso cref="Cypher.DescryptString"/>
		public static string EncryptString(string S) {
			if (S == "") return S;

			EncryptionKey oKey = KeyManager.Instance.EncryptionKeys["PCI"];

			// Convert strings into byte arrays.
			// Let us assume that strings only contain ASCII codes.
			// If strings include Unicode characters, use Unicode, UTF7, or UTF8 
			// encoding.
			byte[] initVectorBytes = Encoding.ASCII.GetBytes(oKey.InitVector);
			byte[] saltValueBytes = Encoding.ASCII.GetBytes(oKey.Salt);

			// Convert our plaintext into a byte array.
			byte[] plainTextBytes = Encoding.ASCII.GetBytes(S);

			// First, we must create a password, from which the key will be derived.
			// This password will be generated from the specified passphrase and 
			// salt value. The password will be created using the specified hash 
			// algorithm. Password creation can be done in several iterations.
			PasswordDeriveBytes password = new PasswordDeriveBytes(
														oKey.PassPhrase,
														saltValueBytes,
														oKey.Hash,
														2);

			// Use the password to generate pseudo-random bytes for the encryption
			// key. Specify the size of the key in bytes (instead of bits).
			byte[] keyBytes = password.GetBytes(oKey.KeySize / 8);

			// Create uninitialized Rijndael encryption object.
			RijndaelManaged symmetricKey = new RijndaelManaged();

			// It is reasonable to set encryption mode to Cipher Block Chaining
			// (CBC). Use default options for other symmetric key parameters.
			symmetricKey.Mode = CipherMode.CBC;

			// Generate encryptor from the existing key bytes and initialization 
			// vector. Key size will be defined based on the number of the key 
			// bytes.
			ICryptoTransform encryptor = symmetricKey.CreateEncryptor(
															 keyBytes,
															 initVectorBytes);

			// Define memory stream which will be used to hold encrypted data.
			MemoryStream memoryStream = new MemoryStream();

			// Define cryptographic stream (always use Write mode for encryption).
			CryptoStream cryptoStream = new CryptoStream(
													memoryStream,
													encryptor,
													CryptoStreamMode.Write);
			// Start encrypting.
			cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

			// Finish encrypting.
			cryptoStream.FlushFinalBlock();

			// Convert our encrypted data from a memory stream into a byte array.
			byte[] cipherTextBytes = memoryStream.ToArray();

			// Close both streams.
			memoryStream.Close();
			cryptoStream.Close();

			// Convert encrypted data into a base64-encoded string.
			string cipherText = Convert.ToBase64String(cipherTextBytes);
            string encodedText = cipherText.Replace("/", "[msl]");
            encodedText = cipherText.Replace(" ", "+");

			// Return encrypted string.
			return encodedText;
		}

		/// <summary>
		///		Reverses the security applied to a plain text string by the EncryptString method.
		/// </summary>
		/// <param name="S">
		///		Base64 encoded, encrypted string representation of a plain text string.
		/// </param>
		/// <returns>
		///		Unsecured, plain text value.
		/// </returns>
		/// <remarks>
		///		This method will fail if the S parameter was either not produced by the EncryptString
		///		method, or was produced by a different initialization key.
		/// </remarks>
		/// <seealso cref="Cypher.EncryptString"/>
		public static string DecryptString(string S) {
			if (S == "") return S;

			EncryptionKey oKey = KeyManager.Instance.EncryptionKeys["PCI"];

			// Convert strings defining encryption key characteristics into byte
			// arrays. Let us assume that strings only contain ASCII codes.
			// If strings include Unicode characters, use Unicode, UTF7, or UTF8
			// encoding.
			byte[] initVectorBytes = Encoding.ASCII.GetBytes(oKey.InitVector);
			byte[] saltValueBytes = Encoding.ASCII.GetBytes(oKey.Salt);

            // Convert our ciphertext into a byte array.
		    S = S.Replace('-', '+');
            byte[] cipherTextBytes = Convert.FromBase64String(S);

			// First, we must create a password, from which the key will be 
			// derived. This password will be generated from the specified 
			// passphrase and salt value. The password will be created using
			// the specified hash algorithm. Password creation can be done in
			// several iterations.
			PasswordDeriveBytes password = new PasswordDeriveBytes(
															oKey.PassPhrase,
															saltValueBytes,
															oKey.Hash,
															2);

			// Use the password to generate pseudo-random bytes for the encryption
			// key. Specify the size of the key in bytes (instead of bits).
			byte[] keyBytes = password.GetBytes(oKey.KeySize / 8);

			// Create uninitialized Rijndael encryption object.
			RijndaelManaged symmetricKey = new RijndaelManaged();

			// It is reasonable to set encryption mode to Cipher Block Chaining
			// (CBC). Use default options for other symmetric key parameters.
			symmetricKey.Mode = CipherMode.CBC;

			// Generate decryptor from the existing key bytes and initialization 
			// vector. Key size will be defined based on the number of the key 
			// bytes.
			ICryptoTransform decryptor = symmetricKey.CreateDecryptor(
															 keyBytes,
															 initVectorBytes);

			// Define memory stream which will be used to hold encrypted data.
			MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

			// Define cryptographic stream (always use Read mode for encryption).
			CryptoStream cryptoStream = new CryptoStream(memoryStream,
														  decryptor,
														  CryptoStreamMode.Read);

			// Since at this point we don't know what the size of decrypted data
			// will be, allocate the buffer long enough to hold ciphertext;
			// plaintext is never longer than ciphertext.
			byte[] plainTextBytes = new byte[cipherTextBytes.Length];

			// Start decrypting.
			int decryptedByteCount = cryptoStream.Read(plainTextBytes,
													   0,
													   plainTextBytes.Length);

			// Close both streams.
			memoryStream.Close();
			cryptoStream.Close();

			// Convert decrypted data into a string. 
			// Let us assume that the original plaintext string was UTF8-encoded.
			string plainText = Encoding.UTF8.GetString(plainTextBytes,
													   0,
													   decryptedByteCount);

			// Return decrypted string.   
			return plainText;

		}
        public static string ReplaceCharcters(string S)
        {

            S = S.Replace("[msl]", "/");

            S = S.Replace(" ", "+");
            
            return S;
        }
	}
}
