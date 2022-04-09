using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace Hashtool
{
    public enum HashAlgType
    {
        MD5, SHA1, SHA256, SHA512, SHA3, SM3, CRC32
    }

    public class HashAlgHandler
    {
        /// <summary>
        /// 获取哈希算法类型
        /// </summary>
        public HashAlgType AlgType { get; }

        /// <summary>
        /// 获得哈希算法字符串名称
        /// </summary>
        public string Name
        {
            get
            {
                switch (AlgType)
                {
                    case HashAlgType.MD5:
                        return "MD5";
                    case HashAlgType.SHA1:
                        return "SHA1";
                    case HashAlgType.SHA256:
                        return "SHA256";
                    case HashAlgType.SHA512:
                        return "SHA512";
                    case HashAlgType.SHA3:
                        return "SHA3";
                    case HashAlgType.SM3:
                        return "SM3";
                    case HashAlgType.CRC32:
                        return "CRC32";
                    default:
                        throw new ArgumentException("Unknown HashAlgType.");
                }
            }
        }

        /// <summary>
        /// 获得哈希算法计算对象
        /// </summary>
        public HashAlgorithm HashObj
        {
            get
            {
                switch (AlgType)
                {
                    case HashAlgType.MD5:
                        return MD5.Create();
                    case HashAlgType.SHA1:
                        return SHA1.Create();
                    case HashAlgType.SHA256:
                        return SHA256.Create();
                    case HashAlgType.SHA512:
                        return SHA512.Create();
                    case HashAlgType.SHA3:
                        return new SHA3();
                    case HashAlgType.SM3:
                        return new SM3();
                    case HashAlgType.CRC32:
                        return new CRC32();
                    default:
                        throw new ArgumentException("Unknown HashAlgType.");
                }
            }
        }

        /// <summary>
        /// 创建指定类型哈希算法
        /// </summary>
        /// <param name="algType"></param>
        public HashAlgHandler(HashAlgType algType) => this.AlgType = algType;

    }


    public class SHA3 : HashAlgorithm
    {
        public override void Initialize()
        {
            throw new NotImplementedException();
        }

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            throw new NotImplementedException();
        }

        protected override byte[] HashFinal()
        {
            throw new NotImplementedException();
        }
    }

    public class SM3 : HashAlgorithm
    {
        private uint[] sm3HashValue = new uint[8];
        private ulong msgLength = 0; // 数据最大长度为 2 的 64 次方 bit

        private byte[] dataBuffer = new byte[64];
        private int dataBufferLen = 0;

        private uint[] wordsBuffer1 = new uint[68];
        private uint[] wordsBuffer2 = new uint[64];

        private uint T(int j)
        {
            return j <= 15 ? 0x79cc4519u : 0x7a879d8au;
        }

        private uint FF(int j, uint X, uint Y, uint Z)
        {
            return j <= 15 ? (X ^ Y ^ Z) : ((X & Y) | (X & Z) | (Y & Z));
        }

        private uint GG(int j, uint X, uint Y, uint Z)
        {
            return j <= 15 ? (X ^ Y ^ Z) : ((X & Y) | (~X & Z));
        }

        private uint ROL(uint X, int count)
        {
            // C# 里移位运算符自动模数值长度, 所以不需要对 count 进行处理
            // 当 count 为 0 或 32 时, 左右两半都没移位, 因此只能用或运算符进行连接
            return (X << count) | (X >> (-count));
        }

        private uint P0(uint X)
        {
            return X ^ ROL(X, 9) ^ ROL(X, 17);
        }

        private uint P1(uint X)
        {
            return X ^ ROL(X, 15) ^ ROL(X, 23);
        }

        /// <summary>
        /// 扩展函数, 把 512 bit 的 16 个字扩展成 132 个字
        /// </summary>
        /// <param name="data">16 个 32 bit 字</param>
        private void Expand(uint[] data)
        {
            Array.Copy(data, 0, wordsBuffer1, 0, 16);
            for (int j = 16; j < 68; j++)
            {
                wordsBuffer1[j] = P1(wordsBuffer1[j - 16] ^ wordsBuffer1[j - 9] ^ ROL(wordsBuffer1[j - 3], 15))
                                ^ ROL(wordsBuffer1[j - 13], 7)
                                ^ wordsBuffer1[j - 6];
            }
            for (int j = 0; j < 64; j++)
            {
                wordsBuffer2[j] = wordsBuffer1[j] ^ wordsBuffer1[j + 4];
            }
        }

        /// <summary>
        /// 压缩函数, 每次接收 16 个 32 bit 字的数据进行压缩, 会更新 sm3HashValue 的值
        /// </summary>
        /// <param name="data"></param>
        private void CF(uint[] data)
        {
            Expand(data);

            uint[] ABCDEFGH = new uint[8];
            uint SS1, SS2, TT1, TT2;
            sm3HashValue.CopyTo(ABCDEFGH, 0);

            for (int j = 0; j < 64; j++)
            {
                SS1 = ROL(ROL(ABCDEFGH[0], 12) + ABCDEFGH[4] + ROL(T(j), j), 7);
                SS2 = SS1 ^ ROL(ABCDEFGH[0], 12);
                TT1 = FF(j, ABCDEFGH[0], ABCDEFGH[1], ABCDEFGH[2]) + ABCDEFGH[3] + SS2 + wordsBuffer2[j];
                TT2 = GG(j, ABCDEFGH[4], ABCDEFGH[5], ABCDEFGH[6]) + ABCDEFGH[7] + SS1 + wordsBuffer1[j];
                ABCDEFGH[3] = ABCDEFGH[2];
                ABCDEFGH[2] = ROL(ABCDEFGH[1], 9);
                ABCDEFGH[1] = ABCDEFGH[0];
                ABCDEFGH[0] = TT1;
                ABCDEFGH[7] = ABCDEFGH[6];
                ABCDEFGH[6] = ROL(ABCDEFGH[5], 19);
                ABCDEFGH[5] = ABCDEFGH[4];
                ABCDEFGH[4] = P0(TT2);
            }

            for (int i = 0; i < 8; i++)
            {
                sm3HashValue[i] = ABCDEFGH[i] ^ sm3HashValue[i];
            }
        }

        /// <summary>
        /// 把 64 字节转换成 16 个 32 bit 字
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dataStart"></param>
        /// <returns></returns>
        private uint[] Bytes2Words(byte[] data, int dataStart = 0)
        {
            uint[] words = new uint[16];
            for (int i = 0; i < 16; i++)
            {
                words[i] = ((uint)data[dataStart + i * 4    ] << 24)
                         | ((uint)data[dataStart + i * 4 + 1] << 16)
                         | ((uint)data[dataStart + i * 4 + 2] <<  8)
                         | ((uint)data[dataStart + i * 4 + 3]      );
            }
            return words;
        }

        public override void Initialize()
        {
            // 初始向量
            sm3HashValue[0] = 0x7380166fu;
            sm3HashValue[1] = 0x4914b2b9u;
            sm3HashValue[2] = 0x172442d7u;
            sm3HashValue[3] = 0xda8a0600u;
            sm3HashValue[4] = 0xa96f30bcu;
            sm3HashValue[5] = 0x163138aau;
            sm3HashValue[6] = 0xe38dee4du;
            sm3HashValue[7] = 0xb0fb0e4eu;

            msgLength = 0;

            dataBufferLen = 0;
        }

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            if (cbSize + dataBufferLen >= 64)
            {
                int readPos = ibStart;

                // 处理上一次缓冲区剩余的数据
                Array.Copy(array, 0, dataBuffer, dataBufferLen, 64 - dataBufferLen);
                readPos += 64 - dataBufferLen;
                CF(Bytes2Words(dataBuffer));

                // 按每 64 个字节来读取数据
                for (; readPos + 64 < ibStart + cbSize; readPos += 64)
                {
                    CF(Bytes2Words(array, ibStart + readPos));
                }

                // 保留本次剩余数据
                Array.Copy(array, ibStart + readPos, dataBuffer, 0, cbSize - (readPos - ibStart));
                dataBufferLen = cbSize - (readPos - ibStart);
            }
            else
            {
                // 向缓冲区增加保留数据
                Array.Copy(array, ibStart, dataBuffer, dataBufferLen, cbSize);
                dataBufferLen += cbSize;
            }

            if (msgLength > (msgLength + (ulong)cbSize))
            {
                throw new OverflowException("Data too long.");
            }
            msgLength += (ulong)cbSize;
        }

        protected override byte[] HashFinal()
        {
            byte[] hashValue = new byte[32];

            // 尾部填充
            dataBuffer[dataBufferLen] = 0x80;
            for (int i = dataBufferLen + 1; i < 64; i++)
            {
                dataBuffer[i] = 0x00;
            }

            ulong msgBitLen = msgLength * 8;
            dataBuffer[56] = (byte)(msgBitLen >> 56);
            dataBuffer[57] = (byte)(msgBitLen >> 48);
            dataBuffer[58] = (byte)(msgBitLen >> 40);
            dataBuffer[59] = (byte)(msgBitLen >> 32);
            dataBuffer[60] = (byte)(msgBitLen >> 24);
            dataBuffer[61] = (byte)(msgBitLen >> 16);
            dataBuffer[62] = (byte)(msgBitLen >>  8);
            dataBuffer[63] = (byte)(msgBitLen      );
            CF(Bytes2Words(dataBuffer));

            for (int i = 0; i < 8; i++)
            {
                hashValue[i * 4    ] = (byte)(sm3HashValue[i] >> 24);
                hashValue[i * 4 + 1] = (byte)(sm3HashValue[i] >> 16);
                hashValue[i * 4 + 2] = (byte)(sm3HashValue[i] >>  8);
                hashValue[i * 4 + 3] = (byte)(sm3HashValue[i]      );
            }
            return hashValue;
        }
    }

    public class CRC32 : HashAlgorithm
    {
        private uint[] modTable = new uint[256] {
            0x00000000, 0x77073096, 0xee0e612c, 0x990951ba, 0x076dc419, 0x706af48f, 0xe963a535, 0x9e6495a3, 0x0edb8832, 0x79dcb8a4, 0xe0d5e91e, 0x97d2d988, 0x09b64c2b, 0x7eb17cbd, 0xe7b82d07, 0x90bf1d91,
            0x1db71064, 0x6ab020f2, 0xf3b97148, 0x84be41de, 0x1adad47d, 0x6ddde4eb, 0xf4d4b551, 0x83d385c7, 0x136c9856, 0x646ba8c0, 0xfd62f97a, 0x8a65c9ec, 0x14015c4f, 0x63066cd9, 0xfa0f3d63, 0x8d080df5,
            0x3b6e20c8, 0x4c69105e, 0xd56041e4, 0xa2677172, 0x3c03e4d1, 0x4b04d447, 0xd20d85fd, 0xa50ab56b, 0x35b5a8fa, 0x42b2986c, 0xdbbbc9d6, 0xacbcf940, 0x32d86ce3, 0x45df5c75, 0xdcd60dcf, 0xabd13d59,
            0x26d930ac, 0x51de003a, 0xc8d75180, 0xbfd06116, 0x21b4f4b5, 0x56b3c423, 0xcfba9599, 0xb8bda50f, 0x2802b89e, 0x5f058808, 0xc60cd9b2, 0xb10be924, 0x2f6f7c87, 0x58684c11, 0xc1611dab, 0xb6662d3d,
            0x76dc4190, 0x01db7106, 0x98d220bc, 0xefd5102a, 0x71b18589, 0x06b6b51f, 0x9fbfe4a5, 0xe8b8d433, 0x7807c9a2, 0x0f00f934, 0x9609a88e, 0xe10e9818, 0x7f6a0dbb, 0x086d3d2d, 0x91646c97, 0xe6635c01,
            0x6b6b51f4, 0x1c6c6162, 0x856530d8, 0xf262004e, 0x6c0695ed, 0x1b01a57b, 0x8208f4c1, 0xf50fc457, 0x65b0d9c6, 0x12b7e950, 0x8bbeb8ea, 0xfcb9887c, 0x62dd1ddf, 0x15da2d49, 0x8cd37cf3, 0xfbd44c65,
            0x4db26158, 0x3ab551ce, 0xa3bc0074, 0xd4bb30e2, 0x4adfa541, 0x3dd895d7, 0xa4d1c46d, 0xd3d6f4fb, 0x4369e96a, 0x346ed9fc, 0xad678846, 0xda60b8d0, 0x44042d73, 0x33031de5, 0xaa0a4c5f, 0xdd0d7cc9,
            0x5005713c, 0x270241aa, 0xbe0b1010, 0xc90c2086, 0x5768b525, 0x206f85b3, 0xb966d409, 0xce61e49f, 0x5edef90e, 0x29d9c998, 0xb0d09822, 0xc7d7a8b4, 0x59b33d17, 0x2eb40d81, 0xb7bd5c3b, 0xc0ba6cad,
            0xedb88320, 0x9abfb3b6, 0x03b6e20c, 0x74b1d29a, 0xead54739, 0x9dd277af, 0x04db2615, 0x73dc1683, 0xe3630b12, 0x94643b84, 0x0d6d6a3e, 0x7a6a5aa8, 0xe40ecf0b, 0x9309ff9d, 0x0a00ae27, 0x7d079eb1,
            0xf00f9344, 0x8708a3d2, 0x1e01f268, 0x6906c2fe, 0xf762575d, 0x806567cb, 0x196c3671, 0x6e6b06e7, 0xfed41b76, 0x89d32be0, 0x10da7a5a, 0x67dd4acc, 0xf9b9df6f, 0x8ebeeff9, 0x17b7be43, 0x60b08ed5,
            0xd6d6a3e8, 0xa1d1937e, 0x38d8c2c4, 0x4fdff252, 0xd1bb67f1, 0xa6bc5767, 0x3fb506dd, 0x48b2364b, 0xd80d2bda, 0xaf0a1b4c, 0x36034af6, 0x41047a60, 0xdf60efc3, 0xa867df55, 0x316e8eef, 0x4669be79,
            0xcb61b38c, 0xbc66831a, 0x256fd2a0, 0x5268e236, 0xcc0c7795, 0xbb0b4703, 0x220216b9, 0x5505262f, 0xc5ba3bbe, 0xb2bd0b28, 0x2bb45a92, 0x5cb36a04, 0xc2d7ffa7, 0xb5d0cf31, 0x2cd99e8b, 0x5bdeae1d,
            0x9b64c2b0, 0xec63f226, 0x756aa39c, 0x026d930a, 0x9c0906a9, 0xeb0e363f, 0x72076785, 0x05005713, 0x95bf4a82, 0xe2b87a14, 0x7bb12bae, 0x0cb61b38, 0x92d28e9b, 0xe5d5be0d, 0x7cdcefb7, 0x0bdbdf21,
            0x86d3d2d4, 0xf1d4e242, 0x68ddb3f8, 0x1fda836e, 0x81be16cd, 0xf6b9265b, 0x6fb077e1, 0x18b74777, 0x88085ae6, 0xff0f6a70, 0x66063bca, 0x11010b5c, 0x8f659eff, 0xf862ae69, 0x616bffd3, 0x166ccf45,
            0xa00ae278, 0xd70dd2ee, 0x4e048354, 0x3903b3c2, 0xa7672661, 0xd06016f7, 0x4969474d, 0x3e6e77db, 0xaed16a4a, 0xd9d65adc, 0x40df0b66, 0x37d83bf0, 0xa9bcae53, 0xdebb9ec5, 0x47b2cf7f, 0x30b5ffe9,
            0xbdbdf21c, 0xcabac28a, 0x53b39330, 0x24b4a3a6, 0xbad03605, 0xcdd70693, 0x54de5729, 0x23d967bf, 0xb3667a2e, 0xc4614ab8, 0x5d681b02, 0x2a6f2b94, 0xb40bbe37, 0xc30c8ea1, 0x5a05df1b, 0x2d02ef8d
        };
        private uint crc32Code = 0;

        private void CalcModTable()
        {
            modTable = new uint[256];
            for (int i = 0; i < 256; i++)
            {
                uint tmp = (uint)i;
                for (int j = 0; j < 8; j++)
                {
                    if ((tmp & 1) == 1)
                    {
                        tmp = (tmp >> 1) ^ 0xEDB88320; // 减法
                    }
                    else
                    {
                        tmp >>= 1;
                    }
                }
                modTable[i] = tmp;
            }
        }

        public override void Initialize()
        {
            crc32Code = 0xffffffffu;
        }

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            for (int i = ibStart; i < cbSize; i++)
            {
                crc32Code = (crc32Code >> 8) ^ modTable[(crc32Code & 0xffu) ^ array[i]];
            }
        }

        protected override byte[] HashFinal()
        {
            byte[] hashValue = new byte[4];
            crc32Code ^= 0xffffffffu;
            hashValue[0] = (byte)((crc32Code >> 24) & 0xff);
            hashValue[1] = (byte)((crc32Code >> 16) & 0xff);
            hashValue[2] = (byte)((crc32Code >> 8) & 0xff);
            hashValue[3] = (byte)(crc32Code & 0xff);
            return hashValue;
        }
    }
}
