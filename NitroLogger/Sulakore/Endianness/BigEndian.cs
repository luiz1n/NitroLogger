using System;
using System.Text;

namespace NitroLogger.Sulakore.Endianness
{
    public static class BigEndian
    {
        public static int GetSize(int value) => 4;
        public static int GetSize(bool value) => 1;
        public static int GetSize(short value) => 2;
        public static int GetSize(string value) => Encoding.UTF8.GetByteCount(value) + 2;

        public static byte[] GetBytes(int value)
        {
            var buffer = new byte[4];
            buffer[0] = (byte)(value >> 24);
            buffer[1] = (byte)(value >> 16);
            buffer[2] = (byte)(value >> 8);
            buffer[3] = (byte)value;

            return buffer;
        }

        public static byte[] GetBytes(bool value)
        {
            var buffer = new byte[1] { 0 };
            buffer[0] = (byte)(value ? 1 : 0);

            return buffer;
        }

        public static byte[] GetBytes(short value)
        {
            var buffer = new byte[2];
            buffer[0] = (byte)(value >> 8);
            buffer[1] = (byte)value;

            return buffer;
        }

        public static byte[] GetBytes(string value)
        {
            byte[] stringData = Encoding.UTF8.GetBytes(value);
            byte[] lengthData = GetBytes((short)stringData.Length);

            var buffer = new byte[lengthData.Length + stringData.Length];
            Buffer.BlockCopy(lengthData, 0, buffer, 0, lengthData.Length);
            Buffer.BlockCopy(stringData, 0, buffer, lengthData.Length, stringData.Length);

            return buffer;
        }

        public static int ToInt32(byte[] value, int startIndex)
        {
            int result = (value[startIndex++] << 24);
            result += (value[startIndex++] << 16);
            result += (value[startIndex++] << 8);
            result += (value[startIndex]);

            return result;
        }

        public static short ToInt16(byte[] value, int startIndex)
        {
            int result = (value[startIndex++] << 8);
            result += (value[startIndex]);
            return (short)result;
        }

        public static string ToString(byte[] value, int startIndex)
        {
            short stringLength =
                ToInt16(value, startIndex);

            string result = Encoding.UTF8
                .GetString(value, startIndex + 2, stringLength);

            return result;
        }

        public static bool ToBoolean(byte[] value, int startIndex) => value[startIndex] == 1;
    }
}