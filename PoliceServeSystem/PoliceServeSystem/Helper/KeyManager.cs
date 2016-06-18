using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Xml;

namespace PoliceServeSystem.Helper
{
	public class KeyManager : ConfigurationSection {

		public KeyManager() {
		}

		[ConfigurationProperty("", IsDefaultCollection = true)]
		public EncryptionKeyCollection EncryptionKeys {
			get { return (EncryptionKeyCollection)base[""]; }
		}

		public static KeyManager Instance {
			get { return (KeyManager)ConfigurationManager.GetSection("encryptionKeys"); }
		}

	}


	public class EncryptionKey : ConfigurationElement {
		private readonly ConfigurationProperty name = new ConfigurationProperty("name", typeof(string), null);
		private readonly ConfigurationProperty passPhrase = new ConfigurationProperty("passPhrase", typeof(string), null);
		private readonly ConfigurationProperty salt = new ConfigurationProperty("salt", typeof(string), null);
		private readonly ConfigurationProperty hash = new ConfigurationProperty("hash", typeof(string), null);
		private readonly ConfigurationProperty initVector = new ConfigurationProperty("initVector", typeof(string), null);
		private readonly ConfigurationProperty keySize = new ConfigurationProperty("keySize", typeof(int), null);
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		public EncryptionKey() {
			properties.Add(name);
			properties.Add(passPhrase);
			properties.Add(salt);
			properties.Add(hash);
			properties.Add(initVector);
			properties.Add(keySize);
		}

		protected override ConfigurationPropertyCollection Properties {
			get { return properties; }
		}

		[StringValidator(MinLength = 1, MaxLength = 50)]
		[ConfigurationProperty("name", IsKey = true, IsRequired = true)]
		public string Name {
			get { return (string)this[name]; }
			set { this[name] = value; }
		}

		[StringValidator(MinLength = 1, MaxLength = 50)]
		[ConfigurationProperty("passPhrase", IsRequired = true)]
		public string PassPhrase {
			get { return (string)this[passPhrase]; }
			set { this[passPhrase] = value; }
		}

		[StringValidator(MinLength = 1, MaxLength = 50)]
		[ConfigurationProperty("salt", IsRequired = true)]
		public string Salt {
			get { return (string)this[salt]; }
			set { this[salt] = value; }
		}

		[RegexStringValidator("(md5)|(sha1)")]
		[ConfigurationProperty("hash", IsRequired = true)]
		public string Hash {
			get { return (string)this[hash]; }
			set {
				if ((value.ToLower() == "md5") || (value.ToLower() == "sha1"))
					this[hash] = value;
				else
					throw new Exception("EncryptionKey hash value must be either md5 or sha1.");
			}
		}

		[StringValidator(MinLength = 16, MaxLength = 16)]
		[ConfigurationProperty("initVector", IsRequired = true)]
		public string InitVector {
			get { return (string)this[initVector]; }
			set {
				if (value.Length == 16)
					this[initVector] = value;
				else
					throw new Exception("EncryptionKey initVector must be exactly 16 characters.");
			}
		}

		[RegexStringValidator("(128)|(192)|(256)")]
		[ConfigurationProperty("keySize", IsRequired = true, DefaultValue = 256)]
		public int KeySize {
			get { return (int)this[keySize]; }
			set {
				if ((value == 128) || (value == 192) || (value == 256))
					this[keySize] = value.ToString();
				else
					throw new Exception("EncryptionKey keySize must be 128, 192, or 256");
			}
		}


		protected override void DeserializeElement(System.Xml.XmlReader reader, bool serializeCollectionKey) {
			base.DeserializeElement(reader, serializeCollectionKey);
		}

		protected override bool SerializeElement(XmlWriter writer, bool serializeCollectionKey) {
			return base.SerializeElement(writer, serializeCollectionKey);
		}

		protected override bool IsModified() {
			return base.IsModified();
		}
	}

	public class EncryptionKeyCollection : ConfigurationElementCollection {

		public EncryptionKey this[int index] {
			get { return (EncryptionKey)base.BaseGet(index); }
			set {
				if (base.BaseGet(index) != null) base.BaseRemoveAt(index);
				this.BaseAdd(index, value);
			}
		}

		public new EncryptionKey this[string name] {
			get { return (EncryptionKey)base.BaseGet(name); }
		}

		protected override string ElementName {
			get { return "key"; }
		}

		public override ConfigurationElementCollectionType CollectionType {
			get { return ConfigurationElementCollectionType.BasicMap; }
		}

		protected override object GetElementKey(ConfigurationElement element) {
			return ((EncryptionKey)element).Name;
		}

		protected override ConfigurationElement CreateNewElement() {
			return new EncryptionKey();
		}
	}
}