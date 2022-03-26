using NitroLogger.Sulakore.Endianness;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace NitroLogger.Sulakore
{
    public class HMessage
    {
        private readonly List<byte> _body;
        private static readonly Regex _valueGrabber;

        private string _toStringCache;
        private byte[] _toBytesCache, _bodyBuffer;

        private int _position;
        public int Position
        {
            get => _position;
            set => _position = value;
        }

        private short _header;
        public short Header
        {
            get => _header;
            set
            {
                if (!IsCorrupted && _header != value)
                {
                    _header = value;
                    ResetCache();
                }
            }
        }

        public int Readable => (_body.Count - Position);
        public int Length => (_body.Count + (!IsCorrupted ? 2 : 0));

        public bool IsCorrupted { get; }
        public bool IsOutgoing { get; }
        public bool IsBlocked { get; set; }

        private readonly List<object> _read;
        public IReadOnlyList<object> ValuesRead => _read;

        private readonly List<object> _written;
        public IReadOnlyList<object> ValuesWritten => _written;

        static HMessage()
        {
            _valueGrabber = new Regex(@"{(?<type>u|s|i|b):(?<value>[^}]*)\}", RegexOptions.IgnoreCase);
        }
        private HMessage()
        {
            _body = new List<byte>();
            _read = new List<object>();
            _written = new List<object>();
        }

        public HMessage(short header, params object[] values)
            : this(Construct(header, values))
        {
            _written.AddRange(values);
        }

        public HMessage(string data)
            : this(ToBytes(data))
        { }

        public HMessage(byte[] data, bool isOutgoing = false)
            : this()
        {
            IsOutgoing = isOutgoing;
            IsCorrupted = (data.Length < 6 ||
                (BigEndian.ToInt32(data, 0) != data.Length - 4));

            if (!IsCorrupted)
            {
                Header = BigEndian.ToInt16(data, 4);

                _bodyBuffer = new byte[data.Length - 6];
                Buffer.BlockCopy(data, 6, _bodyBuffer, 0, data.Length - 6);
            }
            else
            {
                _bodyBuffer = data;
            }

            _body.AddRange(_bodyBuffer);
        }

        #region Read Methods
        public int ReadInteger()
        {
            return ReadInteger(ref _position);
        }
        public int ReadInteger(int position)
        {
            return ReadInteger(ref position);
        }
        public int ReadInteger(ref int position)
        {
            int value = BigEndian.ToInt32(_bodyBuffer, position);
            position += BigEndian.GetSize(value);

            _read.Add(value);
            return value;
        }

        public short ReadShort()
        {
            return ReadShort(ref _position);
        }
        public short ReadShort(int position)
        {
            return ReadShort(ref position);
        }
        public short ReadShort(ref int position)
        {
            short value = BigEndian.ToInt16(_bodyBuffer, position);
            position += BigEndian.GetSize(value);

            _read.Add(value);
            return value;
        }

        public bool ReadBoolean()
        {
            return ReadBoolean(ref _position);
        }
        public bool ReadBoolean(int position)
        {
            return ReadBoolean(ref position);
        }
        public bool ReadBoolean(ref int position)
        {
            bool value = BigEndian.ToBoolean(_bodyBuffer, position);
            position += BigEndian.GetSize(value);

            _read.Add(value);
            return value;
        }

        public string ReadString()
        {
            return ReadString(ref _position);
        }
        public string ReadString(int position)
        {
            return ReadString(ref position);
        }
        public string ReadString(ref int position)
        {
            string value = BigEndian.ToString(_bodyBuffer, position);
            position += BigEndian.GetSize(value);

            _read.Add(value);
            return value;
        }

        public byte[] ReadBytes(int length)
        {
            return ReadBytes(length, ref _position);
        }
        public byte[] ReadBytes(int length, int position)
        {
            return ReadBytes(length, ref position);
        }
        public byte[] ReadBytes(int length, ref int position)
        {
            byte[] value = new byte[length];
            Buffer.BlockCopy(_bodyBuffer, position, value, 0, length);
            position += length;

            _read.Add(value);
            return value;
        }
        #endregion
        #region Write Methods
        public void WriteInteger(int value)
        {
            WriteInteger(value, _body.Count);
        }
        public void WriteInteger(int value, int position)
        {
            byte[] encoded = BigEndian.GetBytes(value);
            WriteObject(encoded, value, position);
        }

        public void WriteShort(short value)
        {
            WriteShort(value, _body.Count);
        }
        public void WriteShort(short value, int position)
        {
            byte[] encoded = BigEndian.GetBytes(value);
            WriteObject(encoded, value, position);
        }

        public void WriteBoolean(bool value)
        {
            WriteBoolean(value, _body.Count);
        }
        public void WriteBoolean(bool value, int position)
        {
            byte[] encoded = BigEndian.GetBytes(value);
            WriteObject(encoded, value, position);
        }

        public void WriteString(string value)
        {
            WriteString(value, _body.Count);
        }
        public void WriteString(string value, int position)
        {
            if (value == null)
            {
                value = string.Empty;
            }

            byte[] encoded = BigEndian.GetBytes(value);
            WriteObject(encoded, value, position);
        }

        public void WriteBytes(byte[] value)
        {
            WriteBytes(value, _body.Count);
        }
        public void WriteBytes(byte[] value, int position)
        {
            WriteObject(value, value, position);
        }

        private void WriteObjects(params object[] values)
        {
            _written.AddRange(values);
            _body.AddRange(GetBytes(values));

            Refresh();
        }
        private void WriteObject(byte[] encoded, object value, int position)
        {
            _written.Add(value);
            _body.InsertRange(position, encoded);

            Refresh();
        }
        #endregion
        #region Remove Methods
        public void RemoveInteger()
        {
            RemoveInteger(_position);
        }
        public void RemoveInteger(int position)
        {
            RemoveBytes(4, position);
        }

        public void RemoveShort()
        {
            RemoveShort(_position);
        }
        public void RemoveShort(int position)
        {
            RemoveBytes(2, position);
        }

        public void RemoveBoolean()
        {
            RemoveBoolean(_position);
        }
        public void RemoveBoolean(int position)
        {
            RemoveBytes(1, position);
        }

        public void RemoveString()
        {
            RemoveString(_position);
        }
        public void RemoveString(int position)
        {
            int readable = (_body.Count - position);
            if (readable < 2)
            {
                return;
            }

            short stringLength =
                BigEndian.ToInt16(_bodyBuffer, position);

            if (readable >= (stringLength + 2))
            {
                RemoveBytes(stringLength + 2, position);
            }
        }

        public void RemoveBytes(int length)
        {
            RemoveBytes(length, _position);
        }
        public void RemoveBytes(int length, int position)
        {
            _body.RemoveRange(position, length);
            Refresh();
        }
        #endregion
        #region Replace Methods
        public void ReplaceInteger(int value)
        {
            ReplaceInteger(value, _position);
        }
        public void ReplaceInteger(int value, int position)
        {
            RemoveInteger(position);
            WriteInteger(value, position);
        }

        public void ReplaceShort(short value)
        {
            ReplaceShort(value, _position);
        }
        public void ReplaceShort(short value, int position)
        {
            RemoveShort(position);
            WriteShort(value, position);
        }

        public void ReplaceBoolean(bool value)
        {
            ReplaceBoolean(value, _position);
        }
        public void ReplaceBoolean(bool value, int position)
        {
            RemoveBoolean(position);
            WriteBoolean(value, position);
        }

        public void ReplaceString(string value)
        {
            ReplaceString(value, _position);
        }
        public void ReplaceString(string value, int position)
        {
            int oldLength = Length;

            RemoveString(position);
            WriteString(value, position);

            if (position < _position)
            {
                _position +=
                    ((oldLength - Length) * -1);
            }
        }
        #endregion

        public int ReadableAt(int position)
        {
            return (_body.Count - position);
        }

        public bool CanReadString()
        {
            return CanReadString(_position);
        }
        public bool CanReadString(int position)
        {
            int readable = (_body.Count - position);
            if (readable < 2)
            {
                return false;
            }

            short stringLength =
                BigEndian.ToInt16(_bodyBuffer, position);

            return (readable >= (stringLength + 2));
        }

        private void Refresh()
        {
            ResetCache();
            _bodyBuffer = _body.ToArray();
        }
        private void ResetCache()
        {
            _toBytesCache = null;
            _toStringCache = null;
        }

        public override string ToString()
        {
            return _toStringCache ??
                (_toStringCache = ToString(ToBytes()));
        }
        public static string ToString(byte[] data)
        {
            string result = Encoding.Default.GetString(data);
            for (int i = 0; i <= 13; i++)
            {
                result = result.Replace(
                    ((char)i).ToString(), "[" + i + "]");
            }
            return result;
        }

        public byte[] ToBytes()
        {
            if (IsCorrupted)
            {
                _toBytesCache = _bodyBuffer;
            }

            return _toBytesCache ??
                (_toBytesCache = Construct(Header, _bodyBuffer));
        }
        public static byte[] ToBytes(string packet)
        {
            for (int i = 0; i <= 13; i++)
            {
                packet = packet.Replace(
                    "[" + i + "]", ((char)i).ToString());
            }

            MatchCollection matches = _valueGrabber.Matches(packet);
            foreach (Match match in matches)
            {
                string type = match.Groups["type"].Value;
                string value = match.Groups["value"].Value;

                byte[] data = null;
                #region Switch: type
                switch (type)
                {
                    case "s":
                        {
                            data = BigEndian.GetBytes(value);
                            break;
                        }
                    case "u":
                        {
                            short.TryParse(value, out short uValue);
                            data = BigEndian.GetBytes(uValue);
                            break;
                        }
                    case "i":
                        {
                            int.TryParse(value, out int iValue);
                            data = BigEndian.GetBytes(iValue);
                            break;
                        }
                    case "b":
                        {
                            if (!byte.TryParse(value, out byte bValue))
                            {
                                data = BigEndian.GetBytes(
                                    (value.ToLower() == "true"));
                            }
                            else
                            {
                                data = new byte[] { bValue };
                            }

                            break;
                        }
                }
                #endregion

                packet = packet.Replace(match.Value,
                    Encoding.Default.GetString(data));
            }
            if (packet.StartsWith("{l}") && packet.Length >= 5)
            {
                byte[] lengthData = BigEndian.GetBytes(packet.Length - 3);
                packet = Encoding.Default.GetString(lengthData) + packet.Substring(3);
            }
            return Encoding.Default.GetBytes(packet);
        }

        public static byte[] GetBytes(params object[] values)
        {
            List<byte> buffer = new List<byte>();
            foreach (object value in values)
            {
                switch (Type.GetTypeCode(value.GetType()))
                {
                    case TypeCode.Byte: buffer.Add((byte)value); break;
                    case TypeCode.Boolean: buffer.Add(Convert.ToByte((bool)value)); break;
                    case TypeCode.Int32: buffer.AddRange(BigEndian.GetBytes((int)value)); break;
                    case TypeCode.Int16: buffer.AddRange(BigEndian.GetBytes((short)value)); break;

                    default:
                    case TypeCode.String:
                        {
                            if (value is byte[] data)
                            {
                                buffer.AddRange(data);
                            }
                            else
                            {
                                buffer.AddRange(BigEndian.GetBytes(value.ToString()));
                            }

                            break;
                        }
                }
            }
            return buffer.ToArray();
        }
        public static byte[] Construct(short header, params object[] values)
        {
            byte[] body = GetBytes(values);
            byte[] buffer = new byte[6 + body.Length];

            byte[] headerData = BigEndian.GetBytes(header);
            byte[] lengthData = BigEndian.GetBytes(2 + body.Length);

            Buffer.BlockCopy(lengthData, 0, buffer, 0, 4);
            Buffer.BlockCopy(headerData, 0, buffer, 4, 2);
            Buffer.BlockCopy(body, 0, buffer, 6, body.Length);
            return buffer;
        }
    }
}