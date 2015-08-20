using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WMSURFIDSYS.Client
{
    class USBApi
    {
        [DllImport("USB_UHFReader.dll")]
        public static extern byte API_OpenUsb();

        [DllImport("USB_UHFReader.dll")]
        public static extern byte API_CloseUsb();

        [DllImport("USB_UHFReader.dll")]
        public static extern byte API_ReadTid(byte[] stbuf, byte Sendlen, byte[] trbuf);

        [DllImport("USB_UHFReader.dll")]
        public static extern byte API_ReadTidFast(byte[] tid, byte Sendlen);

        [DllImport("USB_Encode.dll")]
        public static extern byte API_Encode(byte[] stbuf, byte[] tid, byte[] trbuf);

        [DllImport("USB_UHFReader.dll")]
        public static extern byte API_GetDevieId(byte[] stbuf);

        [DllImport("USB_UHFReader.dll")]
        public static extern byte API_SetDevieId(byte[] stbuf);

        [DllImport("USB_UHFReader.dll")]
        public static extern byte API_SetPower(byte stbuf);

        [DllImport("USB_UHFReader.dll")]
        public static extern byte API_GetRegion(byte[] stbuf);

        [DllImport("USB_UHFReader.dll")]
        public static extern byte API_SetRegion(byte stbuf);

        [DllImport("USB_UHFReader.dll")]
        public static extern byte API_GetRFChanel(byte[] stbuf);

        [DllImport("USB_UHFReader.dll")]
        public static extern byte API_SetRFChanel(byte stbuf);

        [DllImport("USB_UHFReader.dll")]
        public static extern byte API_InventoryOnce(byte[] uReadData, byte[] uLen);

        [DllImport("USB_UHFReader.dll")]
        public static extern byte API_ReadData(byte[] ap, byte ul, byte[] epc, byte mb,
                            byte sa, byte dl, byte[] uReadData);

        [DllImport("USB_UHFReader.dll")]
        public static extern byte API_WriteData(byte[] ap, byte ul, byte[] epc, byte mb,
                             byte sa, byte dl, byte[] uWriteData);

        [DllImport("USB_UHFReader.dll")]
        public static extern byte API_SaveConfig();

        [DllImport("USB_UHFReader.dll")]
        public static extern int API_Lock(byte[] ap, byte epcLen, byte[] epc, int flag);

        [DllImport("USB_UHFReader.dll")]
        public static extern int API_Kill(byte[] ap, byte epcLen, byte[] epc);
    }
}
