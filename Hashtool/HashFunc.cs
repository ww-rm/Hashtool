using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace Hashtool
{
    public enum HashAlgType
    {
        MD5, SHA1, SHA2_256, SHA2_512, SHA3_256, SHA3_512, SM3, CRC32
    }

    public static class HashAlgHandler
    {
        /// <summary>
        /// 获得哈希算法字符串名称
        /// </summary>
        public static string GetName(HashAlgType algType)
        {
            switch (algType)
            {
                case HashAlgType.MD5:
                    return "MD5";
                case HashAlgType.SHA1:
                    return "SHA1";
                case HashAlgType.SHA2_256:
                    return "SHA2-256";
                case HashAlgType.SHA2_512:
                    return "SHA2-512";
                case HashAlgType.SHA3_256:
                    return "SHA3-256";
                case HashAlgType.SHA3_512:
                    return "SHA3-512";
                case HashAlgType.SM3:
                    return "SM3";
                case HashAlgType.CRC32:
                    return "CRC32";
                default:
                    throw new ArgumentException("Unknown HashAlgType.");
            }
        }

        /// <summary>
        /// 获得哈希算法计算对象
        /// </summary>
        public static HashAlgorithm GetHashObj(HashAlgType algType)
        {
            switch (algType)
            {
                case HashAlgType.MD5:
                    return MD5.Create();
                case HashAlgType.SHA1:
                    return SHA1.Create();
                case HashAlgType.SHA2_256:
                    return SHA256.Create();
                case HashAlgType.SHA2_512:
                    return SHA512.Create();
                case HashAlgType.SHA3_256:
                    return new SHA3_256();
                case HashAlgType.SHA3_512:
                    return new SHA3_512();
                case HashAlgType.SM3:
                    return new SM3();
                case HashAlgType.CRC32:
                    return new CRC32();
                default:
                    throw new ArgumentException("Unknown HashAlgType.");
            }
        }
    }

    public abstract partial class SHA3Base
    {
        // 常量
        //protected const int b = 1600;
        //protected const int w = 64;
        //protected const int l = 6;

        private static int[] laneOffset = new int[25] {
               0,   1, 190,  28,  91,
              36, 300,   6,  55, 276,
               3,  10, 171, 153, 231,
             105,  45,  15,  21, 136,
             210,  66, 253, 120,  78,
        };

        private static ulong[] RC_table = new ulong[24]
        {
            0x0000000000000001,
            0x0000000000008082,
            0x800000000000808a,
            0x8000000080008000,
            0x000000000000808b,
            0x0000000080000001,
            0x8000000080008081,
            0x8000000000008009,
            0x000000000000008a,
            0x0000000000000088,
            0x0000000080008009,
            0x000000008000000a,
            0x000000008000808b,
            0x800000000000008b,
            0x8000000000008089,
            0x8000000000008003,
            0x8000000000008002,
            0x8000000000000080,
            0x000000000000800a,
            0x800000008000000a,
            0x8000000080008081,
            0x8000000000008080,
            0x0000000080000001,
            0x8000000080008008
        };

        // 放置顺序是先 x 后 y, 64 bit 小端存储
        // (0, 0) -> (0, 1) -> ... (y, x) -> (y, x + 1) -> (4, 4)
        private ulong[] state = new ulong[25];

        private static void PreCompRCtable()
        {
            ulong rc(int t)
            {
                ulong R = 0x01;
                if (t % 255 != 0)
                {
                    for (int i = 0; i < t % 255; i++)
                    {
                        R <<= 1;
                        R ^= ((R >> 8) & 0x01u) | ((R >> 4) & 0x10u) | ((R >> 3) & 0x20u) | ((R >> 2) & 0x40u);
                        R &= 0xffu;
                    }
                }
                return R & 0x01u;
            }
            for (int i = 0; i < 24; i++)
            {
                ulong RC = 0x00;
                for (int j = 0; j <= 6; j++)
                {
                    RC |= rc(j + (7 * i)) << ((1 << j) - 1);
                }
                RC_table[i] = RC;
            }
        }

        /// <summary>
        /// 循环左移
        /// </summary>
        /// <param name="X"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static ulong ROL(ulong X, int count)
        {
            // C# 里移位运算符自动模数值长度, 所以不需要对 count 进行处理
            // 当 count 为 0 或 64 时, 左右两半都没移位, 因此只能用或运算符进行连接
            return (X << count) | (X >> (-count));
        }

        #region Keccak 的 5 个 Step Mappings 函数

        private ulong[] C = new ulong[5];
        private ulong[] D = new ulong[5];
        private void Theta()
        {
            // 把 5 个 Plane 压缩成 C
            //for (int x = 0; x < 5; x++)
            //{
            //    C[x] = state[0 * 5 + x] ^ state[1 * 5 + x] ^ state[2 * 5 + x] ^ state[3 * 5 + x] ^ state[4 * 5 + x];
            //}
            C[0] = state[0] ^ state[5] ^ state[10] ^ state[15] ^ state[20];
            C[1] = state[1] ^ state[6] ^ state[11] ^ state[16] ^ state[21];
            C[2] = state[2] ^ state[7] ^ state[12] ^ state[17] ^ state[22];
            C[3] = state[3] ^ state[8] ^ state[13] ^ state[18] ^ state[23];
            C[4] = state[4] ^ state[9] ^ state[14] ^ state[19] ^ state[24];

            // 对 C 混合产生 D
            D[0] = C[4] ^ ROL(C[1], 1);
            D[1] = C[0] ^ ROL(C[2], 1);
            D[2] = C[1] ^ ROL(C[3], 1);
            D[3] = C[2] ^ ROL(C[4], 1);
            D[4] = C[3] ^ ROL(C[0], 1);

            // 对每一个 Plane[i] 用 D 异或一次
            //for (int y = 0; y < 5; y++)
            //{
            //    for (int x = 0; x < 5; x++)
            //    {
            //        state[y * 5 + x] ^= D[x];
            //    }
            //}
            state[0] ^= D[0];
            state[1] ^= D[1];
            state[2] ^= D[2];
            state[3] ^= D[3];
            state[4] ^= D[4];

            state[5] ^= D[0];
            state[6] ^= D[1];
            state[7] ^= D[2];
            state[8] ^= D[3];
            state[9] ^= D[4];

            state[10] ^= D[0];
            state[11] ^= D[1];
            state[12] ^= D[2];
            state[13] ^= D[3];
            state[14] ^= D[4];

            state[15] ^= D[0];
            state[16] ^= D[1];
            state[17] ^= D[2];
            state[18] ^= D[3];
            state[19] ^= D[4];

            state[20] ^= D[0];
            state[21] ^= D[1];
            state[22] ^= D[2];
            state[23] ^= D[3];
            state[24] ^= D[4];
        }

        private void Rho()
        {
            //for (int i = 0; i < 25; i++)
            //{
            //    state[i] = ROL(state[i], laneOffset[i]);
            //}
            state[0] = ROL(state[0], 0);
            state[1] = ROL(state[1], 1);
            state[2] = ROL(state[2], 190);
            state[3] = ROL(state[3], 28);
            state[4] = ROL(state[4], 91);
            state[5] = ROL(state[5], 36);
            state[6] = ROL(state[6], 300);
            state[7] = ROL(state[7], 6);
            state[8] = ROL(state[8], 55);
            state[9] = ROL(state[9], 276);
            state[10] = ROL(state[10], 3);
            state[11] = ROL(state[11], 10);
            state[12] = ROL(state[12], 171);
            state[13] = ROL(state[13], 153);
            state[14] = ROL(state[14], 231);
            state[15] = ROL(state[15], 105);
            state[16] = ROL(state[16], 45);
            state[17] = ROL(state[17], 15);
            state[18] = ROL(state[18], 21);
            state[19] = ROL(state[19], 136);
            state[20] = ROL(state[20], 210);
            state[21] = ROL(state[21], 66);
            state[22] = ROL(state[22], 253);
            state[23] = ROL(state[23], 120);
            state[24] = ROL(state[24], 78);
        }

        private void Pi()
        {
            ulong tmp = state[18];
            state[18] = state[17];
            state[17] = state[11];
            state[11] = state[7];
            state[7] = state[10];
            state[10] = state[1];
            state[1] = state[6];
            state[6] = state[9];
            state[9] = state[22];
            state[22] = state[14];
            state[14] = state[20];
            state[20] = state[2];
            state[2] = state[12];
            state[12] = state[13];
            state[13] = state[19];
            state[19] = state[23];
            state[23] = state[15];
            state[15] = state[4];
            state[4] = state[24];
            state[24] = state[21];
            state[21] = state[8];
            state[8] = state[16];
            state[16] = state[5];
            state[5] = state[3];
            state[3] = tmp;
        }

        private void Chi()
        {
            ulong tmp1, tmp2;
            //for (int y = 0; y < 5; y++)
            //{
            //    tmp1 = state[y * 5 + 0];
            //    tmp2 = state[y * 5 + 1];
            //    state[y * 5 + 0] ^= ~state[y * 5 + 1] & state[y * 5 + 2];
            //    state[y * 5 + 1] ^= ~state[y * 5 + 2] & state[y * 5 + 3];
            //    state[y * 5 + 2] ^= ~state[y * 5 + 3] & state[y * 5 + 4];
            //    state[y * 5 + 3] ^= ~state[y * 5 + 4] & tmp1;
            //    state[y * 5 + 4] ^= ~tmp1 & tmp2;
            //}
            tmp1 = state[0];
            tmp2 = state[1];
            state[0] ^= ~state[1] & state[2];
            state[1] ^= ~state[2] & state[3];
            state[2] ^= ~state[3] & state[4];
            state[3] ^= ~state[4] & tmp1;
            state[4] ^= ~tmp1 & tmp2;

            tmp1 = state[5];
            tmp2 = state[6];
            state[5] ^= ~state[6] & state[7];
            state[6] ^= ~state[7] & state[8];
            state[7] ^= ~state[8] & state[9];
            state[8] ^= ~state[9] & tmp1;
            state[9] ^= ~tmp1 & tmp2;

            tmp1 = state[10];
            tmp2 = state[11];
            state[10] ^= ~state[11] & state[12];
            state[11] ^= ~state[12] & state[13];
            state[12] ^= ~state[13] & state[14];
            state[13] ^= ~state[14] & tmp1;
            state[14] ^= ~tmp1 & tmp2;

            tmp1 = state[15];
            tmp2 = state[16];
            state[15] ^= ~state[16] & state[17];
            state[16] ^= ~state[17] & state[18];
            state[17] ^= ~state[18] & state[19];
            state[18] ^= ~state[19] & tmp1;
            state[19] ^= ~tmp1 & tmp2;

            tmp1 = state[20];
            tmp2 = state[21];
            state[20] ^= ~state[21] & state[22];
            state[21] ^= ~state[22] & state[23];
            state[22] ^= ~state[23] & state[24];
            state[23] ^= ~state[24] & tmp1;
            state[24] ^= ~tmp1 & tmp2;
        }

        private void Iota(int roundIndex)
        {
            state[0] ^= RC_table[roundIndex];
        }

        #endregion

        // Keccak-p[1600, 24]
        private void Keccak_p_1600_24()
        {
            for (int i = 0; i < 24; i++)
            {
                Theta();
                Rho();
                Pi();
                Chi();
                Iota(i);
            }
        }
    }

    public abstract partial class SHA3Base : HashAlgorithm
    {
        private byte[] dataBuffer;
        private int dataBufferLen = 0; // 最大长度为 rSize

        private int dSize;
        private int cSize { get { return 2 * dSize; } }
        private int rSize { get { return 200 - cSize; } }
        
        public SHA3Base(int d)
        {
            dSize = d / 8;
            dataBuffer = new byte[rSize];
        }

        /// <summary>
        /// 填充 blockBuffer
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dataStart"></param>
        private void ReadBlock(byte[] data, int dataStart = 0)
        {
            // 按小端序读取数据
            for (int i = 0; i < rSize / 8; i++)
            {
                state[i] ^= ((ulong)data[dataStart + i * 8 + 7] << 56)
                          | ((ulong)data[dataStart + i * 8 + 6] << 48)
                          | ((ulong)data[dataStart + i * 8 + 5] << 40)
                          | ((ulong)data[dataStart + i * 8 + 4] << 32)
                          | ((ulong)data[dataStart + i * 8 + 3] << 24)
                          | ((ulong)data[dataStart + i * 8 + 2] << 16)
                          | ((ulong)data[dataStart + i * 8 + 1] <<  8)
                          | ((ulong)data[dataStart + i * 8    ]      );
            }

            // 剩余的 cSize 部分全部是 0x00 填充, 所以 state 异或之后不变
            //for (int i = rSize / 8; i < 25; i++)
            //{
            //    state[i] ^= 0x00u;
            //}
        }

        public override void Initialize()
        {
            for(int i = 0; i < 25; i++)
            {
                state[i] = 0;
            }

            dataBufferLen = 0;
        }

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            if (cbSize + dataBufferLen >= rSize)
            {
                int readPos = ibStart;

                // 处理上一次缓冲区剩余的数据
                Array.Copy(array, 0, dataBuffer, dataBufferLen, rSize - dataBufferLen);
                readPos += rSize - dataBufferLen;
                ReadBlock(dataBuffer);
                Keccak_p_1600_24();

                // 按每 rSize 个字节来读取数据
                while (readPos + rSize < ibStart + cbSize)
                {
                    ReadBlock(array, ibStart + readPos);
                    readPos += rSize;
                    Keccak_p_1600_24();
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
        }

        protected override byte[] HashFinal()
        {
            byte[] hashValue = new byte[dSize];

            // 尾块填充
            if (dataBufferLen == rSize - 1)
            {
                dataBuffer[rSize - 1] = 0x86;
            }
            else
            {
                dataBuffer[dataBufferLen++] = 0x06;
                for(int i = dataBufferLen; i < rSize - 1; i++)
                {
                    dataBuffer[i] = 0x00;
                }
                dataBuffer[rSize - 1] = 0x80;
            }

            ReadBlock(dataBuffer);
            Keccak_p_1600_24();

            // SHA3 算法的 r 一定大于 d, 所以没有实际挤压过程
            for (int i = 0; i < dSize / 8; i++)
            {
                hashValue[i * 8    ] = (byte)(state[i]      );
                hashValue[i * 8 + 1] = (byte)(state[i] >>  8);
                hashValue[i * 8 + 2] = (byte)(state[i] >> 16);
                hashValue[i * 8 + 3] = (byte)(state[i] >> 24);
                hashValue[i * 8 + 4] = (byte)(state[i] >> 32);
                hashValue[i * 8 + 5] = (byte)(state[i] >> 40);
                hashValue[i * 8 + 6] = (byte)(state[i] >> 48);
                hashValue[i * 8 + 7] = (byte)(state[i] >> 56);
            }
            return hashValue;
        }
    }

    public class SHA3_256 : SHA3Base
    {
        public SHA3_256() : base(256) { }
    }

    public class SHA3_512 : SHA3Base
    {
        public SHA3_512() : base(512) { }
    }

    public class SM3 : HashAlgorithm
    {
        private static uint[] ROL_T_table = new uint[64] {
            0x79cc4519, 0xf3988a32, 0xe7311465, 0xce6228cb, 0x9cc45197, 0x3988a32f, 0x7311465e, 0xe6228cbc,
            0xcc451979, 0x988a32f3, 0x311465e7, 0x6228cbce, 0xc451979c, 0x88a32f39, 0x11465e73, 0x228cbce6,
            0x9d8a7a87, 0x3b14f50f, 0x7629ea1e, 0xec53d43c, 0xd8a7a879, 0xb14f50f3, 0x629ea1e7, 0xc53d43ce,
            0x8a7a879d, 0x14f50f3b, 0x29ea1e76, 0x53d43cec, 0xa7a879d8, 0x4f50f3b1, 0x9ea1e762, 0x3d43cec5,
            0x7a879d8a, 0xf50f3b14, 0xea1e7629, 0xd43cec53, 0xa879d8a7, 0x50f3b14f, 0xa1e7629e, 0x43cec53d,
            0x879d8a7a, 0x0f3b14f5, 0x1e7629ea, 0x3cec53d4, 0x79d8a7a8, 0xf3b14f50, 0xe7629ea1, 0xcec53d43,
            0x9d8a7a87, 0x3b14f50f, 0x7629ea1e, 0xec53d43c, 0xd8a7a879, 0xb14f50f3, 0x629ea1e7, 0xc53d43ce,
            0x8a7a879d, 0x14f50f3b, 0x29ea1e76, 0x53d43cec, 0xa7a879d8, 0x4f50f3b1, 0x9ea1e762, 0x3d43cec5,
        };

        /// <summary>
        /// 预计算 ROL(T(j), j)
        /// </summary>
        private static void PreCompROLTtable()
        {
            for (int j = 0; j < 64; j++)
            {
                ROL_T_table[j] = ROL(T(j), j);
            }
        }

        private uint[] sm3HashValue = new uint[8];
        private ulong msgLength = 0; // 数据最大长度为 2 的 64 次方 bit

        private byte[] dataBuffer = new byte[64];
        private int dataBufferLen = 0;

        private uint[] wordsBuffer1 = new uint[68];
        private uint[] wordsBuffer2 = new uint[64];
        
        #region 静态辅助方法

        private static uint T(int j)
        {
            return j <= 15 ? 0x79cc4519u : 0x7a879d8au;
        }

        private static uint FF(int j, uint X, uint Y, uint Z)
        {
            return j <= 15 ? (X ^ Y ^ Z) : ((X & Y) | (X & Z) | (Y & Z));
        }

        private static uint GG(int j, uint X, uint Y, uint Z)
        {
            return j <= 15 ? (X ^ Y ^ Z) : ((X & Y) | (~X & Z));
        }

        private static uint ROL(uint X, int count)
        {
            // C# 里移位运算符自动模数值长度, 所以不需要对 count 进行处理
            // 当 count 为 0 或 32 时, 左右两半都没移位, 因此只能用或运算符进行连接
            return (X << count) | (X >> (-count));
        }

        private static uint P0(uint X)
        {
            return X ^ ROL(X, 9) ^ ROL(X, 17);
        }

        private static uint P1(uint X)
        {
            return X ^ ROL(X, 15) ^ ROL(X, 23);
        }

        #endregion

        /// <summary>
        /// 扩展函数, 把 512 bit 的 16 个字扩展成 132 个字
        /// </summary>
        private void Expand()
        {
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
        private void CF()
        {
            Expand();

            uint A = sm3HashValue[0];
            uint B = sm3HashValue[1];
            uint C = sm3HashValue[2];
            uint D = sm3HashValue[3];
            uint E = sm3HashValue[4];
            uint F = sm3HashValue[5];
            uint G = sm3HashValue[6];
            uint H = sm3HashValue[7];
            uint SS1, SS2, TT1, TT2;

            for (int j = 0; j < 64; j++)
            {
                //SS1 = ROL(ROL(A, 12) + E + ROL(T(j), j), 7);
                SS1 = ROL(ROL(A, 12) + E + ROL_T_table[j], 7);
                SS2 = SS1 ^ ROL(A, 12);
                TT1 = FF(j, A, B, C) + D + SS2 + wordsBuffer2[j];
                TT2 = GG(j, E, F, G) + H + SS1 + wordsBuffer1[j];
                D = C;
                C = ROL(B, 9);
                B = A;
                A = TT1;
                H = G;
                G = ROL(F, 19);
                F = E;
                E = P0(TT2);
            }

            sm3HashValue[0] = A ^ sm3HashValue[0];
            sm3HashValue[1] = B ^ sm3HashValue[1];
            sm3HashValue[2] = C ^ sm3HashValue[2];
            sm3HashValue[3] = D ^ sm3HashValue[3];
            sm3HashValue[4] = E ^ sm3HashValue[4];
            sm3HashValue[5] = F ^ sm3HashValue[5];
            sm3HashValue[6] = G ^ sm3HashValue[6];
            sm3HashValue[7] = H ^ sm3HashValue[7];
        }

        /// <summary>
        /// 读取 64 字节数据进入待计算缓冲区
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dataStart"></param>
        /// <returns></returns>
        private void ReadBlock(byte[] data, int dataStart = 0)
        {
            for (int i = 0; i < 16; i++)
            {
                wordsBuffer1[i] = ((uint)data[dataStart + i * 4    ] << 24)
                                | ((uint)data[dataStart + i * 4 + 1] << 16)
                                | ((uint)data[dataStart + i * 4 + 2] <<  8)
                                | ((uint)data[dataStart + i * 4 + 3]      );
            }
        }

        public override void Initialize()
        {
            //PreCompROLTtable();
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
                ReadBlock(dataBuffer);
                CF();

                // 按每 64 个字节来读取数据
                while (readPos + 64 < ibStart + cbSize)
                {
                    ReadBlock(array, ibStart + readPos);
                    readPos += 64;
                    CF();
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

            if (msgLength * 8 > (msgLength + (ulong)cbSize) * 8)
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
            if (dataBufferLen < 56)
            {
                for (int i = dataBufferLen + 1; i < 56; i++)
                {
                    dataBuffer[i] = 0x00;
                }
            }
            else
            {
                for (int i = dataBufferLen + 1; i < 64; i++)
                {
                    dataBuffer[i] = 0x00;
                }
                ReadBlock(dataBuffer);
                CF();
                for (int i = 0; i < 56; i++)
                {
                    dataBuffer[i] = 0x00;
                }
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

            ReadBlock(dataBuffer);
            CF();

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
        private static uint[] modTable = new uint[256] {
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

        /// <summary>
        /// 预计算每个字节模多项式余数
        /// </summary>
        private static void PreCompModTable()
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

        private uint crc32Code = 0;

        public override void Initialize()
        {
            //PreCompModTable();
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
