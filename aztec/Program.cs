using System.Text;

namespace aztec
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");



            string b64Input =
                   "BgQAANtYAAJDAPkxAHwAQXIw7zcGNN4ANiox+w81HrUGOP8eUABSAEUA+1oAWQBEDv9OAFQAIABN3wAuClMAvlQPV/eKUhq9Wg5X7k58UtcWSVq9TF5J79pBZ+5PAEsG12bTSm5GVQBM/ntSAEH7L1dj+0MAS1vvMvovewo3Ut4wDi39HjEAN6Pbl0FNe3YgPt5Q3kv3IlSevVnX1z9FMmuCShL2WgBaG9umKADvSAApJnx75k+itwZMAEx9X0rvbkSOTXtOOF/DRy0WOW53fPYLFoMzLr0xAi3DGnevLQOCfJ/vQZ5TcBZrN0oa9k4AfA82Q4QaDzj3q8deN6sN7zIE/1x8lbMnQdwBQi5ZT86jL2tqNAr2MwAw34xSH+uPSVPYFxZThBMzON8AMJM5wQA3MwRcMX7bNcET2jInwyedE01HZ4dlM94qKy0DL38fNgAqeBszSxOvNIeKfHM7fCLxNQAwVkMtdzl7Xiw/YMyrFzxQACBWw+Hza7c3C93/NWuHg1OWRquPQ5KP02K9IBZT4QZC9oNZU7aXFiOX83U4ADJFC7ADhrNVCyOW8w9qMbEnZhdHbHxjdjIT7E4DW0M3OQuGaxYmCSSSSSr/";
            string b64Expected =
          "WABYAEMAMQB8AEEAQQBBADAAMAAwADcAMAA0ADYAfAAxADQANgA1ADAANQA4AHwAUABSAEUAWgBZAEQARQBOAFQAIABNAC4AIABTAFQALgAgAFcAQQBSAFMAWgBBAFcAWQB8AEQAWgBJAEUATABOAEkAQwBBACAATQBPAEsATwBUANMAVwB8AFUATAAuACAAUgBBAEsATwBXAEkARQBDAEsAQQAgADIANQAvADIANwB8ADAAMgAtADUAMQA3ACAAVwBBAFIAUwBaAEEAVwBBAHwARABNAEkAIAAxAFAATgBLAHwAVABPAFkATwBUAEEAfABFADEAMgBKAHwAWgBaAEUAMQAyADAAKABIACkAfAB8AEMATwBSAE8ATABMAEEAfABKAFQARABLAE0AMgA4AEUAMQAwADAAMAA4ADkAMQAyADAAfAAyADAAMQAzAC0AMQAxAC0AMAA2AHwALQAtAC0AfABLAE8AVwBBAEwAUwBLAEkAIABKAEEATgB8AEoAQQBOAHwASwBPAFcAQQBMAFMASwBJAHwAfAA4ADIAMAA5ADEANwAxADEAMAAyADIAfAAwADIALQA1ADEANwB8AFcAQQBSAFMAWgBBAFcAQQB8AHwAVwBBAEEBQgBSAFoAWQBTAEsAQQB8ADIANAB8ADMAMAB8AEsATwBXAEEATABTAEsAQQAgAE0AQQBSAEkAQQB8AE0AQQBSAEkAQQB8AEsATwBXAEEATABTAEsAQQB8AHwAOAA4ADAAMwAwADkANwAxADAAMgAyAHwAMAAyAC0ANQAxADcAfABXAEEAUgBTAFoAQQBXAEEAfAB8AFcAQQBBAUIAUgBaAFkAUwBLAEEAfAAyADQAfAAzADAAfAAxADYANQA1AHwAMQA2ADUANQB8ADIANgA1ADUAfAAxADIAMAA1AHwATQAxAHwAZQAxADEAKgAyADAAMAAxAC8AMQAxADYAKgAwADEAOAAwACoAMAA0AHwAMgB8ADEAMAAwADAAfAA0ADUAMAB8AC0ALQAtAHwAMQAzADkAOAAsADAAMAB8ADcAMQAsADAAMAB8AFAAIAB8ADIAMAAwADUALQAwADcALQAwADEAfAA1AHwALQAtAC0AfABTAEEATQBPAEMASADTAEQAIABPAFMATwBCAE8AVwBZAHwALQAtAC0AfAAyADAAMAA1AHwALQAtAC0AfAA4ACwAOAAyAHwAQQBBAEEAMAAwADAAMAAwADAAMAB8ADAAMgA2ADUAMAAwADAAOAAwADAAMAAxADUAOAB8ADAAMwB8ADAAMgB8ADAAMAAwAHwAMgAwADAAMABOAE4ATgBOAE4ATgBOAE4AfAAwADAAOQAwADAAMgAwADAAMQB8AA==";


            var decoder = new VehicleDocumentAztecDecoder();

        var res =     decoder.Decode(b64Input);
            Console.WriteLine(res.ToString());
        }




        public class VehicleDocumentAztecDecoder
        {
            private const int START_OFFSET = 4;
            private byte[] src;
            private int ilen = START_OFFSET;
            private int currentByte;
            private int currentBit;
            private byte[] dst;

            public string Decode(string text)
            {
                byte[] decoded = Base64Decode(text);
                byte[] decompressed = DecompressNRV2E(decoded);
                return Encoding.Unicode.GetString(decompressed);
            }

            private byte[] DecompressNRV2E(byte[] sourceData)
            {
                src = sourceData;

                uint olen = 0, last_m_off = 1;

                dst = new byte[BitConverter.ToInt32(src, 0)];

                while (ilen < src.Length)
                {
                    uint m_off, m_len;

                    while (GetBit() == 1)
                    {
                        dst[olen++] = src[ilen++];
                    }

                    m_off = 1;
                    while (true)
                    {
                        m_off = m_off * 2 + GetBit();
                        if (GetBit() == 1) break;
                        m_off = (m_off - 1) * 2 + GetBit();
                    }

                    if (m_off == 2)
                    {
                        m_off = last_m_off;
                        m_len = GetBit();
                    }
                    else
                    {
                        m_off = (m_off - 3) * 256 + src[ilen++];
                        if (m_off == 0xffffffff)
                            break;
                        m_len = (m_off ^ 0xffffffff) & 1;
                        m_off >>= 1;
                        last_m_off = ++m_off;
                    }
                    if (m_len > 0)
                        m_len = (uint)1 + GetBit();
                    else if (GetBit() == 1)
                        m_len = (uint)3 + GetBit();
                    else
                    {
                        m_len++;
                        do
                        {
                            m_len = m_len * 2 + GetBit();
                        } while (GetBit() == 0);
                        m_len += 3;
                    }
                    m_len += (uint)(m_off > 0x500 ? 1 : 0);

                    uint m_pos;
                    m_pos = olen - m_off;

                    dst[olen++] = dst[m_pos++];
                    do dst[olen++] = dst[m_pos++]; while (--m_len > 0);
                }
                return dst;
            }

            private byte GetBit()
            {
                if (ilen >= src.Length)
                    throw new Exception("Przesunięcie jest poza zakresem.");

                if (currentBit == 0)
                {
                    currentByte = src[ilen++];
                    currentBit = 8;
                }

                return (byte)(((uint)currentByte >> --currentBit) & 1);
            }

            private byte[] Base64Decode(string textToDecode)
            {
                if (string.IsNullOrWhiteSpace(textToDecode)) return new byte[0];

                if (textToDecode.Length % 2 == 1)
                {
                    textToDecode = textToDecode.Substring(0, textToDecode.Length - 1);
                }

                return Convert.FromBase64String(textToDecode);
            }
        }
    }
}
