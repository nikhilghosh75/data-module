using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ZipCreatorOS
{
    MSDO2 = 0, Amiga = 1,
    OpenVMS = 2,Unix = 3,
    VMCMS = 4, AtariST = 5,
    OS2 = 6, Macintosh = 7,
    ZSystem = 8, CPM = 9,
    Windows = 10, MVS = 11,
    VSE = 12, AcornRisc = 13,
    VFAT = 14, MVSAlternate = 15,
    BeOS = 16, Tandem = 17,
    OS400 = 18, OSX = 19
}

public struct ZipCreator
{
    public ZipCreatorOS OS;
    public int majorVersion;
    public int minorVersion;

    public ZipCreator(byte high, byte low)
    {
        OS = (ZipCreatorOS)high;
        majorVersion = low / 10;
        minorVersion = low % 10;
    }
}

public struct ZipVersion
{
    public int majorVersion;
    public int minorVersion;
    
    public ZipVersion(byte high, byte low)
    {
        majorVersion = high;
        minorVersion = low;
    }
}

public class ZipData
{
    public ZipCreator fileCreator; // Version Made By
    public ZipVersion versionNeededToExtract;
    public bool isEncrypted;
}
