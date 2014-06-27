<Query Kind="Statements">
  <Namespace>System.Security.Cryptography.X509Certificates</Namespace>
</Query>

var stsGuid = Guid.Parse("a83a168d-d1ad-4bde-9b0a-ed570162e695");

var metadataUrl = string.Format("https://login.windows.net/{0}/federationmetadata/2007-06/federationmetadata.xml", stsGuid);
var metadataXml = XDocument.Load(metadataUrl);

var roleDescriptor = metadataXml.Descendants("{urn:oasis:names:tc:SAML:2.0:metadata}RoleDescriptor")
	.Single(rd => rd.Attributes()
		.Any(a => a.Name == "{http://www.w3.org/2001/XMLSchema-instance}type" && a.Value == "fed:SecurityTokenServiceType")
	);
var keyDescriptor = roleDescriptor.Descendants("{urn:oasis:names:tc:SAML:2.0:metadata}KeyDescriptor")
	.Single(rd => rd.Attributes()
		.Any(a => a.Name == "use" && a.Value == "signing")
	);
var certAsBase64 = keyDescriptor.Descendants("{http://www.w3.org/2000/09/xmldsig#}X509Certificate").Single().Value;

var certAsByteArray = Convert.FromBase64String(certAsBase64);
var x509 = new X509Certificate2(certAsByteArray);

x509.Thumbprint.Dump();