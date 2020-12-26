namespace HireMe.Core.Helpers
{
    using System;

    public class EikValidator 
    {
        protected static readonly byte[] Eik13 = new byte[13];
        protected static readonly byte[] Eik09 = new byte[9];

        public static bool Is13 { get; set; }

        public static bool checkEIK(string eik)
        {
            ulong eikUlong = 0;
           
            try
            {
                eikUlong = Convert.ToUInt64(eik);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }

            if (eik.Length != 9 && eik.Length != 13)
            {
             //   Console.WriteLine("EIK(string) Параметърът е извън обхвата!");
             //   throw new ArgumentException("Параметърът е извън обхвата!");
                return false;
            }
            else if (eik.Length == 9)
            {
                Is13 = false;
                for (int i = 8; i >= 0; i--)
                {
                    Eik09[i] = (byte)(eikUlong % 10);
                    eikUlong /= 10;
                }
                return true;
            }
            else
            {
                Is13 = true;
                for (int i = 12; i >= 0; i--)
                {
                    Eik13[i] = (byte)(eikUlong % 10);
                    eikUlong /= 10;
                }
                return true;
            }
            
        }

        protected bool IsValid()
        {
            int result = 0;
            int checksum = 0;
            if (!Is13)
            {
                for (int i = 0; i < 8; i++)
                {
                    result += (i + 1) * Eik09[i];
                }

                checksum = result % 11;

                if (checksum >= 10)
                {
                    result = 0;
                    for (int i = 0; i < 8; i++)
                    {
                        result += (i + 3) * Eik09[i];
                    }

                    if (checksum >= 10)
                    {
                        checksum = 0;
                    }
                }
                if (Eik09[8] != checksum)
                {
                    return false;
                }
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    result += (i + 1) * Eik13[i];
                }

                checksum = result % 11;

                if (checksum >= 10)
                {
                    result = 0;
                    for (int i = 0; i < 8; i++)
                    {
                        result += (i + 3) * Eik13[i];
                    }

                    if (checksum >= 10)
                    {
                        checksum = 0;
                    }
                }
                if (Eik13[8] != checksum)
                {
                    return false;
                }

                result = 2 * Eik13[8];
                result += 7 * Eik13[9];
                result += 3 * Eik13[10];
                result += 5 * Eik13[11];
                checksum = result % 11;

                if (checksum >= 10)
                {
                    result = 4 * Eik13[8];
                    result += 9 * Eik13[9];
                    result += 5 * Eik13[10];
                    result += 7 * Eik13[11];
                    checksum = result % 11;

                    if (checksum >= 10)
                    {
                        checksum = 0;
                    }
                }

                if (Eik13[12] != checksum)
                {
                    return false;
                }
            }

            return true;
        }

        protected void FixChecksum()
        {
            if (this.IsValid())
            {
                return;
            }

            int result = 0;
            int checksum = 0;

            if (!Is13)
            {
                for (int i = 0; i < 8; i++)
                {
                    result += (i + 1) * Eik09[i];
                }

                checksum = result % 11;

                if (checksum >= 10)
                {
                    result = 0;
                    for (int i = 0; i < 8; i++)
                    {
                        result += (i + 3) * Eik09[i];
                    }

                    if (checksum >= 10)
                    {
                        checksum = 0;
                    }
                }
                Eik09[8] = (byte)checksum;
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    result += (i + 1) * Eik13[i];
                }

                checksum = result % 11;

                if (checksum >= 10)
                {
                    result = 0;
                    for (int i = 0; i < 8; i++)
                    {
                        result += (i + 3) * Eik13[i];
                    }

                    if (checksum >= 10)
                    {
                        checksum = 0;
                    }
                }

                Eik13[8] = (byte)checksum;

                result = 2 * Eik13[8];
                result += 7 * Eik13[9];
                result += 3 * Eik13[10];
                result += 5 * Eik13[11];
                checksum = result % 11;

                if (checksum >= 10)
                {
                    result = 4 * Eik13[8];
                    result += 9 * Eik13[9];
                    result += 5 * Eik13[10];
                    result += 7 * Eik13[11];
                    checksum = result % 11;

                    if (checksum >= 10)
                    {
                        checksum = 0;
                    }
                }

                Eik13[12] = (byte)checksum;
            }
        }


    }

    }

