namespace Common.System
{
    public static class DriveInfoService
    {
        public static DriveInfo[] GetCDRomDrives()
        {
            return DriveInfo.GetDrives().Where(drive => drive.DriveType == DriveType.CDRom).ToArray();
        }

        public static DriveInfo[] GetRamDrives()
        {
            return DriveInfo.GetDrives().Where(drive => drive.DriveType == DriveType.Ram).ToArray();
        }

        public static DriveInfo[] GetRemovableDrives()
        {
            return DriveInfo.GetDrives().Where(drive => drive.DriveType == DriveType.Removable).ToArray();
        }

        public static DriveInfo[] GetFixedDrives()
        {
            return DriveInfo.GetDrives().Where(drive => drive.DriveType == DriveType.Fixed).ToArray();
        }

        public static DriveInfo[] GetNetworkDrives()
        {
            return DriveInfo.GetDrives().Where(drive => drive.DriveType == DriveType.Network).ToArray();
        }
    }
}
