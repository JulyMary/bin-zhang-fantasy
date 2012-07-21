package fantasy.jobs;

import fantasy.xserialization.*;


@XSerializable(name = "jobservice", namespaceUri= Consts.LicenceNamespaceURI)
class Licence
{
	@XAttribute(name ="expire")
	public java.util.Date ExpireTime = new java.util.Date(Long.MIN_VALUE);


	@XAttribute(name = "machine")
	public String MachineCode = "";


	@XAttribute(name = "dev")
	public boolean IsDevelopVersion = false;


	@XAttribute(name = "slave")
	public int SlaveCount = 0;

	@XAttribute(name = "key")
	public String key = "";

}