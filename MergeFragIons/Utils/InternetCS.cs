/**
 * Program:     ProteoCombiner - Integrating bottom-up & top-down proteomics data
 * Author:      Diogo Borges Lima
 * Update:      1/23/2015
 * Update by:   Diogo Borges Lima
 * Description: Internet Connection Service
 */
using System;
using System.Runtime;
using System.Runtime.InteropServices;

public class InternetCS
{
    [DllImport("wininet.dll", SetLastError = true)]
    static extern bool InternetCheckConnection(string lpszUrl, int dwFlags, int dwReserved);
    public static bool CanConnectToURL(string url)
    {
        return InternetCheckConnection(url, 1, 0);
    }
}