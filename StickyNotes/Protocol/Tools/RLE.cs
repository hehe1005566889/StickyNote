using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ProjectFChat.Network.Utils
{
    class RLE
    {
		#region "成员"
		private const Int32 BLOCKMAX = 32767;       //块的最大长度
		#endregion

		#region "压缩"
		/// <summary>
		/// 压缩
		/// 特殊情况下，压缩后的数据有可能比压缩前大
		/// </summary>
		/// <param name="p_Data">被压缩的数据</param>
		/// <returns>压缩后的数据</returns>
		public static Byte[] Compress(Byte[] p_Data)
		{
			Byte[] btRet = null;                       //压缩后的数据
			Byte[] btTmp = null;                       //用来保存压缩数据的
			Int32 iCmpressedLength = 0;                //压缩后的数据长度
			Int32 i = 0;                               //循环用
			Int32 j = 0;                               //循环用
			Int16 iContinue = 0;                       //连续相同的字节长度
			Int16 iNContinue = 0;                      //不连续相同的字节长度
			Boolean bContinueOld = false;              //前一个字节是否是连续相同的字节

			Byte[] btBlockHead = new Byte[2];           //块头,2字节
			Byte btChkOld;                              //前一个字节
			Byte btChk;                                 //当前字节

			//去处第一个数据
			btChkOld = p_Data[0];
			iContinue = 1;
			iNContinue = 1;

			//从第二个数据开始循环
			for (i = 1; i < p_Data.Length; i++)
			{
				btChk = p_Data[i];        //去处当前数据

				if (btChk == btChkOld)
				{
					//和前一个数据相同
					if (!bContinueOld && (iNContinue > 1))
					{
						//前面一个数据是属于不连续相同的数据串并且不连续的长度大于一

						//因为和前一个数据相同,所以和前一个数据将构成连续的相同的字节块,
						//而从上次压缩位置到前前一个数据为止的数据将要写入不连续的字节块。

						btTmp = null;
						btTmp = new Byte[iCmpressedLength + 2 + iNContinue];

						//保存原来数据
						if (iCmpressedLength != 0)
						{
							for (j = 0; j < iCmpressedLength; j++)
							{
								btTmp[j] = btRet[j];
							}
						}

						//设定块头
						iNContinue -= 1;          //这个地方要减去一，因为最后一个数据会划分到下一个连续的块中
						btBlockHead = null;
						btBlockHead = System.BitConverter.GetBytes(iNContinue);
						BitArray ba = new BitArray(btBlockHead);
						ba.Set(15, false);                          //最高位设定为0
						ba.CopyTo(btBlockHead, 0);

						btTmp[iCmpressedLength] = btBlockHead[0];
						btTmp[iCmpressedLength + 1] = btBlockHead[1];

						//写数据
						for (j = 0; j < iNContinue; j++)
						{
							btTmp[iCmpressedLength + 2 + j] = p_Data[i - 1 - iNContinue + j];
						}

						iCmpressedLength += 2 + iNContinue;    //压缩长度

						iContinue = 2;                         //前一个数据和当前数据将成为连续的字节块
						iNContinue = 1;
						bContinueOld = true;

						btRet = btTmp;
					}
					else if (bContinueOld && (iContinue == BLOCKMAX))
					{
						//前一个数据是连续的字节块，并且长度达到了最大的字节块长度
						//写入连续字节块数据

						btTmp = null;
						btTmp = new Byte[iCmpressedLength + 3];
						//保存原来数据
						if (iCmpressedLength != 0)
						{
							for (j = 0; j < iCmpressedLength; j++)
							{
								btTmp[j] = btRet[j];
							}
						}

						//设定头
						btBlockHead = null;
						btBlockHead = System.BitConverter.GetBytes(iContinue);
						BitArray ba = new BitArray(btBlockHead);
						ba.Set(15, true);                          //最高位设定为1
						ba.CopyTo(btBlockHead, 0);

						btTmp[iCmpressedLength] = btBlockHead[0];
						btTmp[iCmpressedLength + 1] = btBlockHead[1];

						//写数据
						btTmp[iCmpressedLength + 2] = p_Data[i - 1];

						iCmpressedLength += 3;
						btChkOld = p_Data[i];
						iContinue = 1;
						iNContinue = 1;

						btRet = btTmp;
					}
					else
					{
						bContinueOld = true;
						iContinue++;               //连续子节数自加
					}
				}
				else
				{
					//当前字节和前一个字节不同
					btChkOld = btChk;

					if (!bContinueOld)
					{
						//前一个字节是不连续的字节块
						if (iNContinue == BLOCKMAX)
						{
							//不连续字节长度超过最大长度
							//写入不连续字节块
							btTmp = null;
							btTmp = new Byte[iCmpressedLength + 2 + iNContinue];
							//保存原来数据
							if (iCmpressedLength != 0)
							{
								for (j = 0; j < iCmpressedLength; j++)
								{
									btTmp[j] = btRet[j];
								}
							}

							//设定块头
							btBlockHead = null;
							btBlockHead = System.BitConverter.GetBytes(iNContinue);
							BitArray ba = new BitArray(btBlockHead);
							ba.Set(15, false);                          //最高位设定为0
							ba.CopyTo(btBlockHead, 0);

							btTmp[iCmpressedLength] = btBlockHead[0];
							btTmp[iCmpressedLength + 1] = btBlockHead[1];

							//写数据
							for (j = 0; j < iNContinue; j++)
							{
								btTmp[iCmpressedLength + 2 + j] = p_Data[i - 1 - iNContinue + j];
							}

							iCmpressedLength += 2 + iNContinue;
							iContinue = 1;
							iNContinue = 1;

							btRet = btTmp;
						}
						else
						{
							bContinueOld = false;
							iNContinue++;
						}
					}
					else if (bContinueOld)
					{
						//前一个字节是连续的字节块
						bContinueOld = false;

						//写入连续的字节块

						btTmp = null;
						btTmp = new Byte[iCmpressedLength + 3];
						//保存原来数据
						if (iCmpressedLength != 0)
						{
							for (j = 0; j < iCmpressedLength; j++)
							{
								btTmp[j] = btRet[j];
							}
						}

						//设定头
						btBlockHead = null;
						btBlockHead = System.BitConverter.GetBytes(iContinue);
						BitArray ba = new BitArray(btBlockHead);
						ba.Set(15, true);                          //最高位设定为1
						ba.CopyTo(btBlockHead, 0);

						btTmp[iCmpressedLength] = btBlockHead[0];
						btTmp[iCmpressedLength + 1] = btBlockHead[1];

						//写数据
						btTmp[iCmpressedLength + 2] = p_Data[i - 1];

						iCmpressedLength += 3;
						btChkOld = p_Data[i];
						iContinue = 1;
						iNContinue = 1;

						btRet = btTmp;
					}
				}
			}

			//结束部分 
			if (iContinue > 1)
			{
				//最后为连续的字节块部分
				btTmp = null;
				btTmp = new Byte[iCmpressedLength + 3];
				//保存原来数据
				if (iCmpressedLength != 0)
				{
					for (j = 0; j < iCmpressedLength; j++)
					{
						btTmp[j] = btRet[j];
					}
				}

				//设定头
				btBlockHead = null;
				btBlockHead = System.BitConverter.GetBytes(iContinue);
				BitArray ba = new BitArray(btBlockHead);
				ba.Set(15, true);                          //最高位设定为1
				ba.CopyTo(btBlockHead, 0);

				btTmp[iCmpressedLength] = btBlockHead[0];
				btTmp[iCmpressedLength + 1] = btBlockHead[1];

				//写数据
				btTmp[iCmpressedLength + 2] = p_Data[i - 1];

				iCmpressedLength += 3;
				btRet = btTmp;

			}
			else if (iNContinue >= 1)
			{
				//最后为不连续的字节块部分
				//写入不连续字节块
				btTmp = null;
				btTmp = new Byte[iCmpressedLength + 2 + iNContinue];
				//保存原来数据
				if (iCmpressedLength != 0)
				{
					for (j = 0; j < iCmpressedLength; j++)
					{
						btTmp[j] = btRet[j];
					}
				}

				//设定块头
				btBlockHead = null;
				btBlockHead = System.BitConverter.GetBytes(iNContinue);
				BitArray ba = new BitArray(btBlockHead);
				ba.Set(15, false);                          //最高位设定为0
				ba.CopyTo(btBlockHead, 0);

				btTmp[iCmpressedLength] = btBlockHead[0];
				btTmp[iCmpressedLength + 1] = btBlockHead[1];

				//写数据
				for (j = 0; j < iNContinue; j++)
				{
					btTmp[iCmpressedLength + 2 + j] = p_Data[i - iNContinue + j];
				}

				iCmpressedLength += 2 + iNContinue;
				btRet = btTmp;
			}

			return btRet;
		}
		#endregion

		#region "解压"
		/// <summary>
		/// 解压
		/// </summary>
		/// <param name="p_Data">需要解压得数据</param>
		/// <returns>解压后的数据</returns>
		public static Byte[] UnCompress(Byte[] p_Data)
		{
			Byte[] btRet = null;           //结果
			Byte[] btTmp = null;           //临时用来保存解压数据

			Int32 i = 0;
			Int32 j = 0;
			Byte[] btHead = new Byte[2];   //块头
			Int16 iDataCount = 0;          //数据块的长度
			BitArray ba = null;            //比特数组
			Int32 iDataLen = 0;            //解压后数据长度

			for (i = 0; i < p_Data.Length; i++)
			{
				try
				{
					//读出块头
					btHead[0] = p_Data[i];
					btHead[1] = p_Data[i + 1];

					ba = new BitArray(btHead);

					//计算出块的长度
					iDataCount = (Int16)BitConverter.ToUInt16(btHead, 0);

					if (ba.Get(15))
					{
						//最高位是1，为连续的字节块
						iDataCount = (Int16)((Int32)iDataCount + 32768); //最高位为1，算出的长度其实为负值

						//解压数据
						btTmp = null;
						btTmp = new Byte[iDataLen + iDataCount];

						//保存数据
						if (iDataLen > 0)
						{
							for (j = 0; j < iDataLen; j++)
							{
								btTmp[j] = btRet[j];
							}
						}

						//解压数据
						for (j = 0; j < iDataCount; j++)
						{
							btTmp[iDataLen + j] = p_Data[i + 2];
						}

						i += 2;
					}
					else
					{
						//不连续的字节块
						btTmp = null;
						btTmp = new Byte[iDataLen + iDataCount];
						//保存数据
						if (iDataLen > 0)
						{
							for (j = 0; j < iDataLen; j++)
							{
								btTmp[j] = btRet[j];
							}
						}

						//解压数据
						for (j = 0; j < iDataCount; j++)
						{
							btTmp[iDataLen + j] = p_Data[i + 2 + j];
						}

						i += 1 + iDataCount;
					}

					iDataLen += iDataCount;
					btRet = btTmp;
                }
                catch { }
			}

			return btRet;
		}
		#endregion
	}
}
