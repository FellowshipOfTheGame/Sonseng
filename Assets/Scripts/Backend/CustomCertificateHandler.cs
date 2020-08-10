using System;
using UnityEngine.Networking;
using System.Security.Cryptography.X509Certificates;

public class CustomCertificateHandler : CertificateHandler{
    private static readonly string PUB_KEY="";

    protected override bool ValidateCertificate(byte[] certificateData){
        return true;
    }

}